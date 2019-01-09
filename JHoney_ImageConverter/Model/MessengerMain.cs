using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class MessengerMain
    {
        public string MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }
        string _messageId = "";
        public string MessageValue
        {
            get { return _messageValue; }
            set { _messageValue = value; }
        }
        string _messageValue = "";

        public string[] MessageStrValue
        {
            get { return _messageStrValue; }
            set { _messageStrValue = value; }
        }
        string[] _messageStrValue;

        public ObservableCollection<FileIOModel> TempImageList
        {
            get { return _tempImageList; }
            set { _tempImageList = value; }
        }
        ObservableCollection<FileIOModel> _tempImageList = new ObservableCollection<FileIOModel>();

        public ObservableCollection<ImageConvertInfoModel> TempConvertList
        {
            get { return _tempConvertList; }
            set { _tempConvertList = value; }
        }
        ObservableCollection<ImageConvertInfoModel> _tempConvertList = new ObservableCollection<ImageConvertInfoModel>();
        

        public MessengerMain(string messageId, string messageValue, string[] messageStrValue = null, ObservableCollection<FileIOModel> tempImageList = null, ObservableCollection<ImageConvertInfoModel> tempConvertList = null)
        {
            MessageId = messageId;
            MessageValue = messageValue;
            MessageStrValue = messageStrValue;
            TempImageList = tempImageList;
            TempConvertList = tempConvertList;
        }
    }
}
