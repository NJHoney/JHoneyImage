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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.CodeDom;
using static System.Net.WebRequestMethods;

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
        public bool IsWhiteLabelOn
        {
            get { return _isWhiteLabelOn; }
            set { _isWhiteLabelOn = value; OnPropertyChanged("IsWhiteLabelOn"); }
        }
        private bool _isWhiteLabelOn = false;
        
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


        public ObservableCollection<ShapeListModel> ShapeList
        {
            get { return _shapeList; }
            set { _shapeList = value; OnPropertyChanged("ShapeList"); }
        }
        private ObservableCollection<ShapeListModel> _shapeList = new ObservableCollection<ShapeListModel>();
        #endregion ---------------------------------------------------------------------------------

        public DataGrid Datagrid_Shape
        {
            get { return _datagrid_Shape; }
            set { _datagrid_Shape = value; OnPropertyChanged("datagrid_Shape"); }
        }
        private DataGrid _datagrid_Shape = new DataGrid();

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
        private Polyline polyline = new Polyline();
        private bool drawOnMove = false;

        private System.Windows.Point startPoint =new System.Windows.Point();
        private bool IsPolygonMode = false;
        private bool IsFirstTime = true;
        private Ellipse baseEllipse = new Ellipse(); 
        private Line temppolyline = new Line();
        private PointCollection polyPoints = new PointCollection();
        private bool IsPolygonSolidMode = false;
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
                rectline.StrokeThickness = PenThickness;
                polyline.StrokeThickness = PenThickness;
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
        public RelayCommand<object> CanvasEventSelectionChanged { get; private set; }


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
        public RelayCommand<object> CommandSaveRectToJson { get; private set; }
        public RelayCommand<object> CommandSaveRectToYolov8Txt { get; private set; }
        public RelayCommand<object> CommandLoadRectToJson { get; private set; }

        public RelayCommand<object> CommandSetSavePath { get; private set; }
        public RelayCommand<object> ListBoxSelectionChanged { get; private set; }

        public RelayCommand<InkCanvasStrokeCollectedEventArgs> CanvasStrokeCollected { get; private set; }
        public RelayCommand<KeyEventArgs> CanvasKeyDown { get; private set; }


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
            rectline.Tag = "UI";
            ellipseline.Tag = "UI";
            polyline.Tag = "UI";
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
            CanvasEventSelectionChanged = new RelayCommand<object>((e) => OnCanvasEventSelectionChanged(e));

            CanvasStrokeCollected = new RelayCommand<InkCanvasStrokeCollectedEventArgs>((e) => OnCanvasStrokeCollected(e));

            CommandDropFile = new RelayCommand<DragEventArgs>((e) => OnCommandDropFile(e));


            CommandSetColor = new RelayCommand<object>((param) => OnCommandSetColor(param));
            CommandSelectEditingMode = new RelayCommand<object>((param) => OnCommandSelectEditingMode(param));
            CommandSaveLabelImage = new RelayCommand<object>((param) => OnCommandSaveLabelImage(param));
            CommandSaveRectToJson = new RelayCommand<object>((param) => OnCommandSaveRectToJson(param));
            CommandSaveRectToYolov8Txt = new RelayCommand<object>((param)=> OnCommandSaveRectToYolov8Txt(param));
            CommandLoadRectToJson = new RelayCommand<object>((param) => OnCommandLoadRectToJson(param));
            CommandSetSavePath = new RelayCommand<object>((param) => OnCommandSetSavePath(param));

            ListBoxSelectionChanged = new RelayCommand<object>((e) => OnListBoxSelectionChanged(e));
            CanvasKeyDown = new RelayCommand<KeyEventArgs>((e) => OnCanvasKeyDown(e));
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
                if ((param as DataGrid).Name == "datagrid_Shape")
                {
                    Datagrid_Shape = param as DataGrid;
                    Datagrid_Shape.IsReadOnly = true;
                }
                else
                {
                    ImageInfoDataGrid = param as DataGrid;
                    ImageInfoDataGrid.IsReadOnly = true;
                    ImageInfoDataGridList.Add(ImageInfoDataGridModel);
                    ImageInfoDataGrid.ItemsSource = ImageInfoDataGridList;
                }
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

                polyline.Points = new PointCollection();
                polyline.StrokeThickness = PenThickness;
                polyline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                InkCanvasInfo.Children.Add(polyline);
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

                System.IO.File.Copy(file[0], tempImg, true);
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
                        el.Fill = new SolidColorBrush { Color = Colors.Yellow };

                        el.Margin =
                            new Thickness(left: polyline.Points[0].X - el.Width / 2, top: polyline.Points[0].Y - el.Height / 2, right: 0, bottom: 0);
                        InkCanvasInfo.Children.Add(el);
                    }

                    drawOnMove = true;
                }
            }

            if (IsPolygonSolidMode)
            {
                
                if (IsFirstTime)
                {
                    startPoint = e.GetPosition(InkCanvasInfo);
                    polyPoints.Clear();
                    polyPoints.Add(startPoint);
                    baseEllipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = new SolidColorBrush { Color = Colors.Yellow }
                };
                    InkCanvas.SetLeft(baseEllipse, startPoint.X-5);
                    InkCanvas.SetTop(baseEllipse, startPoint.Y-5);
                    InkCanvasInfo.Children.Add(baseEllipse);

                    temppolyline = new Line
                    {
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y,
                        Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()),
                        StrokeThickness = PenThickness,
                        //StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                    };

                    InkCanvasInfo.Children.Add(temppolyline);
                    IsFirstTime = false;
                }
                else
                {
                    polyPoints.Add(e.GetPosition(InkCanvasInfo));
                    startPoint = e.GetPosition(InkCanvasInfo);
                    temppolyline = new Line
                    {
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y,
                        Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()),
                        StrokeThickness = PenThickness,
                        //StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                    };
                    InkCanvasInfo.Children.Add(temppolyline);
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
                            rectline.Visibility = Visibility.Visible;
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

                        tmpRect.Tag = Guid.NewGuid().ToString();
                        tmpRect.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        tmpRect.Arrange(new System.Windows.Rect(tmpRect.DesiredSize));

                        InkCanvasInfo.Children.Add(tmpRect);
                        ShapeList.Add(new ShapeListModel() { Shape = CopyShape(tmpRect as Rectangle), Point = new System.Windows.Point(InkCanvas.GetLeft(tmpRect), InkCanvas.GetTop(tmpRect)), uuid = tmpRect.Tag.ToString(), Width = tmpRect.ActualWidth, Height = tmpRect.ActualHeight });


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

                        tmpellipse.Tag = Guid.NewGuid().ToString();
                        tmpellipse.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                        tmpellipse.Arrange(new System.Windows.Rect(tmpellipse.DesiredSize));

                        InkCanvasInfo.Children.Add(tmpellipse);
                        ShapeList.Add(new ShapeListModel() { Shape = CopyShape(tmpellipse as Ellipse), Point = new System.Windows.Point(InkCanvas.GetLeft(tmpellipse), InkCanvas.GetTop(tmpellipse)), uuid = tmpellipse.Tag.ToString(), Width = tmpellipse.ActualWidth, Height = tmpellipse.ActualHeight });


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
                    if ((e.OriginalSource as Ellipse).Tag != null)
                    {
                        return;
                    }

                    InkCanvasInfo.Children.Remove((Ellipse)e.OriginalSource);
                    //InkCanvasInfo.Children.Remove(polyline);
                    Polygon tmpPolygon = new Polygon();
                    tmpPolygon.StrokeThickness = PenThickness;
                    tmpPolygon.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    tmpPolygon.Points = polyline.Points.Clone();

                    polyline.Points.Clear();

                    drawOnMove = false;
                    //rbDraw.IsChecked = false;
                    InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                    //polyline = new Polyline();
                    //polyline.Points = new PointCollection();
                    //polyline.StrokeThickness = PenThickness;
                    //polyline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    //InkCanvasInfo.Children.Add(polyline);

                    if (IsFillOn) { tmpPolygon.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }
                    adjustPolygonXY(tmpPolygon);
                    tmpPolygon.Tag = Guid.NewGuid().ToString();
                    tmpPolygon.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    tmpPolygon.Arrange(new System.Windows.Rect(tmpPolygon.DesiredSize));

                    InkCanvasInfo.Children.Add(tmpPolygon);
                    ShapeList.Add(new ShapeListModel() { Shape = CopyShape(tmpPolygon as Polygon), Point = new System.Windows.Point(InkCanvas.GetLeft(tmpPolygon), InkCanvas.GetTop(tmpPolygon)), uuid = tmpPolygon.Tag.ToString(), Width = tmpPolygon.ActualWidth, Height = tmpPolygon.ActualHeight });

                }
                if (drawOnMove)
                {
                    polyline.Points = polyline.Points.Clone();
                    polyline.Points.Add(e.GetPosition(InkCanvasInfo));
                }

            }

            if (IsPolygonSolidMode)
            {
                if (!IsFirstTime)
                {
                    temppolyline.X2 = CurrentMousePoint.X;
                    temppolyline.Y2 = CurrentMousePoint.Y;
                    
                }
                if (e.OriginalSource is Ellipse)
                {
                    if ((e.OriginalSource as Ellipse).Tag != null)
                    {
                        return;
                    }
                    if(polyPoints.Count<3)
                    { return; }
                    polyPoints.Add(e.GetPosition(InkCanvasInfo));
                    InkCanvasInfo.Children.Remove((Ellipse)e.OriginalSource);
                    //InkCanvasInfo.Children.Remove(polyline);
                    Polygon tmpPolygon = new Polygon();
                    tmpPolygon.StrokeThickness = PenThickness;
                    tmpPolygon.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    tmpPolygon.Points = polyPoints.Clone();

                    InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
                    //polyline = new Polyline();
                    //polyline.Points = new PointCollection();
                    //polyline.StrokeThickness = PenThickness;
                    //polyline.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
                    //InkCanvasInfo.Children.Add(polyline);

                    if (IsFillOn) { tmpPolygon.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString()); }
                    adjustPolygonXY(tmpPolygon);
                    tmpPolygon.Tag = Guid.NewGuid().ToString();
                    tmpPolygon.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    tmpPolygon.Arrange(new System.Windows.Rect(tmpPolygon.DesiredSize));

                    InkCanvasInfo.Children.Add(tmpPolygon);
                    ShapeList.Add(new ShapeListModel() { Shape = CopyShape(tmpPolygon as Polygon), Point = new System.Windows.Point(InkCanvas.GetLeft(tmpPolygon), InkCanvas.GetTop(tmpPolygon)), uuid = tmpPolygon.Tag.ToString(), Width = tmpPolygon.ActualWidth, Height = tmpPolygon.ActualHeight });
                    polyPoints.Clear();
                    removeNullChild();
                    IsFirstTime = true;
                }

            }

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
            System.Windows.Point DPICalcPoint = new System.Windows.Point(InputPoint.X * ImageShow.ImageBrush_XDPI/ 96 , InputPoint.Y * ImageShow.ImageBrush_YDPI / 96);
            return DPICalcPoint;
        }
        int DPICaclSingle(double Param)
        {
            Param = Param * (ImageShow.ImageBrush_XDPI / 96);
            return (int)Param;
        }

        public void removeNullChild()
        {
            List<UIElement> tempList = new List<UIElement>();
            for (int i = 0; i < InkCanvasInfo.Children.Count; i++)
            {
                if ((InkCanvasInfo.Children[i] as Shape).Tag == null)
                {
                    tempList.Add(InkCanvasInfo.Children[i]);
                }
            }
            for (int i = 0; i < tempList.Count; i++)
            {
                InkCanvasInfo.Children.Remove(tempList[i]);
            }
        }

        private void OnCommandSelectEditingMode(object param)
        {
            if (param == null) { return; }

            removeNullChild();
            IsPolygonMode = false;
            drawOnMove = false;
            polyline.Points.Clear();
            IsPolygonMode = IsPolygonSolidMode = IsRectangleMode = IsStartRect = IsEllipseMode = false;
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
            }
            if (param.ToString() == "PolygonSolid")
            {
                IsPolygonSolidMode = IsFirstTime = true;
                InkCanvasInfo.EditingMode = InkCanvasEditingMode.None;
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
        
        private void SaveLabelImage(string saveName)
        {
            Thread.Sleep(10);

            System.Windows.Rect bounds = new System.Windows.Rect(0,0, InkCanvasInfo.ActualWidth,InkCanvasInfo.ActualHeight);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)Math.Round(bounds.Width), (int)Math.Round(bounds.Height), dpi, dpi, System.Windows.Media.PixelFormats.Default);
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
                System.IO.File.WriteAllBytes(saveName, ms.ToArray());
                Mat tempMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(new System.Drawing.Bitmap(ms));
                Mat[] tempMatChannel = tempMat.Split();
                Cv2.Threshold(tempMatChannel[3], tempMatChannel[3], 1, 255, ThresholdTypes.Binary);
                tempMatChannel[3] = ~tempMatChannel[3];
                tempMatChannel[3].SaveImage(saveName);
                tempMat.Dispose();
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
                        SaveLabelImage(Dialog.FileName);
                    }
                }
            }
            else
            {
                string savePath = TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_Safe;

                if(System.IO.File.Exists(savePath))
                {
                    savePath = TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_OnlyName + "_label." + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_Extension;
                }
                SaveLabelImage(savePath);
                //Thread.Sleep(10);

                //System.Windows.Rect bounds = VisualTreeHelper.GetDescendantBounds(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo);
                //double dpi = 96d;

                //double scaleX = _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.ActualWidth / bounds.Width;
                //double scaleY = _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.ActualHeight / bounds.Height;
                //System.Windows.Size canvasSize = new System.Windows.Size(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.ActualWidth, _mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo.ActualHeight);

                //RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvasSize.Width, (int)canvasSize.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);
                //DrawingVisual dv = new DrawingVisual();
                //using (DrawingContext dc = dv.RenderOpen())
                //{
                //    VisualBrush vb = new VisualBrush(_mainWindowViewModel.SegmentationLabelViewModel.InkCanvasInfo);
                //    dc.PushTransform(new ScaleTransform(scaleX, scaleY));
                //    dc.DrawRectangle(vb, null, new System.Windows.Rect(new System.Windows.Point(), bounds.Size));
                //}
                //rtb.Render(dv);

                //BitmapEncoder pngEncoder = new PngBitmapEncoder();
                //pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                //{
                //    pngEncoder.Save(ms);
                //    System.IO.File.WriteAllBytes(savePath, ms.ToArray());
                //    Mat tempMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(new System.Drawing.Bitmap(ms));
                //    Mat[] tempMatChannel = tempMat.Split();
                //    Cv2.Threshold(tempMatChannel[3], tempMatChannel[3], 1, 255, ThresholdTypes.Binary);
                //    tempMatChannel[3] = ~tempMatChannel[3];
                //    tempMatChannel[3].SaveImage(savePath);
                //    tempMat.Dispose();
                //}
            }


            InkCanvasInfo.EditingMode = preMode;
        }
        private void OnCommandSaveRectToJson(object param)
        {
            if (TxtSaveFilePath == "")
            {
                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                Dialog.DefaultExt = ".txt";
                Dialog.Filter = "JSon File (*.json)|*.json|All Files (*.*)|*.*";
                bool? result = Dialog.ShowDialog();

                var json = new JObject();
                if (result == true)
                {
                    if (Dialog.FileName != "")
                    {
                        for (int childIndex = 2; childIndex < InkCanvasInfo.Children.Count; childIndex++)
                        {
                            if (InkCanvasInfo.Children[childIndex].GetType() != typeof(Rectangle)) { continue; }
                            var transform = InkCanvasInfo.Children[childIndex].TransformToAncestor(InkCanvasInfo);
                            System.Windows.Point position = transform.Transform(new System.Windows.Point(0, 0));
                            var rectinfo = new[] { position.X.ToString(), position.Y.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Width.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Height.ToString() };


                            //label x y width height
                            //0 0.3466 0.2933 0.261230 0.35666

                            json.Add("Rect" + (childIndex - 2), JArray.FromObject(rectinfo));
                            System.IO.File.WriteAllText(Dialog.FileName, json.ToString());
                        }
                    }
                }
            }
            else
            {
                var json = new JObject();
                for (int childIndex = 2; childIndex < InkCanvasInfo.Children.Count; childIndex++)
                {
                    if (InkCanvasInfo.Children[childIndex].GetType() != typeof(Rectangle)) { continue; }
                    var transform = InkCanvasInfo.Children[childIndex].TransformToAncestor(InkCanvasInfo);
                    System.Windows.Point position = transform.Transform(new System.Windows.Point(0, 0));
                    var rectinfo = new[] { position.X.ToString(), position.Y.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Width.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Height.ToString() };
                    json.Add("Rect" + (childIndex - 2), JArray.FromObject(rectinfo));
                    System.IO.File.WriteAllText(TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_OnlyName + ".json", json.ToString());
                }

            }

        }

        private void OnCommandSaveRectToYolov8Txt(object param)
        {
            if (TxtSaveFilePath == "")
            {
                Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                Dialog.DefaultExt = ".txt";
                Dialog.Filter = "JSon File (*.json)|*.json|All Files (*.*)|*.*";
                bool? result = Dialog.ShowDialog();

                string labelContent = "";
                if (result == true)
                {
                    if (Dialog.FileName != "")
                    {
                        for (int childIndex = 2; childIndex < InkCanvasInfo.Children.Count; childIndex++)
                        {
                            if (InkCanvasInfo.Children[childIndex].GetType() != typeof(Rectangle)) { continue; }
                            var transform = InkCanvasInfo.Children[childIndex].TransformToAncestor(InkCanvasInfo);
                            System.Windows.Point position = transform.Transform(new System.Windows.Point(0, 0));
                            var rectinfo = new[] { position.X.ToString(), position.Y.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Width.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Height.ToString() };

                            labelContent += "0";
                            labelContent += " " + ((position.X + ((InkCanvasInfo.Children[childIndex] as Rectangle).ActualWidth)/2)) /InkCanvasInfo.ActualWidth;
                            labelContent += " " + ((position.Y + ((InkCanvasInfo.Children[childIndex] as Rectangle).ActualHeight)/2)) / InkCanvasInfo.ActualHeight;
                            labelContent += " " + (InkCanvasInfo.Children[childIndex] as Rectangle).Width / InkCanvasInfo.ActualWidth;
                            labelContent += " " + (InkCanvasInfo.Children[childIndex] as Rectangle).Height / InkCanvasInfo.ActualHeight + "\n";
                            //label center-x center-y width height
                            //0 0.3466 0.2933 0.261230 0.35666

                            
                            
                        }
                        System.IO.File.WriteAllText(Dialog.FileName, labelContent);
                    }
                }
            }
            else
            {
                var json = new JObject();
                for (int childIndex = 2; childIndex < InkCanvasInfo.Children.Count; childIndex++)
                {
                    if (InkCanvasInfo.Children[childIndex].GetType() != typeof(Rectangle)) { continue; }
                    var transform = InkCanvasInfo.Children[childIndex].TransformToAncestor(InkCanvasInfo);
                    System.Windows.Point position = transform.Transform(new System.Windows.Point(0, 0));
                    var rectinfo = new[] { position.X.ToString(), position.Y.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Width.ToString(), (InkCanvasInfo.Children[childIndex] as Rectangle).Height.ToString() };
                    json.Add("Rect" + (childIndex - 2), JArray.FromObject(rectinfo));
                    System.IO.File.WriteAllText(TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_OnlyName + ".json", json.ToString());
                }

            }
        }

        private void OnCommandLoadRectToJson(object param)
        {
            JObject json = new JObject();
            if (TxtSaveFilePath != "")
            {
                if (System.IO.File.Exists(TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_OnlyName + ".json"))
                {
                    using (StreamReader file = System.IO.File.OpenText(TxtSaveFilePath + "\\" + _mainWindowViewModel.ImageListViewModel.LastSelectedItem.FileName_OnlyName + ".json"))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        json = (JObject)JToken.ReadFrom(reader);
                        var rectList = json.Children().ToList();
                        for (int i = 0; i < rectList.Count; i++)
                        {
                            Rectangle tempRect = new Rectangle() { Width = (double)rectList[i].ElementAt(0).ElementAt(2), Height = (double)rectList[i].ElementAt(0).ElementAt(3), StrokeThickness = PenThickness };
                            InkCanvas.SetLeft(tempRect, (double)rectList[i].ElementAt(0).ElementAt(0));
                            InkCanvas.SetTop(tempRect, (double)rectList[i].ElementAt(0).ElementAt(1));

                            InkCanvasInfo.Children.Add(tempRect);
                            UpdateColor();
                        }
                    }
                    return;
                }
            }

            Microsoft.Win32.OpenFileDialog Dialog = new Microsoft.Win32.OpenFileDialog();
            Dialog.DefaultExt = ".txt";
            Dialog.Filter = "JSon File (*.json)|*.json|All Files (*.*)|*.*";
            bool? result = Dialog.ShowDialog();


            if (result == true)
            {
                if (Dialog.FileName != "")
                {
                    using (StreamReader file = System.IO.File.OpenText(Dialog.FileName))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        json = (JObject)JToken.ReadFrom(reader);
                        var rectList = json.Children().ToList();
                        for (int i = 0; i < rectList.Count; i++)
                        {
                            Rectangle tempRect = new Rectangle() { Width = (double)rectList[i].ElementAt(0).ElementAt(2), Height = (double)rectList[i].ElementAt(0).ElementAt(3), StrokeThickness = PenThickness };
                            InkCanvas.SetLeft(tempRect, (double)rectList[i].ElementAt(0).ElementAt(0));
                            InkCanvas.SetTop(tempRect, (double)rectList[i].ElementAt(0).ElementAt(1));

                            InkCanvasInfo.Children.Add(tempRect);
                            UpdateColor();
                        }
                    }
                }
            }

        }
        private void OnCanvasEventSelectionResizing(object e)
        {
            if (InkCanvasInfo.GetSelectedElements().Count == 0)
            {
                return;
            }
            var selectedItem = InkCanvasInfo.GetSelectedElements()[0];
            if (selectedItem.GetType() == typeof(Polygon))
            {
                double widthDelta = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Width - (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width;
                double heightDelta = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Height - (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height;
                for (int i = 0; i < (selectedItem as Polygon).Points.Count; i++)
                {
                    double xPoint = 0, yPoint = 0;

                    xPoint = (selectedItem as Polygon).Points[i].X + ((selectedItem as Polygon).Points[i].X / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width) * widthDelta;
                    yPoint = (selectedItem as Polygon).Points[i].Y + ((selectedItem as Polygon).Points[i].Y / (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height) * heightDelta;

                    (selectedItem as Polygon).Points[i] = new System.Windows.Point(xPoint, yPoint);
                }
                ShapeList.Where(x => x.uuid == (selectedItem as Polygon).Tag.ToString()).First().Point = new System.Windows.Point(InkCanvas.GetLeft(selectedItem as Polygon), InkCanvas.GetTop(selectedItem as Polygon));
                ShapeList.Where(x => x.uuid == (selectedItem as Polygon).Tag.ToString()).First().Width = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Width;
                ShapeList.Where(x => x.uuid == (selectedItem as Polygon).Tag.ToString()).First().Height = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Height;
            }
            if (selectedItem.GetType() == typeof(System.Windows.Shapes.Path))
            {
                double widthDelta = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Width - (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Width;
                double heightDelta = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Height - (e as InkCanvasSelectionEditingEventArgs).OldRectangle.Height;

                var path = selectedItem as System.Windows.Shapes.Path;
                var offset = new System.Windows.Point(InkCanvas.GetLeft(path), InkCanvas.GetTop(path));

                path.Stretch = Stretch.Fill;
                
                // Update ShapeList
                ShapeList.Where(x => x.uuid == (selectedItem as System.Windows.Shapes.Path).Tag.ToString()).First().Point = new System.Windows.Point(InkCanvas.GetLeft(selectedItem as System.Windows.Shapes.Path), InkCanvas.GetTop(selectedItem as System.Windows.Shapes.Path));
                ShapeList.Where(x => x.uuid == (selectedItem as System.Windows.Shapes.Path).Tag.ToString()).First().Width = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Width;
                ShapeList.Where(x => x.uuid == (selectedItem as System.Windows.Shapes.Path).Tag.ToString()).First().Height = (e as InkCanvasSelectionEditingEventArgs).NewRectangle.Height;



            }
        }


        private void OnCanvasEventSelectionChanged(object e)
        {
            if (InkCanvasInfo.GetSelectedStrokes().Count < 1)
            { return; }

        }

        private void OnListBoxSelectionChanged(object e)
        {
            List<UIElement> tempList = new List<UIElement>();
            foreach (var item in (e as SelectionChangedEventArgs).AddedItems)
            {
                for (int i = 0; i < InkCanvasInfo.Children.Count; i++)
                {
                    if ((InkCanvasInfo.Children[i] as Shape).Tag.ToString() == (item as ShapeListModel).Shape.Tag.ToString())
                    {
                        tempList.Add(InkCanvasInfo.Children[i]);
                    }
                }

            }
            InkCanvasInfo.Select(tempList);
            //InkCanvasInfo.Select()
        }

        private void OnCanvasStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            StrokeCollection strokes = InkCanvasInfo.Strokes;
            System.Windows.Point offSet = new System.Windows.Point();
            // Create a new StreamGeometry and its context
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                foreach (Stroke stroke in strokes)
                {
                    // Start drawing the new stroke
                    StylusPointCollection points = stroke.StylusPoints;
                    offSet = adjustPoint(points);
                    geometryContext.BeginFigure(new System.Windows.Point(points[0].ToPoint().X - offSet.X, points[0].ToPoint().Y - offSet.Y), false, false);

                    // Draw the remaining points
                    foreach (StylusPoint point in points)
                    {
                        geometryContext.LineTo(new System.Windows.Point(point.ToPoint().X - offSet.X, point.ToPoint().Y - offSet.Y), true, true);
                    }
                }
            }

            // Create a new Path element to display the StreamGeometry
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Data = streamGeometry;
            path.Stroke = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindowViewModel.SelectedColor.ToString());
            path.StrokeThickness = PenThickness;
            path.Tag = Guid.NewGuid().ToString();
            //path.Stretch = Stretch.Fill;
            InkCanvas.SetLeft(path, offSet.X);
            InkCanvas.SetTop(path, offSet.Y);
            // Add the Path to the InkCanvas's children
            InkCanvasInfo.Children.Add(path);

            // Clear the strokes from the InkCanvas
            InkCanvasInfo.Strokes.Clear();

            // Add the Shape to the ShapeList
            ShapeList.Add(new ShapeListModel() { Shape = CopyShape(path), Point = offSet, uuid = path.Tag.ToString(), Width = path.Data.Bounds.Width, Height = path.Data.Bounds.Height });


        }
        
        private void OnCanvasKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var selectedShapes = InkCanvasInfo.GetSelectedElements().OfType<Shape>().ToList();
                foreach (var shape in selectedShapes)
                {
                    ShapeList.Remove(ShapeList.Where(x => x.uuid == shape.Tag.ToString()).First());
                    InkCanvasInfo.Children.Remove(shape);

                }
            }
        }
        void calcLocation(UIElement uIElement)
        {
            ShapeList.Where(x => x.uuid == (uIElement as Shape).Tag.ToString()).First().Point = new System.Windows.Point(InkCanvas.GetLeft(uIElement), InkCanvas.GetTop(uIElement));
        }
        void adjustPolygonXY(Polygon tempPolygon)
        {
            double left = -1, top = -1, right = -1, bottom = -1;
            for (int i = 0; i < tempPolygon.Points.Count; i++)
            {
                if (left == -1)
                {
                    left = tempPolygon.Points[i].X;
                }
                if (left > tempPolygon.Points[i].X)
                {
                    left = tempPolygon.Points[i].X;
                }
                if (top == -1)
                {
                    top = tempPolygon.Points[i].Y;
                }
                if (top > tempPolygon.Points[i].Y)
                {
                    top = tempPolygon.Points[i].Y;
                }

                if (right == -1)
                {
                    right = tempPolygon.Points[i].X;
                }
                if (right < tempPolygon.Points[i].X)
                {
                    right = tempPolygon.Points[i].X;
                }
                if (bottom == -1)
                {
                    bottom = tempPolygon.Points[i].Y;
                }
                if (bottom < tempPolygon.Points[i].Y)
                {
                    bottom = tempPolygon.Points[i].Y;
                }
            }
            for (int i = 0; i < tempPolygon.Points.Count; i++)
            {
                tempPolygon.Points[i] = new System.Windows.Point(tempPolygon.Points[i].X - left, tempPolygon.Points[i].Y - top);
            }


            InkCanvas.SetLeft(tempPolygon, left);
            InkCanvas.SetTop(tempPolygon, top);
        }

        System.Windows.Point adjustPoint(StylusPointCollection Points)
        {
            double left = -1, top = -1, right = -1, bottom = -1;
            for (int i = 0; i < Points.Count; i++)
            {
                if (left == -1)
                {
                    left = Points[i].X;
                }
                if (left > Points[i].X)
                {
                    left = Points[i].X;
                }
                if (top == -1)
                {
                    top = Points[i].Y;
                }
                if (top > Points[i].Y)
                {
                    top = Points[i].Y;
                }

                if (right == -1)
                {
                    right = Points[i].X;
                }
                if (right < Points[i].X)
                {
                    right = Points[i].X;
                }
                if (bottom == -1)
                {
                    bottom = Points[i].Y;
                }
                if (bottom < Points[i].Y)
                {
                    bottom = Points[i].Y;
                }
            }
            return new System.Windows.Point(left, top);
        }
        Shape CopyShape(UIElement element)
        {
            string copyObject = "";
            Shape shape = null;
            copyObject = XamlWriter.Save(element);

            if (copyObject.StartsWith("<Polygon") || copyObject.StartsWith("<Rectangle") || copyObject.StartsWith("<Ellipse") || copyObject.StartsWith("<Path"))

                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        sw.Write(copyObject);
                        sw.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        shape = XamlReader.Load(stream) as Shape;
                    }
                }
            return shape;
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
                    if (data[j].StartsWith("<Polygon") || data[j].StartsWith("<Rectangle") || data[j].StartsWith("<Ellipse"))
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
