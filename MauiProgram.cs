using Microsoft.Extensions.Logging;
using FinanceTrackerApp.Services;

namespace FinanceTrackerApp
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Регистрируем сервис базы данных
            builder.Services.AddSingleton<DatabaseService>();

            return builder.Build();
        }
    }
}
