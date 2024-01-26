using Flitsflirt.App.Models;
using Flitsflirt.App.Services;
using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views;

public partial class MatchedPage : ContentPage
{
    private MatchedViewModel model;

    private StackLayout? stackLayout;
    private Grid? grid;
    private ScrollView? scroll;

    public MatchedPage(MatchedViewModel MV)
    {
        InitializeComponent();
        this.BindingContext = model = MV;
    }

    List<Gebruiker> list;

    public void loadList()
    {
        list = model.LoadingList;
    }

    public void loadFrames()
    {
        /*var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += FrameTapped;*/

        stackLayout = new StackLayout();
        scroll = new ScrollView();
        grid = new Grid();

        var image = new Image
        {
            Source = "canvas.png",
            Aspect = Aspect.AspectFill,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        if (list.Count >= 1)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string firstnameAndAge = list[i].FirstName.Trim() + ", " + list[i].Age.ToString();
                string city = list[i].City;

                var frame = new Frame
                {
                    CornerRadius = 20,
                    Margin = new Thickness(10),
                    HasShadow = true,
                    BorderColor = Color.FromRgba(0, 0, 0, 0),
                    MaximumHeightRequest = 200,
                    Padding = 0
                };

                frame.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command<int>(FrameTapped),
                    CommandParameter = i
                });

                var gradientBrush = new LinearGradientBrush();
                gradientBrush.GradientStops.Add(new GradientStop { Color = Color.FromHex("#A997D9"), Offset = 1 });
                gradientBrush.GradientStops.Add(new GradientStop { Color = Color.FromHex("#9178D4"), Offset = 0 });

                frame.Background = gradientBrush;

                var innerStackLayout = new StackLayout();
                innerStackLayout.Orientation = StackOrientation.Horizontal;
                innerStackLayout.Padding = 0;

                var firstnameLabel = new Label
                {
                    Text = firstnameAndAge,
                    TextColor = Color.FromRgb(255, 255, 255),
                    FontSize = 20,
                    FontFamily = "Hero",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Padding = new Thickness(10),
                    FontAttributes = FontAttributes.Bold
                };

                var cityLabel = new Label
                {
                    Text = city,
                    TextColor = Color.FromRgb(255, 255, 255),
                    FontSize = 20,
                    FontFamily = "Hero",
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Padding = new Thickness(10),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    FontAttributes = FontAttributes.Bold | FontAttributes.Italic
                };

                var profilePicture = new Image
                {
                    Source = model._client.GetImage(list[i].AccountID),
                    HeightRequest = frame.Height,
                    WidthRequest = frame.Width,
                    Aspect = Aspect.Fill
                };
                //var MV = (MatchedViewModel)BindingContext;
                //frame.GestureRecognizers.Add(new TapGestureRecognizer() { Command = MV.test() });
                
                innerStackLayout.Add(profilePicture);
                innerStackLayout.Add(firstnameLabel);
                innerStackLayout.Add(cityLabel);

                frame.Content = innerStackLayout;
                stackLayout.Add(frame);
            
            }
        }
        else
        {
            var geenmatches = new Label
            {
                Text = "Het lijkt het erop dat je nog geen matches hebt",
                FontSize = 20,
                FontFamily = "Hero",
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.FromRgb(255, 255, 255),
            };
            stackLayout.Add(geenmatches);
        }

        scroll.Content = stackLayout;
        grid.Add(image);
        grid.Add(scroll);
        Content = grid;
    }

    private void FrameTapped(int i)
    {
        char gender = list[i].Gender;
        string firstnameAndAge = list[i].FirstName.Trim() + ", " + list[i].Age.ToString();
        string firstname = list[i].FirstName.Trim();
        string email = list[i].Email;

        if (gender.Equals('M'))
        {
            DisplayAlert($"{firstnameAndAge}", $"{firstname} zijn emailadres is: {email}", "OK");
        }
        else
        {
            DisplayAlert($"{firstnameAndAge}", $"{firstname} haar emailadres is: {email}", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Content = null;
        model.getMatch();
        loadList();
        loadFrames();

    }

}