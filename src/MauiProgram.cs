using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;

namespace MauiWinTitle
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
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(windowsLifecycleBuilder =>
                    {
                        windowsLifecycleBuilder.OnWindowCreated(window =>
                        {
                            window.ExtendsContentIntoTitleBar = true;
                            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                            App.appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                            var titleBar = App.appWindow.TitleBar;
                            titleBar.InactiveBackgroundColor = Colors.LightGray.ToWindowsColor();
                            titleBar.InactiveForegroundColor = Colors.LightGray.ToWindowsColor();
                            titleBar.ButtonForegroundColor = Colors.LightGray.ToWindowsColor();
                            titleBar.ButtonInactiveForegroundColor = Colors.LightGray.ToWindowsColor();
                            titleBar.ButtonHoverBackgroundColor = Colors.Transparent.ToWindowsColor();
                            titleBar.ButtonHoverForegroundColor = Colors.LightGray.ToWindowsColor();
                            switch (App.appWindow.Presenter)
                            {
                                case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                    overlappedPresenter.Maximize();
                                    break;
                            }

                            App.appWindow.Closing +=async (s, e) =>
                            {
                                e.Cancel = true;
                                var result = await Application.Current?.MainPage?.DisplayAlert("MauiWinTitle", "Are you sure you want to exit?", "OK", "Cancel");
                                if (result)
                                {
                                    Application.Current.Quit();
                                }
                            };
                        });
                    });
#endif
                });

#if DEBUG
                    builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

     
    }
}
