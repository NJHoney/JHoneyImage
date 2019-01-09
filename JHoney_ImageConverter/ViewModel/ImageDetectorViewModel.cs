using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.ViewModel.Base;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.ViewModel
{
    class ImageDetectorViewModel:CustomViewModelBase
    {

        #region 프로퍼티
        string tempImgPath = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
        string tempImg = AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + "temp.png";

        public ImageControlModel ImageShow
        {
            get { return _imageShow; }
            set { _imageShow = value; RaisePropertyChanged("ImageShow"); }
        }
        private ImageControlModel _imageShow = new ImageControlModel();
        /*
        public int MyVariable
          {
              get { return _myVariable; }
              set { _myVariable = value; RaisePropertyChanged("MyVariable"); }
          }
          private int _myVariable;
          */
        #endregion
        #region 커맨드
        //public RelayCommand<object> MyCommand { get; private set; }
        #endregion

        #region 초기화
        public ImageDetectorViewModel()
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
            //MyCommand = new RelayCommand<object>((param) => OnMyCommand(param));
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
                        //UpdateImageInfo();

                        ImageShow.ImageSourceUpdate(tempImg, "ImageBrush");
                    }
                }
            });
        }
        #endregion

        #region 이벤트
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
        /*
        private void OnMyCommand(object param)
            {

            }
            */
        #endregion

    }
}
