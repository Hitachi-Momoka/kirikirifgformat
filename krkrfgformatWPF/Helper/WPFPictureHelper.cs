using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Li.Krkr.krkrfgformatWPF.Helper;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

namespace Li.Drawing.Wpf
{
    public class WPFPictureHelper
    {
        public static ImageSource MixPicture(BitmapSource image1, BitmapSource image2, Rect rect1, Rect rect2,int opacity1, int opacity2)
        {
            DrawingGroup group1 = new DrawingGroup() { Opacity = opacity1 / 255.0 };
            group1.Children.Add(new ImageDrawing(image1, rect1));
            DrawingGroup group2 = new DrawingGroup() { Opacity = opacity2 / 255.0 };
            group2.Children.Add(new ImageDrawing(image2, rect2));

            DrawingGroup group = new DrawingGroup();
            group.Children.Add(group1);
            group.Children.Add(group2);

            return new DrawingImage() { Drawing = group };

            //throw new NotSupportedException();
        }
        public static BitmapSource CreateAnEmptyBitmapSourceBySize(int width, int height)
        {
            if(width <= 0 || height <= 0)
            {
                return null;
            }
            byte[] transparentColor = new byte[4] { 255, 255, 255, 0 };
            int stride = width * 4;
            int dataSize =Math.Abs(stride * height);
            byte[] bytes = new byte[dataSize];
            for (int h = 0; h < height; h++)
            {
                for (int s = 0; s < stride;s+=4)
                {
                    int pixCount = h * stride + s;
                    bytes[pixCount] = transparentColor[0];
                    bytes[pixCount+1] = transparentColor[1];
                    bytes[pixCount+2] = transparentColor[2];
                    bytes[pixCount+3] = transparentColor[3];
                }
            }
            return BitmapSource.Create(width, height, 96.0, 96.0, PixelFormats.Bgra32, null, bytes, stride);
        }

        public static Stream BitmapToStream(BitmapSource source)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            var stream = new MemoryStream(); 
            encoder.Save(stream);
            return stream;
        }
        public static BitmapSource DrawingImageToBitmapSource(DrawingImage source)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(source, new Rect(new Point(0, 0), new Size(source.Width, source.Height)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)source.Width, (int)source.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }
        public static DrawingImage BitmapSourceToDrawingImage(BitmapSource source)
        {
            Rect imageRect = new Rect(0, 0, source.PixelWidth, source.PixelHeight);
            ImageDrawing drawing = new ImageDrawing(source, imageRect);
            return new DrawingImage(drawing);
        }
       
        public static BitmapSource CutImageBlank(BitmapSource source)
        {
            int keep = 4;
            BitmapSource bitmap = source; //DrawingImageToBitmapSource((DrawingImage)source);

            int RectX = 0;
            int RectY = 0;
            int RectRight = 0;
            int RectBottom = 0;
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;


            int bstride = Math.Abs(width * 4);
            int byteSize = bstride * height;

            byte[] array = new byte[byteSize];
            bitmap.CopyPixels(array, bstride, 0);
                for (int i = 0; i < bstride; i += 4)
                {
                    bool flag = false;
                    for (int j = 0; j < height - 1; j++)
                    {
                        if (array[bstride * j + i + 3] != 0)
                        {
                            RectX = i / 4;
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                for (int k = 0; k < height; k++)
                {
                    bool flag2 = false;
                    for (int l = 0; l < bstride - 1; l += 4)
                    {
                        if (array[bstride * k + l + 3] != 0)
                        {
                            RectY = k;
                            flag2 = true;
                            break;
                        }
                    }
                    if (flag2)
                    {
                        break;
                    }
                }
                for (int num6 = bstride; num6 > 0; num6 -= 4)
                {
                    bool flag3 = false;
                    for (int num7 = height - 1; num7 > 0; num7--)
                    {
                        if (array[bstride * num7 + num6 - 1] != 0)
                        {
                            RectRight = num6 / 4;
                            flag3 = true;
                            break;
                        }
                    }
                    if (flag3)
                    {
                        break;
                    }
                }
                for (int num8 = height - 1; num8 > 0; num8--)
                {
                    bool flag4 = false;
                    for (int num9 = bstride; num9 > 0; num9 -= 4)
                    {
                        if (array[bstride * num8 + num9 - 1] != 0)
                        {
                            RectBottom = num8 + 1;
                            flag4 = true;
                            break;
                        }
                    }
                    if (flag4)
                    {
                        break;
                    }
                }
            Int32Rect result = default;
            if (RectX >= keep)
            {
                result.X = RectX - keep;
            }
            else
            {
                result.X = RectX;
            }
            if (RectY >= keep)
            {
                result.Y = RectY - keep;
            }
            else
            {
                result.Y = RectY;
            }
            if (RectRight >= width - keep)
            {
                result.Width = RectRight - result.X;
            }
            else
            {
                result.Width = RectRight - result.X + keep;
            }
            if (RectBottom >= height - keep)
            {
                result.Height = RectBottom - result.Y;
            }
            else
            {
                result.Height = RectBottom - result.Y + keep;
            }
            return new CroppedBitmap(bitmap, result);
        }

        public static BitmapSource CreateBitmapFromFile(string name)
        {
            if (null == name) return null;
            try
            {
                string ext = System.IO.Path.GetExtension(name);

                if (".png" == ext)
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(name)))
                    {
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                    }
                    return image;
                }
                if(".tlg"==ext)
                {
                    
                    var exePath = Environment.CurrentDirectory;
                    var fileArcFormats = System.IO.Path.Combine(exePath, "GARbro", "ArcFormats.dll");
                    var fileGameRes = System.IO.Path.Combine(exePath, "GARbro", "GameRes.dll");

                    if (!(System.IO.File.Exists(fileGameRes) || System.IO.File.Exists(fileArcFormats))) return null;

                    var asmArcFormats = Assembly.LoadFrom(fileArcFormats);
                    var asmGameRes = Assembly.LoadFrom(fileGameRes);

                    var classBinaryStream = asmGameRes.GetType("GameRes.BinaryStream");
                    var methodBinaryStream = classBinaryStream.GetMethod("FromFile");
                    var binaryStream = methodBinaryStream.Invoke(null, new object[] { name });


                    var classTlgFormat = asmArcFormats.GetType("GameRes.Formats.KiriKiri.TlgFormat");
                    var tlgFormat = Activator.CreateInstance(classTlgFormat);


                    var methodReadMetaData = classTlgFormat.GetMethod("ReadMetaData");
                    var info = methodReadMetaData.Invoke(tlgFormat, new object[] { binaryStream });


                    var methodRead = classTlgFormat.GetMethod("Read");
                    dynamic imageData = methodRead.Invoke(tlgFormat, new object[] { binaryStream, info });
                    var bitmapSource = (BitmapSource)imageData.Bitmap;
                    return bitmapSource;
                }
            }
            catch(Exception ex)
            {

            }
            throw new NotSupportedException("不受支持的文件。");
        }
    }
    public class PictureMixer
    {
        public DrawingImage OutImage => new DrawingImage() { Drawing = Group };
        public DrawingGroup Group { set; get; } = new DrawingGroup() { Opacity = 1 };

        public void AddPicture(BitmapSource bitmapSource, Rect rect, int opacity)
        {
            DrawingGroup group = new DrawingGroup
            {
                Opacity = opacity / 255d
            };
            group.Children.Add(new ImageDrawing(bitmapSource, rect));
            this.Group.Children.Add(group);
        }
    }

    public struct PointRect
    {
        public double X { set; get; }
        public double Y { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }

        public PointRect(double x, double y, double r, double b)
        {
            this.X = x;
            this.Y = y;
            this.Bottom = b;
            this.Right = r;
        }

        public PointRect Covered(PointRect baseRect, PointRect coveRect)
        {
            return new PointRect()
            {
                X = Math.Min(baseRect.X,coveRect.X),
                Y = Math.Min(baseRect.Y,coveRect.Y),
                Bottom = Math.Max(baseRect.Bottom,coveRect.Bottom),
                Right = Math.Max(baseRect.Right,baseRect.Right)
            };
        }

        public Rect ToRect()
        {
            return new Rect()
            {
                X = this.X,
                Y = this.Y,
                Width = Math.Max(this.Right - this.X, 0.0),
                Height = Math.Max(this.Bottom - this.Y, 0.0)
            };
        }
    }

}
