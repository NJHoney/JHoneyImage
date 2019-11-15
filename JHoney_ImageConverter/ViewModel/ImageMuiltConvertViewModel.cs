using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JHoney_ImageConverter.Model;
using JHoney_ImageConverter.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.ViewModel
{
    class ImageMuiltConvertViewModel:CustomViewModelBase
    {

        #region 프로퍼티

        public ObservableCollection<WorkListModel> WorkList
        {
            get { return _workList; }
            set { _workList = value; RaisePropertyChanged("WorkList"); }
        }
        private ObservableCollection<WorkListModel> _workList = new ObservableCollection<WorkListModel>();


        #region ---［ ParamSetting ］---------------------------------------------------------------------

        public bool ParamVisibility1
        {
            get { return _paramVisibility1; }
            set { _paramVisibility1 = value; RaisePropertyChanged("ParamVisibility1"); }
        }
        private bool _paramVisibility1 = false;

        public bool ParamVisibility2
        {
            get { return _paramVisibility2; }
            set { _paramVisibility2 = value; RaisePropertyChanged("ParamVisibility2"); }
        }
        private bool _paramVisibility2 = false;

        public bool ParamVisibility3
        {
            get { return _paramVisibility3; }
            set { _paramVisibility3 = value; RaisePropertyChanged("ParamVisibility3"); }
        }
        private bool _paramVisibility3 = false;

        public bool ParamVisibility4
        {
            get { return _paramVisibility4; }
            set { _paramVisibility4 = value; RaisePropertyChanged("ParamVisibility4"); }
        }
        private bool _paramVisibility4 = false;

        public string ParamText1
        {
            get { return _paramText1; }
            set { _paramText1 = value; RaisePropertyChanged("ParamText1"); }
        }
        private string _paramText1 = "";

        public string ParamText2
        {
            get { return _paramText2; }
            set { _paramText2 = value; RaisePropertyChanged("ParamText2"); }
        }
        private string _paramText2 = "";

        public string ParamText3
        {
            get { return _paramText3; }
            set { _paramText3 = value; RaisePropertyChanged("ParamText3"); }
        }
        private string _paramText3 = "";

        public string ParamText4
        {
            get { return _paramText4; }
            set { _paramText4 = value; RaisePropertyChanged("ParamText4"); }
        }
        private string _paramText4 = "";

        public string Param1
        {
            get { return _param1; }
            set { _param1 = value; RaisePropertyChanged("Param1"); }
        }
        private string _param1 = "";

        public string Param2
        {
            get { return _param2; }
            set { _param2 = value; RaisePropertyChanged("Param2"); }
        }
        private string _param2 = "";

        public string Param3
        {
            get { return _param3; }
            set { _param3 = value; RaisePropertyChanged("Param3"); }
        }
        private string _param3 = "";

        public string Param4
        {
            get { return _param4; }
            set { _param4 = value; RaisePropertyChanged("Param4"); }
        }
        private string _param4 = "";
        #endregion ---------------------------------------------------------------------------------
        public ObservableCollection<ImageConvertInfoModel> ConvertCommandList
        {
            get { return _convertCommandList; }
            set { _convertCommandList = value; RaisePropertyChanged("ConvertCommandList"); }
        }
        private ObservableCollection<ImageConvertInfoModel> _convertCommandList = new ObservableCollection<ImageConvertInfoModel>();


        #endregion
        #region 커맨드
        public RelayCommand<object> CommandRun { get; private set; }
        public RelayCommand<object> CommandConvertSelect { get; private set; }
        public RelayCommand<object> AddCommand { get; private set; }
        public RelayCommand<object> DelCommand { get; private set; }
        
        #endregion

        #region 초기화
        public ImageMuiltConvertViewModel()
        {
            InitData();
            InitCommand();
            InitEvent();
        }

        void InitData()
        {
            WorkList.Add(new WorkListModel() { Header = "GrayScale", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Dilate", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Erode", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Reverse", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Binary", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "EdgeCanny", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Crop", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Resize", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "ChangeChannel", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "FindPattern", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "MergeImage", IsSelected = false });
            WorkList.Add(new WorkListModel() { Header = "Test", IsSelected = false });
        }

        void InitCommand()
        {
            CommandRun = new RelayCommand<object>((param) => OnCommandRun(param));
            CommandConvertSelect = new RelayCommand<object>((param) => OnCommandConvertSelect(param));
            AddCommand = new RelayCommand<object>((param) => OnAddCommand(param));
            DelCommand = new RelayCommand<object>((param) => OnDelCommand(param));
        }

        
        void InitEvent()
        {

        }
        #endregion

        #region 이벤트

        private void OnCommandRun(object param)
        {
            MessengerMain msgData = new MessengerMain("StartConvert", "true", null, null, ConvertCommandList);
            Messenger.Default.Send<MessengerMain>(msgData);
        }

        private void OnAddCommand(object param)
        {
            List<string> tempList = new List<string>() { Param1, Param2, Param3, Param4 };
            foreach (WorkListModel ICM in WorkList)
            {
                if (ICM.IsSelected)
                {
                    switch (ICM.Header)
                    {
                        case "GrayScale":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Dilate":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Erode":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Reverse":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Binary":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "EdgeCanny":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "EdgeSobel":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header });
                            break;
                        case "Crop":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "Resize":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "ChangeChannel":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "FindPattern":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "MergeImage":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                        case "Test":
                            ConvertCommandList.Add(new ImageConvertInfoModel() { ConvertCommandName = ICM.Header, ParamList = tempList });
                            break;
                    }
                }
            }
        }

        private void OnDelCommand(object param)
        {
            if(param.ToString()=="-1")
            {
                if (ConvertCommandList.Count > 0)
                    ConvertCommandList.RemoveAt(0);
            }
            else
            {
                if (ConvertCommandList.Count > 0)
                    ConvertCommandList.RemoveAt(int.Parse(param.ToString()));
            }
        }

        private void OnCommandConvertSelect(object param)
        {

            for (int iLoopCount = 0; iLoopCount < WorkList.Count; iLoopCount++)
            {
                WorkList[iLoopCount].IsSelected = false;
            }

            switch(param.ToString())
            {
                case "GrayScale":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[0].IsSelected = true;
                    break;
                case "Dilate":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[1].IsSelected = true;
                    break;
                case "Erode":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[2].IsSelected = true;
                    break;
                case "Reverse":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[3].IsSelected = true;
                    break;
                case "Binary":
                    ParamText1 = "Threshold";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[4].IsSelected = true;
                    break;
                case "EdgeCanny":
                    ParamText1 = "Threshold";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[5].IsSelected = true;
                    break;
                case "Crop":
                    ParamText1 = "Start-X";
                    ParamText2 = "Start-Y";
                    ParamText3 = "Width";
                    ParamText4 = "Height";
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = true;
                    WorkList[6].IsSelected = true;
                    break;
                case "Resize":
                    ParamText1 = "Width";
                    ParamText2 = "Height";
                    ParamVisibility1 = ParamVisibility2 = true;
                    ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[7].IsSelected = true;
                    break;
                case "ChangeChannel":
                    ParamText1 = "Channel";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[8].IsSelected = true;
                    break;
                case "FindPattern":
                    ParamText1 = "PatternImage-Path";
                    ParamVisibility1 = true;
                    ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[9].IsSelected = true;
                    break;
                case "MergeImage":
                    ParamText1 = "B Channel Path";
                    ParamText2 = "G Channel Path";
                    ParamText3 = "R Channel Path";
                    ParamText4 = "A Channel Path(Empty is disable)";
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = true;
                    WorkList[10].IsSelected = true;
                    break;
                case "Test":
                    ParamVisibility1 = ParamVisibility2 = ParamVisibility3 = ParamVisibility4 = false;
                    WorkList[11].IsSelected = true;
                    break;

            }
        }
        #endregion

    }
}
