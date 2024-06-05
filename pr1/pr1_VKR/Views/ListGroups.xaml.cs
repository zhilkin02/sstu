//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Net.Http;
//using Xamarin.Forms;

//namespace pr1.Views
//{
//    public partial class ListGroups : ContentPage
//    {
//        private ObservableCollection<Group> Groups;
//        private ObservableCollection<Group> filteredGroups;

//        public ListGroups()
//        {
//            InitializeComponent();

//            // Загружаем список групп из внешнего источника (в реальном приложении)
//            LoadGroups();

//            // Используем filteredGroups для отображения и фильтрации
//            filteredGroups = new ObservableCollection<Group>(Groups);
//            groupListView.ItemsSource = filteredGroups;
//        }

//        protected override void OnAppearing()
//        {
//            base.OnAppearing();
//            Shell.SetTabBarIsVisible(this, false);
//        }

//        protected override void OnDisappearing()
//        {
//            base.OnDisappearing();
//            Shell.SetTabBarIsVisible(this, true);
//        }


//        private static Dictionary<string, List<List<string>>> Parsing(string url)
//        {
//            try
//            {
//                Dictionary<string, List<List<string>>> result = new Dictionary<string, List<List<string>>>();

//                using (HttpClientHandler hdl = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None })
//                {
//                    using (var clnt = new HttpClient(hdl))
//                    {
//                        using (HttpResponseMessage resp = clnt.GetAsync(url).Result)
//                        {
//                            if (resp.IsSuccessStatusCode)
//                            {
//                                var html = resp.Content.ReadAsStringAsync().Result;
//                                if (!string.IsNullOrEmpty(html))
//                                {
//                                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
//                                    doc.LoadHtml(html);

//                                    var tables = doc.DocumentNode.SelectNodes(".//div[@class='accordion rasp-structure'][@id='raspStructure']//div[@class='card']");

//                                    if (tables != null && tables.Count > 0)
//                                    {
//                                        foreach (var table in tables)
//                                        {
//                                            var titleNode = table.SelectSingleNode(".//div[@class='collapse']");
//                                            if (titleNode != null)
//                                            {
//                                                var tbl = titleNode.SelectSingleNode(".//div[@class='card-body']");
//                                                if (tbl != null)
//                                                {
//                                                    var grups = tbl.SelectNodes(".//div[@class='row groups']");

//                                                    var res = new List<List<string>>();
//                                                    foreach (var rgrup in grups)
//                                                    {
//                                                        if (rgrup != null && rgrup.InnerText.Length > 0)
//                                                        {
//                                                            var ngrups = rgrup.SelectNodes(".//div[@class='col']//div[@class='row no-gutters']//div[@class='col-auto group']");
//                                                            if (ngrups != null && ngrups.Count > 0)
//                                                            {
//                                                                foreach (var row in ngrups)
//                                                                {
//                                                                    var cells = row.SelectNodes(".//a");
//                                                                    if (cells != null && cells.Count > 0)
//                                                                    {
//                                                                        res.Add(new List<string>(cells.Select(c => (c.InnerText + "|https://rasp.sstu.ru" + c.Attributes["href"].Value))));
//                                                                    }
//                                                                    else
//                                                                    {
//                                                                        Console.WriteLine("No  cells");
//                                                                    }
//                                                                }

//                                                                result[titleNode.InnerText] = res;
//                                                            }
//                                                            else
//                                                            {
//                                                                Console.WriteLine("No  ngrups");
//                                                            }
//                                                        }
//                                                        else
//                                                        {
//                                                            Console.WriteLine("No  grups");
//                                                        }
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    Console.WriteLine("No tbl");
//                                                }
//                                            }
//                                            else
//                                            {
//                                                Console.WriteLine("No  titleNode");
//                                            }
//                                        }

//                                        return result;
//                                    }
//                                    else
//                                    {
//                                        Console.WriteLine("No tables");
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }

//            return null;
//        }

//        private void LoadGroups()
//        {
//            Dictionary<string, List<List<string>>> result = Parsing(url: "https://rasp.sstu.ru/");
//            if (result != null)
//            {
//                Groups = new ObservableCollection<Group>();
//                foreach (var item in result)
//                {
//                    foreach (var r in item.Value)
//                    {
//                        foreach (var data in r)
//                        {
//                            var splitData = data.Split('|');
//                            if (splitData.Length == 2)
//                            {
//                                Groups.Add(new Group { GroupName = splitData[0], ScheduleLink = splitData[1] });
//                            }
//                        }
//                    }
//                }
//            }
//            else
//            {
//                Console.WriteLine("no parsing");
//                Groups = new ObservableCollection<Group>
// {
//     new Group { GroupName = "б-ПИНЖ-11", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" },
//     new Group { GroupName = "б-РКЛМ-21", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8" },
//        new Group { GroupName = "Б1-ИВЧТ-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"  },
//     new Group { GroupName = "б-ТЛВД-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"  },
//        new Group { GroupName = "б-ТЛВД-41", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"  },
//     new Group { GroupName = "Б1-ПИНЖ-31", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"  },
//        new Group { GroupName = "б1-ИВЧТ-11", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"  },
//     new Group { GroupName = "б1-ИВЧТ-21", ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"  },
//        new Group { GroupName = "Б1-ИВЧТ-31" , ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"},
//     new Group { GroupName = "б1-ИВЧТ-41" , ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"},
//      new Group { GroupName = "б-ПИНЖ-31" , ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"},
//       new Group { GroupName = "б-ПИНЖ-32" , ScheduleLink = "https://rasp.sstu.ru/rasp/group/8"},
//     // В реальном приложении загружайте группы из внешнего источника
// };
//            }
//        }



//        //private async void OnGroupTapped(object sender, EventArgs e)
//        //{
//        //    if (sender is ListView listView && listView.SelectedItem is Group selectedGroup)
//        //    {
//        //        // Используем указанный вами метод навигации для перехода на страницу NotesPage
//        //        await Navigation.PushAsync(new NotesPage());

//        //        // Очищаем выделение в списке
//        //        listView.SelectedItem = null;

//        //        Shell.SetTabBarIsVisible(this, true);
//        //    }
//        //}


//        private async void OnGroupTapped(object sender, EventArgs e)
//        {
//            var selectedGroup = (Group)((ListView)sender).SelectedItem;

//            if (selectedGroup != null)
//            {
//                // Используйте selectedGroup.ScheduleLink для передачи ссылки на следующую страницу
//                await Navigation.PushAsync(new NotesPage(selectedGroup.ScheduleLink));
//            }

//            Shell.SetTabBarIsVisible(this, true);
//        }


//        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
//        {
//            // Фильтруем группы по введенному тексту в SearchBar
//            filteredGroups.Clear();
//            if (string.IsNullOrWhiteSpace(e.NewTextValue))
//            {
//                foreach (var group in Groups)
//                    filteredGroups.Add(group);
//            }
//            else
//            {
//                foreach (var group in Groups.Where(g => g.GroupName.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0))
//                    filteredGroups.Add(group);
//            }
//        }
//    }

//    //public class Group
//    //{
//    //    public string GroupName { get; set; }
//    //    public string ScheduleLink { get; set; }

//    //}
//}
