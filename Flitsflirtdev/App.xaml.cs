using Flitsflirtdev.Services;
using Flitsflirtdev.Views;

namespace Flitsflirtdev
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AlgoPage), typeof(AlgoPage));
            Routing.RegisterRoute(nameof(HobbyPage), typeof(HobbyPage));
            MainPage = new AppShell();
        }
        private async void OnMenuItemClicked(System.Object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}