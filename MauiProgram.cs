using crud_maui.DataAcess;
using crud_maui.ViewModels;
using crud_maui.Views;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace crud_maui
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            Batteries.Init();
            var dbContext = new ColaboradorDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

            builder.Services.AddDbContext<ColaboradorDbContext>();
            builder.Services.AddTransient<ColaboradorPage>();
            builder.Services.AddTransient<ColaboradorViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            Routing.RegisterRoute(nameof(ColaboradorPage), typeof(ColaboradorPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
