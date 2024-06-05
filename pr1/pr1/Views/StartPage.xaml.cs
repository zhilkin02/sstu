using pr1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#pragma warning disable CS0105 // Директива Using уже использовалась в этом пространстве имен
using System.Collections.Generic;
#pragma warning restore CS0105 // Директива Using уже использовалась в этом пространстве имен
#pragma warning disable CS0105 // Директива Using уже использовалась в этом пространстве имен
using System.Linq;
using Xamarin.Essentials;
using pr1.Data;
using static System.Net.Mime.MediaTypeNames;
#pragma warning restore CS0105 // Директива Using уже использовалась в этом пространстве имен

namespace pr1.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
	{
        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }

        string setting = "";
        public StartPage ()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Shell.SetFlyoutItemIsVisible(this, false);

            //App.ScheduleDB.DeleteAllSchedules();
            //App.ScheduleTeacherDB.DeleteAllSchedules();

            //App.UserDB.DeleteAll();
            setting = "";
            var sc = App.ScheduleDB.GetScheduls();
            // sc = null;


            if (sc != null && sc.Count > 0)
            {
                var db = App.UserDB.GetUser();
                int it = db.Sch;
                //  Shell.Current.GoToAsync($"//Main/SchedulePage?link=your_link_here");
                //NavigationPage.SetHasNavigationBar(this, true);
                //Shell.SetFlyoutItemIsVisible(this, true);

                if (it == 0)
                    Navigation.PushAsync(new SchedulePage(new Group { GroupName = db.Group, ScheduleLink = db.URL }));
                else
                    Navigation.PushAsync(new SchedulePage2(new Teacher { TeacherName = db.Teacher, ScheduleLink = db.TeacherURL }));

            }

            Shell.SetTabBarIsVisible(this, false);
        }

        public StartPage(string page)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            setting = page;

            Shell.SetFlyoutItemIsVisible(this, false);
            InitializeComponent();
            // Shell.SetTabBarIsVisible(this, false);
        }


#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        private async void LoadNote(string value)
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        {
            //    InitializeComponent();

            try
            {
                int id = Convert.ToInt32(value);
                Note note = App.NotesDB.GetNote(id);
                BindingContext = note;
            }
            catch { }
        }




        private async void Button_Clicked_Group(object sender, EventArgs e)
        {
            // string entryText = nameEntry.Text;
            App.UserDB.AddOrUpdateSch(0);

            // App.UserDB.AddOrUpdateName(entryText);
            var url = App.UserDB.GetUser();
            var schedule = App.ScheduleDB.GetScheduls();
            var current = Connectivity.NetworkAccess;
            //  if (schedule.Count > 0) Console.WriteLine("ScheduleDB: "+ schedule[0].Subject);
            if (setting == "Settings")
            {
                //var db = App.UserDB.GetUser();
                //int it = db.Sch;
                Shell.SetFlyoutItemIsVisible(this, true);
                NavigationPage.SetHasNavigationBar(this, true);
                    Navigation.PushAsync(new SchedulePage());
                
                setting = "";

            }
            else
            if (current == NetworkAccess.Internet)
            {
                if (schedule.Count > 0)
                {

                    await Navigation.PushAsync(new SchedulePage(new Group { GroupName = url.Group, ScheduleLink = url.URL }));
                }
                else
                {
                    await Navigation.PushAsync(new ListGroups());
                }
            }
            else
            {
                if (schedule.Count > 0)
                {
                    await Navigation.PushAsync(new SchedulePage());
                }
                else
                {
                    DisplayAlert("Рассписание не было загружено",
                        "Для загрузки расписания нужен интернет, в дальнейшем раписание сохраняется и работает без интернета", "OK");
                }
            }
        }

        private async void Button_Clicked_Teacher(object sender, EventArgs e)
        {
            //  string entryText = nameEntry.Text;

            //App.UserDB.AddOrUpdateName(entryText);
            App.UserDB.AddOrUpdateSch(1);
            var url = App.UserDB.GetUser();

            var scheduleT = App.ScheduleTeacherDB.GetScheduls();
            var current = Connectivity.NetworkAccess;
          //if(scheduleT.Count>0) 
          //      Console.WriteLine("ScheduleTeacherDB1: " + scheduleT[0].Subject);
          //  var test = App.UserDB.GetUser();
            //Console.WriteLine("selectedTeacher.Teatcher: " + test.Teacher);

            //Console.WriteLine("selectedTeacher.ScheduleLin: " + test.TeacherURL);
            if (current == NetworkAccess.Internet)
            {
                if (scheduleT.Count > 0)
                {
                    await Navigation.PushAsync(new SchedulePage2(new Teacher { TeacherName = url.Teacher, ScheduleLink = url.TeacherURL }));
                }
                else
                {
                    await Navigation.PushAsync(new ListTeacher());
                }
            }
            else
            {
                if (scheduleT.Count > 0)
                {
                    Console.WriteLine("url.Teacher: " + url.Teacher);
                    Console.WriteLine("url.TeacherURL: " + url.TeacherURL);
                    await Navigation.PushAsync(new SchedulePage2());
                }
                else
                {

                    DisplayAlert("Рассписание не было загружено", 
                        "Для загрузки расписания нужен интернет, в дальнейшем раписание сохраняется и работает без интернета", "OK");
                }
            }


        }

    }
}