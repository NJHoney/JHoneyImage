using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.OpenCV;
using JHoney_ImageConverter.Util;
using JHoney_ImageConverter.ViewModel.Base;
using MahApps.Metro.Controls;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JHoney_ImageConverter.ViewModel
{
    class ImagePatternMatchViewModel : CustomViewModelBase
    {
        #region 프로퍼티
        UtilDataGrid _utilDataGrid = new UtilDataGrid();
        public MatchTemplate _matchTemplate = new MatchTemplate();
        public DataGrid ImageInfoDataGrid
        {
            get { return _imageInfoDataGrid; }
            set { _imageInfoDataGrid = value; RaisePropertyChanged("ImageInfoDataGrid"); }
        }
        private DataGrid _imageInfoDataGrid = new DataGrid();

        public TabControl MainTabControl
        {
            get { return _mainTabControl; }
            set { _mainTabControl = value; RaisePropertyChanged("MainTabControl"); }
        }
        private TabControl _mainTabControl = new TabControl();

        bool IsSelectedPattern = true;
        public SolidColorBrush PatternColor
        {
            get { return _patternColor; }
            set { _patternColor = value; RaisePropertyChanged("PatternColor"); }
        }
        private SolidColorBrush _patternColor = new SolidColorBrush(Colors.Red);
        public SolidColorBrush TargetColor
        {
            get { return _targetColor; }
            set { _targetColor = value; RaisePropertyChanged("TargetColor"); }
        }
        private SolidColorBrush _targetColor = new SolidColorBrush(Colors.Blue);

        #region ---［ Canvas ］---------------------------------------------------------------------
        string tempImgPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
        System.Windows.Point CurrentMousePoint
        {
            get { return _currentMousePoint; }
            set { _currentMousePoint = value; RaisePropertyChanged("CurrentMousePoint"); }
        }
        private System.Windows.Point _currentMousePoint = new System.Windows.Point();

        public ObservableCollection<ImageInfoModel> ImageInfoDataGridList
        {
            get { return _imageInfoDataGridList; }
            set { _imageInfoDataGridList = value; RaisePropertyChanged("ImageInfoDataGridList"); }
        }
        private ObservableCollection<ImageInfoModel> _imageInfoDataGridList = new ObservableCollection<ImageInfoModel>();

        public ImageInfoModel ImageInfoDataGridModel
        {
            get { return _imageInfoDataGridModel; }
            set { _imageInfoDataGridModel = value; RaisePropertyChanged("ImageInfoDataGridModel"); }
        }
        private ImageInfoModel _imageInfoDataGridModel = new ImageInfoModel();



        #region ---［ Pattern Canvas ］---------------------------------------------------------------------
        string PatterntempImg = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + "Patterntemp.png";
        Mat PatternTempMat;
        public Canvas PatternCanvasInfo { get; set; }
        public ImageControlModel PatternImageShow
        {
            get { return _patternImageShow; }
            set { _patternImageShow = value; RaisePropertyChanged("PatternImageShow"); }
        }
        private ImageControlModel _patternImageShow = new ImageControlModel();


        #endregion ---------------------------------------------------------------------------------

        #region ---［ Target Canvas ］---------------------------------------------------------------------
        string TargettempImg = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + "Targettemp.png";
        Mat TargetTempMat;
        public Canvas TargetCanvasInfo { get; set; }
        public ImageControlModel TargetImageShow
        {
            get { return _targetImageShow; }
            set { _targetImageShow = value; RaisePropertyChanged("TargetImageShow"); }
        }
        private ImageControlModel _targetImageShow = new ImageControlModel();


        #endregion ---------------------------------------------------------------------------------
        #region ---［ Result Canvas ］---------------------------------------------------------------------
        string ResulttempImg = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + "Resulttemp.png";
        Mat ResultTempMat;
        public Canvas ResultCanvasInfo { get; set; }
        public ImageControlModel ResultImageShow
        {
            get { return _resultImageShow; }
            set { _resultImageShow = value; RaisePropertyChanged("ResultImageShow"); }
        }
        private ImageControlModel _resultImageShow = new ImageControlModel();


        public ObservableCollection<PatternResultModel> ResultRectList
        {
            get { return _resultRectList; }
            set { _resultRectList = value; RaisePropertyChanged("ResultRectList"); }
        }
        private ObservableCollection<PatternResultModel> _resultRectList = new ObservableCollection<PatternResultModel>();

        public ObservableCollection<Rectangle> PrintResultRectList
        {
            get { return _printResultRectList; }
            set { _printResultRectList = value; RaisePropertyChanged("ResultRectList"); }
        }
        private ObservableCollection<Rectangle> _printResultRectList = new ObservableCollection<Rectangle>();

        public DataGrid ResultInfoDataGrid
        {
            get { return _resultInfoDataGrid; }
            set { _resultInfoDataGrid = value; RaisePropertyChanged("ResultInfoDataGrid"); }
        }
        private DataGrid _resultInfoDataGrid = new DataGrid();
        #endregion ---------------------------------------------------------------------------------

        #region ---［ ROI ］---------------------------------------------------------------------
        public bool IsSelectRectangle
        {
            get { return _isSelectRectangle; }
            set { _isSelectRectangle = value; RaisePropertyChanged("IsSelectRectangle"); }
        }
        private bool _isSelectRectangle = false;
        public bool IsStartRect
        {
            get { return _isStartRect; }
            set { _isStartRect = value; RaisePropertyChanged("IsStartRect"); }
        }
        private bool _isStartRect = false;
        public double StartRectPointX
        {
            get { return _startRectPointX; }
            set
            {
                _startRectPointX = value;
                RaisePropertyChanged("StartRectPointX");
            }
        }
        private double _startRectPointX = 0;

        public double StartRectPointY
        {
            get { return _startRectPointY; }
            set
            {
                _startRectPointY = value; RaisePropertyChanged("StartRectPointY");
            }
        }
        private double _startRectPointY = 0;

        public double EndRectPointX
        {
            get { return _endRectPointX; }
            set
            {
                _endRectPointX = value;
                RaisePropertyChanged("EndRectPointX");
            }
        }
        private double _endRectPointX = 0;

        public double EndRectPointY
        {
            get { return _endRectPointY; }
            set
            {
                _endRectPointY = value;
                RaisePropertyChanged("EndRectPointY");
            }
        }
        private double _endRectPointY = 0;

        public int RectWidth
        {
            get { return _rectWidth; }
            set { _rectWidth = value; RaisePropertyChanged("RectWidth"); }
        }
        private int _rectWidth = 0;

        public int RectHeight
        {
            get { return _rectHeight; }
            set { _rectHeight = value; RaisePropertyChanged("RectHeight"); }
        }
        private int _rectHeight = 0;

        public bool IsEndDrawRect
        {
            get { return _isEndDrawRect; }
            set { _isEndDrawRect = value; RaisePropertyChanged("IsEndDrawRect"); }
        }
        private bool _isEndDrawRect = false;
        System.Windows.Point StartMousePoint
        {
            get { return _startMousePoint; }
            set { _startMousePoint = value; RaisePropertyChanged("StartMousePoint"); }
        }
        private System.Windows.Point _startMousePoint = new System.Windows.Point();
        #endregion ---------------------------------------------------------------------------------
        #endregion ---------------------------------------------------------------------------------


        #endregion
        #region 커맨드
        //public RelayCommand<object> MyCommand { get; private set; }
        public RelayCommand<object> CommandLoaded { get; private set; }
        public RelayCommand<object> CommandMatch { get; private set; }
        public RelayCommand<object> SelectPannelCommand { get; private set; }

        #region ---［ CanvasEvent ］---------------------------------------------------------------------
        public KeyEventUtil ImageConvertViewModelKeyEvent
        {
            get { return _imageConvertViewModelKeyEvent; }
            set { _imageConvertViewModelKeyEvent = value; RaisePropertyChanged("ImageConvertViewModelKeyEvent"); }
        }
        private KeyEventUtil _imageConvertViewModelKeyEvent = new KeyEventUtil();

        public RelayCommand<MouseWheelEventArgs> CanvasEventMouseWheel { get; private set; }

        public RelayCommand<MouseEventArgs> CanvasEventMouseMove { get; private set; }

        public RelayCommand<MouseButtonEventArgs> CanvasEventMouseDown { get; private set; }
        public RelayCommand<MouseEventArgs> CanvasEventMouseUp { get; private set; }
        public RelayCommand<object> CanvasContext { get; private set; }

        public RelayCommand<object> CommandSelectDataGridResult { get; private set; }

        #endregion ---------------------------------------------------------------------------------
        #endregion

        #region 초기화
        public ImagePatternMatchViewModel()
        {
            InitData();
            InitCommand();
            InitEvent();
        }

        void InitData()
        {

        }

        void InitCommand()
        {
            CommandMatch = new RelayCommand<object>((param) => OnCommandMatch(param));
            CommandLoaded = new RelayCommand<object>((param) => OnCommandLoaded(param));
            SelectPannelCommand = new RelayCommand<object>((param) => OnSelectPannelCommand(param));

            CanvasEventMouseWheel = new RelayCommand<MouseWheelEventArgs>((param) => OnCanvasEventMouseWheel(param));
            CanvasEventMouseMove = new RelayCommand<MouseEventArgs>((param) => OnCanvasEventMouseMove(param));
            CanvasContext = new RelayCommand<object>((param) => OnCanvasContext(param));
            CanvasEventMouseDown = new RelayCommand<MouseButtonEventArgs>((e) => OnCanvasEventMouseDown(e));
            CanvasEventMouseUp = new RelayCommand<MouseEventArgs>((e) => OnCanvasEventMouseUp(e));

            CommandSelectDataGridResult = new RelayCommand<object>((param) => OnCommandSelectDataGridResult(param));
        }



        void InitEvent()
        {
            //받기(이벤트로 등록)
            Messenger.Default.Register<MessengerImageGetSet>(this, (msgData) =>
            {
                if (Visibility == System.Windows.Visibility.Visible)
                {
                    if (msgData.MessageId == "Selected")
                    {
                        switch (IsSelectedPattern)
                        {
                            case true:
                                File.Copy(msgData.MessageImagePath, PatterntempImg, true);
                                UpdateImageInfo(MainTabControl.SelectedIndex);

                                PatternImageShow.ImageSourceUpdate(PatterntempImg, "ImageBrush");
                                break;
                            case false:
                                File.Copy(msgData.MessageImagePath, TargettempImg, true);
                                UpdateImageInfo(MainTabControl.SelectedIndex);

                                TargetImageShow.ImageSourceUpdate(TargettempImg, "ImageBrush");
                                break;
                        }
                    }
                }
            });
        }
        #endregion

        #region 이벤트
        private void OnCommandLoaded(object param)
        {
            if (param.GetType().Name == "DataGrid")
            {
                DataGrid TempDataGrid = param as DataGrid;
                if (TempDataGrid.Name == "DataGrid1")
                {
                    ResultInfoDataGrid = TempDataGrid;
                    _utilDataGrid.SetDataGrid1(ResultInfoDataGrid, ResultRectList);
                }
                else if (TempDataGrid.Name == "DataGrid")
                {
                    ImageInfoDataGrid = TempDataGrid;
                    ImageInfoDataGrid.IsReadOnly = true;
                    ImageInfoDataGridList.Add(ImageInfoDataGridModel);
                    ImageInfoDataGrid.ItemsSource = ImageInfoDataGridList;
                }

            }
            if (param.GetType().Name == "Canvas")
            {
                Canvas TempCanvas = param as Canvas;
                if (TempCanvas.Name == "PatternCanvas")
                {
                    PatternCanvasInfo = TempCanvas;
                }
                else if (TempCanvas.Name == "TargetCanvas")
                {
                    TargetCanvasInfo = TempCanvas;
                }
                else if (TempCanvas.Name == "ResultCanvas")
                {
                    ResultCanvasInfo = TempCanvas;
                }
            }
            if (param.GetType().Name == "TabControl")
            {
                MainTabControl = param as TabControl;
            }

        }

        private void OnSelectPannelCommand(object param)
        {
            switch (param.ToString())
            {
                case "Pattern":
                    IsSelectedPattern = true;
                    PatternColor.Color = Colors.Red;
                    TargetColor.Color = Colors.Blue;
                    break;
                case "Target":
                    IsSelectedPattern = false;
                    PatternColor.Color = Colors.Blue;
                    TargetColor.Color = Colors.Red;
                    break;
            }
        }

        private void OnCanvasEventMouseWheel(MouseWheelEventArgs param)
        {
            if (param == null)
            {
                return;
            }
            int index = -1;
            Canvas TempCanvas = param.Source as Canvas;
            if (param.Source.GetType().Name == "Rectangle")
            {
                TempCanvas = new Canvas();
                TempCanvas.Name = "ResultCanvas";
            }
            if (TempCanvas == null)
            {
                return;
            }
            switch (TempCanvas.Name)
            {
                case "PatternCanvas":
                    if (ImageConvertViewModelKeyEvent.IsLeftCtrlDown)
                    {
                        if (param.Delta > 0)
                        {
                            PatternImageShow.ImageBrushScaleX *= 1.1;
                            PatternImageShow.ImageBrushScaleY *= 1.1;
                        }
                        else if (param.Delta < 0)
                        {
                            PatternImageShow.ImageBrushScaleX /= 1.1;
                            PatternImageShow.ImageBrushScaleY /= 1.1;
                        }
                    }
                    index = 0;
                    break;
                case "TargetCanvas":
                    if (ImageConvertViewModelKeyEvent.IsLeftCtrlDown)
                    {
                        if (param.Delta > 0)
                        {
                            TargetImageShow.ImageBrushScaleX *= 1.1;
                            TargetImageShow.ImageBrushScaleY *= 1.1;
                        }
                        else if (param.Delta < 0)
                        {
                            TargetImageShow.ImageBrushScaleX /= 1.1;
                            TargetImageShow.ImageBrushScaleY /= 1.1;
                        }
                    }
                    index = 1;
                    break;
                case "ResultCanvas":
                    if (ImageConvertViewModelKeyEvent.IsLeftCtrlDown)
                    {
                        if (param.Delta > 0)
                        {
                            ResultImageShow.ImageBrushScaleX *= 1.1;
                            ResultImageShow.ImageBrushScaleY *= 1.1;
                        }
                        else if (param.Delta < 0)
                        {
                            ResultImageShow.ImageBrushScaleX /= 1.1;
                            ResultImageShow.ImageBrushScaleY /= 1.1;
                        }
                    }
                    index = 2;
                    break;
            }



            UpdateImageInfo(index);
        }

        private void OnCanvasEventMouseMove(MouseEventArgs e)
        {
            switch (MainTabControl.SelectedIndex)
            {
                case 0:
                    CurrentMousePoint = e.GetPosition(PatternCanvasInfo);
                    break;
                case 1:
                    CurrentMousePoint = e.GetPosition(TargetCanvasInfo);
                    break;
            }

            CurrentMousePoint = DPICacl(CurrentMousePoint);
            ImageInfoDataGridModel.Mouse__X = (int)Convert.ToDouble(CurrentMousePoint.X);
            ImageInfoDataGridModel.Mouse__Y = (int)Convert.ToDouble(CurrentMousePoint.Y);

            switch (MainTabControl.SelectedIndex)
            {
                case 0:
                    if (PatternTempMat != null)
                    {
                        switch (PatternTempMat.Channels())
                        {
                            case 1:
                                ImageInfoDataGridModel.Channel__B = PatternTempMat.Get<byte>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X);
                                ImageInfoDataGridModel.Channel__G = ImageInfoDataGridModel.Channel__R = ImageInfoDataGridModel.Channel__A = 0;
                                break;
                            case 3:
                                ImageInfoDataGridModel.Channel__B = PatternTempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
                                ImageInfoDataGridModel.Channel__G = PatternTempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
                                ImageInfoDataGridModel.Channel__R = PatternTempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
                                ImageInfoDataGridModel.Channel__A = 0;
                                break;
                            case 4:
                                ImageInfoDataGridModel.Channel__B = PatternTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
                                ImageInfoDataGridModel.Channel__G = PatternTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
                                ImageInfoDataGridModel.Channel__R = PatternTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
                                ImageInfoDataGridModel.Channel__A = PatternTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[3];
                                break;
                        }
                    }
                    break;
                case 1:
                    if (TargetTempMat != null)
                    {
                        switch (TargetTempMat.Channels())
                        {
                            case 1:
                                ImageInfoDataGridModel.Channel__B = TargetTempMat.Get<byte>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X);
                                ImageInfoDataGridModel.Channel__G = ImageInfoDataGridModel.Channel__R = ImageInfoDataGridModel.Channel__A = 0;
                                break;
                            case 3:
                                ImageInfoDataGridModel.Channel__B = TargetTempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
                                ImageInfoDataGridModel.Channel__G = TargetTempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
                                ImageInfoDataGridModel.Channel__R = TargetTempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
                                ImageInfoDataGridModel.Channel__A = 0;
                                break;
                            case 4:
                                ImageInfoDataGridModel.Channel__B = TargetTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
                                ImageInfoDataGridModel.Channel__G = TargetTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
                                ImageInfoDataGridModel.Channel__R = TargetTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
                                ImageInfoDataGridModel.Channel__A = TargetTempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[3];
                                break;
                        }
                    }
                    break;
            }

            if (IsSelectRectangle)
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    return;
                }

                CurrentMousePoint = e.GetPosition(TargetCanvasInfo);
                CurrentMousePoint = DPICacl(CurrentMousePoint);
                if ((int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).X) < 0 || (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).Y) < 0 || (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).X) > (TargetCanvasInfo.ActualWidth * TargetImageShow.ImageBrush_XDPI / 96) || (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).Y) > (TargetCanvasInfo.ActualHeight * TargetImageShow.ImageBrush_YDPI / 96))
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                {
                    if (!IsStartRect)//마우스 다운을 하고 있는지 여부
                    {
                        return;
                    }
                    else
                    {
                        Mouse.OverrideCursor = Cursors.Cross;
                        CurrentMousePoint = e.GetPosition(TargetCanvasInfo);
                        UpdateRectPosition(e);

                        RectWidth = Math.Abs((int)Convert.ToDouble(EndRectPointX.ToString()) - (int)Convert.ToDouble(StartRectPointX.ToString()));
                        RectHeight = Math.Abs((int)Convert.ToDouble(EndRectPointY.ToString()) - (int)Convert.ToDouble(StartRectPointY.ToString()));
                    }
                }

            }

        }
        void UpdateRectPosition(MouseEventArgs e)
        {
            if (CurrentMousePoint.X >= StartMousePoint.X)
            {
                EndRectPointX = (int)Convert.ToDouble(CurrentMousePoint.X);
            }
            else if (CurrentMousePoint.X < StartMousePoint.X)
            {
                StartRectPointX = (int)Convert.ToDouble(CurrentMousePoint.X);
            }

            if (CurrentMousePoint.Y >= StartMousePoint.Y)
            {
                EndRectPointY = (int)Convert.ToDouble(CurrentMousePoint.Y);
            }
            else if (CurrentMousePoint.Y < StartMousePoint.Y)
            {
                StartRectPointY = (int)Convert.ToDouble(CurrentMousePoint.Y);
            }


        }
        private void OnCanvasEventMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.Source.GetType().Name != "Rectangle")
                {
                    return;
                }
                Rectangle TempList = (Rectangle)e.Source;

                var TempRectList = PrintResultRectList.Where(x => (x.RenderSize.Width == TempList.RenderSize.Width));
                if (TempRectList.Count() > 1)
                {
                    TempRectList = TempRectList.Where(x => (x.RenderSize.Height == TempList.RenderSize.Height));
                    if (TempRectList.Count() > 1)
                    {
                        TempRectList = TempRectList.Where(x => (x.RenderTransform.Value.OffsetX == TempList.RenderTransform.Value.OffsetX));
                        if (TempRectList.Count() > 1)
                        {
                            TempRectList = TempRectList.Where(x => (x.RenderTransform.Value.OffsetY == TempList.RenderTransform.Value.OffsetY));

                        }
                    }
                }
                int Count = 0;


                if (TempRectList.Count() <= 0)
                {
                    return;
                }
                else
                {
                    Count = PrintResultRectList.IndexOf(TempRectList.First());
                }
                for (int iLoofCount = 0; iLoofCount < PrintResultRectList.Count; iLoofCount++)
                {
                    PrintResultRectList[iLoofCount].Stroke = Brushes.Blue;
                }

                PrintResultRectList[Count].Stroke = Brushes.Red;
                ResultInfoDataGrid.SelectedIndex = Count;
            }
        }
        private void OnCanvasEventMouseUp(MouseEventArgs e)
        {
            if (!IsStartRect)
            {
                //시작을 안했으니 무시
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                IsStartRect = false;
                IsEndDrawRect = true;
            }
        }
        private void OnCanvasContext(object param)
        {
            MessengerMain msgData;
            MessengerImageGetSet msgData2;
            DateTime TimeNow = DateTime.Now;
            string TempName = TimeNow.Year +
                                TimeNow.Month +
                                TimeNow.Day + "-" +
                                TimeNow.Hour +
                                TimeNow.Minute +
                                TimeNow.Second +
                                TimeNow.Millisecond.ToString("D2");

            switch (param.ToString())
            {
                case "Save":
                    Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                    Dialog.DefaultExt = ".txt";
                    Dialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.*)|*.jpg|All Files (*.*)|*.*";
                    bool? result = Dialog.ShowDialog();

                    if (result == true)
                    {
                        TargetTempMat = new Mat(TargettempImg, ImreadModes.Unchanged);

                        if (ResultRectList.Count < 0)
                        {
                            TargetTempMat.SaveImage(Dialog.FileName);
                        }
                        else
                        {
                            Mat TempROIMat = TargetTempMat.Clone(new OpenCvSharp.Rect(DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.X), DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.Y), DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.Width), DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.Height)));
                            TempROIMat.SaveImage(Dialog.FileName);
                            TempROIMat.Dispose();
                        }
                        //보내기
                        string[] TempMessage2 = { "Save", Dialog.SafeFileName + " : Save complete" };
                        msgData = new MessengerMain("MessageBox", "", TempMessage2);
                        Messenger.Default.Send<MessengerMain>(msgData);
                    }

                    
                    break;

                case "ToList":
                    TargetTempMat = new Mat(TargettempImg, ImreadModes.Unchanged);
                    if (ResultRectList.Count<0)
                    {
                        TargetTempMat.SaveImage(tempImgPath + TempName + "_Target.png");
                    }
                    else
                    {
                        Mat TempROIMat = TargetTempMat.Clone(new OpenCvSharp.Rect(DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.X), DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.Y), DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.Width), DPICaclSingle(ResultRectList[ResultInfoDataGrid.SelectedIndex].RectInfo.Height)));
                        TempROIMat.SaveImage(tempImgPath + TempName + "_Target.png");
                        TempROIMat.Dispose();
                    }

                    //보내기
                    msgData2 = new MessengerImageGetSet("ToList", tempImgPath + TempName + "_Target.png");
                    Messenger.Default.Send<MessengerImageGetSet>(msgData2);
                    break;
            }


        }

        void UpdateImageInfo(int Index)
        {
            switch (Index)
            {
                case 0:
                    PatternTempMat = new Mat(PatterntempImg, ImreadModes.Unchanged);

                    ImageInfoDataGridModel.Image__Width = PatternTempMat.Width;
                    ImageInfoDataGridModel.Image__Height = PatternTempMat.Height;
                    ImageInfoDataGridModel.Channel = PatternTempMat.Channels();
                    ImageInfoDataGridModel.Scale = double.Parse(PatternImageShow.ImageBrushScaleX.ToString("F2"));
                    PatternImageShow.ImageSourceUpdate(PatterntempImg, "ImageBrush");
                    break;
                case 1:
                    TargetTempMat = new Mat(TargettempImg, ImreadModes.Unchanged);

                    ImageInfoDataGridModel.Image__Width = TargetTempMat.Width;
                    ImageInfoDataGridModel.Image__Height = TargetTempMat.Height;
                    ImageInfoDataGridModel.Channel = TargetTempMat.Channels();
                    ImageInfoDataGridModel.Scale = double.Parse(TargetImageShow.ImageBrushScaleX.ToString("F2"));
                    TargetImageShow.ImageSourceUpdate(TargettempImg, "ImageBrush");
                    break;
                case 2:
                    ResultTempMat = new Mat(TargettempImg, ImreadModes.Unchanged);

                    ImageInfoDataGridModel.Image__Width = ResultTempMat.Width;
                    ImageInfoDataGridModel.Image__Height = ResultTempMat.Height;
                    ImageInfoDataGridModel.Channel = ResultTempMat.Channels();
                    ImageInfoDataGridModel.Scale = double.Parse(TargetImageShow.ImageBrushScaleX.ToString("F2"));
                    TargetImageShow.ImageSourceUpdate(TargettempImg, "ImageBrush");
                    break;
            }

        }
        System.Windows.Point DPICacl(System.Windows.Point InputPoint)
        {
            System.Windows.Point DPICalcPoint = new System.Windows.Point(InputPoint.X * PatternImageShow.ImageBrush_XDPI / 96, InputPoint.Y * PatternImageShow.ImageBrush_YDPI / 96);
            return DPICalcPoint;
        }

        private void OnCommandMatch(object param)
        {
            //Test _test = new Test();
            //_test.TestApp();
            //_matchTemplate.DoMatchTemplateReturnROIList(TargettempImg,PatterntempImg,  1);
            ResultRectList.Clear();
            PrintResultRectList.Clear();
            ResultCanvasInfo.Children.Clear();
            ResultImageShow.ImageSourceUpdate(TargettempImg, "ImageBrush");
            ResultRectList = _matchTemplate.DoMatchTemplateReturnResultList(TargettempImg, PatterntempImg, 0.99);
            //ResultRectList.Add(_matchTemplate.DoMatchTemplateReturnROI(TargettempImg, PatterntempImg));
            _utilDataGrid.SetDataGrid1(ResultInfoDataGrid, ResultRectList);



            for (int iLoofCount = 0; iLoofCount < ResultRectList.Count; iLoofCount++)
            {
                TranslateTransform tg = new TranslateTransform(DPICaclSingleRev(ResultRectList[iLoofCount].RectInfo.X), DPICaclSingleRev(ResultRectList[iLoofCount].RectInfo.Y));

                Rectangle TempRect = new Rectangle() { Width = DPICaclSingleRev(ResultRectList[iLoofCount].RectInfo.Width), Height = DPICaclSingleRev(ResultRectList[iLoofCount].RectInfo.Height), Stroke = Brushes.Blue, StrokeThickness = 2, RenderTransform = tg, Fill = Brushes.Transparent, RenderTransformOrigin = new System.Windows.Point(0.5, 0.5) };

                ResultCanvasInfo.Children.Add(TempRect);
                PrintResultRectList.Add(TempRect);
            }
        }
        private void OnCommandSelectDataGridResult(object param)
        {
            if (ResultInfoDataGrid.SelectedItem == null)
                return;

            int count = ResultInfoDataGrid.SelectedIndex;

            if (count < 0)
                return;


            for (int iLoofCount = 0; iLoofCount < ResultRectList.Count; iLoofCount++)
            {
                PrintResultRectList[iLoofCount].Stroke = Brushes.Blue;
                ResultCanvasInfo.Children[iLoofCount] = PrintResultRectList[iLoofCount];
            }
            PrintResultRectList[count].Stroke = Brushes.Red;
            ResultCanvasInfo.Children[count] = PrintResultRectList[count];
        }
        int DPICaclSingle(double Param)
        {
            Param = Param * (TargetImageShow.ImageBrush_XDPI / 96);
            return (int)Param;
        }
        int DPICaclSingleRev(double Param)
        {
            Param = Param * (96 / TargetImageShow.ImageBrush_XDPI);
            return (int)Param;
        }
        #endregion

    }
}
