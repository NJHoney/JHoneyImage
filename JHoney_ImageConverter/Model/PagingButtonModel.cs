
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class PagingButtonModel:ObservableRecipient
    {
        public string PageNum
        {
            get { return _pageNum; }
            set { _pageNum = value; OnPropertyChanged("PageNum"); }
        }
        private string _pageNum = "";

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private bool _isEnabled = true;

    }
}
