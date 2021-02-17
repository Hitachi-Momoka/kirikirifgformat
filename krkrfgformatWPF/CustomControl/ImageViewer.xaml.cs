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

namespace Li.Krkr.krkrfgformatWPF.CustomControl
{
    /// <summary>
    /// ImageViewer.xaml 的交互逻辑
    /// </summary>
    public partial class ImageViewer : UserControl
    {




        public ImageSource ImageBoxSource
        {
            get { return (ImageSource)GetValue(ImageBoxSourceProperty); }
            set { SetValue(ImageBoxSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageBoxSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageBoxSourceProperty =
            DependencyProperty.Register("ImageBoxSource", typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(0));




        public double ZoomPercent
        {
            get { return (double)GetValue(ZoomPercentProperty); }
            set { SetValue(ZoomPercentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomPersent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomPercentProperty =
            DependencyProperty.Register("ZoomPersent", typeof(double), typeof(ImageViewer), new PropertyMetadata(0));

            
        public ImageViewer()
        {
            InitializeComponent();
        }
    }
}
