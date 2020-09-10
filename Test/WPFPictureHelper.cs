using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media;

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
        public static BitmapSource CreatAnEmptyBitmapSourceBySize(int width, int height)
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

        public static ImageSource CutImageBlank(ImageSource source)
        {
            int RectX = 0;
            int RectY = 0;
            int RectRight = 0;
            int RectBottom = 0;

            BitmapSource bitmap = (BitmapSource)source;//((DrawingImage)source).ToBitmapSource();
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
            int keep = 4;
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
            return null;
        }
    }
    public class PictureMixer
    {
        public DrawingImage OutImage { 
            get
            {
                return new DrawingImage() { Drawing = Group };
            } 
        }
        public DrawingGroup Group { set; get; } = new DrawingGroup() { Opacity = 1 };
        public PictureMixer(int width,int height)
        {
            this.Group.Children.Add(new ImageDrawing(WPFPictureHelper.CreatAnEmptyBitmapSourceBySize(width, height), new Rect(0, 0, width, height)));
        }

        public void AddPicture(BitmapSource bitmapSource, Rect rect, int opacity)
        {
            DrawingGroup group = new DrawingGroup();
            group.Opacity = opacity / 255.0;
            group.Children.Add(new ImageDrawing(bitmapSource, rect));
            this.Group.Children.Add(group);
        }
    }

}
