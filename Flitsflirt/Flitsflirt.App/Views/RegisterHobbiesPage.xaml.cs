using Flitsflirt.App.ViewModels;
namespace Flitsflirt.App.Views;

public partial class RegisterHobbiesPage : ContentPage
{
	public RegisterHobbiesPage(RegisterViewModel RVM)
	{
		InitializeComponent();
        BindingContext = RVM;
    }
}