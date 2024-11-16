using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SimpleNotes.PageModel;
using SimpleNotes.Pages;
using Syncfusion.Maui.Toolkit.Hosting;


namespace SimpleNotes
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                // .ConfigureMauiHandlers(handlers =>
                // {
                // })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "notes.db3");
            builder.Services.AddSingleton<Repository>(s => ActivatorUtilities.CreateInstance<Repository>(s, dbPath));

            builder.Services.AddTransient<MainPageModel>();
            builder.Services.AddTransientWithShellRoute<EditNotePage, EditNotePageModel>(nameof(EditNotePage));

            return builder.Build();
        }
    }
}
