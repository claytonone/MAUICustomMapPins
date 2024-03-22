using Microsoft.Extensions.Logging;

namespace MauiCustomMapIcons
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
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if ANDROID
			        handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, MauiCustomMapIcons.Platforms.Android.CustomMapHandler>();
#elif IOS
                    handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, MauiCustomMapIcons.Platforms.iOS.CustomMapHandler>();
#endif
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
