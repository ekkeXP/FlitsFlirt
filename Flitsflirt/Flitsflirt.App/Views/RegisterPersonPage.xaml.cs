using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views;

public partial class RegisterPersonPage : ContentPage
{
    public RegisterPersonPage(RegisterViewModel RVM)
    {
        InitializeComponent();
        BindingContext = RVM;
    }
}