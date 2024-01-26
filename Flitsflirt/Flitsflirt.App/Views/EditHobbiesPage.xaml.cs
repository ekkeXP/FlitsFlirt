using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditHobbiesPage : ContentPage
    {
        EditViewModel _viewmodel;
        public EditHobbiesPage(EditViewModel EV)
        {
            InitializeComponent();
            this.BindingContext = _viewmodel = EV;
        }
        protected override void OnAppearing()
        {
            _viewmodel.Load();
        }
    }
}