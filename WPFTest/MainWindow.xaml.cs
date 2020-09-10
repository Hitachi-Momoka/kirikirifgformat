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

namespace WPFTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<ViewListItem> lst = new ObservableCollection<ViewListItem>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LB1.ItemsSource = new ObservableCollection<ViewListItem>() { new ViewListItem() { Str = "list" }, new ViewListItem() { Str = "box" }, new ViewListItem() { Str = "one" } };
            LB2.ItemsSource = new ObservableCollection<ViewListItem>() { new ViewListItem() { Str = "list" }, new ViewListItem() { Str = "box" }, new ViewListItem() { Str = "two" } };
            LB3.ItemsSource = new ObservableCollection<ViewListItem>() { new ViewListItem() { Str = "list" }, new ViewListItem() { Str = "box" }, new ViewListItem() { Str = "three" } };
        }

        private void LB1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            TB1.Text = "";
            foreach (var item in G1.Children)
            {
                ListBox lb = item as ListBox;
                if(lb.SelectedItem!=null)
                {
                    lst.Add(new ViewListItem() { Str = lb.SelectedItem.ToString() });
                }
            }
        }
    }
}
