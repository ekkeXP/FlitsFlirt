using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditInfoPage : ContentPage
    {
        public EditInfoPage(EditViewModel EV)
        {
            InitializeComponent();
            this.BindingContext = EV;
        }
    }
}