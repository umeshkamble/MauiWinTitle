
namespace MauiWinTitle
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            TitleContainer.PerformClick += async () => 
            {
               await DisplayAlert("Alert", "Hello Umesh", "Ok");
            };
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
        }

        
    }

}
