using CommunityToolkit.Mvvm.Input;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.OpenCV;
using JHoney_ImageConverter.Util;
using JHoney_ImageConverter.ViewModel.Base;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.IO;
using System.Windows.Ink;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.AccessControl;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Web.Hosting;
using System.Security.Cryptography;

namespace JHoney_ImageConverter.ViewModel
{
    internal class SegmentationLabelViewModel : CustomViewModelBase
    {
        private System.Windows.Point _justlastMousePosition;
        private System.Windows.Point _lastMousePosition;
        private StrokeCollection _copystrokeCollection = new StrokeCollection();
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set { _mainWindowViewModel = value; OnPropertyChanged("MainWindowViewModel"); }
        }
        private MainWindowViewModel _mainWindowViewModel;


        public string TxtSaveFilePath
        {
            get { return _txtSaveFilePath; }
            set { _txtSaveFilePath = value; OnPropertyChanged("TxtSaveFilePath"); }
        }
        private string _txtSaveFilePath = "";

        public bool IsFillOn
        {
            get { return _isFillOn; }
            set { _isFillOn = value; OnPropertyChanged("IsFillOn"); }
        }
        private bool _isFillOn = true;
        public bool IsAutoClearOn
        {
            get { return _isAutoClearOn; }
            set { _isAutoClearOn = value; OnPropertyChanged("IsAutoClearOn"); }
        }
        private bool _isAutoClearOn = false;

        
        #region 프로퍼티

        #region ---［ OpenCV ］---------------------------------------------------------------------
        public GrayScale _grayScale = new GrayScale();
        public Reverse _reverse = new Reverse();
        public ImagePyramids _imagePyramids = new ImagePyramids();
        public ColorExport _colorExport = new ColorExport();
        public Binary _binary = new Binary();
        public GaussianBlur _gaussianBlur = new GaussianBlur();
        public CannyEdge _cannyEdge = new CannyEdge();
        public MedianBlur _medianBlur = new MedianBlur();
        public ErodeDilate _erodeDilate = new ErodeDilate();
        public Rotate _rotate = new Rotate();
        public EdgePair _edgePair = new EdgePair();
        public Crop _crop = new Crop();
        public ChaangeImageBits _chaangeImageBits = new ChaangeImageBits();
        public Resize _resize = new Resize();
        public Blob _blob = new Blob();
        #endregion ---------------------------------------------------------------------------------

        #region ---［ Canvas ］---------------------------------------------------------------------
        string tempImgPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
        string tempImg = AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + "temp.png";
        public Mat TempMat, TempConvertedMat;
        public Canvas CanvasInfo { get; set; }

        public InkCanvas InkCanvasInfo
        {
            get { return _inkCanvasInfo; }
            set { _inkCanvasInfo = value; OnPropertyChanged("InkCanvasInfo"); }
        }
        private InkCanvas _inkCanvasInfo = new InkCanvas();

        System.Windows.Point CurrentMousePoint
        {
            get { return _currentMousePoint; }
            set { _currentMousePoint = value; OnPropertyChanged("CurrentMousePoint"); }
        }
        private System.Windows.Point _currentMousePoint = new System.Windows.Point();
        public ImageControlModel ImageShow
        {
            get { return _imageShow; }
            set { _imageShow = value; OnPropertyChanged("ImageShow"); }
        }
        private ImageControlModel _imageShow = new ImageControlModel();

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
        #endregion ---------------------------------------------------------------------------------

        #region ---［ ROI ］---------------------------------------------------------------------

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

        //public bool IsStartDrawRect
        //{
        //    get { return _isStartDrawRect; }
        //    set { _isStartDrawRect = value; OnPropertyChanged("IsStartDrawRect"); }
        //}
        //private bool _isStartDrawRect = false;
        //public bool IsEndDrawRect
        //{
        //    get { return _isEndDrawRect; }
        //    set { _isEndDrawRect = value; OnPropertyChanged("IsEndDrawRect"); }
        //}
        //private bool _isEndDrawRect = false;
        System.Windows.Point StartMousePoint
        {
            get { return _startMousePoint; }
            set { _startMousePoint = value; OnPropertyChanged("StartMousePoint"); }
        }
        private System.Windows.Point _startMousePoint = new System.Windows.Point();
        #endregion ---------------------------------------------------------------------------------

        #region Polygon
        private Polyline polyline;
        private bool drawOnMove = false;
        private List<Polygon> polygons = new List<Polygon>();
        private bool IsPolygonMode = false;
        #endregion
        #region Rectangle
        public Rectangle rectline = new Rectangle();
        //private bool drawOnMove = false;
        //private List<Polygon> polygons = new List<Polygon>();
        private bool IsRectangleMode = false;
        #endregion
        #region Ellipse
        public Ellipse ellipseline = new Ellipse();
        //private bool drawOnMove = false;
        //private List<Polygon> polygons = new List<Polygon>();
        private bool IsEllipseMode = false;
        #endregion


        #region ---［ Option ］---------------------------------------------------------------------
        public bool IsToggled
        {
            get { return _isToggled; }
            set { _isToggled = value; OnPropertyChanged("IsToggled"); }
        }
        private bool _isToggled = false;


        public ObservableCollection<bool> TogleButtonEnabled
        {
            get { return _togleButtonEnabled; }
            set { _togleButtonEnabled = value; OnPropertyChanged("TogleButtonEnabled"); }
        }
        private ObservableCollection<bool> _togleButtonEnabled = new ObservableCollection<bool>();


        string OptionMode = "";

        public string OptionParamText_01
        {
            get { return _optionParamText_01; }
            set { _optionParamText_01 = value; OnPropertyChanged("OptionParamText_01"); }
        }
        private string _optionParamText_01 = "Threshold";

        public int OptionParamSlider_01_Min
        {
            get { return _optionParamSlider_01_Min; }
            set { _optionParamSlider_01_Min = value; OnPropertyChanged("OptionParamSlider_01_Min"); }
        }
        private int _optionParamSlider_01_Min = 0;

        public int OptionParamSlider_01_Max
        {
            get { return _optionParamSlider_01_Max; }
            set { _optionParamSlider_01_Max = value; OnPropertyChanged("OptionParamSlider_01_Max"); }
        }
        private int _optionParamSlider_01_Max = 255;

        #endregion ---------------------------------------------------------------------------------



        public int PenThickness
        {
            get { return _penThickness; }
            set
            {
                _penThickness = value;
                if (InkCanvasInfo.EditingMode == InkCanvasEditingMode.EraseByPoint)
                {
                    InkCanvasInfo.EraserShape = new RectangleStylusShape(value, value);
                    InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                    InkCanvasInfo.EditingMode = InkCanvasEditingMode.EraseByPoint;
                }
                InkCanvasInfo.DefaultDrawingAttributes.Width = value; InkCanvasInfo.DefaultDrawingAttributes.Height = value;

                OnPropertyChanged("PenThickness");
            }
        }
        private int _penThickness = 10;

        public DataGrid ImageInfoDataGrid
        {
            get { return _imageInfoDataGrid; }
            set { _imageInfoDataGrid = value; OnPropertyChanged("ImageInfoDataGrid"); }
        }
        private DataGrid _imageInfoDataGrid = new DataGrid();

        DrawingAttributes DrawingAttributes { get; set; }

        #endregion
        #region 커맨드
        public RelayCommand<object> CommandLoaded { get; private set; }
        public RelayCommand<object> CommandSingleConvert { get; private set; }
        //public RelayCommand<object> CanvasContext { get; private set; }


        #region ---［ CanvasEvent ］---------------------------------------------------------------------
        public KeyEventUtil ImageConvertViewModelKeyEvent
        {
            get { return _imageConvertViewModelKeyEvent; }
            set { _imageConvertViewModelKeyEvent = value; OnPropertyChanged("ImageConvertViewModelKeyEvent"); }
        }
        private KeyEventUtil _imageConvertViewModelKeyEvent = new KeyEventUtil();

        public RelayCommand<MouseWheelEventArgs> CanvasEventMouseWheel { get; private set; }

        public RelayCommand<MouseEventArgs> CanvasEventMouseMove { get; private set; }

        public RelayCommand<MouseEventArgs> CanvasEventPreviewMouseMove { get; private set; }


        public RelayCommand<MouseButtonEventArgs> CanvasEventPreviewMouseDown { get; private set; }
        public RelayCommand<MouseButtonEventArgs> CanvasEventPreviewMouseUp { get; private set; }
        public RelayCommand<object> CanvasEventSelectionResizing { get; private set; }

        public RelayCommand CopyCommand { get; private set; }
        public RelayCommand PasteCommand { get; private set; }
        public RelayCommand<DragEventArgs> CommandDropFile { get; private set; }
        #endregion ---------------------------------------------------------------------------------

        public RelayCommand<object> CommandChangeSliderValue { get; private set; }
        public RelayCommand<object> CommandSetColor { get; private set; }
        public RelayCommand<object> CommandToggle { get; private set; }

        public RelayCommand<object> CommandCropResize { get; private set; }

        public RelayCommand<object> CommandSelectEditingMode { get; private set; }

        public RelayCommand<object> CommandSaveLabelImage { get; private set; }

        public RelayCommand<object> CommandSetSavePath { get; private set; }

        #endregion

        #region 초기화
        public SegmentationLabelViewModel()
        {
            InitData();
            InitCommand();
            InitEvent();

        }

        void InitData()
        {
            for (int iLoofCount = 0; iLoofCount < 6; iLoofCount++)
            {
                TogleButtonEnabled.Add(true);
            }

        }

        void InitCommand()
        {
            CommandLoaded = new RelayCommand<object>((param) => OnCommandLoaded(param));
            CanvasEventMouseWheel = new RelayCommand<MouseWheelEventArgs>((param) => OnCanvasEventMouseWheel(param));
            CanvasEventMouseMove = new RelayCommand<MouseEventArgs>((param) => OnCanvasEventMouseMove(param));

            //CanvasContext = new RelayCommand<object>((param) => OnCanvasContext(param));
            CanvasEventPreviewMouseMove = new RelayCommand<MouseEventArgs>((e) => OnCanvasEventPreviewMouseMove(e));
            CanvasEventPreviewMouseDown = new RelayCommand<MouseButtonEventArgs>((e) => OnCanvasEventPreviewMouseDown(e));
            CanvasEventPreviewMouseUp = new RelayCommand<MouseButtonEventArgs>((e) => OnCanvasEventPreviewMouseUp(e));

            CopyCommand = new RelayCommand(() => ExecuteCopy());
            PasteCommand = new RelayCommand(() => ExecutePaste());

            CanvasEventSelectionResizing = new RelayCommand<object>((e) => OnCanvasEventSelectionResizing(e));

            CommandDropFile = new RelayCommand<DragEventArgs>((e) => OnCommandDropFile(e));


            CommandSetColor = new RelayCommand<object>((param) => OnCommandSetColor(param));
            CommandSelectEditingMode = new RelayCommand<object>((param) => OnCommandSelectEditingMode(param));
            CommandSaveLabelImage = new RelayCommand<object>((param) => OnCommandSaveLabelImage(param));
            CommandSetSavePath = new RelayCommand<object>((param) => OnCommandSetSavePath(param));
        }



        void InitEvent()
        {

        }
        #endregion

        #region 이벤트
        private void OnCommandLoaded(object param)
        {
            if (param.GetType().Name == "DataGrid")
            {
                ImageInfoDataGrid = param as DataGrid;
                ImageInfoDataGrid.IsReadOnly = true;
                ImageInfoDataGridList.Add(ImageInfoDataGridModel);
                ImageInfoDataGrid.ItemsSource = ImageInfoDataGridList;
            }
            if (param.GetType().Name == "Canvas")
            {
                CanvasInfo = param as Canvas;

            }
            if (param.GetType().Name == "InkCanvas")
            {
                InkCanvasInfo = param as InkCanvas;
                InkCanvasInfo.DefaultDrawingAttributes.Color = MainWindowViewModel.SelectedColor;
                InkCanvasInfo.DefaultDrawingAttributes.Width = PenThickness; InkCanvasInfo.DefaultDrawingAttributes.Height = PenThickness;
                InkCanvasInfo.EraserShape = new RectangleStylusShape(PenThickness, PenThickness);
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.Select;
                rectline.StrokeThickness = PenThickness;
                rectline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                InkCanvasInfo.Children.Add(rectline);
                rectline.Visibility = Visibility.Collapsed;

                ellipseline.StrokeThickness = PenThickness;
                ellipseline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                InkCanvasInfo.Children.Add(ellipseline);
                ellipseline.Visibility = Visibility.Collapsed;
            }


        }

        private void OnCommandDropFile(DragEventArgs e)
        {
            string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
            {
                if (file.Length > 1)
                {
                    return;
                }

                File.Copy(file[0], tempImg, true);
                UpdateImageInfo(file[0]);

                ImageShow.ImageSourceUpdate(tempImg, "ImageBrush");

            }

            DateTime TimeNow = DateTime.Now;
            string TempName = TimeNow.Year +
                                TimeNow.Month +
                                TimeNow.Day + "-" +
                                TimeNow.Hour +
                                TimeNow.Minute +
                                TimeNow.Second +
                                TimeNow.Millisecond.ToString("D2");
            //if (IsSelectRectangle)
            //{

            //    if (RectWidth < 2 || RectHeight < 2)
            //    {
            //        TempMat.SaveImage(tempImgPath + TempName + ".png");
            //    }
            //    else
            //    {
            //        Mat TempROIMat = TempMat.Clone(new OpenCvSharp.Rect(DPICaclSingle(StartRectPointX), DPICaclSingle(StartRectPointY), DPICaclSingle(RectWidth), DPICaclSingle(RectHeight)));
            //        TempROIMat.SaveImage(tempImgPath + TempName + ".png");
            //        TempROIMat.Dispose();
            //    }
            //}
            //else
            //{
            //    //TempMat.SaveImage(tempImgPath + TempName + ".png");
            //    //_mainWindowViewModel.ImageListViewModel.LoadImageListCurrent.Clear();
            //    //_mainWindowViewModel.ImageListViewModel.SelectNumPageList.Clear();

            //    //_mainWindowViewModel.ImageListViewModel.AddFileThreadMethod(new string[1] { tempImgPath + TempName + ".png" });
            //    //_mainWindowViewModel.ImageListViewModel.PageListExtract("");

            //    //if (_mainWindowViewModel.ImageListViewModel.BoolSelectImageView)
            //    //{
            //    //    _mainWindowViewModel.IsEnabled = false;
            //    //    _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;
            //    //    _mainWindowViewModel.ImageListViewModel.AddFileThread = new Thread(() => _mainWindowViewModel.ImageListViewModel.MakeThumbnailandList(_mainWindowViewModel.ImageListViewModel.LoadImageListCurrent));
            //    //    _mainWindowViewModel.ImageListViewModel.AddFileThread.Start();
            //    //}
            //}


        }
        private void OnCanvasEventPreviewMouseMove(MouseEventArgs e)
        {
            _justlastMousePosition = e.GetPosition(InkCanvasInfo);
            //Hand
            if ((e.Source as InkCanvas) != null)
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    ScrollViewer scrollViewer = ((((e.Source as InkCanvas).Parent as Border).Parent as DockPanel).Parent as Canvas).Parent as ScrollViewer;
                    double deltaX = e.GetPosition(InkCanvasInfo).X - _lastMousePosition.X;
                    double deltaY = e.GetPosition(InkCanvasInfo).Y - _lastMousePosition.Y;
                    _lastMousePosition = e.GetPosition(InkCanvasInfo);
                    //if ((e.OriginalSource as Canvas).Parent == null)
                    //{ return; }
                    scrollViewer.ScrollToHorizontalOffset((scrollViewer).HorizontalOffset - deltaX);
                    scrollViewer.ScrollToVerticalOffset((scrollViewer).VerticalOffset - deltaY);

                }
            }

        }
        private void OnCanvasEventPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _lastMousePosition = e.GetPosition(InkCanvasInfo);
            }
            if (IsPolygonMode)
            {
                if (e.OriginalSource is Ellipse)
                {
                    InkCanvasInfo.Children.Remove((Ellipse)e.OriginalSource);
                    InkCanvasInfo.Children.Remove(polyline);
                    Polygon tmpPolygon = new Polygon();
                    tmpPolygon.StrokeThickness = PenThickness;
                    tmpPolygon.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    tmpPolygon.Points = polyline.Points.Clone();
                    polyline.Points.Clear();

                    polygons.Add(tmpPolygon);
                    drawOnMove = false;
                    //rbDraw.IsChecked = false;
                    InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;

                    polyline = new Polyline();
                    polyline.Points = new PointCollection();
                    polyline.StrokeThickness = PenThickness;
                    polyline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    InkCanvasInfo.Children.Add(polyline);
                    if (IsFillOn)
                    { tmpPolygon.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }

                    InkCanvasInfo.Children.Add(tmpPolygon);
                    //tmpPolygon.MouseDown += Polygon_MouseUp;
                }
                else
                {
                    polyline.Points.Add(e.GetPosition(InkCanvasInfo));
                    polyline.Points = polyline.Points.Clone();

                    if (polyline.Points.Count == 1)
                    {
                        Ellipse el = new Ellipse();
                        el.Width = 10;
                        el.Height = 10;
                        el.Stroke = Brushes.Black;
                        el.StrokeThickness = 2;
                        if (IsFillOn) { el.Fill = new SolidColorBrush { Color = Colors.Yellow }; }

                        el.Margin =
                            new Thickness(left: polyline.Points[0].X - el.Width / 2, top: polyline.Points[0].Y - el.Height / 2, right: 0, bottom: 0);
                        InkCanvasInfo.Children.Add(el);
                    }

                    drawOnMove = true;
                }
            }
            if (IsRectangleMode)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //마우스 왼쪽버튼이 눌렸을 경우
                    if (!IsStartRect)
                    {
                        //시작을 안했으니 시작점을 기록
                        CurrentMousePoint = DPICacl(CurrentMousePoint);
                        if ((int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) < 0 ||
                            (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) < 0 ||
                            (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) > (int)Convert.ToDouble(InkCanvasInfo.ActualWidth * (96 / ImageShow.ImageBrush_XDPI)) ||
                            (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) > (int)Convert.ToDouble(InkCanvasInfo.ActualHeight * (96 / ImageShow.ImageBrush_YDPI)))
                        {
                            IsStartRect = false;

                        }
                        else
                        {
                            //초기화
                            StartRectPointX = StartRectPointY = EndRectPointX = EndRectPointY = RectWidth = RectHeight = 0;
                            //IsEndDrawRect = false;

                            StartRectPointX = (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X);
                            StartRectPointY = (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y);
                            StartMousePoint = e.GetPosition(InkCanvasInfo);
                            EndRectPointX = StartRectPointX;
                            EndRectPointY = StartRectPointY;
                            IsStartRect = true;
                            rectline.Visibility = Visibility.Visible;

                            UpdateRectPosition(e, rectline);
                        }
                    }

                    else
                    {


                        Rectangle tmpRect = (Rectangle)XamlReader.Parse(XamlWriter.Save(rectline));
                        tmpRect.StrokeThickness = PenThickness;
                        tmpRect.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                        InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                        if (IsFillOn)
                        { tmpRect.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }

                        InkCanvasInfo.Children.Add(tmpRect);
                        rectline.Visibility = Visibility.Collapsed;
                        IsStartRect = false;

                    }

                }

            }

            if (IsEllipseMode)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //마우스 왼쪽버튼이 눌렸을 경우
                    if (!IsStartRect)
                    {
                        //시작을 안했으니 시작점을 기록
                        CurrentMousePoint = DPICacl(CurrentMousePoint);
                        if ((int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) < 0 ||
                            (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) < 0 ||
                            (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) > (int)Convert.ToDouble(InkCanvasInfo.ActualWidth * (96 / ImageShow.ImageBrush_XDPI)) ||
                            (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) > (int)Convert.ToDouble(InkCanvasInfo.ActualHeight * (96 / ImageShow.ImageBrush_YDPI)))
                        {
                            IsStartRect = false;

                        }
                        else
                        {
                            //초기화
                            StartRectPointX = StartRectPointY = EndRectPointX = EndRectPointY = RectWidth = RectHeight = 0;
                            //IsEndDrawRect = false;

                            StartRectPointX = (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X);
                            StartRectPointY = (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y);
                            StartMousePoint = e.GetPosition(InkCanvasInfo);
                            EndRectPointX = StartRectPointX;
                            EndRectPointY = StartRectPointY;
                            IsStartRect = true;
                            ellipseline.Visibility = Visibility.Visible;

                            UpdateRectPosition(e, ellipseline);
                        }
                    }

                    else
                    {


                        Ellipse tmpellipse = (Ellipse)XamlReader.Parse(XamlWriter.Save(ellipseline));
                        tmpellipse.StrokeThickness = PenThickness;
                        tmpellipse.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                        InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                        if (IsFillOn)
                        { tmpellipse.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }

                        InkCanvasInfo.Children.Add(tmpellipse);
                        ellipseline.Visibility = Visibility.Collapsed;
                        IsStartRect = false;

                    }

                }

            }
        }

        private void OnCanvasEventPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Released)
            {
                _lastMousePosition = e.GetPosition(InkCanvasInfo);
            }

        }

        private void OnCanvasEventMouseWheel(MouseWheelEventArgs param)
        {
            if (ImageShow.ImageBrush.ImageSource == null)
            {
                return;
            }

            if (ImageConvertViewModelKeyEvent.IsLeftCtrlDown)
            {
                if (param.Delta > 0)
                {
                    ImageShow.ImageBrushScaleX *= 1.1;
                    ImageShow.ImageBrushScaleY *= 1.1;
                }
                else if (param.Delta < 0)
                {
                    ImageShow.ImageBrushScaleX /= 1.1;
                    ImageShow.ImageBrushScaleY /= 1.1;
                }
            }
            ImageInfoDataGridModel.Scale = double.Parse(ImageShow.ImageBrushScaleX.ToString("F2"));
            //UpdateImageInfo();
        }

        private void OnCanvasEventMouseMove(MouseEventArgs e)
        {
            CurrentMousePoint = e.GetPosition(CanvasInfo);
            CurrentMousePoint = DPICacl(CurrentMousePoint);
            ImageInfoDataGridModel.Mouse__X = (int)Convert.ToDouble(CurrentMousePoint.X);
            ImageInfoDataGridModel.Mouse__Y = (int)Convert.ToDouble(CurrentMousePoint.Y);


            //Polygon
            if (IsPolygonMode)
            {
                if (e.OriginalSource is Ellipse)
                {
                    if (polyline.Points.Count < 5)
                    {
                        return;
                    }
                    InkCanvasInfo.Children.Remove((Ellipse)e.OriginalSource);
                    InkCanvasInfo.Children.Remove(polyline);
                    Polygon tmpPolygon = new Polygon();
                    tmpPolygon.StrokeThickness = PenThickness;
                    tmpPolygon.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    tmpPolygon.Points = polyline.Points.Clone();

                    polyline.Points.Clear();
                    polygons.Add(tmpPolygon);
                    drawOnMove = false;
                    //rbDraw.IsChecked = false;
                    InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                    polyline = new Polyline();
                    polyline.Points = new PointCollection();
                    polyline.StrokeThickness = PenThickness;
                    polyline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    InkCanvasInfo.Children.Add(polyline);

                    if (IsFillOn) { tmpPolygon.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }


                    double left = -1, top = -1, right = -1, bottom = -1;
                    for (int i = 0; i < tmpPolygon.Points.Count; i++)
                    {
                        if (left == -1)
                        {
                            left = tmpPolygon.Points[i].X;
                        }
                        if (left > tmpPolygon.Points[i].X)
                        {
                            left = tmpPolygon.Points[i].X;
                        }
                        if (top == -1)
                        {
                            top = tmpPolygon.Points[i].Y;
                        }
                        if (top > tmpPolygon.Points[i].Y)
                        {
                            top = tmpPolygon.Points[i].Y;
                        }

                        if (right == -1)
                        {
                            right = tmpPolygon.Points[i].X;
                        }
                        if (right < tmpPolygon.Points[i].X)
                        {
                            right = tmpPolygon.Points[i].X;
                        }
                        if (bottom == -1)
                        {
                            bottom = tmpPolygon.Points[i].Y;
                        }
                        if (bottom < tmpPolygon.Points[i].Y)
                        {
                            bottom = tmpPolygon.Points[i].Y;
                        }
                    }
                    for (int i = 0; i < tmpPolygon.Points.Count; i++)
                    {
                        tmpPolygon.Points[i] = new System.Windows.Point(tmpPolygon.Points[i].X - left, tmpPolygon.Points[i].Y - top);
                    }


                    InkCanvas.SetLeft(tmpPolygon, left);
                    InkCanvas.SetTop(tmpPolygon, top);
                    //tmpPolygon.SetValue(InkCanvas.LeftProperty, -left);
                    //tmpPolygon.SetValue(InkCanvas.TopProperty, -top);
                    //tmpPolygon.SetValue(InkCanvas.BottomProperty, -bottom / 2);
                    //tmpPolygon.SetValue(InkCanvas.RightProperty, -right / 2);
                    InkCanvasInfo.Children.Add(tmpPolygon);



                    //tmpPolygon.MouseDown += Polygon_MouseUp;
                }
                if (drawOnMove)
                {
                    polyline.Points = polyline.Points.Clone();
                    polyline.Points.Add(e.GetPosition(InkCanvasInfo));
                }

            }


            //if (TempMat != null)
            //{
            //    switch (TempMat.Channels())
            //    {
            //        case 1:
            //            ImageInfoDataGridModel.Channel__B = TempMat.Get<byte>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X);
            //            ImageInfoDataGridModel.Channel__G = ImageInfoDataGridModel.Channel__R = ImageInfoDataGridModel.Channel__A = 0;
            //            break;
            //        case 3:
            //            ImageInfoDataGridModel.Channel__B = TempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
            //            ImageInfoDataGridModel.Channel__G = TempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
            //            ImageInfoDataGridModel.Channel__R = TempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
            //            ImageInfoDataGridModel.Channel__A = 0;
            //            break;
            //        case 4:
            //            ImageInfoDataGridModel.Channel__B = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
            //            ImageInfoDataGridModel.Channel__G = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
            //            ImageInfoDataGridModel.Channel__R = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
            //            ImageInfoDataGridModel.Channel__A = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[3];
            //            break;
            //    }
            //}
            if (IsRectangleMode)
            {

                CurrentMousePoint = e.GetPosition(InkCanvasInfo);
                CurrentMousePoint = DPICacl(CurrentMousePoint);
                if ((int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) < 0 ||
                    (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) < 0 ||
                    (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) > (int)Convert.ToDouble(InkCanvasInfo.ActualWidth * (96 / ImageShow.ImageBrush_XDPI)) ||
                    (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) > (int)Convert.ToDouble(InkCanvasInfo.ActualHeight * (96 / ImageShow.ImageBrush_YDPI)))
                {
                    //Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                {
                    if (IsStartRect)
                    {
                        CurrentMousePoint = e.GetPosition(InkCanvasInfo);
                        UpdateRectPosition(e, rectline);
                    }
                }
            }

            if (IsEllipseMode)
            {

                CurrentMousePoint = e.GetPosition(InkCanvasInfo);
                CurrentMousePoint = DPICacl(CurrentMousePoint);
                if ((int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) < 0 ||
                    (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) < 0 ||
                    (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).X) > (int)Convert.ToDouble(InkCanvasInfo.ActualWidth * (96 / ImageShow.ImageBrush_XDPI)) ||
                    (int)Convert.ToDouble(e.GetPosition(InkCanvasInfo).Y) > (int)Convert.ToDouble(InkCanvasInfo.ActualHeight * (96 / ImageShow.ImageBrush_YDPI)))
                {
                    //Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                {
                    if (IsStartRect)
                    {
                        CurrentMousePoint = e.GetPosition(InkCanvasInfo);
                        UpdateRectPosition(e, ellipseline);
                    }

                }

            }
        }



        void UpdateRectPosition(MouseEventArgs e, Shape shape)
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
            InkCanvas.SetLeft(shape, StartRectPointX);
            InkCanvas.SetTop(shape, StartRectPointY);
            shape.Width = EndRectPointX - StartRectPointX;
            shape.Height = EndRectPointY - StartRectPointY;
        }


        //private void OnCanvasContext(object param)
        //{
        //    switch (param.ToString())
        //    {
        //        case "Save":
        //            Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
        //            Dialog.DefaultExt = ".txt";
        //            Dialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.*)|*.jpg|All Files (*.*)|*.*";
        //            bool? result = Dialog.ShowDialog();

        //            if (result == true)
        //            {
        //                TempMat = new Mat(tempImg, ImreadModes.Unchanged);
        //                if (IsSelectRectangle)
        //                {

        //                    if (RectWidth < 2 || RectHeight < 2)
        //                    {
        //                        TempMat.SaveImage(Dialog.FileName);
        //                    }
        //                    else
        //                    {
        //                        Mat TempROIMat = TempMat.Clone(new OpenCvSharp.Rect((int)StartRectPointX, (int)StartRectPointY, RectWidth, RectHeight));
        //                        TempROIMat.SaveImage(Dialog.FileName);
        //                        TempROIMat.Dispose();
        //                    }

        //                }
        //                else
        //                {
        //                    TempMat.SaveImage(Dialog.FileName);
        //                }

        //            }
        //            break;
        //        case "ToList":
        //            DateTime TimeNow = DateTime.Now;
        //            string TempName = TimeNow.Year +
        //                                TimeNow.Month +
        //                                TimeNow.Day + "-" +
        //                                TimeNow.Hour +
        //                                TimeNow.Minute +
        //                                TimeNow.Second +
        //                                TimeNow.Millisecond.ToString("D2");
        //            if (IsSelectRectangle)
        //            {

        //                if (RectWidth < 2 || RectHeight < 2)
        //                {
        //                    TempMat.SaveImage(tempImgPath + TempName + ".png");
        //                }
        //                else
        //                {
        //                    Mat TempROIMat = TempMat.Clone(new OpenCvSharp.Rect(DPICaclSingle(StartRectPointX), DPICaclSingle(StartRectPointY), DPICaclSingle(RectWidth), DPICaclSingle(RectHeight)));
        //                    TempROIMat.SaveImage(tempImgPath + TempName + ".png");
        //                    TempROIMat.Dispose();
        //                    _mainWindowViewModel.ImageListViewModel.LoadImageListCurrent.Clear();
        //                    _mainWindowViewModel.ImageListViewModel.SelectNumPageList.Clear();

        //                    _mainWindowViewModel.ImageListViewModel.AddFileThreadMethod(new string[1] { tempImgPath + TempName + ".png" });
        //                    _mainWindowViewModel.ImageListViewModel.PageListExtract("");

        //                    if (_mainWindowViewModel.ImageListViewModel.BoolSelectImageView)
        //                    {
        //                        _mainWindowViewModel.IsEnabled = false;
        //                        _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;
        //                        _mainWindowViewModel.ImageListViewModel.AddFileThread = new Thread(() => _mainWindowViewModel.ImageListViewModel.MakeThumbnailandList(_mainWindowViewModel.ImageListViewModel.LoadImageListCurrent));
        //                        _mainWindowViewModel.ImageListViewModel.AddFileThread.Start();
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                TempMat.SaveImage(tempImgPath + TempName + ".png");
        //                _mainWindowViewModel.ImageListViewModel.LoadImageListCurrent.Clear();
        //                _mainWindowViewModel.ImageListViewModel.SelectNumPageList.Clear();

        //                _mainWindowViewModel.ImageListViewModel.AddFileThreadMethod(new string[1] { tempImgPath + TempName + ".png" });
        //                _mainWindowViewModel.ImageListViewModel.PageListExtract("");

        //                if (_mainWindowViewModel.ImageListViewModel.BoolSelectImageView)
        //                {
        //                    _mainWindowViewModel.IsEnabled = false;
        //                    _mainWindowViewModel.ProgressLoadingViewModel.Visibility = Visibility.Visible;
        //                    _mainWindowViewModel.ImageListViewModel.AddFileThread = new Thread(() => _mainWindowViewModel.ImageListViewModel.MakeThumbnailandList(_mainWindowViewModel.ImageListViewModel.LoadImageListCurrent));
        //                    _mainWindowViewModel.ImageListViewModel.AddFileThread.Start();
        //                }
        //            }


        //            break;
        //    }
        //}

        private void OnCommandSetColor(object param)
        {
            _mainWindowViewModel.IsOpenColorPicker = true;
        }


        public void UpdateImageInfo(string imagePath)
        {
            TempMat = new Mat(imagePath, ImreadModes.Unchanged);

            if (TempMat.Type().Channels == 512)
            {
                TempMat.Dispose();
                TempMat = new Mat(tempImg, ImreadModes.Grayscale);
            }

            ImageInfoDataGridModel.Image__Width = TempMat.Width;
            ImageInfoDataGridModel.Image__Height = TempMat.Height;
            ImageInfoDataGridModel.Channel = TempMat.Channels();
            ImageInfoDataGridModel.Scale = double.Parse(ImageShow.ImageBrushScaleX.ToString("F2"));
            ImageShow.ImageSourceUpdate(imagePath, "ImageBrush");
        }

        public void UpdateImageInfo(Mat targetImage)
        {
            ImageInfoDataGridModel.Image__Width = targetImage.Width;
            ImageInfoDataGridModel.Image__Height = targetImage.Height;
            ImageInfoDataGridModel.Channel = targetImage.Channels();
            ImageInfoDataGridModel.Scale = double.Parse(ImageShow.ImageBrushScaleX.ToString("F2"));
            ImageShow.ImageSourceUpdate(targetImage, "ImageBrush");
        }

        System.Windows.Point DPICacl(System.Windows.Point InputPoint)
        {
            System.Windows.Point DPICalcPoint = new System.Windows.Point(InputPoint.X * ImageShow.ImageBrush_XDPI / 96, InputPoint.Y * ImageShow.ImageBrush_YDPI / 96);
            return DPICalcPoint;
        }
        int DPICaclSingle(double Param)
        {
            Param = Param * (ImageShow.ImageBrush_XDPI / 96);
            return (int)Param;
        }

        private void OnCommandSelectEditingMode(object param)
        {
            if (param == null) { return; }

            IsPolygonMode = IsRectangleMode = IsStartRect = IsEllipseMode = false;
            rectline.Visibility = Visibility.Collapsed;
            ellipseline.Visibility = Visibility.Collapsed;

            if (param.ToString() == "Select")
            {
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.Select;
            }

            if (param.ToString() == "Pen")
            {
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.Ink;
            }
            if (param.ToString() == "Eraser")
            {
                InkCanvasInfo.EraserShape = new RectangleStylusShape(PenThickness, PenThickness);
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }

            if (param.ToString() == "Eraser_line")
            {
                InkCanvasInfo.EraserShape = new RectangleStylusShape(PenThickness, PenThickness);
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }

            if (param.ToString() == "Square")
            {
                IsRectangleMode = true;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
            }
            if (param.ToString() == "Ellipse")
            {
                IsEllipseMode = true;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
            }

            if (param.ToString() == "Polygon")
            {
                IsPolygonMode = true;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;

                polyline = new Polyline();
                polyline.Points = new PointCollection();
                polyline.StrokeThickness = PenThickness;
                polyline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                InkCanvasInfo.Children.Add(polyline);
            }
        }

        public void UpdateColor()
        {
            foreach (var item in InkCanvasInfo.Strokes)
            {
                item.DrawingAttributes.Color = MainWindowViewModel.SelectedColor;
            }

            foreach (var item in InkCanvasInfo.Children)
            {
                if ((item as Shape) != null)
                {
                    (item as Shape).Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());

                    if (IsFillOn) { (item as Shape).Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }

                }
            }

        }
        #endregion
        private void OnCommandSetSavePath(object param)
        {

            WPFFolderBrowser.WPFFolderBrowserDialog fbd = new WPFFolderBrowser.WPFFolderBrowserDialog();
            bool? resfolder = fbd.ShowDialog();

            if (resfolder == true)
            {
                TxtSaveFilePath = fbd.FileName;
            }


        }
        private void OnCommandSaveLabelImage(object param)
        {
            var preMode = InkCanvasInfo.EditingMode;
            InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
            if (TxtSaveFilePath == "")
            {
                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                Dialog.DefaultExt = ".txt";
                Dialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                bool? result = Dialog.ShowDialog();

                if (result == true)
                {
                    if (Dialog.FileName != "")
                    {


                        Thread.Sleep(10);

                        System.Windows.Rect bounds = VisualTreeHelper.GetDescendantBounds(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo);
                        double dpi = 96d;

                        RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);
                        DrawingVisual dv = new DrawingVisual();
                        using (DrawingContext dc = dv.RenderOpen())
                        {
                            VisualBrush vb = new VisualBrush(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo);
                            dc.DrawRectangle(vb, null, new System.Windows.Rect(new System.Windows.Point(), bounds.Size));
                        }
                        rtb.Render(dv);

                        BitmapEncoder pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            pngEncoder.Save(ms);
                            System.IO.File.WriteAllBytes(Dialog.FileName, ms.ToArray());
                            Mat tempMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(new System.Drawing.Bitmap(ms));
                            Mat[] tempMatChannel = tempMat.Split();
                            Cv2.Threshold(tempMatChannel[3], tempMatChannel[3], 1, 255, ThresholdTypes.Binary);
                            tempMatChannel[3] = ~tempMatChannel[3];
                            tempMatChannel[3].SaveImage(Dialog.FileName);
                            tempMat.Dispose();
                        }
                    }
                }
            }
            else
            {
                string savePath = TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_Safe;

                Thread.Sleep(10);

                System.Windows.Rect bounds = VisualTreeHelper.GetDescendantBounds(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo);
                double dpi = 96d;

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo);
                    dc.DrawRectangle(vb, null, new System.Windows.Rect(new System.Windows.Point(), bounds.Size));
                }
                rtb.Render(dv);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    pngEncoder.Save(ms);
                    System.IO.File.WriteAllBytes(savePath, ms.ToArray());
                    Mat tempMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(new System.Drawing.Bitmap(ms));
                    Mat[] tempMatChannel = tempMat.Split();
                    Cv2.Threshold(tempMatChannel[3], tempMatChannel[3], 1, 255, ThresholdTypes.Binary);
                    tempMatChannel[3] = ~tempMatChannel[3];
                    tempMatChannel[3].SaveImage(savePath);
                    tempMat.Dispose();
                }
            }


            InkCanvasInfo.EditingMode = preMode;
        }

        private void OnCanvasEventSelectionResizing(object e)
        {
            var selectedItem = InkCanvasInfo.GetSelectedElements()[0];
            if (selectedItem.GetType() == typeof(Polygon))
            {
                //(e as InkCanvasSelectionEditingEventArgs).OldRectangle
                //(e as InkCanvasSelectionEditingEventArgs).
                //bool xDirection = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.X >= (e as InkCanvasSelectionEditingEventArgs).OldRectangle.X;
                //bool yDirection = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Y >= (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Y;

                double widthDelta = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Width - (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width;
                double heightDelta = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Height - (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height;
                for (int i = 0; i < (selectedItem as Polygon).Points.Count; i++)
                {
                    double xPoint = 0, yPoint = 0;
                    //if (xDirection) { xPoint= (selectedItem as Polygon).Points[i].X + ((selectedItem as Polygon).Points[i].X / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width) * widthDelta; }
                    //else { xPoint = (selectedItem as Polygon).Points[i].X + ((selectedItem as Polygon).Points[i].X / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width) * widthDelta; }
                    //if(yDirection) { yPoint = (selectedItem as Polygon).Points[i].Y + ((selectedItem as Polygon).Points[i].Y / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height) * heightDelta; }
                    //else { yPoint = (selectedItem as Polygon).Points[i].Y + ((selectedItem as Polygon).Points[i].Y / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height) * heightDelta; }

                    xPoint = (selectedItem as Polygon).Points[i].X + ((selectedItem as Polygon).Points[i].X / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width) * widthDelta;
                    yPoint = (selectedItem as Polygon).Points[i].Y + ((selectedItem as Polygon).Points[i].Y / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height) * heightDelta;

                    (selectedItem as Polygon).Points[i] = new System.Windows.Point(xPoint, yPoint);
                }

                //(selectedItem as Polygon).Points
                //selectedItem.RenderSize = new System.Windows.Size((e as InkCanvasSelectionEditingEventArgs).NewRectangle.Width, (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Height);
            }
        }

        private void ExecuteCopy()
        {
            //InkCanvasInfo.CopySelection();
            List<string> copyObject = new List<string>();
            for (int i = 0; i < InkCanvasInfo.GetSelectedElements().Count; i++)
            {
                copyObject.Add(XamlWriter.Save(InkCanvasInfo.GetSelectedElements()[i]));
            }
            if (copyObject.Count > 0) { Clipboard.SetDataObject(copyObject); }
            if (InkCanvasInfo.GetSelectedStrokes().Count > 0) { InkCanvasInfo.CopySelection(); }//_copystrokeCollection = InkCanvasInfo.GetSelectedStrokes().Clone(); }
            else { if (_copystrokeCollection != null) { _copystrokeCollection.Clear(); } }
            




        }
        private void ExecutePaste()
        {
            //for (int i = 0; i < _copystrokeCollection.Count; i++)
            {
                InkCanvasInfo.Paste();
                //_copystrokeCollection[i].Transform(new Matrix(1,0,0,1,_lastMousePosition.X, _lastMousePosition.Y),true);   
                //InkCanvasInfo.Strokes.Add(_copystrokeCollection[i]);
            }
            IDataObject CanExecute = Clipboard.GetDataObject();

            for (int i = 0; i < CanExecute.GetFormats().Length; i++)
            {
                List<string> data = CanExecute.GetData(CanExecute.GetFormats()[i]) as List<string>;
                if (data == null) { return; }

                for (int j = 0; j < data.Count; j++)
                {
                    if (data[j].StartsWith("<Polygon") || data[j].StartsWith("<Rectangle")||data[j].StartsWith("<Ellipse"))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            using (StreamWriter sw = new StreamWriter(stream))
                            {
                                sw.Write(data[j]);
                                sw.Flush();
                                stream.Seek(0, SeekOrigin.Begin);
                                Shape shape = XamlReader.Load(stream) as Shape;

                                InkCanvas.SetLeft(shape, _justlastMousePosition.X);
                                InkCanvas.SetTop(shape, _justlastMousePosition.Y);
                                InkCanvasInfo.Children.Add(shape);
                            }
                        }
                    }
                }
            }


        }
    }
}
