using Prism.Mvvm;
using Microsoft.Win32;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using Li.Drawing.Wpf;
using Li.Krkr.krkrfgformatWPF.Helper;
using Li.Krkr.krkrfgformatWPF.Models;
using static System.IO.Path;

namespace Li.Krkr.krkrfgformatWPF.ViewModes
{
    //public delegate ImageSource CutImageBlankHandler(BitmapSource source);
    public partial class MainWindowViewModel : BindableBase
    {
        private string GetRulePath(string imagePath)
        {
            var dir = GetDirectoryName(imagePath);
            var namePart = GetFileNameWithoutExtension(imagePath).Split('_');
            var sb = new StringBuilder();
            for (var i = 0; i < namePart.Length - 1; i++)
            {
                sb.Append(namePart[i]);
                if (i < namePart.Length - 2)
                {
                    sb.Append('_');
                }
            }
            var ruleFileName = sb.ToString();
            return SupportedFileExtension.RuleDataExtension.Select(format => $"{dir}\\{ruleFileName}{format}").FirstOrDefault(File.Exists);
        }

        private string CreateDefaultSavePath(string v)
        {
            var newDir = GetDirectoryName(v) + @"\合成输出\";
            if(!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }
            return newDir;
        }
        private void UpDataAllItems(SelectedItemWithIndexModel item)
        {
            if (item == null) return;
            var needToDelete = AllItems.Any(i => i.Key == item.Index);
            if (needToDelete) { AllItems.Remove(item.Index); }
            AllItems.Add(item.Index,
                new Tuple<string, BitmapSource>(item.SelectedItem.ToString(),
                                                WPFPictureHelper.CreateBitmapFromFile(item.SelectedItem.ToString())
                                                ));
            UpdateImage();
        }
        private void UpdateImage()
        {
            if (AllItems.Count == 0) return;
            if (RuleData == null)
            {
                WithoutRuleDataMode();
                return;
            }
            var strtmp = GetFileNameWithoutExtension(RulePath);
            var mixer = new PictureMixer();
            foreach (var item in AllItems)
            {
                strtmp += "+";
                LineDataModel line;
                if (this.IsSideOnly)
                {
                    line = this.RuleData.GetLineDataBySize(item.Value.Item2.PixelWidth, item.Value.Item2.PixelHeight);
                }
                else
                {
                    line = this.RuleData.GetLineDataByID(Helper.Helper.GetFileCode(item.Value.Item1)); ;
                }
                strtmp += line.LayerId;
                mixer.AddPicture(item.Value.Item2, line.ToRect(), Convert.ToInt32(line.Opacity));
            }
            SaveName = strtmp;
            this.ImageBoxSource = mixer.OutImage;
        }

        private void WithoutRuleDataMode()
        {
            if (!messageBoxIsShow)
            {
                var result = MessageBox.Show("需要一个规则文件或者规则文件不受支持。\n是否手动选择文件？\n（否只会显示当前最后一次选择的图片。）", "错误", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    this.SelectRulePath();
                }
                messageBoxIsShow = true;
                return;
            }
            if (SelectItemTmp == null) return;
            var image = new BitmapImage(new Uri(this.SelectItemTmp.SelectedItem.ToString()));
            ImageBoxSource = new System.Windows.Media.DrawingImage() { Drawing = new ImageDrawing(image, new Rect(0, 0, image.PixelWidth, image.PixelHeight))};
        }

        public void FormatSelected()
        {
            if (this.ImageBoxSource == null) return;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(((DrawingImage)ImageBoxSource).ToBitmapSource()));
            using (var stream = new FileStream($"{SavePath}\\{SaveName}.png", FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
        public void SelectRulePath()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "文本文档(*.txt)|*.txt|json文件(*.json)|*.json|所有文件 (*.*)|*.*",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                RulePath = openFileDialog.FileName;
            }
        }
        public void OpenSaveFolder()
        {
            if (!string.IsNullOrEmpty(SavePath))
                System.Diagnostics.Process.Start("explorer.exe", SavePath);
        }
        public void EmptyMethod()
        {

        }
        public void ClearSelected()
        {
            
            this.AllItems.Clear();
            this.ImageBoxSource = null;
            GC.Collect();
        }
        public void ClearAll()
        {
            this.messageBoxIsShow = true;

            this.SelectItemTmp = null;
            this.RulePath = "";
            this.IsSideOnly = false;
            this.SavePath = "";
            this.SaveName = "";
            this.RuleData = null;
            this.ImageBoxSource = null;

            this.AllItems = new SortedDictionary<int, Tuple<string, BitmapSource>>();
            this.messageBoxIsShow = false;

            GC.Collect();
        }
    }
}
