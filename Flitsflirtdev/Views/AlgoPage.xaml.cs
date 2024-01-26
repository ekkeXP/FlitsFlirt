using Flitsflirtdev.Services;
using Flitsflirtdev.ViewModels;

namespace Flitsflirtdev.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlgoPage : ContentPage
    {

        public AlgoPage(AlgoViewModel AV)
        {
            InitializeComponent();
            this.BindingContext = AV;
        }
    }
}
