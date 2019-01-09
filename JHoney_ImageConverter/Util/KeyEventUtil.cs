using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JHoney_ImageConverter.Util
{
    class KeyEventUtil
    {
        public RelayCommand<KeyEventArgs> EventKeyDown { get; set; }
        public RelayCommand<KeyEventArgs> EventKeyUp { get; set; }
        public bool IsLeftCtrlDown { get; private set; }
        public KeyEventUtil()
        {
            EventKeyDown = new RelayCommand<KeyEventArgs>(EventKeyDownOnImage);
            EventKeyUp = new RelayCommand<KeyEventArgs>(EventKeyUpOnImage);
        }

        #region ---［ 키보드이벤트 ］---------------------------------------------------------------------

        public void EventKeyDownOnImage(KeyEventArgs obj)
        {
            if (Key.LeftCtrl == obj.Key)
            {
                IsLeftCtrlDown = true;
            }
        }

        public void EventKeyUpOnImage(KeyEventArgs obj)
        {
            IsLeftCtrlDown = false;
        }


        #endregion ---------------------------------------------------------------------------------
    }
}
