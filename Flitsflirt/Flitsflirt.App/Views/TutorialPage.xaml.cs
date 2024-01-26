using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views;

public partial class TutorialPage : ContentPage
{
    public TutorialPage(TutorialViewModel TV)
    {
        InitializeComponent();
        BindingContext = TV;
    }
}