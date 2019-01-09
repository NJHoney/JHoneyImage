using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JHoney_ImageConverter.Util.Loading.ViewModel
{
    class LoadingViewModel:CustomViewModelBase
    {
        #region 프로퍼티

        public Canvas MainCanvas
        {
            get { return _mainCanvas; }
            set { _mainCanvas = value; RaisePropertyChanged("MainCanvas"); }
        }
        private Canvas _mainCanvas = new Canvas();

        public double CanvasRotateTransform
        {
            get { return _canvasRotateTransform; }
            set { _canvasRotateTransform = value; RaisePropertyChanged("CanvasRotateTransform"); }
        }
        private double _canvasRotateTransform = 0;

        public string LoadingText
        {
            get { return _loadingText; }
            set { _loadingText = value; RaisePropertyChanged("LoadingText"); }
        }
        private string _loadingText = "Now Loading";

        #endregion
        #region 커맨드
        public RelayCommand<object> CanvasLoaded { get; private set; }
        #endregion

        #region 초기화
        public LoadingViewModel()
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
            CanvasLoaded = new RelayCommand<object>((param) => OnCanvasLoaded(param));
        }

        void InitEvent()
        {

            //받기(이벤트로 등록)
            Messenger.Default.Register<BaseMessageData>(this, (msgData) =>
            {
                if (msgData.MessageId == "Loading")
                {
                    if (msgData.MessageValue == 0)
                    {

                    }
                    else if (msgData.MessageValue == 1)
                    {
                        Visibility = System.Windows.Visibility.Visible;
                    }
                    else if (msgData.MessageValue == 2)
                    {
                        Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                else if (msgData.MessageId == "Close")
                {
                }
            });



        }
        #endregion

        #region 이벤트

        private void OnCanvasLoaded(object param)
        {
            if (param == null)
            {
                return;
            }

            MainCanvas = param as Canvas;

            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;


            CanvasRotateTransform = 0;


        }
        private static void SetPosition(DependencyObject ellipse, double offset, double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 50 + (Math.Sin(offset + (posOffSet * step)) * 50));
            ellipse.SetValue(Canvas.TopProperty, 50 + (Math.Cos(offset + (posOffSet * step)) * 50));
        }

        #endregion
    }
}
