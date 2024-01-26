using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel LV)
        {
            InitializeComponent();
            this.BindingContext = LV;
        }
    }
}