using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Util.Loading.ViewModel
{
    class ProgressLoadingViewModel:CustomViewModelBase
    {
        #region 프로퍼티

        public int ProgressMin
        {
            get { return _progressMin; }
            set { _progressMin = value; RaisePropertyChanged("ProgressMin"); }
        }
        private int _progressMin = 0;
        public int ProgressMax
        {
            get { return _progressMax; }
            set { _progressMax = value; RaisePropertyChanged("ProgressMax"); }
        }
        private int _progressMax = 1;

        public int ProgressCurrent
        {
            get { return _progressCurrent; }
            set { _progressCurrent = value; RaisePropertyChanged("ProgressCurrent"); }
        }
        private int _progressCurrent = 1;
        public string LoadingText
        {
            get { return _loadingText; }
            set { _loadingText = value; RaisePropertyChanged("LoadingText"); }
        }
        private string _loadingText = "Now Loading";


        public string SubMessageText
        {
            get { return _subMessageText; }
            set { _subMessageText = value; RaisePropertyChanged("SubMessageText"); }
        }
        private string _subMessageText = "";



        #endregion
        #region 커맨드
        //public RelayCommand<object> MyCommand { get; private set; }
        #endregion

        #region 초기화
        public ProgressLoadingViewModel()
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
            Messenger.Default.Register<BaseMessageData>(this, (msgData) =>
            {
                if (msgData.MessageId == "ProgressLoading")
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
                    else if (msgData.MessageValue == 3)
                    {
                        ProgressCurrent = int.Parse(msgData.MessageStrValue[0]);
                        ProgressMax = int.Parse(msgData.MessageStrValue[1]);
                        SubMessageText = msgData.MessageStrValue[2].ToString();
                    }
                }
            });
        }
        #endregion

        #region 이벤트
        /*
        private void OnMyCommand(object param)
            {

            }
            */
        #endregion
    }
}
