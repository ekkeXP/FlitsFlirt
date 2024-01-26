using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        public EditPage(EditViewModel EV)
        {
            InitializeComponent();
            this.BindingContext = EV;
        }
    }
}