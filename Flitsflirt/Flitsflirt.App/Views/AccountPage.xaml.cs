using Flitsflirt.App.ViewModels;
using System.Diagnostics;

namespace Flitsflirt.App.Views;

public partial class AccountPage : ContentPage
{
	AccountViewModel _viewmodel;
	public AccountPage(AccountViewModel AV)
	{
		InitializeComponent();
		BindingContext = _viewmodel = AV;
	}

	protected override void OnAppearing()
	{
		_viewmodel.init();
	}
}