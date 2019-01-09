using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.OpenCV;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace JHoney_ImageConverter.ViewModel
{
    class ImageConverterViewModel : CustomViewModelBase
    {

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
        public Crop _crop = new Crop();
        public ChaangeImageBits _chaangeImageBits = new ChaangeImageBits();
        public Resize _resize = new Resize();
        public Blob _blob = new Blob();
        #endregion ---------------------------------------------------------------------------------

        #region ---［ Canvas ］---------------------------------------------------------------------
        string tempImgPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
        string tempImg = AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + "temp.png";
        Mat TempMat, TempConvertedMat;
        public Canvas CanvasInfo { get; set; }

        System.Windows.Point CurrentMousePoint
        {
            get { return _currentMousePoint; }
            set { _currentMousePoint = value; RaisePropertyChanged("CurrentMousePoint"); }
        }
        private System.Windows.Point _currentMousePoint = new System.Windows.Point();
        public ImageControlModel ImageShow
        {
            get { return _imageShow; }
            set { _imageShow = value; RaisePropertyChanged("ImageShow"); }
        }
        private ImageControlModel _imageShow = new ImageControlModel();

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

        #region ---［ Option ］---------------------------------------------------------------------
        public bool IsToggled
        {
            get { return _isToggled; }
            set { _isToggled = value; RaisePropertyChanged("IsToggled"); }
        }
        private bool _isToggled = false;


        public ObservableCollection<bool> TogleButtonEnabled
        {
            get { return _togleButtonEnabled; }
            set { _togleButtonEnabled = value; RaisePropertyChanged("TogleButtonEnabled"); }
        }
        private ObservableCollection<bool> _togleButtonEnabled = new ObservableCollection<bool>();


        string OptionMode = "";

        public string OptionParamText_01
        {
            get { return _optionParamText_01; }
            set { _optionParamText_01 = value; RaisePropertyChanged("OptionParamText_01"); }
        }
        private string _optionParamText_01 = "Threshold";

        public int OptionParamSlider_01_Min
        {
            get { return _optionParamSlider_01_Min; }
            set { _optionParamSlider_01_Min = value; RaisePropertyChanged("OptionParamSlider_01_Min"); }
        }
        private int _optionParamSlider_01_Min = 0;

        public int OptionParamSlider_01_Max
        {
            get { return _optionParamSlider_01_Max; }
            set { _optionParamSlider_01_Max = value; RaisePropertyChanged("OptionParamSlider_01_Max"); }
        }
        private int _optionParamSlider_01_Max = 255;

        #endregion ---------------------------------------------------------------------------------

        public DataGrid ImageInfoDataGrid
        {
            get { return _imageInfoDataGrid; }
            set { _imageInfoDataGrid = value; RaisePropertyChanged("ImageInfoDataGrid"); }
        }
        private DataGrid _imageInfoDataGrid = new DataGrid();

        #endregion
        #region 커맨드
        public RelayCommand<object> CommandLoaded { get; private set; }
        public RelayCommand<object> CommandSingleConvert { get; private set; }
        public RelayCommand<object> CanvasContext { get; private set; }
        public RelayCommand<MouseEventArgs> CanvasEventMouseUp { get; private set; }

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


        #endregion ---------------------------------------------------------------------------------

        public RelayCommand<object> CommandChangeSliderValue { get; private set; }
        public RelayCommand<object> CommandConfirmChange { get; private set; }
        public RelayCommand<object> CommandToggle { get; private set; }

        public RelayCommand<object> CommandCropResize { get; private set; }
        #endregion

        #region 초기화
        public ImageConverterViewModel()
        {
            InitData();
            InitCommand();
            InitEvent();
        }

        void InitData()
        {
            for (int iLoofCount = 0; iLoofCount < 5; iLoofCount++)
            {
                TogleButtonEnabled.Add(true);
            }

        }

        void InitCommand()
        {
            CommandLoaded = new RelayCommand<object>((param) => OnCommandLoaded(param));
            CanvasEventMouseWheel = new RelayCommand<MouseWheelEventArgs>((param) => OnCanvasEventMouseWheel(param));
            CanvasEventMouseMove = new RelayCommand<MouseEventArgs>((param) => OnCanvasEventMouseMove(param));
            CommandSingleConvert = new RelayCommand<object>((param) => OnCommandSingleConvert(param));
            CanvasContext = new RelayCommand<object>((param) => OnCanvasContext(param));
            CanvasEventMouseDown = new RelayCommand<MouseButtonEventArgs>((e) => OnCanvasEventMouseDown(e));
            CanvasEventMouseUp = new RelayCommand<MouseEventArgs>((e) => OnCanvasEventMouseUp(e));

            CommandChangeSliderValue = new RelayCommand<object>((param) => OnCommandChangeSliderValue(param));
            CommandConfirmChange = new RelayCommand<object>((param) => OnCommandConfirmChange(param));
            CommandToggle = new RelayCommand<object>((param) => OnCommandToggle(param));
            CommandCropResize = new RelayCommand<object>((param) => OnCommandCropResize(param));
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
                                  File.Copy(msgData.MessageImagePath, tempImg, true);
                                  UpdateImageInfo();

                                  ImageShow.ImageSourceUpdate(tempImg, "ImageBrush");
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
                ImageInfoDataGrid = param as DataGrid;
                ImageInfoDataGrid.IsReadOnly = true;
                ImageInfoDataGridList.Add(ImageInfoDataGridModel);
                ImageInfoDataGrid.ItemsSource = ImageInfoDataGridList;
            }
            if (param.GetType().Name == "Canvas")
            {
                CanvasInfo = param as Canvas;
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
            UpdateImageInfo();
        }

        private void OnCanvasEventMouseMove(MouseEventArgs e)
        {
            CurrentMousePoint = e.GetPosition(CanvasInfo);
            CurrentMousePoint = DPICacl(CurrentMousePoint);
            ImageInfoDataGridModel.Mouse__X = (int)Convert.ToDouble(CurrentMousePoint.X);
            ImageInfoDataGridModel.Mouse__Y = (int)Convert.ToDouble(CurrentMousePoint.Y);

            if (TempMat != null)
            {
                switch (TempMat.Channels())
                {
                    case 1:
                        ImageInfoDataGridModel.Channel__B = TempMat.Get<byte>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X);
                        ImageInfoDataGridModel.Channel__G = ImageInfoDataGridModel.Channel__R = ImageInfoDataGridModel.Channel__A = 0;
                        break;
                    case 3:
                        ImageInfoDataGridModel.Channel__B = TempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
                        ImageInfoDataGridModel.Channel__G = TempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
                        ImageInfoDataGridModel.Channel__R = TempMat.Get<Vec3b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
                        ImageInfoDataGridModel.Channel__A = 0;
                        break;
                    case 4:
                        ImageInfoDataGridModel.Channel__B = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[0];
                        ImageInfoDataGridModel.Channel__G = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[1];
                        ImageInfoDataGridModel.Channel__R = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[2];
                        ImageInfoDataGridModel.Channel__A = TempMat.Get<Vec4b>(ImageInfoDataGridModel.Mouse__Y, ImageInfoDataGridModel.Mouse__X)[3];
                        break;
                }
            }
            if (IsSelectRectangle)
            {
                CurrentMousePoint = e.GetPosition(CanvasInfo);
                CurrentMousePoint = DPICacl(CurrentMousePoint);
                if ((int)Convert.ToDouble(e.GetPosition(CanvasInfo).X) < 0 || (int)Convert.ToDouble(e.GetPosition(CanvasInfo).Y) < 0 || (int)Convert.ToDouble(e.GetPosition(CanvasInfo).X) > (int)Convert.ToDouble(CanvasInfo.ActualWidth * (96 / ImageShow.ImageBrush_XDPI)) || (int)Convert.ToDouble(e.GetPosition(CanvasInfo).Y) > (int)Convert.ToDouble(CanvasInfo.ActualHeight * (96 / ImageShow.ImageBrush_YDPI)))
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
                        CurrentMousePoint = e.GetPosition(CanvasInfo);
                        UpdateRectPosition(e);
                        CurrentMousePoint = DPICacl(CurrentMousePoint);

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
                        if ((int)Convert.ToDouble(e.GetPosition(CanvasInfo).X) < 0 || (int)Convert.ToDouble(e.GetPosition(CanvasInfo).Y) < 0 || (int)Convert.ToDouble(e.GetPosition(CanvasInfo).X) > (int)Convert.ToDouble(CanvasInfo.ActualWidth * (96 / ImageShow.ImageBrush_XDPI )) || (int)Convert.ToDouble(e.GetPosition(CanvasInfo).Y) > (int)Convert.ToDouble(CanvasInfo.ActualHeight * (96 / ImageShow.ImageBrush_YDPI)))
                        {
                            IsStartRect = false;
                        }
                        else
                        {
                            //초기화
                            StartRectPointX = StartRectPointY = EndRectPointX = EndRectPointY = RectWidth = RectHeight = 0;
                            IsEndDrawRect = false;

                            StartRectPointX = (int)Convert.ToDouble(e.GetPosition(CanvasInfo).X);
                            StartRectPointY = (int)Convert.ToDouble(e.GetPosition(CanvasInfo).Y);
                            StartMousePoint = e.GetPosition(CanvasInfo);
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
            switch (param.ToString())
            {
                case "Save":
                    Microsoft.Win32.SaveFileDialog Dialog = new Microsoft.Win32.SaveFileDialog();
                    Dialog.DefaultExt = ".txt";
                    Dialog.Filter = "PNG Files (*.png)|*.png|JPG Files (*.*)|*.jpg|All Files (*.*)|*.*";
                    bool? result = Dialog.ShowDialog();

                    if (result == true)
                    {
                        TempMat = new Mat(tempImg, ImreadModes.Unchanged);
                        if (IsSelectRectangle)
                        {

                            if (RectWidth < 2 || RectHeight < 2)
                            {
                                TempMat.SaveImage(Dialog.FileName);
                            }
                            else
                            {
                                Mat TempROIMat = TempMat.Clone(new OpenCvSharp.Rect((int)StartRectPointX, (int)StartRectPointY, RectWidth, RectHeight));
                                TempROIMat.SaveImage(Dialog.FileName);
                                TempROIMat.Dispose();
                            }

                        }
                        else
                        {
                            TempMat.SaveImage(Dialog.FileName);
                        }

                        //보내기
                        string[] TempMessage = { "Save", Dialog.SafeFileName + " : Save complete" };
                        MessengerMain msgData = new MessengerMain("MessageBox", "", TempMessage);
                        Messenger.Default.Send<MessengerMain>(msgData);
                    }
                    break;
                case "ToList":
                    DateTime TimeNow = DateTime.Now;
                    string TempName = TimeNow.Year +
                                        TimeNow.Month +
                                        TimeNow.Day + "-" +
                                        TimeNow.Hour +
                                        TimeNow.Minute +
                                        TimeNow.Second +
                                        TimeNow.Millisecond.ToString("D2");
                    if (IsSelectRectangle)
                    {

                        if (RectWidth < 2 || RectHeight < 2)
                        {
                            TempMat.SaveImage(tempImgPath + TempName + ".png");
                        }
                        else
                        {
                            Mat TempROIMat = TempMat.Clone(new OpenCvSharp.Rect(DPICaclSingle(StartRectPointX), DPICaclSingle(StartRectPointY), DPICaclSingle(RectWidth), DPICaclSingle(RectHeight)));
                            TempROIMat.SaveImage(tempImgPath + TempName + ".png");
                            TempROIMat.Dispose();
                        }
                    }
                    else
                    {
                        TempMat.SaveImage(tempImgPath + TempName + ".png");
                    }

                    //보내기
                    MessengerImageGetSet msgData2 = new MessengerImageGetSet("ToList", tempImgPath + TempName + ".png");
                    Messenger.Default.Send<MessengerImageGetSet>(msgData2);
                    break;
            }
        }
        private void OnCommandSingleConvert(object param)
        {
            if (TempMat == null || !File.Exists(tempImg))
            {
                return;
            }

            switch (param.ToString())
            {
                case "GrayScale":
                    if (ImageInfoDataGridModel.Channel == 1)
                    {
                        return;
                    }
                    _grayScale.imgToGrayscale(tempImg, tempImg);
                    break;
                case "Dilate":
                    _erodeDilate.Dilate(tempImg, tempImg);
                    break;
                case "Erode":
                    _erodeDilate.Erode(tempImg, tempImg);
                    break;
                case "Reverse":
                    _reverse.ImgReverse(tempImg, tempImg);
                    break;
                case "Red":
                    if (ImageInfoDataGridModel.Channel < 3)
                    {
                        return;
                    }
                    _colorExport.SingleChannelExport(tempImg, tempImg, 2);
                    break;
                case "Blue":
                    if (ImageInfoDataGridModel.Channel < 3)
                    {
                        return;
                    }
                    _colorExport.SingleChannelExport(tempImg, tempImg, 0);
                    break;
                case "Green":
                    if (ImageInfoDataGridModel.Channel < 3)
                    {
                        return;
                    }
                    _colorExport.SingleChannelExport(tempImg, tempImg, 1);
                    break;
                case "Test":
                    _blob.GetBlob(tempImg,tempImg);
                    break;
            }
            UpdateImageInfo();
        }

        private void OnCommandChangeSliderValue(object param)
        {
            if (TempMat == null) { return; }
            TempConvertedMat = TempMat.Clone();

            switch(OptionMode)
            {
                case "ToggleBinary":
                    _binary.imgTobinary(TempConvertedMat, System.Convert.ToInt32(param), 255).SaveImage(tempImgPath + "TempConverted.png");
                    break;

                case "ToggleGaussian":
                    _gaussianBlur.gaussianToImg(TempConvertedMat, Convert.ToInt32(param)).SaveImage(tempImgPath + "TempConverted.png");
                    break;

                case "ToggleCanny":
                    _cannyEdge.cannyToImage(TempConvertedMat, Convert.ToInt32(param)).SaveImage(tempImgPath + "TempConverted.png");
                    break;

                case "ToggleMedian":
                    _medianBlur.imgToMedian(TempConvertedMat, Convert.ToInt32(param)).SaveImage(tempImgPath + "TempConverted.png"); 
                    break;

                case "ToggleRotate":
                    _rotate.RotateFromMat(TempConvertedMat, Convert.ToInt32(param)).SaveImage(tempImgPath + "TempConverted.png");
                    break;
            }
            ImageShow.ImageSourceUpdate(tempImgPath + "TempConverted.png", "ImageBrush");

        }

        private void OnCommandCropResize(object param)
        {
            if(param.ToString()=="Crop")
            {
                if (IsSelectRectangle)
                {

                    if (RectWidth < 2 || RectHeight < 2)
                    {
                        return;
                    }
                    else
                    {
                        Mat TempROIMat = TempMat.Clone(new OpenCvSharp.Rect(DPICaclSingle(StartRectPointX), DPICaclSingle(StartRectPointY), DPICaclSingle(RectWidth), DPICaclSingle(RectHeight)));
                        TempROIMat.SaveImage(tempImg);
                        TempROIMat.Dispose();
                        UpdateImageInfo();
                        IsSelectRectangle = false;
                    }

                }
            }

            if(param.ToString()=="ReSize")
            {
                if (IsSelectRectangle)
                {

                    if (RectWidth < 2 || RectHeight < 2)
                    {
                        return;
                    }
                    else
                    {

                        Mat TempROIMat = TempMat.Resize(new OpenCvSharp.Size(DPICaclSingle(RectWidth), DPICaclSingle(RectHeight)));
                        TempROIMat.SaveImage(tempImg);
                        TempROIMat.Dispose();
                        UpdateImageInfo();
                        IsSelectRectangle = false;
                    }

                }
            }
        }

        private void OnCommandConfirmChange(object param)
        {
            if(TempConvertedMat==null)
            {
                return;
            }
            TempMat = new Mat(tempImgPath + "TempConverted.png", ImreadModes.Unchanged);
            TempMat.SaveImage(tempImg);
            UpdateImageInfo();

        }

        private void OnCommandToggle(object param)
        {
            ToggleButton TempToggleButton = param as ToggleButton;
            if(TempToggleButton.IsChecked==false)
            {
                IsToggled = false;

                for (int iLoofCount = 0; iLoofCount < TogleButtonEnabled.Count; iLoofCount++)
                {
                    TogleButtonEnabled[iLoofCount] = true;
                }

                return;
            }
            IsToggled = true;
            OptionMode = TempToggleButton.Name;

            for (int iLoofCount = 0; iLoofCount < TogleButtonEnabled.Count; iLoofCount++)
            {
                TogleButtonEnabled[iLoofCount] = false;
            }

            switch (TempToggleButton.Name)
            {
                case "ToggleBinary":
                    OptionParamText_01 = "Threshold";
                    OptionParamSlider_01_Min = 0;
                    OptionParamSlider_01_Max = 255;
                    TogleButtonEnabled[0] = true;
                    break;
                case "ToggleGaussian":
                    OptionParamText_01 = "Threshold";
                    OptionParamSlider_01_Min = 0;
                    OptionParamSlider_01_Max = 100;
                    TogleButtonEnabled[1] = true;
                    break;
                case "ToggleCanny":
                    OptionParamText_01 = "Threshold";
                    OptionParamSlider_01_Min = 0;
                    OptionParamSlider_01_Max = 255;
                    TogleButtonEnabled[2] = true;
                    break;
                case "ToggleMedian":
                    OptionParamText_01 = "Threshold";
                    OptionParamSlider_01_Min = 0;
                    OptionParamSlider_01_Max = 255;
                    TogleButtonEnabled[3] = true;
                    break;
                case "ToggleRotate":
                    OptionParamText_01 = "Angle";
                    OptionParamSlider_01_Min = 0;
                    OptionParamSlider_01_Max = 360;
                    TogleButtonEnabled[4] = true;
                    break;
            }
        }

        void UpdateImageInfo()
        {
            TempMat = new Mat(tempImg, ImreadModes.Unchanged);

            if(TempMat.Type().Channels==512)
            {
                TempMat.Dispose();
                TempMat = new Mat(tempImg, ImreadModes.GrayScale);
            }

            ImageInfoDataGridModel.Image__Width = TempMat.Width;
            ImageInfoDataGridModel.Image__Height = TempMat.Height;
            ImageInfoDataGridModel.Channel = TempMat.Channels();
            ImageInfoDataGridModel.Scale = double.Parse(ImageShow.ImageBrushScaleX.ToString("F2"));
            ImageShow.ImageSourceUpdate(tempImg, "ImageBrush");
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
        #endregion

    }
}
