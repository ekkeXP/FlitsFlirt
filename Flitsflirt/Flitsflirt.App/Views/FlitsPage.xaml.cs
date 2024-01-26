using Flitsflirt.App.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Flitsflirt.App.Views;

public partial class FlitsPage : ContentPage
{
	uint timerMS = 8000;
	uint Animationspeed = 400;
    private FlitsViewModel _viewmodel;

	public FlitsPage(FlitsViewModel FM)
	{
		InitializeComponent();
		BindingContext = _viewmodel = FM;
        
    }

    public async void SwipedLeft(object sender, EventArgs e)
    {
        await TimerBar.ProgressTo(1, 1, Easing.Linear);
        TimerBar.Progress = 1;

        var a1 = myFrame.RotateTo(-360, Animationspeed, Easing.Linear);
        var a2 = myFrame.TranslateTo(-900, myFrame.Y, Animationspeed, Easing.Linear);

        await Task.WhenAll(a1, a2);
        _viewmodel.SwipedLeft();

        myFrame.Rotation = 0;
        myFrame.TranslationX = 0;
        await TimerBar.ProgressTo(0, timerMS, Easing.Linear);
        if (TimerBar.Progress <= 0)
        {
            SwipedLeft(sender, e);

        }
    }

    public async void SwipedRight(object sender, EventArgs e)
    {

        await TimerBar.ProgressTo(1, 1, Easing.Linear);
        TimerBar.Progress = 1;

        await myFrame.TranslateTo(900, myFrame.Y, Animationspeed, Easing.Linear);

        _viewmodel.SwipedRight();
        myFrame.TranslationX = 0;
        await TimerBar.ProgressTo(0, timerMS, Easing.Linear);
        if (TimerBar.Progress <= 0)
        {
            SwipedLeft(sender, e);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        TimerBar.ProgressTo(1, 1, Easing.Linear);
        TimerBar.Progress = 1;
        _viewmodel.ResetGebruikers();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.UpdateProperties();
    }
}