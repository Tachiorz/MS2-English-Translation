using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace ms2_translate_gui
{
    public partial class MainWindow : System.Windows.Window
    {
        private void MakeDraggable()
        {
            RootLayerGrid.MouseDown += MakeDraggable_MouseDown;
        }

        void MakeDraggable_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left) {
                try { this.DragMove(); }
                catch { }
            }
        }

    }
}
