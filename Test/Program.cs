using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using Li.Drawing.Wpf;

namespace Li.Test
{
    public class Test
    {
        public static void Main()
        {
            string filename = @"C:\Users\Administrator\Desktop\合成输出\栞那a+1888+1912.png";
            BitmapSource source = new BitmapImage(new Uri(filename));
            var o = Li.Drawing.Wpf.WPFPictureHelper.CutImageBlank(source);

        //    string basename = @"C:\Users\Administrator\Desktop\QQ截图20200829092529.png";
        //    string covername = @"C:\Users\Administrator\Desktop\头像挂件设计模版.png";

            //    var b = new BitmapImage(new Uri(basename));
            //    var c = new BitmapImage(new Uri(covername));
            //    DrawingGroup dg = new DrawingGroup
            //    {
            //        Opacity = 1
            //    };
            //    dg.Children.Add(new ImageDrawing(b, new Rect(0, 0, b.PixelWidth, b.PixelHeight)));
            //    dg.Children.Add(new ImageDrawing(c, new Rect(0, 0, c.PixelWidth, c.PixelHeight)));
            //    DrawingImage di = new DrawingImage
            //    {
            //        Drawing = dg
            //    };
            //    BitmapEncoder encoder = new PngBitmapEncoder();
            //    encoder.Frames.Add(BitmapFrame.Create(di.ToBitmapSource()));
            //    string newname = @"C:\Users\Administrator\Desktop\out.png";
            //    using (var stream = new FileStream(newname, FileMode.Create))
            //    {
            //        encoder.Save(stream);
            //    }
            }

        }
    public static class Extension
    {
        //Code from "https://www.it1352.com/1253670.html".
        public static BitmapSource ToBitmapSource(this DrawingImage source)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(source, new Rect(new Point(0, 0), new Size(source.Width, source.Height)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)source.Width, (int)source.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }
        public static DrawingImage ToDrawingImage(this BitmapSource source)
        {
            Rect imageRect = new Rect(0, 0, source.PixelWidth, source.PixelHeight);
            ImageDrawing drawing = new ImageDrawing(source, imageRect);
            return new DrawingImage(drawing);
        }
    }
}