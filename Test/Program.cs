using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using Li.Drawing.Wpf;
using Li.Drawing;

namespace Li.Test
{
    public class Test
    {
        public static unsafe void Main()
        {
            string name = @"C:\Users\Administrator\Desktop\未标题-1.png";
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(name);
             bitmap.Clone( Li.Drawing.Picture.GetRectFromPictureWithouBlank(bitmap),bitmap.PixelFormat).Save(@"C:\Users\Administrator\Desktop\out.png");
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