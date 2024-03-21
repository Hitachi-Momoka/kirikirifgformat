using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Li.Krkr.krkrfgformatWPF.Helper;

public static class Helper
{
    public static int GetFileCode(string path)
    {
        var part = System.IO.Path.GetFileName(path).Split('.')[0].Split('_');
        return Convert.ToInt32(part[^1]);
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        System.Windows.Point ToWPFPoint()
        {
            return new System.Windows.Point(this.X, this.Y);
        }
    }
}

public static class Extension
{
    public static Int32Rect ToInt32Rect(this Rect rect)
    {
        return new Int32Rect()
        {
            X = (int)rect.X,
            Y = (int)rect.Y,
            Width = (int)rect.Width,
            Height = (int)rect.Height
        };
    }

    //Code from "https://www.it1352.com/1253670.html".
    public static BitmapSource ToBitmapSource(this DrawingImage source)
    {
        var drawingVisual = new DrawingVisual();
        var drawingContext = drawingVisual.RenderOpen();
        drawingContext.DrawImage(
            source,
            new Rect(new Point(0, 0), new Size(source.Width, source.Height))
        );
        drawingContext.Close();

        var bmp = new RenderTargetBitmap(
            (int)source.Width,
            (int)source.Height,
            96,
            96,
            PixelFormats.Pbgra32
        );
        bmp.Render(drawingVisual);
        return bmp;
    }

    public static DrawingImage ToDrawingImage(this BitmapSource source)
    {
        var imageRect = new Rect(0, 0, source.PixelWidth, source.PixelHeight);
        var drawing = new ImageDrawing(source, imageRect);
        return new DrawingImage(drawing);
    }
}
