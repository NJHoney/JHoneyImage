using CommunityToolkit.Mvvm.Input;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.Util.Loading.ViewModel;
using JHoney_ImageConverter.ViewModel.Base;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace JHoney_ImageConverter.ViewModel
{
    class MainWindowViewModel:CustomViewModelBase
    {
        #region 프로퍼티
        
        Thread ConvertWork;

        public bool IsOpenColorPicker
        {
            get { return _isOpenColorPicker; }
            set { _isOpenColorPicker = value; OnPropertyChanged("IsOpenColorPicker"); }
        }
        private bool _isOpenColorPicker = false;

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; SegmentationLabelViewModel.InkCanvasInfo.DefaultDrawingAttributes.Color = value; SegmentationLabelViewModel.UpdateColor(); OnPropertyChanged("SelectedColor"); }
        }
        private Color _selectedColor = Color.FromArgb(100,255,0,0);

        private IDialogCoordinator _dialogCoordinator;


        #region ---［ ViewModel ］---------------------------------------------------------------------

        public ImageListViewModel ImageListViewModel
        {
            get { return _imageListViewModel; }
            set { _imageListViewModel = value; OnPropertyChanged("ImageListViewModel"); }
        }
        private ImageListViewModel _imageListViewModel = new ImageListViewModel();

        public LoadingViewModel LoadingViewModel
        {
            get { return _loadingViewModel; }
            set { _loadingViewModel = value; OnPropertyChanged("LoadingViewModel"); }
        }
        private LoadingViewModel _loadingViewModel = new LoadingViewModel();

        public ProgressLoadingViewModel ProgressLoadingViewModel
        {
            get { return _progressLoadingViewModel; }
            set { _progressLoadingViewModel = value; OnPropertyChanged("ProgressLoadingViewModel"); }
        }
        private ProgressLoadingViewModel _progressLoadingViewModel = new ProgressLoadingViewModel();
        public ImageConverterViewModel ImageConverterViewModel
        {
            get { return _imageConverterViewModel; }
            set { _imageConverterViewModel = value; OnPropertyChanged("ImageConverterViewModel"); }
        }
        private ImageConverterViewModel _imageConverterViewModel = new ImageConverterViewModel();

        public ImagePatternMatchViewModel ImagePatternMatchViewModel
        {
            get { return _imagePatternMatchViewModel; }
            set { _imagePatternMatchViewModel = value; OnPropertyChanged("ImagePatternMatchViewModel"); }
        }
        private ImagePatternMatchViewModel _imagePatternMatchViewModel = new ImagePatternMatchViewModel();

        public ImageDifferenceViewModel ImageDifferenceViewModel
        {
            get { return _imageDifferenceViewModel; }
            set { _imageDifferenceViewModel = value; OnPropertyChanged("ImageDifferenceViewModel"); }
        }
        private ImageDifferenceViewModel _imageDifferenceViewModel = new ImageDifferenceViewModel();

        public ImageMuiltConvertViewModel ImageMuiltConvertViewModel
        {
            get { return _imageMuiltConvertViewModel; }
            set { _imageMuiltConvertViewModel = value; OnPropertyChanged("ImageMuiltConvertViewModel"); }
        }
        private ImageMuiltConvertViewModel _imageMuiltConvertViewModel = new ImageMuiltConvertViewModel();

        public ImageDetectorViewModel ImageDetectorViewModel
        {
            get { return _imageDetectorViewModel; }
            set { _imageDetectorViewModel = value; OnPropertyChanged("ImageDetectorViewModel"); }
        }
        private ImageDetectorViewModel _imageDetectorViewModel = new ImageDetectorViewModel();

        public SegmentationLabelViewModel SegmentationLabelViewModel
        {
            get { return _segmentationLabelViewModel; }
            set { _segmentationLabelViewModel = value; OnPropertyChanged("SegmentationLabelViewModel"); }
        }
        private SegmentationLabelViewModel _segmentationLabelViewModel = new SegmentationLabelViewModel();

        #endregion ---------------------------------------------------------------------------------
        public double GridSplitterPreViewLength
        {
            get { return _gridSplitterPreViewLength; }
            set { _gridSplitterPreViewLength = value; OnPropertyChanged("GridSplitterPreViewLength"); }
        }
        private double _gridSplitterPreViewLength = 220;
        public GridLength GridSplitterLength
        {
            get { return _gridSplitterLength; }
            set { _gridSplitterLength = value; OnPropertyChanged("GridSplitterLength"); }
        }
        private GridLength _gridSplitterLength = GridLength.Auto;
        #endregion
        #region 커맨드
        public RelayCommand<object> CloseCommand { get; private set; }
        public RelayCommand<object> CommandSelectMenu { get; private set; }

        

        #endregion

        #region 초기화
        public MainWindowViewModel()
        {
            _dialogCoordinator = DialogCoordinator.Instance;
            InitData();
            InitCommand();
            InitEvent();
        }

        void InitData()
        {
            ImageConverterViewModel.Visibility = Visibility.Visible;
            ImageListViewModel.MainWindowViewModel = this;
            ImageConverterViewModel._mainWindowViewModel = this;
            SegmentationLabelViewModel.MainWindowViewModel=this;
        }

        void InitCommand()
        {
            CloseCommand = new RelayCommand<object>((param) => OnCloseCommand(param));
            CommandSelectMenu = new RelayCommand<object>((param) => OnCommandSelectMenu(param));
        }

        void InitEvent()
        {
            
            
        }
        #endregion

        #region 이벤트
        void ConvertWorkThreadMethod(ObservableCollection<ImageConvertInfoModel> TempConvertList)
        {
            OnOffLoading(true, "1");
            IsEnabled = false;
            
            for (int iLoofCount = 0; iLoofCount < ImageListViewModel.LoadImageListAll.Count; iLoofCount++)
            {
                for (int jLoofCount = 0; jLoofCount < TempConvertList.Count; jLoofCount++)
                {
                    RunConvertQueue(TempConvertList[jLoofCount].ConvertCommandName, ImageListViewModel.LoadImageListAll[iLoofCount].FileName_Full, ImageListViewModel.LoadImageListAll[iLoofCount].FileName_Full, TempConvertList[jLoofCount].ParamList);
                }
                
            }
            IsEnabled = true;
            OnOffLoading(false, "1");
            FooMessage("Convert", "Finished");
        }

        void RunConvertQueue(string ConvertCommand, string InputPath, string OutputPath, List<string> Param)
        {
            switch (ConvertCommand)
            {
                case "Grayscale":
                    ImageConverterViewModel._grayScale.imgToGrayscale(InputPath, OutputPath);
                    break;
                case "Dilate":
                    ImageConverterViewModel._erodeDilate.Dilate(InputPath, OutputPath);
                    break;
                case "Erode":
                    ImageConverterViewModel._erodeDilate.Erode(InputPath, OutputPath);
                    break;
                case "Reverse":
                    ImageConverterViewModel._reverse.ImgReverse(InputPath, OutputPath);
                    break;
                case "Binary":
                    ImageConverterViewModel._binary.imgTobinary(InputPath, OutputPath, int.Parse(Param[0]), 255);
                    break;
                case "EdgeCanny":
                    ImageConverterViewModel._cannyEdge.cannyToImage(InputPath, OutputPath, int.Parse(Param[0]));
                    break;
                case "Crop":
                    ImageConverterViewModel._crop.cropImageSave(InputPath, OutputPath, int.Parse(Param[0]), int.Parse(Param[1]), int.Parse(Param[2]), int.Parse(Param[3]));
                    break;
                case "Resize":
                    ImageConverterViewModel._resize.ResizeFromPath(InputPath, OutputPath, int.Parse(Param[0]), int.Parse(Param[1]));
                    break;
                case "EdgeSobel":
                    // _edgeSobel.sobelToImage(InputPath, OutputPath);
                    break;
                
                
                case "ChangeChannel":
                    ImageConverterViewModel._chaangeImageBits.ChangeImageBits(InputPath, OutputPath, int.Parse(Param[0]));
                    break;
                case "FindPattern":
                    ImagePatternMatchViewModel._matchTemplate.DoMatchTemplateAndSave(InputPath, Param[0], 1, OutputPath);
                    break;
                case "MergeImage":

                    break;
                case "Test":
                    ImageConverterViewModel._blob.TestBlob(InputPath, OutputPath);
                    break;
            }
        }
        /*
        private void OnMyCommand(object param)
            {

            }
            */
        private void OnCommandSelectMenu(object param)
        {
            ImageConverterViewModel.Visibility = Visibility.Collapsed;
            ImagePatternMatchViewModel.Visibility = Visibility.Collapsed;
            ImageDifferenceViewModel.Visibility = Visibility.Collapsed;
            ImageMuiltConvertViewModel.Visibility = Visibility.Collapsed;
            ImageDetectorViewModel.Visibility = Visibility.Collapsed;
            SegmentationLabelViewModel.Visibility = Visibility.Collapsed;

            switch(param.ToString())
            {
                case "Convert":
                    ImageConverterViewModel.Visibility = Visibility.Visible;
                    break;
                case "Pattern":
                    ImagePatternMatchViewModel.Visibility = Visibility.Visible;
                    break;
                case "Difference":
                    ImageDifferenceViewModel.Visibility = Visibility.Visible;
                    break;
                case "MuiltConvert":
                    ImageMuiltConvertViewModel.Visibility = Visibility.Visible;
                    break;
                case "Detect":
                    ImageDetectorViewModel.Visibility = Visibility.Visible;
                    break;
                case "Segmentation":
                    SegmentationLabelViewModel.Visibility = Visibility.Visible;
                    break;
            }
        }

        // Methods
        void OnOffLoading(bool TurnOn, string type)
        {
            if (type == "0")
            {
                if (TurnOn)
                {
                    LoadingViewModel.Visibility = Visibility.Visible;
                }
                else
                {
                    LoadingViewModel.Visibility = Visibility.Collapsed;
                }
            }
            if (type == "1")
            {
                if (TurnOn)
                {
                    ProgressLoadingViewModel.Visibility = Visibility.Visible;
                }
                else
                {
                    ProgressLoadingViewModel.Visibility = Visibility.Collapsed;
                }
            }


        }
        private async void FooMessage(string header, string message)
        {
            await _dialogCoordinator.ShowMessageAsync(this, header, message);
        }

        private async void FooProgress(string header, string message)
        {
            // Show...
            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, header, message);
            controller.SetIndeterminate();

            // Do your work... 

            // Close...
            await controller.CloseAsync();
        }


        private void OnCloseCommand(object param)
        {
            App.Current.Shutdown();
        }

        public async Task<MessageDialogResult> FooMessage(string title, string message, MessageDialogStyle messageDialogStyle)
        {
            var result = this._dialogCoordinator.ShowMessageAsync(this, title,message,messageDialogStyle);
            return await result;
        }
        
        #endregion

    }
}
