using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class BaseMessageData
    {
        public string MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }
        string _messageId = "";
        public int MessageValue
        {
            get { return _messageValue; }
            set { _messageValue = value; }
        }
        int _messageValue = 0;

        public string[] MessageStrValue
        {
            get { return _messageStrValue; }
            set { _messageStrValue = value; }
        }
        string[] _messageStrValue;

        public BaseMessageData(string messageId, int messageValue, string[] messageStrValue = null)
        {
            MessageId = messageId;
            MessageValue = messageValue;
            MessageStrValue = messageStrValue;
        }
    }
}
