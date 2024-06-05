using pr1.Models;
using pr1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pr1.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    //[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoteAddingPage : ContentPage
	{
        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }


        public NoteAddingPage ()
		{
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            BindingContext = new Note();
		}
        //public  NoteAddingPage(string subject, string type, string assignment)
        //{
        //    NavigationPage.SetHasNavigationBar(this, false);

        //    InitializeComponent();
        //   Note note = new Note();
        //    note.Date = DateTime.Now;

        //    note.Category = "ДЗ";
        //    note.Text = subject+" "+ type;
        //    note.Description = assignment;
        //    if (!string.IsNullOrWhiteSpace(note.Text))
        //    {
        //        App.NotesDB.SaveNote(note);
        //    }
        //     Shell.Current.GoToAsync("..");
        //}

#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        private async void LoadNote(string value)
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        {
            //    InitializeComponent();

            try
            {
                int id = Convert.ToInt32(value);
                Note note =  App.NotesDB.GetNote(id);
                BindingContext = note;
            }
            catch { }
        }

        private async void OnSaveButton_Clicked(object sender, EventArgs e)
        {
            Note note = (Note)BindingContext;
            if (note.Category == "ДЗ")
            {
                var str = note.Text.Split('(');
                str[1] = "(" + str[1] + ")";
                Console.WriteLine(str[0] + "|" + str[1]);

                App.ScheduleDB.GetSchedulesBySubject(str[1], str[2], note.Description);
            }
            note.Date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(note.Text))
            {
                 App.NotesDB.SaveNote(note);
            }
            await Shell.Current.GoToAsync("..");

        }

        private async void OnDeleteButton_Clicked(object sender, EventArgs e)
        {
            Note note = (Note)BindingContext;
           App.NotesDB.DeleteNote(note);
            await Shell.Current.GoToAsync("..");
        }

        //---------------------------------   

        


private void OnEntryChanged(object sender, TextChangedEventArgs e)
{
           List<Note> list = App.NotesDB.GetNotes();
            List<String> list2 = new List<String>();
            foreach (Note lis in list) { list2.Add(lis.Category); }
            list2.Add("привет");
            List<String> _suggestion = list2; 
            if (entryMain.Text != null && lstSuggest.ItemsSource != null)
    {                  
        if (_suggestion.Any(x=> x.StartsWith(e.NewTextValue)) && entryMain.Text != string.Empty)
        {
            var items = new List<string>();



            foreach (var item in _suggestion.FindAll(x => x.StartsWith(e.NewTextValue)))
            {
                items.Add(item);
            }



            lstSuggest.ItemsSource = items;
            lstSuggest.IsVisible = true;
        }
        else
        { 
            lstSuggest.IsVisible = false;
        }
    }
}



private void ItemSelected(object sender, EventArgs args)
{
    if (((ListView)sender).SelectedItem == null)
        return;



    entryMain.Text = lstSuggest.SelectedItem.ToString();
    ((ListView)sender).SelectedItem = null;
    lstSuggest.IsVisible = false;
}

        //----------------------------------



    }
}