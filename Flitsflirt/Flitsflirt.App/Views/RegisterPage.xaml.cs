using Flitsflirt.App.Services;
using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel RVM)
        {
            InitializeComponent();
            BindingContext = RVM;
        }
    }
}