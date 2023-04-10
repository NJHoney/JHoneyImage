using CommunityToolkit.Mvvm.Input;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.OpenCV;
using JHoney_ImageConverter.ViewModel.Base;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JHoney_ImageConverter.ViewModel
{
    class ImageMuiltConvertViewModel : CustomViewModelBase
    {
        Thread thread;
        #region 프로퍼티
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set { _mainWindowViewModel = value; OnPropertyChanged("MainWindowViewModel"); }
        }
        private MainWindowViewModel _mainWindowViewModel;
        public ObservableCollection<WorkListModel> WorkList
        {
            get { return _workList; }
            set { _workList = value; OnPropertyChanged("WorkList"); }
        }
        private ObservableCollection<WorkListModel> _workList = new ObservableCollection<WorkListModel>();


        #region ---［ ParamSetting ］---------------------------------------------------------------------

        public bool ParamVisibility1
        {
            get { return _paramVisibility1; }
            set { _paramVisibility1 = value; OnPropertyChanged("ParamVisibility1"); }
        }
        private bool _paramVisibility1 = false;

        public bool ParamVisibility2
        {
            get { return _paramVisibility2; }
            set { _paramVisibility2 = value; OnPropertyChanged("ParamVisibility2"); }
        }
        private bool _paramVisibility2 = false;

        public bool ParamVisibility3
        {
            get { return _paramVisibility3; }
            set { _paramVisibility3 = value; OnPropertyChanged("ParamVisibility3"); }
        }
        private bool _paramVisibility3 = false;

        public bool ParamVisibility4
        {
            get { return _paramVisibility4; }
            set { _paramVisibility4 = value; OnPropertyChanged("ParamVisibility4"); }
        }
        private bool _paramVisibility4 = false;

        public string ParamText1
        {
            get { return _paramText1; }
            set { _paramText1 = value; OnPropertyChanged("ParamText1"); }
        }
        private string _paramText1 = "";

        public string ParamText2
        {
            get { return _paramText2; }
            set { _paramText2 = value; OnPropertyChanged("ParamText2"); }
        }
        private string _paramText2 = "";

        public string ParamText3
        {
            get { return _paramText3; }
            set { _paramText3 = value; OnPropertyChanged("ParamText3"); }
        }
        private string _paramText3 = "";

        public string ParamText4
        {
            get { return _paramText4; }
            set { _paramText4 = value; OnPropertyChanged("ParamText4"); }
        }
        private string _paramText4 = "";

        public string Param1
        {
            get { return _param1; }
            set { _param1 = value; OnPropertyChanged("Param1"); }
        }
        private string _param1 = "";

        public string Param2
        {
            get { return _param2; }
            set { _param2 = value; OnPropertyChanged("Param2"); }
        }
        private string _param2 = "";

        public string Param3
        {
            get { return _param3; }
            set { _param3 = value; OnPropertyChanged("Param3"); }
        }
        private string _param3 = "";

        public string Param4
        {
            get { return _param4; }
            set { _param4 = value; OnPropertyChanged("Param4"); }
        }
        private string _param4 = "";
        #endregion ---------------------------------------------------------------------------------
        public ObservableCollection<ImageConvertInfoModel> ConvertCommandList
        {
            get { return _convertCommandList; }
            set { _convertCommandList = value; OnPropertyChanged("ConvertCommandList"); }
        }
        private ObservableCollection<ImageConvertInfoModel> _convertCommandList = new ObservableCollection<ImageConvertInfoModel>();


        #endregion
        #region 커맨드
        public RelayCommand<object> CommandRun { get; private set; }
        public RelayCommand<object> CommandConvertSelect { get; private set; }
        public RelayCommand<object> AddCommand { get; private set; }
        public RelayCommand<object> DelCommand { get; private set; }

        #endregion

        #region 초기화
        public ImageMuiltConvertViewModel()
        {
            InitData();
            InitCommand();
            InitEvent();
        }

        void InitData()
        {
            WorkList.Add(new WorkListModel() { Header = "GrayScale", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Dilate", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Erode", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Reverse", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Binary", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "EdgeCanny", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Crop", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Resize", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Resize_Ratio", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "ChangeChannel", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "FindPattern", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "MergeImage", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Test", IsSelected = false });
        }

        void InitCommand()
        {
            CommandRun = new RelayCommand<object>((param) => OnCommandRun(param));
            CommandConvertSelect = new RelayCommand<object>((param) => OnCommandConvertSelect(param));
            AddCommand = new RelayCommand<object>((param) => OnAddCommand(param));
            DelCommand = new RelayCommand<object>((param) => OnDelCommand(param));
        }


        void InitEvent()
        {

        }
        #endregion

        #region 이벤트

        private void OnCommandRun(object param)
        {
            MainWindowViewModel.IsEnabled = false;
            MainWindowViewModel.ProgressLoadingViewModel.Visibility = System.Windows.Visibility.Visible;

            thread = new Thread(() => ThreadRun());
            thread.Start();

        }
        private void ThreadRun()
        {
            
            for (int i = 0; i < MainWindowViewModel.ImageListViewModel.LoadImageListAll.Count; i++)
            {
                MainWindowViewModel.ProgressLoadingViewModel.SetProgress(i + 1, MainWindowViewModel.ImageListViewModel.LoadImageListAll.Count);
                Mat temp = new Mat(MainWindowViewModel.ImageListViewModel.LoadImageListAll[i].FileName_Full);
                for (int j = 0; j < ConvertCommandList.Count; j++)
                {
                    switch (ConvertCommandList[j].ConvertCommandName)
                    {
                        case "GrayScale":
                            if (temp.Channels() == 1)
                            {
                                continue;
                            }
                            temp = MainWindowViewModel.ImageConverterViewModel._grayScale.imgToGrayscale(temp);
                            break;
                        case "Dilate":
                            temp = MainWindowViewModel.ImageConverterViewModel._erodeDilate.Dilate(temp);
                            break;
                        case "Erode":
                            temp = MainWindowViewModel.ImageConverterViewModel._erodeDilate.Erode(temp);
                            break;
                        case "Reverse":
                            temp = MainWindowViewModel.ImageConverterViewModel._reverse.ImgReverse(temp);
                            break;
                        case "Blue":
                            if (temp.Channels() < 3)
                            {
                                return;
                            }
                            temp = MainWindowViewModel.ImageConverterViewModel._colorExport.SingleChannelExport(temp, 0);
                            break;
                        case "Green":
                            if (temp.Channels() < 3)
                            {
                                return;
                            }
                            temp = MainWindowViewModel.ImageConverterViewModel._colorExport.SingleChannelExport(temp, 1);
                            break;
                        case "Red":
                            if (temp.Channels() < 3)
                            {
                                return;
                            }
                            temp = MainWindowViewModel.ImageConverterViewModel._colorExport.SingleChannelExport(temp, 2);
                            break;
                        case "Binary":
                            temp = MainWindowViewModel.ImageConverterViewModel._binary.imgTobinary(temp, int.Parse(ConvertCommandList[j].ParamList[0]), 255);
                            break;

                        case "Gaussian":
                            temp = MainWindowViewModel.ImageConverterViewModel._gaussianBlur.gaussianToImg(temp, int.Parse(ConvertCommandList[j].ParamList[0]));
                            break;

                        case "Canny":
                            temp = MainWindowViewModel.ImageConverterViewModel._cannyEdge.cannyToImage(temp, int.Parse(ConvertCommandList[j].ParamList[0]), int.Parse(ConvertCommandList[j].ParamList[1]));
                            break;

                        case "Median":
                            temp = MainWindowViewModel.ImageConverterViewModel._medianBlur.imgToMedian(temp, int.Parse(ConvertCommandList[j].ParamList[0]));
                            break;

                        case "Rotation":
                            temp = MainWindowViewModel.ImageConverterViewModel._rotate.RotateFromMat(temp, int.Parse(ConvertCommandList[j].ParamList[0]));
                            break;
                        case "Crop":
                            temp = temp.Clone(new OpenCvSharp.Rect(int.Parse(ConvertCommandList[j].ParamList[0]), int.Parse(ConvertCommandList[j].ParamList[1]), int.Parse(ConvertCommandList[j].ParamList[2]), int.Parse(ConvertCommandList[j].ParamList[3])));
                            break;
                        case "Resize":
                            temp = MainWindowViewModel.ImageConverterViewModel._resize.ResizeFromMat(temp, int.Parse(ConvertCommandList[j].ParamList[0]), int.Parse(ConvertCommandList[j].ParamList[1]));
                            break;
                        case "Resize_Ratio":
                            temp = MainWindowViewModel.ImageConverterViewModel._resize.Resize_RatioFromMat(temp, int.Parse(ConvertCommandList[j].ParamList[0]), int.Parse(ConvertCommandList[j].ParamList[1]), int.Parse(ConvertCommandList[j].ParamList[2]));
                            break;
                        default:
                            break;
                    }
                }
                temp.SaveImage(MainWindowViewModel.ImageListViewModel.LoadImageListAll[i].FileName_Full);
                temp.Dispose();

            }
            MainWindowViewModel.IsEnabled = true;
            MainWindowViewModel.ProgressLoadingViewModel.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void OnAddCommand(object param)
        {
            List<string> tempList = new List<string>() { Param1, Param2, Param3, Param4 };
            foreach (WorkListModel ICM in WorkList)
            {
                if (ICM.IsSelected)
                {
                    switch (ICM.Header)
                    {
                        case "GrayScale":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Dilate":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Erode":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Reverse":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Binary":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "EdgeCanny":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "EdgeSobel":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Crop":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "Resize":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "Resize_Ratio":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "ChangeChannel":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "FindPattern":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "MergeImage":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "Test":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                    }
                }
            }
        }

        private void OnDelCommand(object param)
        {
            if (param.ToString() == "-1")
            {
                if (ConvertCommandList.Count > 0)
                    ConvertCommandList.RemoveAt(0);
            }
            else
            {
                if (ConvertCommandList.Count > 0)
                    ConvertCommandList.RemoveAt(int.Parse(param.ToString()));
            }
        }

        private void OnCommandConvertSelect(object param)
        {

            switch (param.ToString())
            {
                case "GrayScale":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "Dilate":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "Erode":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "Reverse":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "Binary":
                    ParamText1 = "Threshold";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "EdgeCanny":
                    ParamText1 = "Threshold";
                    ParamText2 = "Threshold2";
                    ParamVisibility1 = true;
                    ParamVisibility2 = true;
                    ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "Crop":
                    ParamText1 = "Start-X";
                    ParamText2 = "Start-Y";
                    ParamText3 = "Width";
                    ParamText4 = "Height";
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = true;
                    break;
                case "Resize":
                    ParamText1 = "Width";
                    ParamText2 = "Height";
                    ParamVisibility1 = ParamVisibility2 = true;
                    ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "ChangeChannel":
                    ParamText1 = "Channel";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "FindPattern":
                    ParamText1 = "PatternImage-Path";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;
                case "MergeImage":
                    ParamText1 = "B Channel Path";
                    ParamText2 = "G Channel Path";
                    ParamText3 = "R Channel Path";
                    ParamText4 = "A Channel Path(Empty is disable)";
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = true;
                    break;
                case "Resize_Ratio":
                    ParamText1 = "Target Width";
                    ParamText2 = "Target Height";
                    ParamText3 = "Padding Value";
                    ParamVisibility1 = ParamVisibility2 =ParamVisibility3 = true;
                    ParamVisibility4 = false;
                    break;

                case "Test":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    break;

            }
        }
        #endregion

    }
}
