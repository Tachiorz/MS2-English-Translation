using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ms2_translate_gui
{
    public partial class MainWindow : System.Windows.Window
    {
        void AddEventHandlers()
        {
            ExitButtonImage.Click += ExitButtonImage_Click;
            MinimizeButtonImage.Click += MinimizeButtonImage_Click;
            RefreshButtonImage.Click += RefreshButtonImage_Click;

        }


        void RefreshButtonImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        void MinimizeButtonImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        void ExitButtonImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
