using Flitsflirt.App.Services;
using Flitsflirt.App.Views;

namespace Flitsflirt.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(FlitsPage), typeof(FlitsPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
            Routing.RegisterRoute(nameof(TutorialPage), typeof(TutorialPage));

            MainPage = new AppShell();
        }
        private async void OnMenuItemClicked(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}