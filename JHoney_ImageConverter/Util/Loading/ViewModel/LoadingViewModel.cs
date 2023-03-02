
using CommunityToolkit.Mvvm.Input;
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
            set { _mainCanvas = value; OnPropertyChanged("MainCanvas"); }
        }
        private Canvas _mainCanvas = new Canvas();

        public double CanvasRotateTransform
        {
            get { return _canvasRotateTransform; }
            set { _canvasRotateTransform = value; OnPropertyChanged("CanvasRotateTransform"); }
        }
        private double _canvasRotateTransform = 0;

        public string LoadingText
        {
            get { return _loadingText; }
            set { _loadingText = value; OnPropertyChanged("LoadingText"); }
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
