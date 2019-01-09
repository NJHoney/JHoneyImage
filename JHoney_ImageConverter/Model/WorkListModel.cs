using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class WorkListModel:ViewModelBase
    {
        public string Header
        {
            get { return _header; }
            set { _header = value; RaisePropertyChanged("Header"); }
        }
        private string _header = "";

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged("IsSelected"); }
        }
        private bool _isSelected = false;
    }
}
