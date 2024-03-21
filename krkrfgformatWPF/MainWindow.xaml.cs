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
using Li.Krkr.krkrfgformatWPF;
using Li.Krkr.krkrfgformatWPF.Converter;
using Li.Krkr.krkrfgformatWPF.ViewModes;

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

        private void AddNewBox_Click(object sender, RoutedEventArgs e)
        {
            var index = FileGrid.Children.IndexOf((Button)sender);
            var box1 = new ListBox { Style = (Style)FindResource("MWListBox") };
            box1.SetBinding(
                Selector.SelectedItemProperty,
                new Binding()
                {
                    Path = new PropertyPath("SelectedItemTemp"),
                    Mode = BindingMode.TwoWay,
                    Converter = new SelectedItemCombineIndexConverter(),
                    ConverterParameter = index
                }
            );
            FileGrid.Children.Insert(index, box1);
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            var listBox = (ListBox)sender;
            listBox.ItemsSource = null;
            var data = (string[])e.Data.GetData(DataFormats.FileDrop);
            var obs = new ObservableCollection<string>();
            var items = data.Where(item =>
            {
                var ext = System.IO.Path.GetExtension(item).ToLower();
                return Helper.SupportedFileExtension.ImageExtension.Any(ex => ex.Equals(ext));
            });
            foreach (var item in items)
            {
                obs.Add(item);
            }
            listBox.ItemsSource = obs;
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.All
                : DragDropEffects.None;
        }

        private void SetSelectedItemBinding()
        {
            foreach (var listBox in this.FileGrid.Children.OfType<ListBox>())
            {
                var index = this.FileGrid.Children.IndexOf(listBox);
                listBox.SetBinding(
                    Selector.SelectedItemProperty,
                    new Binding
                    {
                        Path = new PropertyPath("SelectedItemTemp"),
                        Mode = BindingMode.OneWayToSource,
                        Converter = new SelectedItemCombineIndexConverter(),
                        ConverterParameter = index
                    }
                );
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var listBox in this.FileGrid.Children.OfType<ListBox>())
            {
                listBox.ItemsSource = null;
            }
            //slider1.Value = slider1.Minimum;
        }

        private void ClearSelected_Click(object sender, RoutedEventArgs e)
        {
            foreach (var listBox in this.FileGrid.Children.OfType<ListBox>())
            {
                listBox.SelectedIndex = -1;
            }
        }
    }
}
