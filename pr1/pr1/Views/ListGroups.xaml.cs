using AngleSharp.Html.Parser;
using AngleSharp.Io;
using pr1.Helpers;
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
    public partial class ListGroups : ContentPage
    {
        public ObservableCollection<Group> Groups { get; set; }

        public ListGroups()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Shell.SetFlyoutItemIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            Groups = new ObservableCollection<Group>();

            // Пример: загрузка данных извне
            //  LoadGroupsData();
            // Вызываем метод в обработчике события загрузки страницы
            LoadGroupsDataFromParser();

            groupListView.ItemsSource = Groups;
        }

        private async void OnGroupSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedGroup = (Group)e.SelectedItem;

            // Используем указанный вами метод навигации для перехода на страницу NotesPage
            await Navigation.PushAsync(new NotesPage());

            // Очищаем выделение в списке
            groupListView.SelectedItem = null;

            Shell.SetTabBarIsVisible(this, true);
        }

        //private void LoadGroupsData()
        //{

        //    Groups.Add(new Group { GroupName = "б-ПИНЖ-11", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б-РКЛМ-21", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "Б1-ИВЧТ-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б-ТЛВД-41", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "Б1-ПИНЖ-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б1-ИВЧТ-11", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б1-ИВЧТ-21", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "Б1-ИВЧТ-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б1-ИВЧТ-41", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б-ПИНЖ-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //    Groups.Add(new Group { GroupName = "б-ПИНЖ-32", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" });
        //}


        private async void LoadGroupsDataFromParser()
        {
           // var parserResult = await ParsingAsync("https://rasp.sstu.ru/");
           // var parserResult = await ParsingAsync("https://web.archive.org/web/20220125183833/https://rasp.sstu.ru/");
            var parserResult = await ParsingAsync("https://sstu.my1.ru/groups.html");


            if (parserResult != null && parserResult.ContainsKey("Группы и ссылки на расписание"))
            {
                var groupsFromParser = parserResult["Группы и ссылки на расписание"];

                foreach (var groupInfo in groupsFromParser)
                {
                    // Предположим, что groupInfo[0] содержит название группы, а groupInfo[1] - ссылку
                    Groups.Add(new Group { GroupName = groupInfo[0], ScheduleLink = groupInfo[1] });
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

                    var raspBlock = document.QuerySelector(".rasp-block");

                    if (raspBlock != null)
                    {
                        var groups = raspBlock.QuerySelectorAll(".group");

                        if (groups != null && groups.Length > 0)
                        {
                            var res = new List<List<string>>();

                            foreach (var group in groups)
                            {
                                var groupLink = group.QuerySelector("a");
                                if (groupLink != null)
                                {
                                    var groupName = groupLink.TextContent.Trim();
                                    var groupUrlRelative = groupLink.GetAttribute("href");
                                   // var groupUrlFull = new Uri(new Uri("https://rasp.sstu.ru"), groupUrlRelative).ToString();
                                    //  var groupUrlFull = new Uri(new Uri("https://web.archive.org"), groupUrlRelative).ToString();
                                
                                    var groupUrlFull = new Uri(new Uri("https://sstu.my1.ru/group"), groupUrlRelative).ToString();
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
            if (sender is ListView listView && listView.SelectedItem is Group selectedGroup)
            {

                if (Shell.Current.Navigation.ModalStack.Count > 0)
                {


                    App.UserDB.AddOrUpdateURL(selectedGroup.ScheduleLink);
                    App.UserDB.AddOrUpdateGroup(selectedGroup.GroupName);
                    await Shell.Current.Navigation.PushAsync(new SchedulePage(selectedGroup));
                    await Shell.Current.Navigation.PopAsync();
                  //  await Shell.Current.Navigation.PopModalAsync();

                }
                else
                {
                    Console.WriteLine("assig^ " + selectedGroup.ScheduleLink + " |  " + selectedGroup.GroupName);
                    App.UserDB.AddOrUpdateURL(selectedGroup.ScheduleLink);
                    App.UserDB.AddOrUpdateGroup(selectedGroup.GroupName);
                    await Shell.Current.Navigation.PushAsync(new SchedulePage(selectedGroup));

                }

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
                groupListView.ItemsSource = Groups;
            }
            else
            {
                // Иначе фильтруем группы по введенному тексту
                var filteredGroups = Groups.Where(group => group.GroupName.ToLower().Contains(searchText));
                groupListView.ItemsSource = new ObservableCollection<Group>(filteredGroups);
            }
        }

    }

    public class Group
    {
        public string GroupName { get; set; }
        public string ScheduleLink { get; set; }
    }
}
