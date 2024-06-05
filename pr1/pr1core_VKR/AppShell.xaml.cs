using pr1.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace pr1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();

            // Добавление кнопки вызова меню в правом верхнем углу
            var hamburgerIcon = new Image { Source = "icon_about.png", HeightRequest = 40, WidthRequest = 40 };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "OpenMenuCommand"); // Измените на вашу команду
            hamburgerIcon.GestureRecognizers.Add(tapGestureRecognizer);

           
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(NoteAddingPage), typeof(NoteAddingPage));
            Routing.RegisterRoute(nameof(ListGroups), typeof(ListGroups));
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            // Другие маршруты
        }
    }
}
