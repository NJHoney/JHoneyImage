using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.ViewModel.Base;
using MahApps.Metro.Controls;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace JHoney_ImageConverter.ViewModel
{
    class ImageListViewModel : CustomViewModelBase
    {

        #region 프로퍼티
        Thread AddFileThread;
        #region ---［ Pagging ］---------------------------------------------------------------------
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; RaisePropertyChanged("CurrentPage"); }
        }
        private int _currentPage = 1;

        public int MaxPage
        {
            get { return _maxPage; }
            set { _maxPage = value; RaisePropertyChanged("MaxPage"); }
        }
        private int _maxPage = 1;

        public int PagingSize
        {
            get { return _pagingSize; }
            set { _pagingSize = value; RaisePropertyChanged("PagingSize"); }
        }
        private int _pagingSize = 30;

        public ObservableCollection<PagingButtonModel> SelectNumPageList
        {
            get { return _selectNumPageList; }
            set { _selectNumPageList = value; RaisePropertyChanged("SelectNumPageList"); }
        }
        private ObservableCollection<PagingButtonModel> _selectNumPageList = new ObservableCollection<PagingButtonModel>();
        #endregion ---------------------------------------------------------------------------------

        private Object LockObject = new object();


        public ListBox ImageListBox
        {
            get { return _imageListBox; }
            set { _imageListBox = value; RaisePropertyChanged("ImageListBox"); }
        }
        private ListBox _imageListBox = new ListBox();
        public ObservableCollection<FileIOModel> LoadImageListAll
        {
            get { return _loadImageListAll; }
            set { _loadImageListAll = value; RaisePropertyChanged("LoadImageListAll"); }
        }
        private ObservableCollection<FileIOModel> _loadImageListAll = new ObservableCollection<FileIOModel>();

        public ObservableCollection<FileIOModel> LoadImageListCurrent
        {
            get { return _loadImageListCurrent; }
            set { _loadImageListCurrent = value; RaisePropertyChanged("LoadImageListCurrent"); }
        }
        private ObservableCollection<FileIOModel> _loadImageListCurrent = new ObservableCollection<FileIOModel>();

        string ThumbnailImgPath = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\Thumbnail\\";

        #region ---［ Menu ］---------------------------------------------------------------------

        public int FileOpenSelectedIndex
        {
            get { return _fileOpenSelectedIndex; }
            set { _fileOpenSelectedIndex = value; RaisePropertyChanged("FileOpenSelectedIndex"); }
        }
        private int _fileOpenSelectedIndex = 0;


        public ObservableCollection<string> FileOpenMenuList
        {
            get { return _fileOpenMenuList; }
            set { _fileOpenMenuList = value; RaisePropertyChanged("FileOpenMenuList"); }
        }
        private ObservableCollection<string> _fileOpenMenuList = new ObservableCollection<string>();

        public int FileDelSelectedIndex
        {
            get { return _fileDelSelectedIndex; }
            set { _fileDelSelectedIndex = value; RaisePropertyChanged("FileDelSelectedIndex"); }
        }
        private int _fileDelSelectedIndex = 0;
        public ObservableCollection<string> FileDelMenuList
        {
            get { return _fileDelMenuList; }
            set { _fileDelMenuList = value; RaisePropertyChanged("FileDelMenuList"); }
        }
        private ObservableCollection<string> _fileDelMenuList = new ObservableCollection<string>();
        #endregion ---------------------------------------------------------------------------------


        public Visibility OpenCloseVisibility
        {
            get { return _openCloseVisibility; }
            set { _openCloseVisibility = value; RaisePropertyChanged("OpenCloseVisibility"); }
        }
        private Visibility _openCloseVisibility = Visibility.Collapsed;
        public string OpenCloseText
        {
            get { return _openCloseText; }
            set { _openCloseText = value; RaisePropertyChanged("OpenCloseText"); }
        }
        private string _openCloseText = "▶";

        #endregion
        #region 커맨드
        public RelayCommand<object> CommandOpenMenu { get; private set; }
        public RelayCommand<object> CommandDelMenu { get; private set; }
        public RelayCommand<object> CommandOpenClose { get; private set; }
        public RelayCommand<object> CommandSetPage { get; private set; }
        public RelayCommand<object> CommandSelectImage { get; private set; }
        public RelayCommand<object> CommandLoaded { get; private set; }

        public RelayCommand<DragEventArgs> CommandDropFile { get; private set; }


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
            CommandOpenMenu = new RelayCommand<object>((param) => OnCommandOpenMenu(param));
            CommandDelMenu = new RelayCommand<object>((param) => OnCommandDelMenu(param));
            CommandOpenClose = new RelayCommand<object>((param) => OnCommandOpenClose(param));
            CommandSetPage = new RelayCommand<object>((param) => OnCommandSetPage(param));
            CommandSelectImage = new RelayCommand<object>((param) => OnCommandSelectImage(param));
            CommandLoaded = new RelayCommand<object>((param) => OnCommandLoaded(param));

            CommandDropFile = new RelayCommand<DragEventArgs>((e) => OnCommandDropFile(e));
        }



        void InitEvent()
        {
            //받기(이벤트로 등록)
            Messenger.Default.Register<MessengerImageGetSet>(this, (msgData) =>
                      {
                          if (msgData.MessageId == "ToList")
                          {
                              LoadImageListAll.Add(new FileIOModel(msgData.MessageImagePath));
                              PageListExtract("");
                              ListNumRefresh();
                          }
                      });
        }
        #endregion

        #region 이벤트
        private void OnCommandDropFile(DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            {
                LoadImageListCurrent.Clear();
                SelectNumPageList.Clear();
                MessengerMain msgData = new MessengerMain("ProgressLoading", "true", null, null);
                Messenger.Default.Send<MessengerMain>(msgData);

                AddFileThread = new Thread(() => AddFileThreadMethod(file));
                AddFileThread.Start();
                AddFileThread.Join();
                PageListExtract("");
                ListNumRefresh();


            }

        }

        private void OnCommandOpenMenu(object param)
        {
            MessengerMain msgData;
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
                        msgData = new MessengerMain("ProgressLoading", "true", null, null);
                        Messenger.Default.Send<MessengerMain>(msgData);
                        AddFileThread = new Thread(() => AddFileThreadMethod(Dialog.FileNames));
                        AddFileThread.Start();
                        AddFileThread.Join();
                        PageListExtract("");
                        ListNumRefresh();
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
        private void OnCommandDelMenu(object param)
        {
            switch (FileDelSelectedIndex)
            {
                case 0:
                    if (ImageListBox.SelectedItems.Count < 1)
                    {
                        if (LoadImageListCurrent.Count < 1)
                        {
                            return;
                        }
                        LoadImageListAll.RemoveAt(0);
                        LoadImageListCurrent.RemoveAt(0);
                    }
                    else
                    {
                        int LoofCount = ImageListBox.SelectedItems.Count;
                        for (int iLoofCount = 0; iLoofCount < LoofCount; iLoofCount++)
                        {
                            LoadImageListAll.Remove((FileIOModel)ImageListBox.SelectedItems[0]);
                            LoadImageListCurrent.Remove((FileIOModel)ImageListBox.SelectedItems[0]);
                        }
                    }
                    break;
                case 1:
                    LoadImageListCurrent.Clear();
                    LoadImageListAll.Clear();
                    CurrentPage = 1;
                    MaxPage = 1;
                    PageListExtract("");
                    ListNumRefresh();
                    break;
            }
        }
        private void OnCommandOpenClose(object param)
        {
            if (OpenCloseVisibility == Visibility.Collapsed)
            {
                OpenCloseVisibility = Visibility.Visible;
                OpenCloseText = "◀";
            }
            else if (OpenCloseVisibility == Visibility.Visible)
            {
                OpenCloseVisibility = Visibility.Collapsed;
                OpenCloseText = "▶";
            }
        }
        private void AddFileThreadMethod(string[] files)
        {
            BaseMessageData msgData;
            MessengerMain msgData2;
            for (int iLoofCount = 0; iLoofCount < files.Count(); iLoofCount++)
            {
                FileAttributes attr = File.GetAttributes(files[iLoofCount]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    for (int iLoopCount = 0; iLoopCount < files.Count(); iLoopCount++)
                    {
                        DirectoryInfo di = new DirectoryInfo(files[iLoofCount]);
                        AddFileThreadMethod(di.GetFiles("*",SearchOption.AllDirectories));
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
                


                //보내기
                string[] TempProgressParam = new string[] { (iLoofCount + 1).ToString(), files.Count().ToString(), "Add Image..." };
                msgData = new BaseMessageData("ProgressLoading", 3, TempProgressParam);
                Messenger.Default.Send<BaseMessageData>(msgData);
            }

            msgData2 = new MessengerMain("ProgressLoading", "false", null, null);
            Messenger.Default.Send<MessengerMain>(msgData2);

            MaxPage = (LoadImageListAll.Count() / PagingSize) + 1;
            PageListExtract("");
        }
        private void AddFileThreadMethod(FileInfo[] files)
        {
            BaseMessageData msgData;
            MessengerMain msgData2;
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
                //보내기
                string[] TempProgressParam = new string[] { (iLoofCount + 1).ToString(), files.Count().ToString(), "Add Image..." };
                msgData = new BaseMessageData("ProgressLoading", 3, TempProgressParam);
                Messenger.Default.Send<BaseMessageData>(msgData);
            }

            msgData2 = new MessengerMain("ProgressLoading", "false", null, null);
            Messenger.Default.Send<MessengerMain>(msgData2);

            MaxPage = (LoadImageListAll.Count() / PagingSize) + 1;
            PageListExtract("");
        }
        void PageListExtract(string findPageCommand)
        {
            if (findPageCommand == "First")
            {
                CurrentPage = 1;
                //var k = LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize);
                LoadImageListCurrent = new ObservableCollection<FileIOModel>(LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize));
                MakeThumbnailandList(LoadImageListCurrent);
                return;
            }

            if (findPageCommand == "Last")
            {
                CurrentPage = MaxPage;
                LoadImageListCurrent = new ObservableCollection<FileIOModel>(LoadImageListAll.Skip((CurrentPage - 1) * PagingSize).Take(PagingSize));
                MakeThumbnailandList(LoadImageListCurrent);
                return;
            }

            if (findPageCommand == "Next")
            {
                if (LoadImageListAll.Count < (CurrentPage) * PagingSize)
                {

                    return;
                }
            }
            else if (findPageCommand == "Back")
            {
                if (CurrentPage <= 1)
                {

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
            MakeThumbnailandList(LoadImageListCurrent);
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
        }
        void ListNumRefresh()
        {
            SelectNumPageList.Clear();
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

        private void OnCommandLoaded(object param)
        {
            if (param.GetType().Name == "ListBox")
            {
                ImageListBox = param as ListBox;
                ImageListBox.SelectionMode = SelectionMode.Extended;
            }



        }
        private void OnCommandSelectImage(object param)
        {
            if (ImageListBox.SelectedItems.Count == 1)
            {
                FileIOModel k = ImageListBox.SelectedItem as FileIOModel;
                //보내기
                MessengerImageGetSet msgData = new MessengerImageGetSet("Selected", k.FileName_Full);
                Messenger.Default.Send<MessengerImageGetSet>(msgData);
            }
        }

        void MakeThumbnailandList(ObservableCollection<FileIOModel> TempList)
        {
            DirectoryInfo di = new DirectoryInfo(ThumbnailImgPath);
            if (di.Exists == false)
            {
                di.Create();
            }
            for (int iLoofCount = 0; iLoofCount < TempList.Count; iLoofCount++)
            {
                Mat TempThumbnail = new Mat(TempList[iLoofCount].FileName_Full);
                TempThumbnail = TempThumbnail.Resize(new OpenCvSharp.Size(100, 100));
                TempThumbnail.ImWrite(ThumbnailImgPath + iLoofCount + ".png");

                TempList[iLoofCount].ThumbnailBitmapImage = SetThumbnailBitmap(ThumbnailImgPath + iLoofCount + ".png");

                TempThumbnail.Dispose();
            }



        }
        BitmapImage SetThumbnailBitmap(string ImagePath)
        {
            BitmapImage b = new BitmapImage();
            b.UriSource = null;
            var stream = File.OpenRead(ImagePath);
            b.BeginInit();
            b.CacheOption = BitmapCacheOption.OnLoad;
            b.StreamSource = stream;
            b.EndInit();
            stream.Close();
            stream.Dispose();

            return b;
        }
        #endregion

    }
}
