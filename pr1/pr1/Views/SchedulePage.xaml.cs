using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using pr1.Data;
using pr1.Models;
using pr1;
using Xamarin.Forms.Xaml;
using SQLitePCL;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using Xamarin.Essentials;
namespace pr1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        private int bol = 1;
        private static string dayOfWeek = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("ru-RU"));
        private static string cDayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);
        private List<Day> listDays;
        private static string socrDayOfWeek = DateTime.Now.ToString("ddd", new System.Globalization.CultureInfo("ru-RU"));
        ObservableCollection<Day> allLessons = new ObservableCollection<Day>();
        List<Week> weeks = new List<Week>();
        Day data;
        int ni=0;
        private static int countDay, countWeek;
        private string currentDate;
        private int targetWeekNumber=0;
        public void getDate(string date)
        {
            currentDate = date;
        }
        public string setDate()
        {
            return currentDate;
        }
        public SchedulePage()
        {
            InitializeComponent();
            bol = 0;
            //List<Week> listNoFullDays = setRasp(); ;
            //listDays = ParsButton(listNoFullDays);
            //collectionView.ItemsSource = listDays[ni].Lessons; // bol = 0;
            var r = ShowScheduls();
            targetWeekNumber = 0;
            if (r != null)
                ParsButton(r);
            collectionView.ItemsSource = listDays[ni].Lessons;
            ColorButton(listDays);
           
            // collectionView.ItemsSource = listDays[ni].Lessons;
        }
        public  SchedulePage(Views.Group grup)
        {

            InitializeComponent();
            var current = Connectivity.NetworkAccess;
            App.UserDB.AddOrUpdateSch(0);
             var web = new HtmlWeb
                {
                    OverrideEncoding = Encoding.UTF8 // Укажите правильную кодировку
                };
            if (current == NetworkAccess.Internet)
            {
                document = web.Load(grup.ScheduleLink);
                //HtmlWeb web = new HtmlWeb();
                //document = web.Load(grup.ScheduleLink);
                // DisplayAlert(" test URL: ", grup.ScheduleLink, "OK");
                // Выбор блока с расписанием
                if(document !=null)
                calendarNode = document.DocumentNode.SelectSingleNode("//div[@class='calendar']");
                else Console.WriteLine("document not");
            }
                if (calendarNode != null)
                {
                    targetWeekNumber = 0;
                    var listNoFullDays = setRasp();
               
                    listDays = ParsButton(listNoFullDays);
                // Парсинг списка зачетов

                GetExams(calendarNode);

                    currentDate = DateTime.Now.ToString("dd.MM");
                    ni = listDays.Count - k;
                collectionView.ItemsSource = listDays[ni].Lessons;

            }
            else
                {

                var db = App.ScheduleDB.GetScheduls();
                if (db.Count>0) {
                    var sch = ConvertToWeeks(db);
                    listDays = ParsButton(sch);
                }
                else
                {
                    DisplayAlert("Ошибка!", "Блок 'calendar' не найден на странице.", "OK");
                    Console.WriteLine("Блок 'calendar' не найден на странице.");
                }
              
                    
                }
                ColorButton(listDays);
           

        }
        private async void GetExams(HtmlNode calendarNode)
        {
            var exams = ParseExams(calendarNode);
            var str = App.UserDB.GetUser().Exams;
            App.UserDB.AddOrUpdateExams(exams);
            // Вывод списка зачетов
            var current = Connectivity.NetworkAccess;
            //  if (schedule.Count > 0) Console.WriteLine("ScheduleDB: "+ schedule[0].Subject);
            if (current == NetworkAccess.Internet)
                if (exams.Any())
            {
                bool rez;
                if (str.Contains(exams))
                {
                    App.UserDB.AddOrUpdateExams(exams);
                    DisplayAlert("Пользователь, у вас начинается сессия", exams, "OK");


                }
                else
                {
                    rez = await DisplayAlert("Пользователь, у вас начинается сессия", exams, "Сохранить", "OK");
                    if (rez) { App.UserDB.AddOrUpdateExams(exams); }
                }

                }
                else
                {
                    if(str != null && str.Length>0)
                        DisplayAlert("Пользователь, у вас начинается сессия", exams, "OK");

                }
        }
     private List<Day> GetSchedule() { return new List<Day>(); }
        private List<Week> ShowScheduls()
        {
            var db = App.UserDB;
            List<Week> weeks;
            List<Schedule> sc;
            if(db.GetUser().Sch==0)
             sc = App.ScheduleDB.GetScheduls();
            else sc = App.ScheduleTeacherDB.GetScheduls();
            if (sc.Count>0)
                weeks = ConvertToWeeks(sc);
            else
                weeks = null;
            return weeks;
        }
        
        public List<Week> ConvertToWeeks(List<Schedule> schedules)
        {
            var weeks = new List<Week>();
            // Группировка расписания по номеру недели
            var groupedByWeek = schedules.GroupBy(s => s.WeekNumber);
            foreach (var weekGroup in groupedByWeek)
            {
                var week = new Week { WeekNumber = weekGroup.Key };
                // Группировка занятий по дню недели и дате
                var groupedByDay = weekGroup.GroupBy(s => new { s.DayName, s.Date });
                foreach (var dayGroup in groupedByDay)
                {
                    var day = new Day { DayName = dayGroup.Key.DayName, Date = dayGroup.Key.Date };
                    // Создание занятий
                    var lessons = dayGroup.Select(s => new Lesson
                    {
                        Hour = s.Hour,
                        Subject = s.Subject,
                        Type = s.Type,
                        Teacher = s.Teacher,
                        Room = s.Room,
                        Assignment = s.Assignment
                    }).ToList();
                    day.Lessons.AddRange(lessons);
                    week.Days.Add(day);
                }
                weeks.Add(week);
            }
            countWeek = weeks.Count() - 1;
            return weeks;
        }
        
        /*
        public List<Week> ConvertToWeeks(List<Schedule> schedules)
        {
            var weeks = new List<Week>();
            var noteDB = App.NotesDB;  // Используем правильное имя переменной

            // Группировка расписания по номеру недели
            var groupedByWeek = schedules.GroupBy(s => s.WeekNumber);
            foreach (var weekGroup in groupedByWeek)
            {
                var week = new Week { WeekNumber = weekGroup.Key };

                // Группировка занятий по дню недели и дате
                var groupedByDay = weekGroup.GroupBy(s => new { s.DayName, s.Date });
                foreach (var dayGroup in groupedByDay)
                {
                    var day = new Day { DayName = dayGroup.Key.DayName, Date = dayGroup.Key.Date };

                    // Создание занятий
                    var lessons = dayGroup.Select(s =>
                    {
                        var schedule = new Schedule
                        {
                            Subject = s.Subject,
                            Type = s.Type,
                            Assignment = s.Assignment
                        };

                        var updatedSchedule = noteDB.AssignmentGet(schedule);
                        return new Lesson
                        {
                            Hour = s.Hour,
                            Subject = s.Subject,
                            Type = s.Type,
                            Teacher = s.Teacher,
                            Room = s.Room,
                            Assignment = s.Assignment
                        };
                    }).ToList();

                    day.Lessons.AddRange(lessons);
                    week.Days.Add(day);
                }

                weeks.Add(week);
            }

            countWeek = weeks.Count() - 1;
            return weeks;
        }
        */


        protected override void OnAppearing()
        {
            List<Week> listNoFullDays = setRasp(); ;
            if (listNoFullDays.Count > 0) 
            listDays = ParsButton(listNoFullDays);
            collectionView.ItemsSource = listDays[ni].Lessons;
        }
        private HtmlDocument document;
        private HtmlNode calendarNode;
        int m = 0, k = 0;
        private List<Week> setRasp()
        {
            Week week1 = new Week();
            // Парсинг расписания
            HtmlNode weekNode;

            allLessons.Clear();
            if (document != null)
                weekNode = document.DocumentNode.SelectSingleNode("//div[@class='week']");
            else weekNode = null;
            if (weekNode != null)
            {
                // Извлечение информации из блока
                string calendarContent = calendarNode.InnerHtml;
                List<Week> schedule = ParseSchedule(calendarContent);
                // Определение номера интересующей недели
                // Измените это значение на нужный номер недели
                // Вывод расписания
                schedule.RemoveAll(item => item == null);
                countWeek = schedule.Count - 1;
                countDay = 0;
                int nWeek = 0;
                foreach (var week in schedule)
                {
                    nWeek++;
                    week.Days.RemoveAll(item => item.DayName == null);
                    countDay += week.Days.Count;
                    var newWeek = new Week();
                    newWeek.WeekNumber = week.WeekNumber;
                    foreach (var day in week.Days) newWeek.Days.Add(day);
                    week1 = newWeek;
                    weeks.Add(week1);
                    if (week.WeekNumber == targetWeekNumber || 1 == 1)
                    {
                        Console.WriteLine($"Неделя {week.WeekNumber}:");
                        foreach (var day in week.Days)
                        {
                            day.Lessons.RemoveAll(item => (item.Hour.Trim() == "-" || item == null));
                            if (day.Date.Replace(day.DayName, "").Trim() == DateTime.Now.ToString("dd.MM") && week.WeekNumber == 0) k = 0;
                            if (week.WeekNumber == 0) k++;
                            foreach (var lesson in day.Lessons)
                            {
                                Schedule schedule1 = new Schedule
                                {
                                    WeekNumber = week.WeekNumber,
                                    DayName = day.DayName,
                                    Date = day.Date,
                                    Hour = lesson.Hour,
                                    Subject = lesson.Subject,
                                    Type = lesson.Type,
                                    Room = lesson.Room,
                                    Teacher = lesson.Teacher,
                                    Assignment = lesson.Assignment,
                                };
                                //schedule1.Assignment=App.NotesDB.AssignmentGet(schedule1).Assignment;
                                App.ScheduleDB.SaveSchedule(schedule1);
                            }
                        }
                    }
                }
            }
            else 
            if (App.ScheduleDB.GetScheduls().Count > 0) {
                    var r = ShowScheduls();
                    targetWeekNumber = 0;
                    if (r != null)
                        ParsButton(r);
                    collectionView.ItemsSource = listDays[ni].Lessons;
                }  else
                {
                    DisplayAlert("Ошибка!", "Блок 'week' не найден на странице.", "OK");
                    Console.WriteLine("Блок 'week' не найден на странице.");
                }
           
            return weeks;
        }
        private List<Day> ParsButton(List<Week> allLessons)
        {
            string[] masDay = new string[] { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
            List<Day> allLessons2 = new List<Day>();
            int i = 0;
            Lesson noSch = new Lesson { Hour = "Нет расписания", Room = " ", Subject = " ", Teacher = " ", Type = " " };
            bool[] hasDay = new bool[6];
            for (int j = 0; j < 6; j++)
            {
                hasDay[j] = allLessons[targetWeekNumber].Days.Any(day => day.DayName.Trim() == masDay[j]);
                if (!hasDay[j]) { allLessons2.Add(new Day { WeekNumber = targetWeekNumber, Date = " ", DayName = masDay[j] }); allLessons2[i].Lessons.Add(noSch); i++; }
            }
            i = 0;

            foreach (var item in allLessons[targetWeekNumber].Days)
            {
                //Console.WriteLine(item.DayName);
                //Console.WriteLine(masDay[Array.IndexOf(masDay, item.DayName.Trim())]);
                var test = Array.IndexOf(masDay, item.DayName.Trim());
                allLessons2.Insert(test, item);
            }
            foreach (var item in allLessons2)
            {
                var buttonName = "Button" + (i + 1);
                var button = this.FindByName<Button>(buttonName);
                if (button != null)
                {
                    //  нужное значение Text
                    button.Text =
                         DWeek(allLessons2[i].DayName) + allLessons2[i].Date.Replace(allLessons2[i].DayName, "");
                }
                i++;
            }
            listDays = allLessons2;
            return allLessons2;
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработка выбора элемента (если необходимо)

        }
        private void ButtonL_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }
        //Парсеры:
        static string ParseExams(HtmlNode calendarNode)
        {
            var exams = new List<string>();
            var credits = new List<string>();
            var gradedCredits = new List<string>();

            var lessonWarningsNode = calendarNode.SelectSingleNode(".//div[@class='lesson-warnings']");
            if (lessonWarningsNode != null)
            {
                string currentCategory = null;
                foreach (var node in lessonWarningsNode.ChildNodes)
                {
                    if (node.Name == "div" && node.HasClass("lesson-warning"))
                    {
                        currentCategory = node.InnerText.Trim();
                    }
                    else if (node.Name == "div" && node.HasClass("lesson-warning-text"))
                    {
                        var examText = node.InnerText.Trim();
                        switch (currentCategory)
                        {
                            case "Зачеты:":
                                credits.Add(examText);
                                break;
                            case "Зачет:":
                                credits.Add(examText);
                                break;
                            case "Зачеты с оценкой:":
                                gradedCredits.Add(examText);
                                break;
                            case "Зачет с оценкой:":
                                gradedCredits.Add(examText);
                                break;
                            case "Экзамены:":
                                exams.Add(examText);
                                break;
                            case "Экзамен:":
                                exams.Add(examText);
                                break;

                        }
                    }
                }
            }
            // Объединяем все списки в одну строку
            var allExams = new List<string>();
            if (credits.Any())
            {
                allExams.Add("Зачеты:");
                allExams.AddRange(credits);
            }
            if (gradedCredits.Any())
            {
                allExams.Add("\nЗачеты с оценкой:");
                allExams.AddRange(gradedCredits);
            }
            if (exams.Any())
            {
                allExams.Add("\nЭкзамены:");
                allExams.AddRange(exams);
            }
            return string.Join("\n", allExams);
        }

        static List<Week> ParseSchedule(string htmlContent)
        {
            var schedule = new List<Week>();
            var document = new HtmlDocument();
            document.LoadHtml(htmlContent);
            int n = 0;
            var weekNodes = document.DocumentNode.SelectNodes("//div[@class='week']");
            if (weekNodes != null)
            {
                App.ScheduleDB.DeleteAllSchedules();
                foreach (var weekNode in weekNodes)
                {
                    var week = new Week
                    {
                        WeekNumber = n++
                    };
                    var dayNodes = weekNode.SelectNodes(".//div[contains(@class, 'day')]");
                    if (dayNodes != null)
                    {
                        foreach (var dayNode in dayNodes)
                        {
                            var day = new Day
                            {
                                DayName = dayNode.SelectSingleNode(".//div[@class='day-header']/div/span")?.InnerText.Trim(),
                                Date = dayNode.SelectSingleNode(".//div[@class='day-header']/div")?.InnerText.Trim(),
                                WeekNumber = n - 1
                            };

                            var lessonNodes = dayNode.SelectNodes(".//div[contains(@class, 'day-lesson')]");
                            if (lessonNodes != null)
                            {
                                foreach (var lessonNode in lessonNodes)
                                {
                                    var lesson = ParseLesson(lessonNode);
                                    day.Lessons.Add(lesson);
                                }
                            }
                            week.Days.Add(day);
                        }
                    }
                    schedule.Add(week);
                }
            }
            return schedule;
        }
        // Используем метод, который извлекает нужный текст или возвращает "-"
        static string ExtractText(HtmlNodeCollection nodes)
        {
            return nodes != null && nodes.Count > 0 ? nodes[0]?.InnerText?.Trim() ?? "-" : "-";
        }
        static Lesson ParseLesson(HtmlNode lessonNode)
        {
            var lessonRooms = lessonNode.SelectNodes(".//div[@class='lesson-room']");
            var lessonRoomMt1 = lessonNode.SelectNodes(".//div[@class='lesson-room mt-1']");
            string[] mas = new string[8];
            mas[0] = lessonNode.SelectSingleNode(".//div[@class='lesson-hour']")?.InnerText.Trim().Replace("&mdash;", "") ?? "-";
            mas[1] = lessonNode.SelectSingleNode(".//div[@class='lesson-name']")?.InnerText.Trim() ?? "-";
            mas[2] = lessonNode.SelectSingleNode(".//div[@class='lesson-type']")?.InnerText.Trim() ?? "-";
            mas[3] = (lessonNode.SelectNodes(".//div[@class='lesson-teacher']/a") != null && lessonNode.SelectNodes(".//div[@class='lesson-teacher']/a").Count > 0) ? lessonNode.SelectNodes(".//div[@class='lesson-teacher']/a")?[0]?.InnerText?.Trim() ?? "-" : "-";
            mas[4] = (lessonNode.SelectNodes(".//div[@class='lesson-teacher']/a") != null && lessonNode.SelectNodes(".//div[@class='lesson-teacher']/a").Count > 1) ? lessonNode.SelectNodes(".//div[@class='lesson-teacher']/a")?[1]?.InnerText?.Trim() ?? "-" : "-";

            // Выбираем, какие узлы использовать
            if (lessonRooms != null && lessonRooms.Count > 0)
            {
                mas[5] = ExtractText(lessonRooms);
            }
            else
            {
                mas[5] = ExtractText(lessonRoomMt1);
            }
            mas[6] = ExtractText(lessonRoomMt1);
           var db= App.NotesDB.GetNotes();
            if (db != null)
                mas[7] =" test" ;
            //App.NotesDB.AssignmentGet(new Schedule { Subject = mas[1], Type = mas[2] }).Assignment
            else mas[7] = " ";
          //  Console.WriteLine("assig^ " + mas[7]);

          //  if (mas[7]!=null)
         //   Console.WriteLine("assig^ "+ mas[7]);
            var lesson = new Lesson
            {
                Hour = mas[0],
                Subject = mas[1],
                Type = mas[2],
                Teacher = (mas[4].Trim() == "-") ? mas[3] : (mas[3] + " | " + mas[4]),
                Room = (mas[6].Trim() == "-") ? mas[5] : (mas[5] + " | " + mas[6]),
                //Assignment = (mas[7].Trim())
            };
            // Если все поля пустые, заменить весь объект одним символом "-"
            if ((lesson.Hour == "-") &&
                (lesson.Subject == "-") &&
                (lesson.Type == "-") &&
                (lesson.Teacher == "-") &&
                (lesson.Room == "-")
                )
            {
                return new Lesson
                {
                    Hour = "-",
                    Subject = "",
                    Type = "",
                    Teacher = "",
                    Room = "",
                };
            }
            return lesson;
        }
        private string DWeek(string dayName)
        {
            switch (dayName.Trim())
            {
                case "Понедельник": return "Пн ";
                case "Вторник": return "Вт ";
                case "Среда": return "Ср ";
                case "Четверг": return "Чт ";
                case "Пятница": return "Пт ";
                case "Суббота": return "Сб ";
                default: return dayName.Trim();
            }
        }
        
        private void Button_Clicked_sch(object sender, EventArgs e)
        {
            Xamarin.Forms.Button clickedButton = (Xamarin.Forms.Button)sender;
            List<Week> listNoFullDays;
            if (clickedButton != null)
            {
                switch (clickedButton.ClassId)
                {
                    case "Button1":
                        ni = 0;
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "Button2":
                        ni = 1;
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "Button3":
                        ni = 2;
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "Button4":
                        ni = 3;
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "Button5":
                        ni = 4;
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "Button6":
                        ni = 5;
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "ButtonL":
                        if (targetWeekNumber > 0) targetWeekNumber--;
                        listNoFullDays = setRasp();
                        listDays = ParsButton(listNoFullDays);
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                    case "ButtonR":
                        if (targetWeekNumber < countWeek) targetWeekNumber++;
                        listNoFullDays = setRasp();
                        listDays = ParsButton(listNoFullDays);
                        collectionView.ItemsSource = listDays[ni].Lessons;
                        ColorButton(listDays);
                        break;
                }
            }
            else
            {
                DisplayAlert("Ошибка!", "clickedButton нет", "OK");
            }
        }
        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            List<Week> listNoFullDays;
            switch (e.Direction.ToString())
            {
                case "Left":
                    if (targetWeekNumber < countWeek) targetWeekNumber++;
                    listNoFullDays = setRasp();
                    listDays = ParsButton(listNoFullDays);
                    collectionView.ItemsSource = listDays[ni].Lessons;
                    ColorButton(listDays);
                    break;
                case "Right":
                    if (targetWeekNumber > 0) targetWeekNumber--;
                    listNoFullDays = setRasp();
                    listDays = ParsButton(listNoFullDays);
                    collectionView.ItemsSource = listDays[ni].Lessons;
                    ColorButton(listDays);
                    break;
            }
        }
        public void ColorButton(List<Day> allLessons2)
        {
            bool clik;
            for (var i = 1; i <= 6; i++)
            {
                if (i == ni + 1) clik = true; else clik = false;
                string buttonName = "Button" + (i);
                Button button = this.FindByName<Button>(buttonName);
                if (button != null)
                {
                    if (clik) button.BackgroundColor = Color.FromRgba(255, 255, 255, 74);
                    else button.BackgroundColor = Color.FromRgba(255, 255, 255, 144);
                }
            }
        }
        private void OnSwipedDay(object sender, SwipedEventArgs e)
        {
            List<Week> listNoFullDays = setRasp(); ;
            listDays = ParsButton(listNoFullDays);
            switch (e.Direction.ToString())
            {
                case "Left":
                    if (ni < listDays.Count - 1)
                    {
                        ni++;
                        listDays = ParsButton(listNoFullDays);
                        collectionView.ItemsSource = listDays[ni].Lessons;
                    }
                    else
                    {
                        ni = 0;
                        if (targetWeekNumber < countWeek) targetWeekNumber++;
                        listDays = ParsButton(listNoFullDays);
                        collectionView.ItemsSource = listDays[ni].Lessons;
                    }
                    ColorButton(listDays);
                    break;
                case "Right":
                    if (ni > 0)
                    {
                        ni--;
                        listDays = ParsButton(listNoFullDays);
                        collectionView.ItemsSource = listDays[ni].Lessons;
                    }
                    else
                    {
                        ni = 5;
                        if (targetWeekNumber > 0) targetWeekNumber--;
                        listDays = ParsButton(listNoFullDays);
                        collectionView.ItemsSource = listDays[ni].Lessons;
                    }
                    ColorButton(listDays);
                    break;
            }
        }
        private void AssignmentEntry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            var assignmentText = entry.Text;
            string str = "";
            string subject = "", type = "", assignment = "";
            Note note = new Note();
            note.Date = DateTime.Now;
            note.Category = "ДЗ";
            // Получаем родительский элемент Entry
            var parentStackLayout = entry.Parent as StackLayout;

            if (parentStackLayout != null)
            {
                // Проходим по каждому элементу StackLayout
                foreach (var child in parentStackLayout.Children)
                {
                    if (child is Label label)
                    {

                        if (label.ClassId == "SubjectLabel") {

                            Console.WriteLine("note: " + label.ClassId + " " + label.Text );

                            subject = label.Text; }

                        if (label.ClassId == "TypeLabel") {
                            Console.WriteLine("note: " + label.ClassId + " " + label.Text);

                            type = label.Text; }

                    }
                    else
                    if (child is Entry entryElement)
                        if (entryElement.ClassId == "AssignmentEntry") assignment = entryElement.Text;
                }
                note.Text = subject + " " + type;
                note.Description = assignment;
                if (!string.IsNullOrWhiteSpace(note.Text))
                {
                    App.NotesDB.Delete(note);
                    if (!string.IsNullOrWhiteSpace(assignment)) App.NotesDB.SaveNote(note);
                    App.ScheduleDB.GetSchedulesBySubject(subject, type, assignment);
                    Console.WriteLine("note: " + subject + " " + type + " " + assignment);
                    List<Week> listNoFullDays = ShowScheduls(); ;
                    listDays = ParsButton(listNoFullDays);
                    collectionView.ItemsSource = listDays[ni].Lessons;

                }
                else DisplayAlert("Ошибка", "not note", "OK");

            }
            else
            {
                DisplayAlert("Ошибка", "Родительский StackLayout не найден", "OK");
            }
        }
    }



    public class Week
    {
        public int WeekNumber { get; set; }
        public List<Day> Days { get; } = new List<Day>();
    }

    public class Day
    {
        public string DayName { get; set; }
        public string Date { get; set; }
        public List<Lesson> Lessons { get; } = new List<Lesson>();
        public int WeekNumber { get; set; }

    }

    public class Lesson
    {
        public string Hour { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }
        public string Assignment { get; set; }


    }
}

