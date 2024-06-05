using pr1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Maui.Controls.Xaml;
#pragma warning disable CS0105 // Директива Using уже использовалась в этом пространстве имен
using System.Collections.Generic;
#pragma warning restore CS0105 // Директива Using уже использовалась в этом пространстве имен
#pragma warning disable CS0105 // Директива Using уже использовалась в этом пространстве имен
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
#pragma warning restore CS0105 // Директива Using уже использовалась в этом пространстве имен

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




#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        private async void LoadNote(string value)
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
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