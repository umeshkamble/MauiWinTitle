using Microsoft.Maui.Graphics;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinColor = Windows.UI.Color;
using Rect = Windows.Foundation.Rect;
using System.Diagnostics;

namespace MauiWinTitle
{
    public class TitleContainer : Microsoft.UI.Xaml.Controls.Grid
    {
        static TitleContainer? titleGrid = null;
        static Color? titlebarColor = null;
        static string? currentTitle = null;
       
        public static Action? PerformClick { get; set; }
        public TitleContainer()
        {
            titleGrid = this;
            this.Loaded += TitleContainer_Loaded;
        }

        private void TitleContainer_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            SetTitlebarColor(titlebarColor);
            SetTitle(currentTitle);
            SetRootGrid();
            SetRegionsForCustomTitleBar();
            SetAlertAction();
        }

        private void SetAlertAction()
        {
            var alertButton = titleGrid?.FindName("btnAlert") as Microsoft.UI.Xaml.Controls.Button;
            if (alertButton is not null)
            {
                alertButton.Click += (sender, args) =>
                {
                    PerformClick?.Invoke();
                };
            }
        }

        private void SetRootGrid()
        {
                var grdRoot = titleGrid?.FindName("grdRoot") as Microsoft.UI.Xaml.Controls.Grid;

                if (grdRoot is not null)
                {
                    grdRoot.SizeChanged += (s, e) =>
                    {
                        SetRegionsForCustomTitleBar();
                    };

                }
        }

        public static void SetRegionsForCustomTitleBar()
        {
            try
            {
                // Specify the interactive regions of the title bar.
                var grdRoot = titleGrid?.FindName("grdRoot") as Microsoft.UI.Xaml.Controls.Grid;
                if (grdRoot is not null)
                {
                    double scaleAdjustment = grdRoot.XamlRoot.RasterizationScale;

                    // Get the rectangle around the appStatus control.
                    var titleBarButton = grdRoot.FindName("btnAlert") as Microsoft.UI.Xaml.Controls.Button;
                    var titleBarSearchBox = grdRoot.FindName("TitleBarSearchBox") as AutoSuggestBox;
                    var titleBarPersonPic = grdRoot.FindName("PersonPic") as PersonPicture;
                    if (titleBarButton is not null && titleBarSearchBox is not null && titleBarPersonPic is not null)
                    {

                        var appButtonRect = GetControlRect(grdRoot, titleBarButton);
                        var titleBarSearchBoxRect = GetControlRect(grdRoot, titleBarSearchBox);
                        var titleBarPersonPicRect = GetControlRect(grdRoot, titleBarPersonPic);

                        var rectArray = new Windows.Graphics.RectInt32[] { appButtonRect, titleBarSearchBoxRect, titleBarPersonPicRect };

                        InputNonClientPointerSource nonClientInputSrc =
                            InputNonClientPointerSource.GetForWindowId(App.appWindow.Id);
                        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static Windows.Graphics.RectInt32 GetControlRect(Microsoft.UI.Xaml.UIElement rootControl, Microsoft.UI.Xaml.FrameworkElement tragetControl)
        {
            double scaleAdjustment = rootControl.XamlRoot.RasterizationScale;
            var displayWidth = Convert.ToInt32(DeviceDisplay.Current.MainDisplayInfo.Width);
            var currentWidth = App.appWindow.ClientSize.Width;
            GeneralTransform transform;
            if (displayWidth == currentWidth)
                transform = tragetControl.TransformToVisual(rootControl);
            else
                transform = tragetControl.TransformToVisual(null);
            Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                             tragetControl.ActualWidth,
                                                             tragetControl.ActualHeight));
            return GetRect(bounds, scaleAdjustment);
        }

        public static void SetTitlebarColor(Microsoft.Maui.Graphics.Color? color)
        {
            if (color != null)
            {
                titlebarColor = color;
                if (titleGrid != null)
                {
                    titleGrid.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(ToWindowsColor(titlebarColor));
                }
            }
        }

        public static void SetTitle(string? title)
        {
            currentTitle = title;
            if (title != null)
            {
                var titleBlock = titleGrid?.FindName("AppTitle") as TextBlock;
                if (titleBlock != null)
                {
                    titleBlock.Text = title;
                }
            }
        }

        public static WinColor ToWindowsColor(Microsoft.Maui.Graphics.Color color)
        {
            var a = (byte)(color.Alpha * 255);
            var r = (byte)(color.Red * 255);
            var g = (byte)(color.Green * 255);
            var b = (byte)(color.Blue * 255);
            return WinColor.FromArgb(a, r, g, b);
        }

        private static Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
        {
            return new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
        }
    }
}
