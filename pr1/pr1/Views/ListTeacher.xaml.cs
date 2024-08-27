using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlCSS.Utils;

namespace pr1.Views
{
    public partial class ListTeacher : ContentPage
    {
        public ObservableCollection<Teacher> Teachers { get; set; }

        public ListTeacher()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Shell.SetFlyoutItemIsVisible(this, false);
            Teachers = new ObservableCollection<Teacher>();
            NavigationPage.SetHasNavigationBar(this, false);
            Shell.SetFlyoutItemIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            LoadTeachersDataFromParser();

            teacherListView.ItemsSource = Teachers;
        }

        private async void OnGroupSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedTeacher = (Teacher)e.SelectedItem;

            // Используем указанный вами метод навигации для перехода на страницу NotesPage
            await Navigation.PushAsync(new NotesPage());

            // Очищаем выделение в списке
            teacherListView.SelectedItem = null;

            Shell.SetTabBarIsVisible(this, true);
        }

    
        private async void LoadTeachersDataFromParser()
        {
             var parserResult = await ParsingAsync("https://rasp.sstu.ru/rasp/teachers/");
            //var parserResult = await ParsingAsync("https://pr1.my1.ru/teachers.html");


            if (parserResult != null && parserResult.ContainsKey("Группы и ссылки на расписание"))
            {
                var teachersFromParser = parserResult["Группы и ссылки на расписание"];

                foreach (var groupInfo in teachersFromParser)
                {
                    // Предположим, что groupInfo[0] содержит название группы, а groupInfo[1] - ссылку
                    Teachers.Add(new Teacher { TeacherName = groupInfo[0], ScheduleLink = groupInfo[1] });
                }
            }
            else
            {
                // Обработка ошибки загрузки данных из парсера
                Console.WriteLine("Ошибка при загрузке данных из парсера");
            }
        }



        private static async Task<Dictionary<string, List<List<string>>>> ParsingAsync(string url)
        {
            try
            {
                Dictionary<string, List<List<string>>> result = new Dictionary<string, List<List<string>>>();

                using (var client = new HttpClient())
                {
                    var html = await client.GetStringAsync(url);

                    var parser = new HtmlParser();
                    var document = parser.ParseDocument(html);

                    var raspBlock = document.QuerySelector(".teachers_list");

                    if (raspBlock != null)
                    {
                        var teachers = raspBlock.QuerySelectorAll(".teacher");

                        if (teachers != null && teachers.Length > 0)
                        {
                            var res = new List<List<string>>();

                            foreach (var teacher in teachers)
                            {
                                Console.WriteLine("prepod: "+ teacher.Text());

                                var teacherLink = teacher.QuerySelector("a");
                                if (teacherLink != null)
                                {
                                    var groupName = teacherLink.TextContent.Trim();
                                    var teacherUrlRelative = teacherLink.GetAttribute("href");
                                    var groupUrlFull = new Uri(new Uri("https://rasp.sstu.ru"), teacherUrlRelative).ToString();
                                    //   var groupUrlFull = new Uri(new Uri("https://pr1.my1.ru"), groupUrlRelative).ToString();
                                  //  Console.WriteLine("тест teacherUrlRelative: " + teacherUrlRelative);
                                     res.Add(new List<string> { groupName, groupUrlFull });
                                }
                            }

                            result["Группы и ссылки на расписание"] = res;
                            return result;
                        }
                        else
                        {
                            Console.WriteLine("No groups");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rasp-block");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return null;
        }




        private async void OnGroupTapped(object sender, EventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Teacher selectedTeacher)
            {
              //  Views.Teacher grup = new Views.Teacher() { TeacherName = " ", ScheduleLink = "https://pr1.my1.ru/36284.html" };

               // DisplayAlert("тест", "выбран: " + selectedTeacher.TeacherName + ": " + selectedTeacher.ScheduleLink+"\n"+
              //   "создан: " + grup.TeacherName+": "+ grup.ScheduleLink, "OK");

                App.UserDB.AddOrUpdateTeacherURL(selectedTeacher.ScheduleLink);
                App.UserDB.AddOrUpdateTeacher(selectedTeacher.TeacherName);
                Console.WriteLine("selectedTeacher.ScheduleLin: "+ selectedTeacher.ScheduleLink);
                var test = App.UserDB.GetUser();
                Console.WriteLine("selectedTeacher.Teatcher: " + test.Teacher);

                Console.WriteLine("selectedTeacher.ScheduleLin: " + test.TeacherURL);

                //
                //   await Shell.Current.GoToAsync($"//schedulePage2?grup={grup}");
                // new SchedulePage2(selectedTeacher);  // await Shell.Current.Navigation.P
                await Shell.Current.Navigation.PushAsync(new SchedulePage2(selectedTeacher));

                // Очищаем выделение в списке
                listView.SelectedItem = null;

            }
        }



        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {   // Приводим к нижнему регистру для регистронезависимого поиска
            string searchText = searchBar.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Если строка поиска пуста, отображаем все группы
                teacherListView.ItemsSource = Teachers;
            }
            else
            {
                // Иначе фильтруем группы по введенному тексту
                var filteredTeachers = Teachers.Where(teacher => teacher.TeacherName.ToLower().Contains(searchText));
                teacherListView.ItemsSource = new ObservableCollection<Teacher>(filteredTeachers);
            }
        }

    }

    public class Teacher
    {
        public string TeacherName { get; set; }
        public string ScheduleLink { get; set; }
    }
}
