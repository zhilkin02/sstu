using pr1.Views;
using System;
using Xamarin.Forms;

namespace pr1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ChangeContentTemplate();
             RegisterRoutes();

            // Добавление кнопки вызова меню в правом верхнем углу
            var hamburgerIcon = new Image { Source = "icon_about.png", HeightRequest = 40, WidthRequest = 40 };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "OpenMenuCommand"); // Измените на вашу команду
            hamburgerIcon.GestureRecognizers.Add(tapGestureRecognizer);

           
        }

        public  void ChangeContentTemplate()
        {
            // Получите ссылку на ShellContent, который вы хотите изменить.
            var shellContent = this.FindByName<ShellContent>("ShellContentSch");

            if (shellContent != null)
            {
                var db = App.UserDB;
                DataTemplate newDataTemplate;
                // Создайте новый DataTemplate для SchedulePage2.
                if (db.GetUser()  != null)
                {
                    if (db.GetUser().Sch == 0)
                        newDataTemplate = new DataTemplate(typeof(SchedulePage));
                    else
                        newDataTemplate = new DataTemplate(typeof(SchedulePage2));

                    // Установите новый DataTemplate для ContentTemplate.
                    shellContent.ContentTemplate = newDataTemplate;
                }
            }
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(NoteAddingPage), typeof(NoteAddingPage));
            Routing.RegisterRoute(nameof(ListGroups), typeof(ListGroups));
            Routing.RegisterRoute(nameof(ListTeacher), typeof(ListTeacher));
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
            Routing.RegisterRoute(nameof(SchedulePage2), typeof(SchedulePage2));
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));



            // Другие маршруты
        }

        private async void OnSchedulePageClicked(object sender, EventArgs e)
        {
            // Здесь добавьте код для перехода на страницу SchedulePage и передачи ссылки
            await Shell.Current.GoToAsync($"//Main/SchedulePage?link=your_link_here");
        }


    }
}
