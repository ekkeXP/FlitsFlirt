using Flitsflirt.App.Models;
using Flitsflirt.App.Services;
using Flitsflirt.App.ViewModels;
using Flitsflirt.App.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace Flitsflirt.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                    fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                    fonts.AddFont("Flamante-Roma-Medium.ttf", "FlamanteRomaMedium");
                    fonts.AddFont("Flamante-Roma-MediumItalic.ttf", "FlamanteRomaMediumItalic");
                    fonts.AddFont("Hero.otf", "Hero");
                    fonts.AddFont("Hero-Light.otf", "HeroLight");
                }).UseMauiCommunityToolkit();

#if DEBUG
            builder.Logging.AddDebug();
		builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif
            builder.Services.AddSingleton<IDBConnect, DBConnect>();
            builder.Services.AddSingleton<IUser, User>();
            builder.Services.AddSingleton<IHTTP, HTTP>();
            builder.Services.AddSingleton<IAlgoritme, Algoritme>();

            builder.Services.AddScoped<AccountPage>();
            builder.Services.AddScoped<FlitsPage>();
            builder.Services.AddScoped<LoginPage>();
            builder.Services.AddScoped<RegisterHobbiesPage>();
            builder.Services.AddScoped<RegisterPage>();
            builder.Services.AddScoped<RegisterPersonPage>();
            builder.Services.AddScoped<DeletePage>();
            builder.Services.AddScoped<EditPage>();
            builder.Services.AddScoped<EditHobbiesPage>();
            builder.Services.AddScoped<EditInfoPage>();
            builder.Services.AddScoped<TutorialPage>();
            builder.Services.AddScoped<MatchedPage>();

            builder.Services.AddScoped<AccountViewModel>();
            builder.Services.AddScoped<FlitsViewModel>();
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<RegisterViewModel>();
            builder.Services.AddScoped<DeleteViewModel>();
            builder.Services.AddScoped<EditViewModel>();
            builder.Services.AddScoped<TutorialViewModel>();
            builder.Services.AddScoped<MatchedViewModel>();


            return builder.Build();
        }
    }
}