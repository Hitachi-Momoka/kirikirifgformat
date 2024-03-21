using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Li.Drawing.Wpf;
using Li.Krkr.krkrfgformatWPF.Helper;
using Li.Krkr.krkrfgformatWPF.Models;
using Li.Krkr.krkrfgformatWPF.Serves;
using Microsoft.Win32;
using static System.IO.Path;
using ItemDict = System.Collections.Generic.SortedDictionary<
    int,
    System.Tuple<string, System.Windows.Media.Imaging.BitmapSource>
>;

namespace Li.Krkr.krkrfgformatWPF.ViewModes
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool messageBoxIsShow = false;

        [ObservableProperty]
        private SelectedItemWithIndexModel selectedItemTemp = null;

        [ObservableProperty]
        private ItemDict allItems = [];

        [ObservableProperty]
        private string rulePath = "";

        [ObservableProperty]
        private string ruleName;

        [ObservableProperty]
        private string savePath = "";

        [ObservableProperty]
        private string saveName = "";

        [ObservableProperty]
        private bool isSideOnly = false;

        [ObservableProperty]
        private ImageSource imageBoxSource = null;
        private RuleDataModel RuleData { get; set; }

        #region OnPropertyChangede

        partial void OnSelectedItemTempChanged(SelectedItemWithIndexModel value)
        {
            if (string.IsNullOrEmpty(RulePath))
            {
                RulePath = GetRulePath(SelectedItemTemp.SelectedItem.ToString());
                SavePath = CreateDefaultSavePath(SelectedItemTemp.SelectedItem.ToString());
            }
            UpDataAllItems(SelectedItemTemp);
        }

        partial void OnRulePathChanged(string value)
        {
            RuleData = RuleDataServes.CreateFromFile(RulePath);
        }

        partial void OnIsSideOnlyChanged(bool value)
        {
            UpdateImage();
        }

        private void UpDataAllItems(SelectedItemWithIndexModel item)
        {
            if (item == null)
                return;
            var needToDelete = AllItems.Any(i => i.Key == item.Index);
            if (needToDelete)
            {
                AllItems.Remove(item.Index);
            }
            AllItems.Add(
                item.Index,
                new Tuple<string, BitmapSource>(
                    item.SelectedItem.ToString(),
                    WPFPictureHelper.CreateBitmapFromFile(item.SelectedItem.ToString())
                )
            );
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (AllItems.Count == 0)
                return;
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
                if (IsSideOnly)
                {
                    line = RuleData.GetLineDataBySize(
                        item.Value.Item2.PixelWidth,
                        item.Value.Item2.PixelHeight
                    );
                }
                else
                {
                    line = RuleData.GetLineDataById(
                        Helper.Helper.GetFileCode(item.Value.Item1)
                    );
                    ;
                }
                strtmp += line.LayerId;
                mixer.AddPicture(item.Value.Item2, line.ToRect(), Convert.ToInt32(line.Opacity));
            }
            SaveName = strtmp;
            ImageBoxSource = mixer.OutImage;
        }

        private void WithoutRuleDataMode()
        {
            if (!messageBoxIsShow)
            {
                var result = MessageBox.Show(
                    "需要一个规则文件或者规则文件不受支持。\n是否手动选择文件？\n（否只会显示当前最后一次选择的图片。）",
                    "错误",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (result == MessageBoxResult.Yes)
                {
                    SelectRulePath();
                }
                messageBoxIsShow = true;
                return;
            }

            var image = new BitmapImage(
                new Uri(SelectedItemTemp.SelectedItem.ToString() ?? string.Empty)
            );
            ImageBoxSource = new DrawingImage()
            {
                Drawing = new ImageDrawing(
                    image,
                    new Rect(0, 0, image.PixelWidth, image.PixelHeight)
                )
            };
        }

        private string CreateDefaultSavePath(string v)
        {
            var newDir = GetDirectoryName(v) + @"\合成输出\";
            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }
            return newDir;
        }

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
            return SupportedFileExtension
                .RuleDataExtension.Select(format => @$"{dir}\{ruleFileName}{format}")
                .FirstOrDefault(File.Exists);
        }

        #endregion

        #region RelayCommand

        [RelayCommand]
        public void FormatSelected()
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(((DrawingImage)ImageBoxSource).ToBitmapSource()));
            using var stream = new FileStream($"{SavePath}\\{SaveName}.png", FileMode.Create);
            encoder.Save(stream);
        }

        [RelayCommand]
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

        [RelayCommand]
        public void OpenSaveFolder()
        {
            if (!string.IsNullOrEmpty(SavePath))
                System.Diagnostics.Process.Start("explorer.exe", SavePath);
        }

        [RelayCommand(CanExecute = "CantExecute")]
        public void SelectSavePath() { }

        private bool CantExecute()
        {
            return false;
        }

        [RelayCommand]
        public void ClearSelected()
        {
            AllItems.Clear();
            ImageBoxSource = null;
            GC.Collect();
        }

        [RelayCommand]
        public void ClearAll()
        {
            messageBoxIsShow = true;

            SelectedItemTemp = null;
            RulePath = "";
            IsSideOnly = false;
            SavePath = "";
            SaveName = "";
            RuleData = null;
            ImageBoxSource = null;

            AllItems = [];
            messageBoxIsShow = false;

            GC.Collect();
        }
        #endregion
    }
}
