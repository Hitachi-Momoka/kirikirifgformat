using Li.Krkr.krkrfgformatWPF;
using Li.Krkr.krkrfgformatWPF.Converter;
using Li.Krkr.krkrfgformatWPF.ViewModes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Li.Krkr.krkrfgformatWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFullWindow = false;
        public MainWindow()
        {
            InitializeComponent();
            this.SetSelectedItemBinding();
        }

        private void addNewBox_Click(object sender, RoutedEventArgs e)
        {
            var index = FileGrid.Children.IndexOf((Button)sender);
            var box1 = new ListBox()
            {
                Style = (Style)FindResource("MWListBox") ?? default
            };
            box1.SetBinding(ListBox.SelectedItemProperty, new Binding()
            {
                Path = new PropertyPath("SelectItemTmp"),
                Mode = BindingMode.TwoWay,
                Converter = new SelectedItemCombineIndexConverter(),
                ConverterParameter = index
            });
            FileGrid.Children.Insert(index, box1);
        }
        private void ListBox_Drop(object sender, DragEventArgs e)
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

        private void ListBox_DragEnter(object sender, DragEventArgs e)
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

        private void SetSelectedItemBinding()
        {
            foreach (var item in this.FileGrid.Children)
            {
                if(item is ListBox)
                {
                    ListBox listBox = item as ListBox;
                    int index = this.FileGrid.Children.IndexOf(listBox);
                    listBox.SetBinding(ListBox.SelectedItemProperty, 
                        new Binding() { Path = new PropertyPath("SelectItemTmp"), 
                            Mode = BindingMode.OneWayToSource,
                            Converter = new SelectedItemCombineIndexConverter(),
                            ConverterParameter = index 
                        });
                }
            }
        }

        private void clearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.FileGrid.Children)
            {
                if (item is ListBox)
                {
                    ListBox listBox = item as ListBox;
                    listBox.ItemsSource = null;
                }
            }
            //slider1.Value = slider1.Minimum;
        }

        private void clearSelected_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.FileGrid.Children)
            {
                if (item is ListBox)
                {
                    ListBox listBox = item as ListBox;
                    listBox.SelectedIndex = -1;
                }
            }
        }

    }
}
