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
            SaveButtonImage.Click += SaveButtonImage_Click;
            StringsListView.SelectionChanged += StringsListView_SelectionChanged;
            FilesListView.SelectionChanged += FilesListView_SelectionChanged;
            EnglishString.TextChanged += EnglishString_TextChanged;
        }

        bool byUser = true;
        void EnglishString_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (byUser) {
                xmlDirtyFlag[FilesListView.SelectedIndex] = true;
                SaveString();
            }
        }



        void StringsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        void FilesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            int newFileIdx = FilesListView.SelectedIndex;
            if (previousFileIdx != newFileIdx && newFileIdx >= 0) {
                SaveData();
                previousFileIdx = newFileIdx;
                UpdateStringData();
            }
        }


        void SaveButtonImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveData();
        }

        void MinimizeButtonImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        void ExitButtonImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveData();
        }
    }
}
