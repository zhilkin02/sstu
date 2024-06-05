using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pr1.Helpers;
using pr1.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pr1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
        {
            InitializeComponent();

            var db = App.UserDB.GetUser();
          if(db.Sch==0)  name.Text = db.Group;  else name.Text = Teachrer(db.Teacher);
            switch (Settings.Theme)
            {
                case 0:
                    RadioButtonSystem.IsChecked = true;
                    break;
                case 1:
                    RadioButtonLight.IsChecked = true;
                    break;
                case 2:
                    RadioButtonDark.IsChecked = true;
                    break;
            }
        }

        bool loaded;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loaded = true;


            var db = App.UserDB.GetUser();
            if (db.Sch == 0) name.Text = db.Group; else name.Text = Teachrer(db.Teacher);

        }
        private string Teachrer(string teacher)
        {
            var str = teacher.Split(' ');

            return str[0] +" "+ str[1][0]+"."+ str[2][0]+".";
        }
        void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!loaded)
                return;

            if (!e.Value)
                return;

            var val = (sender as RadioButton)?.Value as string;
            if (string.IsNullOrWhiteSpace(val))
                return;

            switch (val)
            {
                case "System":
                    Settings.Theme = 0;
                    break;
                case "Light":
                    Settings.Theme = 1;
                    break;
                case "Dark":
                    Settings.Theme = 2;
                    break;
            }

            TheTheme.SetTheme();
        }

        private async void Button_Clicked_Group(object sender, EventArgs e)
        {
            bool result = false;
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
                 result = await DisplayAlert("Подтвердить действие", "Для открытия списка требуется интернет, вы уверены?", "Да", "Нет");
            if (result || current == NetworkAccess.Internet)
                //  await Navigation.PushAsync(new ListGroups());
                // await App.Current.MainPage.Navigation.PushAsync(new ListGroups());
            //    await Shell.Current.Navigation.PopToRootAsync();

            //await Shell.Current.GoToAsync("/" + nameof(StartPage) + "/" + nameof(ListGroups));
            //await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushModalAsync(new ListGroups());
           if(Shell.Current.Navigation.ModalStack.Count==0)
            await Shell.Current.Navigation.PopAsync();
            NavigationPage.SetHasNavigationBar(this, false);

            Shell.SetFlyoutItemIsVisible(this, false);
            //App.ScheduleDB.DeleteAllSchedules();
            //App.ScheduleTeacherDB.DeleteAllSchedules();


        }
        private async void Button_Clicked_Teacher(object sender, EventArgs e)
        {
            bool result= false;
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet) 
                 result = await DisplayAlert("Подтвердить действие", "Для открытия списка требуется интернет, вы уверены?", "Да", "Нет");
            if (result || current == NetworkAccess.Internet)
                //  await Navigation.PushAsync(new ListTeacher());
                // await App.Current.MainPage.Navigation.PushAsync(new ListTeacher());
            //    await Shell.Current.Navigation.PopToRootAsync();

            //await Shell.Current.GoToAsync("/" + nameof(StartPage)  +"/"+ nameof(ListTeacher));
            //await Shell.Current.Navigation.PopAsync();
           // await Shell.Current.Navigation.PushModalAsync(new ListTeacher());
          //  await Shell.Current.Navigation.PopToRootAsync();

            // Добавить страницы в нужном порядке
           // await Shell.Current.Navigation.PushAsync(new StartPage());
            await Shell.Current.Navigation.PushAsync(new ListTeacher());
            //App.ScheduleDB.DeleteAllSchedules();
            //App.ScheduleTeacherDB.DeleteAllSchedules();
            NavigationPage.SetHasNavigationBar(this, false);

            Shell.SetFlyoutItemIsVisible(this, false);

        }
        private async void  Button_Start(object sender, EventArgs e)
        {


            //  Navigation.PushAsync(new StartPage("settings"));
            //   await App.Current.MainPage.Navigation.PushAsync( new StartPage());
            //await Shell.Current.GoToAsync("/" + nameof(StartPage));
            //while (Shell.Current.Navigation.NavigationStack.Count > 1)
            //{
            //    await Shell.Current.Navigation.PopAsync();

            //}

           // await Shell.Current.Navigation.PushAsync(new StartPage("Settings"));
            Navigation.PushAsync(new StartPage("Settings"));
            NavigationPage.SetHasNavigationBar(this, false);
            Shell.SetFlyoutItemIsVisible(this, false);
        }

        private void ButtonL_Clicked(object sender, EventArgs e)
        {

            Shell.Current.FlyoutIsPresented = true;


        }
    }
}