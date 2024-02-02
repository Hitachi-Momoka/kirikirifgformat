﻿using Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Li.Krkr.krkrfgformatWPF.Serves;
using Li.Krkr.krkrfgformatWPF.Models;

namespace Li.Krkr.krkrfgformatWPF.ViewModes
{
    public partial class MainWindowViewModel : BindableBase
    {
        #region Command
        public DelegateCommand ClearAllCommand { set; get; }
        public DelegateCommand ClearSelectedCommand { get; set; }
        //public DelegateCommand SideOnlyCommand { get; set; }
        public DelegateCommand HelpButtonCommand { set; get; }
        public DelegateCommand SelectRulePathCommand { get; set; }
        public DelegateCommand SelectSavePathCommand { get; set; }
        public DelegateCommand FormatSelectedCommand { get; set; }
        public DelegateCommand OpenSaveFolderCommand { get; set; }
        #endregion

        #region Mumber
        private bool messageBoxIsShow;
        
        private SelectedItemWithIndexModel _selectItemTmp;

        public SelectedItemWithIndexModel SelectItemTmp
        {
            get => _selectItemTmp;
            set
            {
                _selectItemTmp = value;
                if (string.IsNullOrEmpty(RulePath) && this.SelectItemTmp != null)
                {
                    RulePath = GetRulePath(SelectItemTmp.SelectedItem.ToString());
                    SavePath = CreateDefaultSavePath(SelectItemTmp.SelectedItem.ToString());
                }
                UpDataAllItems(SelectItemTmp);
                base.RaisePropertyChanged();
            }
        }

        private SortedDictionary<int,Tuple<string,BitmapSource>> _allItems;

        public SortedDictionary<int, Tuple<string, BitmapSource>> AllItems
        {
            get => _allItems;
            set 
            {
                _allItems = value;
                this.UpdateImage();
            }
        }

        private string _rulePath;

        public string RulePath
        {
            get => _rulePath;
            set
            {
                _rulePath = value;
                RuleData = RuleDataServes.CreateFromFile(RulePath);
                base.RaisePropertyChanged();
            }
        }

        private string _savePath;

        public string SavePath
        {
            get => _savePath;
            set
            {
                _savePath = value;
                base.RaisePropertyChanged();
            }
        }

        private string _saveName;

        public string SaveName
        {
            get => _saveName;
            set
            {
                _saveName = value;
                base.RaisePropertyChanged();
            }
        }

        private bool _isSideOnly;
        public bool IsSideOnly
        {
            get => _isSideOnly;
            set
            {
                _isSideOnly = value;
                base.RaisePropertyChanged();
                UpdateImage();
                //this.CollectionChanged(null,null);//选择状态变化就引起一次合成刷新。
            }
        }
        private ImageSource _imageBoxSource;

        public ImageSource ImageBoxSource
        {
            get => _imageBoxSource;
            set
            {
                _imageBoxSource = value;
                base.RaisePropertyChanged();
            }
        }

        public RuleDataModel RuleData { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            this.Init();
        }
        private void Init()
        {
            messageBoxIsShow = false;

            _selectItemTmp = null;
            _rulePath = "";
            _isSideOnly = false;
            _savePath = "";
            _saveName = "";
            RuleData = null;
            _imageBoxSource = null;
            _allItems = new SortedDictionary<int, Tuple<string, BitmapSource>>();
            
            this.SelectSavePathCommand = new DelegateCommand(this.EmptyMethod, () => false); //关闭按钮，不予以支持
            this.ClearSelectedCommand = new DelegateCommand(this.ClearSelected);
            this.ClearAllCommand = new DelegateCommand(this.ClearAll);
            this.FormatSelectedCommand = new DelegateCommand(this.FormatSelected);
            this.SelectRulePathCommand = new DelegateCommand(this.SelectRulePath);
            this.OpenSaveFolderCommand = new DelegateCommand(this.OpenSaveFolder);
        }
    }
}
