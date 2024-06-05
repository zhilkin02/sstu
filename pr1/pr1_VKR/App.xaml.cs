using System;
using Microsoft.Maui.Controls.Xaml;
using pr1.Data;
using System.IO;
using pr1.Views;
using pr1.Helpers;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;

namespace pr1
{
    public partial class App : Application
    {
        private static NotesDB notesDB;

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
        public App()
        {
            InitializeComponent();

            TheTheme.SetTheme();
            // MainPage = new  StartPage();
          //
          //
         //   MainPage = new NavigationPage(new AppShell());

           MainPage = new AppShell();
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
