using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using pr1.Data;
using System.IO;
using pr1.Views;
using pr1.Helpers;
using Xamarin.Essentials;
using pr1.Models;
using System.Text.RegularExpressions;

namespace pr1
{
    public partial class App : Application
    {
        private static NotesDB notesDB;
        private static ScheduleDB scheduleDB;
        private static ScheduleDB scheduleTeacherDB;

        public static NotesDB NotesDB
        {
            get
            {
                if (notesDB == null)
                {
                    notesDB = new NotesDB(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "NoteDatabase.db3"));
                }
                return notesDB;
            }
        }

        private static UserDB userDB;
        public static UserDB UserDB
        {
            get
            {
                if (userDB == null)
                {
                    userDB = new UserDB(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "UserDB.db3"));
                }
                return userDB;
            }
        }
        public static ScheduleDB ScheduleDB
        {
            get
            {
                if (scheduleDB == null)
                {
                    scheduleDB = new ScheduleDB(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "ScheduleDatabase.db3"));
                }
                return scheduleDB;
            }
        }
        public static ScheduleDB ScheduleTeacherDB
        {
            get
            {
                if (scheduleTeacherDB == null)
                {
                    scheduleTeacherDB = new ScheduleDB(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "ScheduleTeacherDatabase.db3"));
                }
                return scheduleTeacherDB;
            }
        }
        private void InitializeDatabase()
        {
            // Создание экземпляра класса базы данных
            // scheduleDB = new ScheduleDB();
        }

        public App()
        {
            InitializeComponent();
            InitializeDatabase();

            TheTheme.SetTheme();
            // MainPage = new  StartPage();
            //
            //
            //   MainPage = new NavigationPage(new AppShell());
            MainPage =  new AppShell();
           // MainPage = new NavigationPage(new StartPage());

            //  Views.Teacher grup = new Views.Teacher() { TeacherName = " ", ScheduleLink= "https://pr1.my1.ru/36284.html" };
            //MainPage = new ListTeacher();
            //  MainPage = new SchedulePage2(grup);

        }
    
        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
    }
}
