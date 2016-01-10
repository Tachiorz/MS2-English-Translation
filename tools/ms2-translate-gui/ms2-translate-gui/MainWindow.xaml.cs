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
        List<String> fileNames;
        List<XDocument> enXmls, krXmls;
        List<XElement> enStrings, krStrings;
        List<bool> xmlDirtyFlag, stringDirtyFlag;

        int previousFileIdx = -2; 
        HashSet<XName> attributes;



        public MainWindow()
        {
            InitializeComponent();
            MakeDraggable();
            AddEventHandlers();
            InitializeFilesData();
            InitializeFilesListView();

            attributes = new HashSet<XName>();
            foreach (var x in krXmls) {
                if (x.Root.Elements().Count() > 0) {
                    foreach (var elem in x.Root.Elements()) {
                        foreach (var attr in elem.Attributes()) {
                            attributes.Add(attr.Name);
                        }
                    }
                }
            }
        }

        public void InitializeFilesData()
        {
            fileNames = new List<string>();
            xmlDirtyFlag = new List<bool>();
            krXmls = new List<XDocument>();
            enXmls = new List<XDocument>();

            string[] files_kr = System.IO.Directory.GetFiles("./kr_original");
            string[] files_en = System.IO.Directory.GetFiles("./kr");
            int size = Math.Min(files_en.Length, files_kr.Length), i=0, j=0;
            while (i < size && j < size) 
            {
                fileNames.Add(System.IO.Path.GetFileName(files_kr[i]));
                xmlDirtyFlag.Add(false);
                string kr = System.IO.Path.GetFileName(files_kr[i]);
                string en = System.IO.Path.GetFileName(files_en[j]);
                int res = kr.CompareTo(en);
                if (res == 0) {
                    krXmls.Add(XDocument.Load(files_kr[i]));
                    enXmls.Add(XDocument.Load(files_en[j])); 
                }
                if (res <= 0) { i++; }
                if (res >= 0) { j++; }
            }
        }

        public void InitializeFilesListView()
        {
            ListView lv = FilesListView;
            lv.ItemsSource = fileNames;
        }

        void UpdateStringData()
        {
            int selectedFileIdx = FilesListView.SelectedIndex;
            if (selectedFileIdx < 0) {
                return;
            }
            krStrings = new List<XElement>();
            enStrings = new List<XElement>();
            var kr_root = krXmls[selectedFileIdx].Root;
            var en_root = enXmls[selectedFileIdx].Root;
            foreach (var elem in kr_root.Elements()) { krStrings.Add(elem); }
            foreach (var elem in en_root.Elements()) { enStrings.Add(elem); }
            stringDirtyFlag = new List<bool>();
            for (int i=0; i<=krStrings.Count; i++) { stringDirtyFlag.Add(false); }
            StringsListView.ItemsSource = krStrings;
        }

        void UpdateData()
        {
            int idx = StringsListView.SelectedIndex;
            if (idx >= 0) {
                KoreanString.Text = krStrings[idx].ToString();
                byUser = false;
                EnglishString.Text = enStrings[idx].ToString();
                byUser = true;
            }
        }

        void SaveString()
        {
            int selectedFileIdx = FilesListView.SelectedIndex;
            int selectedStringIdx = StringsListView.SelectedIndex;
            if (selectedFileIdx >= 0 && EnglishString.Text.Length > 0) {
                enStrings[selectedStringIdx] = XElement.Parse(EnglishString.Text);
                var root = enXmls[selectedFileIdx].Root;
                root.Elements().ElementAt(selectedStringIdx).AddAfterSelf(enStrings[selectedStringIdx]);
                root.Elements().ElementAt(selectedStringIdx).Remove();

            }
        }

        void SaveData()
        {
            int size = enXmls.Count;
            for (int i=0; i<size; i++) {
                if (!xmlDirtyFlag[i]) { continue; }
                xmlDirtyFlag[i] = false;
                enXmls[i].Save(@"./kr/" + fileNames[i]);
            }
        }
    }
}
