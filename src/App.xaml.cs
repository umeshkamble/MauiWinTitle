using Microsoft.UI.Windowing;

namespace MauiWinTitle
{
    public partial class App : Application
    {
        public static AppWindow appWindow { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
