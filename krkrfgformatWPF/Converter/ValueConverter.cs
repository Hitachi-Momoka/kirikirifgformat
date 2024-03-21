using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using Li.Krkr.krkrfgformatWPF.Models;
using Li.Krkr.krkrfgformatWPF.Helper;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Li.Krkr.krkrfgformatWPF.Converter;

internal class ScaleTransformToPercent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TransformGroup transGroup) return "0%";
        return ((int)(((ScaleTransform)transGroup.Children[0]).ScaleX * 100)).ToString() + "%";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class PathToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Path.GetFileNameWithoutExtension(value as string);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class SelectedItemCombineIndexConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value != null && parameter != null)
        {
            return new SelectedItemWithIndexModel((int)parameter, value);
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && parameter != null)
        {
            return new SelectedItemWithIndexModel((int)parameter, value);
        }
        return null;

        //return value==null ? null : ((SelectedItemWithIndexModel)value).SelectedItem;
    }
}
//最后的图片合成
internal class AllDataToImage : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values[2] != null ? new BitmapImage(new Uri(((SelectedItemWithIndexModel)values[2]).SelectedItem.ToString())) : null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}