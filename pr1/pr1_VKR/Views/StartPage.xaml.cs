using pr1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Maui.Controls.Xaml;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace pr1.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
	{
        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }


        public StartPage ()
		{
            Shell.SetFlyoutItemIsVisible(this, false);

            InitializeComponent();
           // Shell.SetTabBarIsVisible(this, false);
        }




        private async void LoadNote(string value)
        {
            //    InitializeComponent();

            try
            {
                int id = Convert.ToInt32(value);
                Note note = App.NotesDB.GetNote(id);
                BindingContext = note;
            }
            catch { }
        }




        private async void Button_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ListGroups2());

        }
    }
}