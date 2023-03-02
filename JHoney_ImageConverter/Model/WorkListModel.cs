using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class WorkListModel:ObservableRecipient
    {
        public string Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged("Header"); }
        }
        private string _header = "";

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        private bool _isSelected = false;
    }
}
