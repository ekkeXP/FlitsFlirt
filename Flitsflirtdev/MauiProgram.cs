using Flitsflirtdev.Models;
using Flitsflirtdev.Services;
using Flitsflirtdev.ViewModels;
using Flitsflirtdev.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace Flitsflirtdev
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
            builder.Services.AddScoped<LoginPage>();
            builder.Services.AddScoped<AlgoPage>();
            builder.Services.AddScoped<HobbyPage>();

            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddScoped<AlgoViewModel>();
            builder.Services.AddScoped<HobbyViewModel>();

            return builder.Build();
        }
    }
}