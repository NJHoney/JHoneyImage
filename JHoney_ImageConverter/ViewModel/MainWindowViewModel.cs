using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

namespace JHoney_ImageConverter.ViewModel
{
    class MainWindowViewModel:CustomViewModelBase
    {

        #region 프로퍼티
        Window parentWindow;
        Thread ConvertWork;
        /*
        public int MyVariable
          {
              get { return _myVariable; }
              set { _myVariable = value; RaisePropertyChanged("MyVariable"); }
          }
          private int _myVariable;
          */
        private IDialogCoordinator _dialogCoordinator;
        #region ---［ ViewModel ］---------------------------------------------------------------------

        public ImageListViewModel ImageListViewModel
        {
            get { return _imageListViewModel; }
            set { _imageListViewModel = value; RaisePropertyChanged("ImageListViewModel"); }
        }
        private ImageListViewModel _imageListViewModel = new ImageListViewModel();

        public LoadingViewModel LoadingViewModel
        {
            get { return _loadingViewModel; }
            set { _loadingViewModel = value; RaisePropertyChanged("LoadingViewModel"); }
        }
        private LoadingViewModel _loadingViewModel = new LoadingViewModel();

        public ProgressLoadingViewModel ProgressLoadingViewModel
        {
            get { return _progressLoadingViewModel; }
            set { _progressLoadingViewModel = value; RaisePropertyChanged("ProgressLoadingViewModel"); }
        }
        private ProgressLoadingViewModel _progressLoadingViewModel = new ProgressLoadingViewModel();
        public ImageConverterViewModel ImageConverterViewModel
        {
            get { return _imageConverterViewModel; }
            set { _imageConverterViewModel = value; RaisePropertyChanged("ImageConverterViewModel"); }
        }
        private ImageConverterViewModel _imageConverterViewModel = new ImageConverterViewModel();

        public ImagePatternMatchViewModel ImagePatternMatchViewModel
        {
            get { return _imagePatternMatchViewModel; }
            set { _imagePatternMatchViewModel = value; RaisePropertyChanged("ImagePatternMatchViewModel"); }
        }
        private ImagePatternMatchViewModel _imagePatternMatchViewModel = new ImagePatternMatchViewModel();

        public ImageDifferenceViewModel ImageDifferenceViewModel
        {
            get { return _imageDifferenceViewModel; }
            set { _imageDifferenceViewModel = value; RaisePropertyChanged("ImageDifferenceViewModel"); }
        }
        private ImageDifferenceViewModel _imageDifferenceViewModel = new ImageDifferenceViewModel();

        public ImageMuiltConvertViewModel ImageMuiltConvertViewModel
        {
            get { return _imageMuiltConvertViewModel; }
            set { _imageMuiltConvertViewModel = value; RaisePropertyChanged("ImageMuiltConvertViewModel"); }
        }
        private ImageMuiltConvertViewModel _imageMuiltConvertViewModel = new ImageMuiltConvertViewModel();

        public ImageDetectorViewModel ImageDetectorViewModel
        {
            get { return _imageDetectorViewModel; }
            set { _imageDetectorViewModel = value; RaisePropertyChanged("ImageDetectorViewModel"); }
        }
        private ImageDetectorViewModel _imageDetectorViewModel = new ImageDetectorViewModel();

        #endregion ---------------------------------------------------------------------------------


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
        }

        void InitCommand()
        {
            CloseCommand = new RelayCommand<object>((param) => OnCloseCommand(param));
            CommandSelectMenu = new RelayCommand<object>((param) => OnCommandSelectMenu(param));
        }

        void InitEvent()
        {
            //받기(이벤트로 등록)
            Messenger.Default.Register<MessengerMain>(this, (msgData) =>
            {
                if (msgData.MessageId == "Loading")
                {
                    if (msgData.MessageValue == "true")
                    {
                        OnOffLoading(true, "0");
                    }
                    else
                    {
                        OnOffLoading(false, "0");
                    }
                }
                if (msgData.MessageId == "ProgressLoading")
                {
                    if (msgData.MessageValue == "true")
                    {
                        OnOffLoading(true, "1");
                    }
                    else
                    {
                        OnOffLoading(false, "1");
                    }
                }
                if (msgData.MessageId == "MessageBox")
                {
                    FooMessage(msgData.MessageStrValue[0], msgData.MessageStrValue[1]);
                }
                if(msgData.MessageId == "StartConvert")
                {
                    ConvertWork = new Thread(()=> ConvertWorkThreadMethod(msgData.TempConvertList));
                    ConvertWork.Start();
                }
            });
        }
        #endregion

        #region 이벤트
        void ConvertWorkThreadMethod(ObservableCollection<ImageConvertInfoModel> TempConvertList)
        {
            OnOffLoading(true, "1");
            IsEnabled = false;
            string[] TempProgressParam;
            TempProgressParam = new string[] { "0", ImageListViewModel.LoadImageListAll.Count.ToString(), "Initialize Inspect" };
            BaseMessageData msgData = new BaseMessageData("ProgressLoading", 3, TempProgressParam);
            for (int iLoofCount = 0; iLoofCount < ImageListViewModel.LoadImageListAll.Count; iLoofCount++)
            {
                for (int jLoofCount = 0; jLoofCount < TempConvertList.Count; jLoofCount++)
                {
                    RunConvertQueue(TempConvertList[jLoofCount].ConvertCommandName, ImageListViewModel.LoadImageListAll[iLoofCount].FileName_Full, ImageListViewModel.LoadImageListAll[iLoofCount].FileName_Full, TempConvertList[jLoofCount].ParamList);
                }
                TempProgressParam = new string[] { (iLoofCount + 1).ToString(), ImageListViewModel.LoadImageListAll.Count.ToString(), "Convert Image..." };
                msgData = new BaseMessageData("ProgressLoading", 3, TempProgressParam);
                Messenger.Default.Send<BaseMessageData>(msgData);
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
            parentWindow = Window.GetWindow((DependencyObject)param);
            parentWindow.Close();
        }


        #endregion

    }
}
