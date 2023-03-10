using CommunityToolkit.Mvvm.Input;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.ViewModel.Base;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

namespace JHoney_ImageConverter.ViewModel
{
    class ImageListViewModel : CustomViewModelBase
    {
        public MainWindowViewModel _mainWindowViewModel;
        public Thread AddFileThread;
        bool preventRepeat = false;
        bool isRefresh = false;
        #region 프로퍼티
        #region ---［ Pagging ］---------------------------------------------------------------------
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
        }
        private int _currentPage = 1;

        public int MaxPage
        {
            get { return _maxPage; }
            set { _maxPage = value; OnPropertyChanged("MaxPage"); }
        }
        private int _maxPage = 1;

        public int PagingSize
        {
            get { return _pagingSize; }
            set { _pagingSize = value; PageListExtract(""); OnPropertyChanged("PagingSize");  }
        }
        private int _pagingSize = 10;

        public int ThumbNailSize
        {
            get { return _thumbNailSize; }
            set { _thumbNailSize = value; PageListExtract(""); OnPropertyChanged("ThumbNailSize"); }
        }
        private int _thumbNailSize = 100;
        #endregion ---------------------------------------------------------------------------------
        public bool BoolListViewFold
        {
            get { return _boolListViewFold; }
            set { _boolListViewFold = value; OnPropertyChanged("BoolListViewFold"); }
        }
        private bool _boolListViewFold = true;

        public bool BoolSelectImageView
        {
            get { return _boolSelectImageView; }
            set { _boolSelectImageView = value; OnPropertyChanged("BoolSelectImageView"); }
        }
        private bool _boolSelectImageView = true;

        public Visibility OpenCloseVisibility
        {
            get { return _openCloseVisibility; }
            set { _openCloseVisibility = value; OnPropertyChanged("OpenCloseVisibility"); }
        }
        private Visibility _openCloseVisibility = Visibility.Visible;

        public Visibility ImageModeVisibility
        {
            get { return _imageModeVisibility; }
            set { _imageModeVisibility = value; OnPropertyChanged("ImageModeVisibility"); }
        }
        private Visibility _imageModeVisibility = Visibility.Visible;

        public int FileOpenSelectedIndex
        {
            get { return _fileOpenSelectedIndex; }
            set { _fileOpenSelectedIndex = value; OnPropertyChanged("FileOpenSelectedIndex"); }
        }
        private int _fileOpenSelectedIndex = 0;

        public GridLength GridSplitterLength
        {
            get { return _gridSplitterLength; }
            set { _gridSplitterLength = value; OnPropertyChanged("GridSplitterLength"); }
        }
        private GridLength _gridSplitterLength = GridLength.Auto;

        public ObservableCollection<string> FileOpenMenuList
        {
            get { return _fileOpenMenuList; }
            set { _fileOpenMenuList = value; OnPropertyChanged("FileOpenMenuList"); }
        }
        private ObservableCollection<string> _fileOpenMenuList = new ObservableCollection<string>();

        public int FileDelSelectedIndex
        {
            get { return _fileDelSelectedIndex; }
            set { _fileDelSelectedIndex = value; OnPropertyChanged("FileDelSelectedIndex"); }
        }
        private int _fileDelSelectedIndex = 0;
        public ObservableCollection<string> FileDelMenuList
        {
            get { return _fileDelMenuList; }
            set { _fileDelMenuList = value; OnPropertyChanged("FileDelMenuList"); }
        }
        private ObservableCollection<string> _fileDelMenuList = new ObservableCollection<string>();

        public ObservableCollection<PagingButtonModel> SelectNumPageList
        {
            get { return _selectNumPageList; }
            set { _selectNumPageList = value; OnPropertyChanged("SelectNumPageList"); }
        }
        private ObservableCollection<PagingButtonModel> _selectNumPageList = new ObservableCollection<PagingButtonModel>();

        public ObservableCollection<FileIOModel> LoadImageListAll
        {
            get { return _loadImageListAll; }
            set { _loadImageListAll = value; OnPropertyChanged("LoadImageListAll"); }
        }
        private ObservableCollection<FileIOModel> _loadImageListAll = new ObservableCollection<FileIOModel>();

        public ObservableCollection<FileIOModel> LoadImageListCurrent
        {
            get { return _loadImageListCurrent; }
            set { _loadImageListCurrent = value; OnPropertyChanged("LoadImageListCurrent"); }
        }
        private ObservableCollection<FileIOModel> _loadImageListCurrent = new ObservableCollection<FileIOModel>();

        public FileIOModel LastSelectedItem
        {
            get { return _lastSelectedItem; }
            set { _lastSelectedItem = value; OnPropertyChanged("LastSelectedItem"); }
        }
        private FileIOModel _lastSelectedItem;
        public string OpenCloseText
        {
            get { return _openCloseText; }
            set { _openCloseText = value; OnPropertyChanged("OpenCloseText"); }
        }
        private string _openCloseText = "◀";
        #endregion
        #region 커맨드
        public RelayCommand<object> CommandOpenClose { get; private set; }
        public RelayCommand<object> CommandSelectMode { get; private set; }
        public RelayCommand<object> CommandOpenMenu { get; private set; }
        public RelayCommand<object> CommandSetPage { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> CommandSelectImage { get; private set; }
        public RelayCommand<MouseEventArgs> ListBoxPreviewMouseDown { get; private set; }
        
        #endregion

        #region 초기화
        public ImageListViewModel()
        {
            InitData();
            InitCommand();
            InitEvent();
        }

        void InitData()
        {
            FileOpenMenuList.Add("Open File ");
            FileOpenMenuList.Add("Open Folder ");
            FileOpenMenuList.Add("Open Folder-All");

            FileDelMenuList.Add("Del Selected");
            FileDelMenuList.Add("Del All");

            SelectNumPageList.Add(new PagingButtonModel() { PageNum = "1", IsEnabled = false });
        }

        void InitCommand()
        {
            CommandOpenClose = new RelayCommand<object>((param) => OnCommandOpenClose(param));
            CommandSelectMode = new RelayCommand<object>((param) => OnCommandSelectMode(param));
            CommandOpenMenu = new RelayCommand<object>((param) => OnCommandOpenMenu(param));
            CommandSetPage = new RelayCommand<object>((param) => OnCommandSetPage(param));
            CommandSelectImage = new RelayCommand<SelectionChangedEventArgs>((e) => OnCommandSelectImage(e));

            ListBoxPreviewMouseDown= new RelayCommand<MouseEventArgs>((e) => OnListBoxPreviewMouseDown(e));
        }

        void InitEvent()
        {

        }
        #endregion

        #region 이벤트
        private void OnCommandOpenClose(object param)
        {

            BoolListViewFold = !BoolListViewFold;
            if (BoolListViewFold)
            {
                _mainWindowViewModel.GridSplitterLength = new GridLength(_mainWindowViewModel.GridSplitterPreViewLength);
                OpenCloseVisibility = Visibility.Visible;
                OpenCloseText = "◀";
            }
            else
            {
                //_mainWindowViewModel.GridSplitterPreViewLength = _mainWindowViewModel.GridSplitterLength.Value;
                OpenCloseVisibility = Visibility.Collapsed;
                OpenCloseText = "▶";
                _mainWindowViewModel.GridSplitterLength = new GridLength(1, GridUnitType.Auto);
                GridSplitterLength = new GridLength(1, GridUnitType.Star);

            }
        }
        private void OnCommandSelectMode(object param)
        {
            if (param == null)
            {
                return;
            }
            if (param.ToString() == "Image")
            {
                BoolSelectImageView = true;
                ImageModeVisibility = Visibility.Visible;

                _mainWindowViewModel.IsEnabled = false;
                _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;
                ObservableCollection<FileIOModel> NoThumbTemp = new ObservableCollection<FileIOModel>();
                for (int i = 0; i < LoadImageListCurrent.Count; i++)
                {
                    if (LoadImageListCurrent[i].ThumbnailBitmapImage.StreamSource == null)
                    {
                        NoThumbTemp.Add(LoadImageListCurrent[i]);
                    }
                }

                AddFileThread = new Thread(() => MakeThumbnailandList(NoThumbTemp));
                AddFileThread.Start();
            }
            else
            {
                BoolSelectImageView = false;

                ImageModeVisibility = Visibility.Collapsed;

            }
        }

        private void OnCommandOpenMenu(object param)
        {
            DirectoryInfo di;
            WPFFolderBrowser.WPFFolderBrowserDialog fbd;

            switch (FileOpenSelectedIndex)
            {
                case 0:
                    Microsoft.Win32.OpenFileDialog Dialog = new Microsoft.Win32.OpenFileDialog();
                    Dialog.DefaultExt = ".txt";
                    Dialog.Filter = "JPG Files (*.jpg),JPEG Files (*.jpeg), PNG Files (*.png), Bmp Files(*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*";
                    Dialog.Multiselect = true;
                    bool? result = Dialog.ShowDialog();

                    if (result == true)
                    {
                        LoadImageListCurrent.Clear();
                        SelectNumPageList.Clear();

                        AddFileThreadMethod(Dialog.FileNames);
                        PageListExtract("");

                        if (BoolSelectImageView)
                        {
                            _mainWindowViewModel.IsEnabled = false;
                            _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;
                            AddFileThread = new Thread(() => MakeThumbnailandList(LoadImageListCurrent));
                            AddFileThread.Start();
                        }
                    }
                    break;
                case 1:
                    fbd = new WPFFolderBrowser.WPFFolderBrowserDialog();
                    bool? resfolder = fbd.ShowDialog();

                    if (resfolder == true)
                    {
                        LoadImageListCurrent.Clear();
                        SelectNumPageList.Clear();
                        di = new DirectoryInfo(fbd.FileName);
                        AddFileThread = new Thread(() => AddFileThreadMethod(di.GetFiles()));
                        AddFileThread.Start();
                        AddFileThread.Join();
                        PageListExtract("");
                        ListNumRefresh();
                    }
                    break;
                case 2:
                    fbd = new WPFFolderBrowser.WPFFolderBrowserDialog();
                    bool? resfolder2 = fbd.ShowDialog();

                    if (resfolder2 == true)
                    {
                        LoadImageListCurrent.Clear();
                        SelectNumPageList.Clear();
                        di = new DirectoryInfo(fbd.FileName);
                        AddFileThread = new Thread(() => AddFileThreadMethod(di.GetFiles("*.*", SearchOption.AllDirectories)));
                        AddFileThread.Start();
                        AddFileThread.Join();
                        PageListExtract("");
                        ListNumRefresh();
                    }
                    break;
            }
        }
        public void MakeThumbnailandList(ObservableCollection<FileIOModel> TempList)
        {

            for (int iLoofCount = 0; iLoofCount < TempList.Count; iLoofCount++)
            {
                int index = iLoofCount;
                _mainWindowViewModel.ProgressLoadingViewModel.SetProgress(index, TempList.Count);

                Mat TempThumbnail = new Mat(TempList[index].FileName_Full);
                double width = TempThumbnail.Width;
                double height = TempThumbnail.Height;
                double ash = ThumbNailSize / height;
                double asw = ThumbNailSize / width;
                OpenCvSharp.Size sizeas;
                if (asw < ash)
                {
                    sizeas = new OpenCvSharp.Size((width * asw), (height * asw));
                }
                else
                {
                    sizeas = new OpenCvSharp.Size(width * ash, height * ash);
                }
                Mat TempResized = Mat.Zeros(ThumbNailSize, ThumbNailSize, MatType.CV_8UC3);
                TempThumbnail = TempThumbnail.Resize(sizeas);
                var roi = new Mat(TempResized, new OpenCvSharp.Rect((ThumbNailSize / 2) - (sizeas.Width / 2), (ThumbNailSize / 2) - (sizeas.Height / 2), sizeas.Width, sizeas.Height));
                TempThumbnail.CopyTo(roi);
                TempList[index].ThumbnailBitmapImage = SetThumbnailBitmap(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(TempResized));
                roi.Dispose();
                TempResized.Dispose();
                TempThumbnail.Dispose();
            }
            _mainWindowViewModel.IsEnabled = true;
            _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Collapsed;
        }
        BitmapImage SetThumbnailBitmap(Bitmap inputBitmap)
        {
            using (var memory = new MemoryStream())
            {
                inputBitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }

            //BitmapImage b = new BitmapImage();
            //b.UriSource = null;
            //var stream = File.OpenRead(ImagePath);
            //b.BeginInit();
            //b.CacheOption = BitmapCacheOption.OnLoad;
            //b.StreamSource = stream;
            //b.EndInit();
            //stream.Close();
            //stream.Dispose();

            //return b;
        }
        public void PageListExtract(string findPageCommand)
        {
            if (findPageCommand == "First")
            {
                CurrentPage = 1;
                //var k = LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize);
                LoadImageListCurrent = new ObservableCollection<FileIOModel>(LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize));
                if (BoolSelectImageView)
                {
                    _mainWindowViewModel.IsEnabled = false;
                    _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;

                    ObservableCollection<FileIOModel> NoThumbTemp = new ObservableCollection<FileIOModel>();
                    for (int i = 0; i < LoadImageListCurrent.Count; i++)
                    {
                        if (LoadImageListCurrent[i].ThumbnailBitmapImage.StreamSource == null)
                        {
                            NoThumbTemp.Add(LoadImageListCurrent[i]);
                        }
                    }

                    AddFileThread = new Thread(() => MakeThumbnailandList(NoThumbTemp));
                    AddFileThread.Start();
                }
                return;
            }

            if (findPageCommand == "Last")
            {
                CurrentPage = MaxPage;
                LoadImageListCurrent = new ObservableCollection<FileIOModel>(LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize));
                if (BoolSelectImageView)
                {
                    _mainWindowViewModel.IsEnabled = false;
                    _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;

                    ObservableCollection<FileIOModel> NoThumbTemp = new ObservableCollection<FileIOModel>();
                    for (int i = 0; i < LoadImageListCurrent.Count; i++)
                    {
                        if (LoadImageListCurrent[i].ThumbnailBitmapImage.StreamSource == null)
                        {
                            NoThumbTemp.Add(LoadImageListCurrent[i]);
                        }
                    }

                    AddFileThread = new Thread(() => MakeThumbnailandList(NoThumbTemp));
                    AddFileThread.Start();
                }
                return;
            }

            if (findPageCommand == "Next")
            {
                if (LoadImageListAll.Count < (CurrentPage) * PagingSize)
                {
                    if (BoolSelectImageView)
                    {
                        _mainWindowViewModel.IsEnabled = false;
                        _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;

                        ObservableCollection<FileIOModel> NoThumbTemp = new ObservableCollection<FileIOModel>();
                        for (int i = 0; i < LoadImageListCurrent.Count; i++)
                        {
                            if (LoadImageListCurrent[i].ThumbnailBitmapImage.StreamSource == null)
                            {
                                NoThumbTemp.Add(LoadImageListCurrent[i]);
                            }
                        }

                        AddFileThread = new Thread(() => MakeThumbnailandList(NoThumbTemp));
                        AddFileThread.Start();
                    }
                    return;
                }
            }
            else if (findPageCommand == "Back")
            {
                if (CurrentPage <= 1)
                {
                    if (BoolSelectImageView)
                    {
                        _mainWindowViewModel.IsEnabled = false;
                        _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;

                        ObservableCollection<FileIOModel> NoThumbTemp = new ObservableCollection<FileIOModel>();
                        for (int i = 0; i < LoadImageListCurrent.Count; i++)
                        {
                            if (LoadImageListCurrent[i].ThumbnailBitmapImage.StreamSource == null)
                            {
                                NoThumbTemp.Add(LoadImageListCurrent[i]);
                            }
                        }

                        AddFileThread = new Thread(() => MakeThumbnailandList(NoThumbTemp));
                        AddFileThread.Start();
                    }
                    return;
                }
            }

            if (findPageCommand == "Next")
            {
                CurrentPage++;

            }
            else if (findPageCommand == "Back")
            {
                CurrentPage--;
            }
            if (LoadImageListCurrent.Count > 0)
            {
                
                    LoadImageListCurrent.Clear();
                
            }

            LoadImageListCurrent = new ObservableCollection<FileIOModel>(LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize));

            ListNumRefresh();

            if (BoolSelectImageView)
            {
                _mainWindowViewModel.IsEnabled = false;
                _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;

                AddFileThread = new Thread(() => MakeThumbnailandList(LoadImageListCurrent));
                AddFileThread.Start();
            }

        }
        void ListNumRefresh()
        {
            
                SelectNumPageList.Clear();

            MaxPage =(int)Math.Ceiling(((double)LoadImageListAll.Count / (double)PagingSize));
            int TempPage = (((CurrentPage - 1) / 5) * 5) + 1;
            for (int iLoofCount = 0; iLoofCount < 5; iLoofCount++)
            {
                if ((TempPage + iLoofCount) <= MaxPage)
                {
                    SelectNumPageList.Add(new PagingButtonModel() { PageNum = (TempPage + iLoofCount).ToString(), IsEnabled = true });
                }
            }
            var a = from b in SelectNumPageList
                    where b.PageNum == (CurrentPage).ToString()
                    select b;
            if (a.Count() > 0)
            {
                int TempCount = SelectNumPageList.IndexOf(a.First());
                SelectNumPageList[TempCount].IsEnabled = false;
            }
        }

        public void AddFileThreadMethod(string[] files)
        {
            for (int iLoofCount = 0; iLoofCount < files.Count(); iLoofCount++)
            {
                FileAttributes attr = File.GetAttributes(files[iLoofCount]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    for (int iLoopCount = 0; iLoopCount < files.Count(); iLoopCount++)
                    {
                        DirectoryInfo di = new DirectoryInfo(files[iLoofCount]);
                        AddFileThreadMethod(di.GetFiles("*", SearchOption.AllDirectories));
                    }
                    return;
                }
                else
                {
                    FileIOModel TempFileIO = new FileIOModel(files[iLoofCount]);
                    if (TempFileIO.FileName_Extension.ToLower() == "jpg" || TempFileIO.FileName_Extension.ToLower() == "png" || TempFileIO.FileName_Extension.ToLower() == "bmp" || TempFileIO.FileName_Extension.ToLower() == "tif")
                    {
                        LoadImageListAll.Add(new FileIOModel(files[iLoofCount]));
                    }
                }
            }
            MaxPage = (LoadImageListAll.Count() / PagingSize) + 1;
        }
        private void AddFileThreadMethod(FileInfo[] files)
        {
            for (int iLoofCount = 0; iLoofCount < files.Count(); iLoofCount++)
            {
                if (files[iLoofCount].Extension != ".db")
                {
                    FileIOModel TempFileIO = new FileIOModel(files[iLoofCount].FullName);
                    if (TempFileIO.FileName_Extension.ToLower() == "jpg" || TempFileIO.FileName_Extension.ToLower() == "png" || TempFileIO.FileName_Extension.ToLower() == "bmp" || TempFileIO.FileName_Extension.ToLower() == "tif")
                    {
                        LoadImageListAll.Add(new FileIOModel(files[iLoofCount].FullName));
                    }
                    //LoadImageListAll.Add(new FileIOModel(files[iLoofCount].FullName));
                }

            }

            MaxPage = (LoadImageListAll.Count() / PagingSize) + 1;
            PageListExtract("");
            ListNumRefresh();
            _mainWindowViewModel.IsEnabled = true;
            _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Collapsed;
        }
        private void OnCommandSetPage(object param)
        {
            if (param.ToString() == "BackList")
            {
                if (CurrentPage - 5 <= 1)
                {
                    CurrentPage = 1;

                }
                else if (CurrentPage - 5 > 1)
                {
                    CurrentPage = CurrentPage - 5;
                }
            }

            if (param.ToString() == "NextList")
            {
                if (CurrentPage + 5 >= MaxPage)
                {
                    CurrentPage = MaxPage;

                }
                else if (CurrentPage + 5 < MaxPage)
                {
                    CurrentPage = CurrentPage + 5;
                }
            }

            int TargetPage = 0;
            if (int.TryParse(param.ToString(), out TargetPage))
            {
                if (TargetPage == 0)
                {
                    return;
                }
                CurrentPage = TargetPage;
            }
            PageListExtract(param.ToString());
            ListNumRefresh();

            if (BoolSelectImageView)
            {
                _mainWindowViewModel.IsEnabled = false;
                _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;

                ObservableCollection<FileIOModel> NoThumbTemp = new ObservableCollection<FileIOModel>();
                for (int i = 0; i < LoadImageListCurrent.Count; i++)
                {
                    if (LoadImageListCurrent[i].ThumbnailBitmapImage.StreamSource == null)
                    {
                        NoThumbTemp.Add(LoadImageListCurrent[i]);
                    }
                }

                AddFileThread = new Thread(() => MakeThumbnailandList(NoThumbTemp));
                AddFileThread.Start();
            }
        }
        private async void OnCommandSelectImage(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1) { return; }

            if(_mainWindowViewModel.ImageConverterViewModel.Visibility == Visibility.Visible) { LastSelectedItem = (e.AddedItems[0] as FileIOModel); _mainWindowViewModel.ImageConverterViewModel.UpdateImageInfo(LastSelectedItem.FileName_Full); }
            if (_mainWindowViewModel.SegmentationLabelViewModel.Visibility == Visibility.Visible)
            {
                if(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Strokes.Count > 0 || _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Count>2)
                {
                    if(preventRepeat)
                    {
                        preventRepeat = false;
                        return;
                    }
                    if(_mainWindowViewModel.SegmentationLabelViewModel.IsAutoClearOn)
                    {
                        _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Strokes.Clear();
                        LastSelectedItem = e.AddedItems[0] as FileIOModel;

                        _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Clear();

                        //reset
                        _mainWindowViewModel.SegmentationLabelViewModel.rectline = new System.Windows.Shapes.Rectangle();
                        _mainWindowViewModel.SegmentationLabelViewModel.rectline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(_mainWindowViewModel.SelectedColor.ToString());
                        _mainWindowViewModel.SegmentationLabelViewModel.rectline.StrokeThickness = _mainWindowViewModel.SegmentationLabelViewModel.PenThickness;
                        _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Add(_mainWindowViewModel.SegmentationLabelViewModel.rectline);
                        _mainWindowViewModel.SegmentationLabelViewModel.rectline.Visibility = Visibility.Collapsed;
                        _mainWindowViewModel.SegmentationLabelViewModel.ellipseline = new System.Windows.Shapes.Ellipse();
                        _mainWindowViewModel.SegmentationLabelViewModel.ellipseline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(_mainWindowViewModel.SelectedColor.ToString());
                        _mainWindowViewModel.SegmentationLabelViewModel.ellipseline.StrokeThickness = _mainWindowViewModel.SegmentationLabelViewModel.PenThickness;
                        _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Add(_mainWindowViewModel.SegmentationLabelViewModel.ellipseline);
                        _mainWindowViewModel.SegmentationLabelViewModel.ellipseline.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        var result = await _mainWindowViewModel.FooMessage("Label clear?", "If you click OK button, clear all label in previous image.", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);
                        if (result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative)
                        {
                            _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Strokes.Clear();
                            LastSelectedItem = e.AddedItems[0] as FileIOModel;

                            _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Clear();

                            //reset
                            _mainWindowViewModel.SegmentationLabelViewModel.rectline = new System.Windows.Shapes.Rectangle();
                            _mainWindowViewModel.SegmentationLabelViewModel.rectline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(_mainWindowViewModel.SelectedColor.ToString());
                            _mainWindowViewModel.SegmentationLabelViewModel.rectline.StrokeThickness = _mainWindowViewModel.SegmentationLabelViewModel.PenThickness;
                            _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Add(_mainWindowViewModel.SegmentationLabelViewModel.rectline);
                            _mainWindowViewModel.SegmentationLabelViewModel.rectline.Visibility = Visibility.Collapsed;
                            _mainWindowViewModel.SegmentationLabelViewModel.ellipseline = new System.Windows.Shapes.Ellipse();
                            _mainWindowViewModel.SegmentationLabelViewModel.ellipseline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(_mainWindowViewModel.SelectedColor.ToString());
                            _mainWindowViewModel.SegmentationLabelViewModel.ellipseline.StrokeThickness = _mainWindowViewModel.SegmentationLabelViewModel.PenThickness;
                            _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.Children.Add(_mainWindowViewModel.SegmentationLabelViewModel.ellipseline);
                            _mainWindowViewModel.SegmentationLabelViewModel.ellipseline.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            preventRepeat = true;
                            (e.Source as ListBox).SelectedItem = LastSelectedItem;
                            return;
                        }
                    }

                    
                }
                _mainWindowViewModel.SegmentationLabelViewModel.UpdateImageInfo((e.AddedItems[0] as FileIOModel).FileName_Full);
                LastSelectedItem = e.AddedItems[0] as FileIOModel;
            }

        }
        private void OnListBoxPreviewMouseDown(MouseEventArgs e)
        {
            if(LastSelectedItem==null)
            { return; }
            if (_mainWindowViewModel.ImageConverterViewModel.Visibility == Visibility.Visible)
            {
                (e.Source as ListBox).SelectedItem = null;
            }
            if (_mainWindowViewModel.SegmentationLabelViewModel.Visibility == Visibility.Visible)
            {
                (e.Source as ListBox).SelectedItem = null;
            }
        }
        /*
        private void OnMyCommand(object param)
            {
            }
            */
        #endregion

    }
}
