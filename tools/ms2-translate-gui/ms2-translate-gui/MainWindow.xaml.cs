using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ms2_translate_gui
{
    public partial class MainWindow : Window
    {
        List<String> Files;
        List<Label> filesLabel;
        int selectedFile = -1;
        List<Label> stringsLabel;
        int selectedStringIdx = -1;
        String selectedStringName = "";
        HashSet<XName> attributes;
        List<String> enStrings, krStrings;


        public MainWindow()
        {
            InitializeComponent();
            MakeDraggable();
            AddEventHandlers();
            InitializeFilesData();
            CreateFilesLabel();

            attributes = new HashSet<XName>();
            foreach (var f in Files) {
                var xml = XDocument.Load(f);
                if (xml.Root.Elements().Count() > 0) {
                    foreach (var elem in xml.Root.Elements()) {
                        foreach (var attr in elem.Attributes()) {
                            attributes.Add(attr.Name);
                        }
                    }
                }
            }
        }

        public void InitializeFilesData()
        {
            string[] files_kr = System.IO.Directory.GetFiles("./kr_original");
            string[] files_en = System.IO.Directory.GetFiles("./kr");

            Files = new List<String>();
            int size = Math.Min(files_en.Length, files_kr.Length), i=0, j=0;
            while (i < size && j < size)
            {
                string kr = System.IO.Path.GetFileName(files_kr[i]);
                string en = System.IO.Path.GetFileName(files_en[j]);
                int res = kr.CompareTo(en);
                if (res == 0) {
                    Files.Add(files_kr[i]);
                }
                if (res <= 0) {
                    i++;
                }
                if (res >= 0) {
                    j++;
                }
            }
        }

        public void CreateFilesLabel()
        {
            filesLabel = new List<Label>();
            StackPanel sp = FilesListStackPanel;
            for (int i=0; i<Files.Count; i++) {
                Label l = new Label();
                String f = System.IO.Path.GetFileName(Files[i]);

                l.Name = String.Format("file_{0}", i);
                l.Content = f;
                l.HorizontalContentAlignment = HorizontalAlignment.Left;
                l.Margin = new Thickness(0, 0, 0, 0);
                l.FontSize = 12;
                l.Background = Brushes.White;

                l.MouseDown += l_file_MouseDown;
                
                filesLabel.Add(l);
                sp.Children.Add(l);
            }
        }

        bool Ascii_only(String s)
        {
            foreach (var c in s) {
                if ((int)c > 127) {
                    return false;
                }
            }
            return true;
        }

        void UpdateStringData()
        {
            for (int i=0; i<filesLabel.Count; i++) {
                filesLabel[i].Background = (i == selectedFile) ? Brushes.LightGray : Brushes.White;
            }
            if (selectedFile < 0) {
                return;
            }
            StackPanel sp = StringsListStackPanel;
            var filename = System.IO.Path.GetFileName(Files[selectedFile]);
            sp.Children.Clear();
            stringsLabel = new List<Label>();
            krStrings = new List<string>();
            enStrings = new List<string>();
            var kr_root = XDocument.Load(@"./kr_original/" + filename).Root;
            var en_root = XDocument.Load(@"./kr/" + filename).Root;
            int idx=0;
            foreach (var elem in kr_root.Elements().Zip(en_root.Elements(), (f,s)=>(new Tuple<XElement,XElement>(f,s)))) {
                foreach (var name in attributes) {
                    XAttribute attr_kr = elem.Item1.Attribute(name);
                    XAttribute attr_en = elem.Item2.Attribute(name);
                    if (attr_kr == null) {
                        continue;
                    }
                    if (attr_kr.Value == "" || Ascii_only(attr_kr.Value)) {
                        continue;
                    }
                    Label l = new Label();
                    l.Name = String.Format("string_{0}_{1}", name.ToString(), idx);
                    l.Content = attr_kr.Value;
                    l.HorizontalContentAlignment = HorizontalAlignment.Left;
                    l.Margin = new Thickness(0, 0, 0, 0);
                    l.FontSize = 12;
                    l.Background = Brushes.White;

                    l.MouseDown +=l_string_MouseDown;

                    stringsLabel.Add(l);
                    sp.Children.Add(l);
                    krStrings.Add(attr_kr.Value);
                    enStrings.Add(attr_en.Value);
                    idx++;
                }
            }
        }

        void UpdateData()
        {
            for (int i=0; i<stringsLabel.Count; i++) {
                Label l = stringsLabel[i];
                bool flag = (l.Name == "string_" + selectedStringName + "_" + selectedStringIdx);
                l.Background = (flag ? Brushes.LightGray : Brushes.White);

                if (selectedStringIdx >= 0) {
                    KoreanString.Text = krStrings[selectedStringIdx];
                    EnglishString.Text = enStrings[selectedStringIdx];
                }
            }
        }

        void l_string_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label l = (Label)sender;
            String[] strs = l.Name.Split('_');
            selectedStringName = strs[strs.Count() - 2];
            selectedStringIdx = Int32.Parse(strs[strs.Count() - 1]);

            UpdateData();
        }

        void l_file_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label l = (Label)sender;
            selectedFile = Int32.Parse(l.Name.Split('_').Last());
            if (selectedFile >= 0) {
                String filename = System.IO.Path.GetFileName(Files[selectedFile]);
            }
            UpdateStringData();
        }

    }
}
