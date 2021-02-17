using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
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
using Li.Drawing.Wpf;

namespace WPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void lst_Drop(object sender, DragEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            listBox.ItemsSource = null;
            var array = (string[])e.Data.GetData(DataFormats.FileDrop);
            var obsList = new ObservableCollection<string>();
            foreach (var item in array)
            {
                string ext = System.IO.Path.GetExtension(item).ToLower();
                if (ext == ".png" || ext == ".tlg")
                {
                    obsList.Add(item);
                }
            }
            listBox.ItemsSource = obsList;
        }

        private void lst_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            box.Source = WPFPictureHelper.GreateBitmapFromFile(list.SelectedItem.ToString());
        }
    }
}
