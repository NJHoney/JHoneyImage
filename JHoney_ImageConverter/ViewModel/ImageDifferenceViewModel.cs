using CommunityToolkit.Mvvm.Input;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.Util;
using JHoney_ImageConverter.ViewModel.Base;
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

namespace JHoney_ImageConverter.ViewModel
{
    class ImageDifferenceViewModel:CustomViewModelBase
    {
        #region 프로퍼티
        UtilDataGrid _utilDataGrid = new UtilDataGrid();
        public DataGrid ImageInfoDataGrid
        {
            get { return _imageInfoDataGrid; }
            set { _imageInfoDataGrid = value; OnPropertyChanged("ImageInfoDataGrid"); }
        }
        private DataGrid _imageInfoDataGrid = new DataGrid();

        public TabControl MainTabControl
        {
            get { return _mainTabControl; }
            set { _mainTabControl = value; OnPropertyChanged("MainTabControl"); }
        }
        private TabControl _mainTabControl = new TabControl();

        #region ---［ Canvas ］---------------------------------------------------------------------
        string tempImgPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
        System.Windows.Point CurrentMousePoint
        {
            get { return _currentMousePoint; }
            set { _currentMousePoint = value; OnPropertyChanged("CurrentMousePoint"); }
        }
        private System.Windows.Point _currentMousePoint = new System.Windows.Point();

        public ObservableCollection<ImageInfoModel> ImageInfoDataGridList
        {
            get { return _imageInfoDataGridList; }
            set { _imageInfoDataGridList = value; OnPropertyChanged("ImageInfoDataGridList"); }
        }
        private ObservableCollection<ImageInfoModel> _imageInfoDataGridList = new ObservableCollection<ImageInfoModel>();

        public ImageInfoModel ImageInfoDataGridModel
        {
            get { return _imageInfoDataGridModel; }
            set { _imageInfoDataGridModel = value; OnPropertyChanged("ImageInfoDataGridModel"); }
        }
        private ImageInfoModel _imageInfoDataGridModel = new ImageInfoModel();



        #region ---［ Pattern Canvas ］---------------------------------------------------------------------
        string PatterntempImg = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + "SrcImage.png";
        Mat PatternTempMat;
        public Canvas PatternCanvasInfo { get; set; }
        public ImageControlModel PatternImageShow
        {
            get { return _patternImageShow; }
            set { _patternImageShow = value; OnPropertyChanged("PatternImageShow"); }
        }
        private ImageControlModel _patternImageShow = new ImageControlModel();


        #endregion ---------------------------------------------------------------------------------

        #region ---［ Target Canvas ］---------------------------------------------------------------------
        string TargettempImg = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + "DstImage.png";
        Mat TargetTempMat;
        public Canvas TargetCanvasInfo { get; set; }
        public ImageControlModel TargetImageShow
        {
            get { return _targetImageShow; }
            set { _targetImageShow = value; OnPropertyChanged("TargetImageShow"); }
        }
        private ImageControlModel _targetImageShow = new ImageControlModel();


        #endregion ---------------------------------------------------------------------------------
        #region ---［ Result Canvas ］---------------------------------------------------------------------
        string ResulttempImg = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + "DifferentImage.png";
        Mat ResultTempMat = new Mat();
        public Canvas ResultCanvasInfo { get; set; }
        public ImageControlModel ResultImageShow
        {
            get { return _resultImageShow; }
            set { _resultImageShow = value; OnPropertyChanged("ResultImageShow"); }
        }
        private ImageControlModel _resultImageShow = new ImageControlModel();


        public ObservableCollection<OpenCvSharp.Rect> ResultRectList
        {
            get { return _resultRectList; }
            set { _resultRectList = value; OnPropertyChanged("ResultRectList"); }
        }
        private ObservableCollection<OpenCvSharp.Rect> _resultRectList = new ObservableCollection<OpenCvSharp.Rect>();

        public DataGrid ResultInfoDataGrid
        {
            get { return _resultInfoDataGrid; }
            set { _resultInfoDataGrid = value; OnPropertyChanged("ResultInfoDataGrid"); }
        }
        private DataGrid _resultInfoDataGrid = new DataGrid();
        #endregion ---------------------------------------------------------------------------------

        #region ---［ ROI ］---------------------------------------------------------------------
        public bool IsSelectRectangle
        {
            get { return _isSelectRectangle; }
            set { _isSelectRectangle = value; OnPropertyChanged("IsSelectRectangle"); }
        }
        private bool _isSelectRectangle = false;
        public bool IsStartRect
        {
            get { return _isStartRect; }
            set { _isStartRect = value; OnPropertyChanged("IsStartRect"); }
        }
        private bool _isStartRect = false;
        public double StartRectPointX
        {
            get { return _startRectPointX; }
            set
            {
                _startRectPointX = value;
                OnPropertyChanged("StartRectPointX");
            }
        }
        private double _startRectPointX = 0;

        public double StartRectPointY
        {
            get { return _startRectPointY; }
            set
            {
                _startRectPointY = value; OnPropertyChanged("StartRectPointY");
            }
        }
        private double _startRectPointY = 0;

        public double EndRectPointX
        {
            get { return _endRectPointX; }
            set
            {
                _endRectPointX = value;
                OnPropertyChanged("EndRectPointX");
            }
        }
        private double _endRectPointX = 0;

        public double EndRectPointY
        {
            get { return _endRectPointY; }
            set
            {
                _endRectPointY = value;
                OnPropertyChanged("EndRectPointY");
            }
        }
        private double _endRectPointY = 0;

        public int RectWidth
        {
            get { return _rectWidth; }
            set { _rectWidth = value; OnPropertyChanged("RectWidth"); }
        }
        private int _rectWidth = 0;

        public int RectHeight
        {
            get { return _rectHeight; }
            set { _rectHeight = value; OnPropertyChanged("RectHeight"); }
        }
        private int _rectHeight = 0;

        public bool IsEndDrawRect
        {
            get { return _isEndDrawRect; }
            set { _isEndDrawRect = value; OnPropertyChanged("IsEndDrawRect"); }
        }
        private bool _isEndDrawRect = false;
        System.Windows.Point StartMousePoint
        {
            get { return _startMousePoint; }
            set { _startMousePoint = value; OnPropertyChanged("StartMousePoint"); }
        }
        private System.Windows.Point _startMousePoint = new System.Windows.Point();
        #endregion ---------------------------------------------------------------------------------
        #endregion ---------------------------------------------------------------------------------
        bool IsSelectedPattern = true;
        public SolidColorBrush PatternColor
        {
            get { return _patternColor; }
            set { _patternColor = value; OnPropertyChanged("PatternColor"); }
        }
        private SolidColorBrush _patternColor = new SolidColorBrush(Colors.Red);
        public SolidColorBrush TargetColor
        {
            get { return _targetColor; }
            set { _targetColor = value; OnPropertyChanged("TargetColor"); }
        }
        private SolidColorBrush _targetColor = new SolidColorBrush(Colors.Blue);
        #endregion
        #region 커맨드
        public RelayCommand<object> CommandLoaded { get; private set; }
        public RelayCommand<object> CommandDifference { get; private set; }
        
        #region ---［ CanvasEvent ］---------------------------------------------------------------------
        public KeyEventUtil ImageConvertViewModelKeyEvent
        {
            get { return _imageConvertViewModelKeyEvent; }
            set { _imageConvertViewModelKeyEvent = value; OnPropertyChanged("ImageConvertViewModelKeyEvent"); }
        }
        private KeyEventUtil _imageConvertViewModelKeyEvent = new KeyEventUtil();

        public RelayCommand<MouseWheelEventArgs> CanvasEventMouseWheel { get; private set; }

        public RelayCommand<MouseEventArgs> CanvasEventMouseMove { get; private set; }

        public RelayCommand<MouseButtonEventArgs> CanvasEventMouseDown { get; private set; }
        public RelayCommand<MouseEventArgs> CanvasEventMouseUp { get; private set; }
        public RelayCommand<object> CanvasContext { get; private set; }
        #endregion ---------------------------------------------------------------------------------
        public RelayCommand<object> SelectPannelCommand { get; private set; }
        
        #endregion

        #region 초기화
        public ImageDifferenceViewModel()
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
            CommandLoaded = new RelayCommand<object>((param) => OnCommandLoaded(param));
            CanvasEventMouseWheel = new RelayCommand<MouseWheelEventArgs>((param) => OnCanvasEventMouseWheel(param));
            CanvasEventMouseMove = new RelayCommand<MouseEventArgs>((param) => OnCanvasEventMouseMove(param));
            CanvasContext = new RelayCommand<object>((param) => OnCanvasContext(param));
            CanvasEventMouseDown = new RelayCommand<MouseButtonEventArgs>((e) => OnCanvasEventMouseDown(e));
            CanvasEventMouseUp = new RelayCommand<MouseEventArgs>((e) => OnCanvasEventMouseUp(e));
            CommandDifference = new RelayCommand<object>((param) => OnCommandDifference(param));

            SelectPannelCommand = new RelayCommand<object>((param) => OnSelectPannelCommand(param));
        }

        
        void InitEvent()
        {
            
        }
        #endregion

        #region 이벤트
        private void OnSelectPannelCommand(object param)
        {
            switch (param.ToString())
            {
                case "Src":
                    IsSelectedPattern = true;
                    PatternColor.Color = Colors.Red;
                    TargetColor.Color = Colors.Blue;
                    break;
                case "Dst":
                    IsSelectedPattern = false;
                    PatternColor.Color = Colors.Blue;
                    TargetColor.Color = Colors.Red;
                    break;
            }
        }
        private void OnCommandLoaded(object param)
        {
            if (param.GetType().Name == "DataGrid")
            {
                DataGrid TempDataGrid = param as DataGrid;
                //if (TempDataGrid.Name == "DataGrid1")
                //{
                //    ResultInfoDataGrid = TempDataGrid;
                //    _utilDataGrid.SetDataGrid1(ResultInfoDataGrid, ResultRectList);
                //}
                if (TempDataGrid.Name == "DataGrid")
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
            if (IsSelectRectangle)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //마우스 왼쪽버튼이 눌렸을 경우
                    if (!IsStartRect)
                    {
                        //시작을 안했으니 시작점을 기록
                        CurrentMousePoint = DPICacl(CurrentMousePoint);
                        if ((int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).X) < 0 || (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).Y) < 0 || (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).X) > (int)Convert.ToDouble(TargetCanvasInfo.ActualWidth * TargetImageShow.ImageBrush_XDPI / 96) || (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).Y) > (int)Convert.ToDouble(TargetCanvasInfo.ActualHeight * TargetImageShow.ImageBrush_YDPI / 96))
                        {
                            IsStartRect = false;
                        }
                        else
                        {
                            //초기화
                            StartRectPointX = StartRectPointY = EndRectPointX = EndRectPointY = RectWidth = RectHeight = 0;
                            IsEndDrawRect = false;

                            StartRectPointX = (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).X);
                            StartRectPointY = (int)Convert.ToDouble(e.GetPosition(TargetCanvasInfo).Y);
                            StartMousePoint = e.GetPosition(TargetCanvasInfo);
                            EndRectPointX = StartRectPointX;
                            EndRectPointY = StartRectPointY;
                            IsStartRect = true;
                        }
                    }

                }
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
                        switch (MainTabControl.SelectedIndex)
                        {
                            case 0:
                                PatternTempMat = new Mat(PatterntempImg, ImreadModes.Unchanged);
                                PatternTempMat.SaveImage(Dialog.FileName);

                                break;

                            case 1:
                                TargetTempMat = new Mat(TargettempImg, ImreadModes.Unchanged);
                                if (IsSelectRectangle)
                                {

                                    if (RectWidth < 2 || RectHeight < 2)
                                    {
                                        TargetTempMat.SaveImage(Dialog.FileName);
                                    }
                                    else
                                    {
                                        Mat TempROIMat = TargetTempMat.Clone(new OpenCvSharp.Rect((int)StartRectPointX, (int)StartRectPointY, RectWidth, RectHeight));
                                        TempROIMat.SaveImage(Dialog.FileName);
                                        TempROIMat.Dispose();
                                    }

                                }
                                else
                                {
                                    TargetTempMat.SaveImage(Dialog.FileName);
                                }

                                
                                break;
                            case 2:
                                ResultTempMat = new Mat(PatterntempImg, ImreadModes.Unchanged);
                                ResultTempMat.SaveImage(Dialog.FileName);

                                
                                break;
                        }
                    }
                    break;
                case "ToList":
                    switch (MainTabControl.SelectedIndex)
                    {
                        case 0:
                            PatternTempMat.SaveImage(tempImgPath + TempName + "_Pattern.png");
                            
                            break;
                        case 1:
                            if (IsSelectRectangle)
                            {
                                if (RectWidth < 2 || RectHeight < 2)
                                {
                                    TargetTempMat.SaveImage(tempImgPath + TempName + "_Target.png");
                                }
                                else
                                {
                                    Mat TempROIMat = TargetTempMat.Clone(new OpenCvSharp.Rect((int)StartRectPointX, (int)StartRectPointY, RectWidth, RectHeight));
                                    TempROIMat.SaveImage(tempImgPath + TempName + "_Target.png");
                                    TempROIMat.Dispose();
                                }
                            }
                            else
                            {
                                TargetTempMat.SaveImage(tempImgPath + TempName + "_Target.png");
                            }
                            
                            break;
                        case 2:
                            ResultTempMat.SaveImage(tempImgPath + TempName + "_Different.png");
                            
                            break;
                    }
                    break;
            }
        }
        void UpdateImageInfo(int Index)
        {
            switch (Index)
            {
                case 0:
                    PatternTempMat = new Mat(PatterntempImg, ImreadModes.Unchanged);
                    if (PatternTempMat.Type().Channels == 512)
                    {
                        PatternTempMat.Dispose();
                        PatternTempMat = new Mat(PatterntempImg, ImreadModes.Grayscale);
                    }
                    ImageInfoDataGridModel.Image__Width = PatternTempMat.Width;
                    ImageInfoDataGridModel.Image__Height = PatternTempMat.Height;
                    ImageInfoDataGridModel.Channel = PatternTempMat.Channels();
                    ImageInfoDataGridModel.Scale = double.Parse(PatternImageShow.ImageBrushScaleX.ToString("F2"));
                    PatternImageShow.ImageSourceUpdate(PatterntempImg, "ImageBrush");
                    break;
                case 1:
                    TargetTempMat = new Mat(TargettempImg, ImreadModes.Unchanged);
                    if (TargetTempMat.Type().Channels == 512)
                    {
                        TargetTempMat.Dispose();
                        TargetTempMat = new Mat(TargettempImg, ImreadModes.Grayscale);
                    }
                    ImageInfoDataGridModel.Image__Width = TargetTempMat.Width;
                    ImageInfoDataGridModel.Image__Height = TargetTempMat.Height;
                    ImageInfoDataGridModel.Channel = TargetTempMat.Channels();
                    ImageInfoDataGridModel.Scale = double.Parse(TargetImageShow.ImageBrushScaleX.ToString("F2"));
                    TargetImageShow.ImageSourceUpdate(TargettempImg, "ImageBrush");
                    break;
                case 2:
                    ImageInfoDataGridModel.Image__Width = ResultTempMat.Width;
                    ImageInfoDataGridModel.Image__Height = ResultTempMat.Height;
                    ImageInfoDataGridModel.Channel = ResultTempMat.Channels();
                    ImageInfoDataGridModel.Scale = double.Parse(ResultImageShow.ImageBrushScaleX.ToString("F2"));
                    ResultImageShow.ImageSourceUpdate(ResulttempImg, "ImageBrush");
                    break;
            }

        }
        System.Windows.Point DPICacl(System.Windows.Point InputPoint)
        {
            System.Windows.Point DPICalcPoint = new System.Windows.Point(InputPoint.X * PatternImageShow.ImageBrush_XDPI / 96, InputPoint.Y * PatternImageShow.ImageBrush_YDPI / 96);
            return DPICalcPoint;
        }

        private void OnCommandDifference(object param)
        {
            if (PatternTempMat.Size() != TargetTempMat.Size())
            {
                
                return;
            }
            if(PatternTempMat.Channels()!= TargetTempMat.Channels())
            {
                
                return;
            }
            Cv2.Absdiff(PatternTempMat, TargetTempMat, ResultTempMat);
            ResultTempMat.SaveImage(ResulttempImg);
            UpdateImageInfo(2);
        }

        #endregion

    }
}
