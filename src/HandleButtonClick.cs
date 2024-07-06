using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWinTitle
{
    public class HandleButtonClick : Behavior<Button>
    {
        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);
            //AssociatedObject.Click += AssociatedObject_Click;
        }
       
        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            //Move your MainWindow.Button_Click here;
        }
        protected override void OnDetachingFrom(Button bindable)
        {
            base.OnDetachingFrom(bindable);
           // AssociatedObject.Click -= AssociatedObject_Click;
        }
    }
}
