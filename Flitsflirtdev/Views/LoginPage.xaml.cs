using Flitsflirtdev.Services;
using Flitsflirtdev.ViewModels;

namespace Flitsflirtdev.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel LV)
        {
            InitializeComponent();
            this.BindingContext = LV;
        }
    }
}