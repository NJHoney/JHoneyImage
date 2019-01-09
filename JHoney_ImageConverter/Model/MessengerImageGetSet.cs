using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class MessengerImageGetSet
    {
        public string MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }
        string _messageId = "";
        public string MessageImagePath
        {
            get { return _messageImagePath; }
            set { _messageImagePath = value; }
        }
        string _messageImagePath = "";

        public string[] MessageStrValue
        {
            get { return _messageStrValue; }
            set { _messageStrValue = value; }
        }
        string[] _messageStrValue;

        public MessengerImageGetSet(string messageId, string messageImagePath, string[] messageStrValue = null)
        {
            MessageId = messageId;
            MessageImagePath = messageImagePath;
            MessageStrValue = messageStrValue;
        }
    }
}
