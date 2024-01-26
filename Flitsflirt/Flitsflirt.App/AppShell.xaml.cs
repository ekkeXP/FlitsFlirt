using Flitsflirt.App.ViewModels;
using Flitsflirt.App.Views;
using System.Diagnostics;

namespace Flitsflirt.App
{
    public partial class AppShell : Shell
    {
        public static AppShell? CurrentAppShell { get; private set; } = default!;

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FlitsPage), typeof(FlitsPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
            Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
            Routing.RegisterRoute(nameof(TutorialPage), typeof(TutorialPage));
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