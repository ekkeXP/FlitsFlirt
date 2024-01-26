using Flitsflirtdev.Services;
using Flitsflirtdev.ViewModels;

namespace Flitsflirtdev.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HobbyPage : ContentPage
    {

        public HobbyPage(HobbyViewModel HV)
        {
            InitializeComponent();
            this.BindingContext = HV;
        }
    }
}
    