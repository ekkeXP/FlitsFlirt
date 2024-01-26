using Flitsflirtdev.ViewModels;
using Flitsflirtdev.Views;
using System.Diagnostics;

namespace Flitsflirtdev
{
    public partial class AppShell : Shell
    {
        public static AppShell? CurrentAppShell { get; private set; } = default!;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AlgoPage), typeof(AlgoPage));
            Routing.RegisterRoute(nameof(HobbyPage), typeof(HobbyPage));
        }

        /// <summary>
        /// Logout
        /// </summary>
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("AppShell: Logout");

            await Current.GoToAsync("//LoginPage");
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Current != null)
            {
                Debug.WriteLine($"AppShell: source={args.Current.Location}, target={args.Target.Location}");
            }
        }
    }
}