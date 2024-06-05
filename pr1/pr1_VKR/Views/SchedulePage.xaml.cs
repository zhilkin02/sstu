using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace pr1.Views
{
    public partial class SchedulePage : ContentPage
    {
        public ObservableCollection<ScheduleItem> ScheduleItems { get; set; }

        public SchedulePage()
        {
            InitializeComponent();

            // Инициализация коллекции с расписанием (загрузка данных из внешнего источника)
            ScheduleItems = new ObservableCollection<ScheduleItem>
            {
                new ScheduleItem { Time = "9:45 - 11:15", Subject = "Физика", LessonType = "Практика", Classroom = "2/201", Teacher = "Отставнова Лилия Алексеевна", Assignment = "  " },
                new ScheduleItem { Time = "11:30 - 13:00", Subject = "Математика", LessonType = "Лекция", Classroom = "3/305", Teacher = "Отставнова Лилия Алексеевна", Assignment = "  " },
                new ScheduleItem { Time = "13:40 - 15:10", Subject = "Физика", LessonType = "Практика", Classroom = "2/201", Teacher = "Отставнова Лилия Алексеевна", Assignment = "  " },

                // Добавьте свои элементы расписания сюда
            };

            collectionView.ItemsSource = ScheduleItems;
        }
       // String grupN, grupURL;
        public SchedulePage(Views.Group grup)
        {
            InitializeComponent();
            // this.grup = grup.GroupName;
            // Инициализация коллекции с расписанием (загрузка данных из внешнего источника)
             DisplayAlert("Всплывающее меню", grup.GroupName, "OK");
            ScheduleItems = new ObservableCollection<ScheduleItem>
            {
                 new ScheduleItem { Time = "9:45 - 11:15", Subject = "Физика", LessonType = "Практика", Classroom = "2/201", Teacher = "Отставнова Лилия Алексеевна", Assignment = "  " },
                new ScheduleItem { Time = "11:30 - 13:00", Subject = "Математика", LessonType = "Лекция", Classroom = "3/305", Teacher = "Отставнова Лилия Алексеевна", Assignment = "  " },
                new ScheduleItem { Time = "13:40 - 15:10", Subject = "Физика", LessonType = "Практика", Classroom = "2/201", Teacher = "Отставнова Лилия Алексеевна", Assignment = "  " },

                // Добавьте свои элементы расписания сюда
            };

            collectionView.ItemsSource = ScheduleItems;
        }



        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработка выбора элемента (если необходимо)
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            // Обработка нажатия на кнопку "Добавить" (если необходимо)
        }

        private void ButtonL_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }
    }

    // Класс для представления элемента расписания
    public class ScheduleItem
    {
        public string Time { get; set; }
        public string Subject { get; set; }
        public string LessonType { get; set; }
        public string Classroom { get; set; }
        public string Teacher { get; set; }
        public string Assignment { get; set; }
    }
}

