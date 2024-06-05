using pr1.Models;
using System;
using System.Linq;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace pr1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
#pragma warning disable CS0169 // Поле "NotesPage.width" никогда не используется.
        private double width;
#pragma warning restore CS0169 // Поле "NotesPage.width" никогда не используется.
#pragma warning disable CS0169 // Поле "NotesPage.height" никогда не используется.
        private double height;
#pragma warning restore CS0169 // Поле "NotesPage.height" никогда не используется.

        public NotesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            ShowNotes();
            Shell.SetTabBarIsVisible(this, true);
        }

        private void ShowNotes()
        {
            collectionView.ItemsSource = App.NotesDB.GetNotes();
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NoteAddingPage));
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Note note = (Note)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(NoteAddingPage)}?{nameof(NoteAddingPage.ItemId)}={note.Id.ToString()}");
            }
        }
       

        private async void ButtonSetting_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private async void ButtonNotes_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//NotesPage");
        }

        private async void ButtonSchedule_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }

        private void ButtonL_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;

        }
    }
}
