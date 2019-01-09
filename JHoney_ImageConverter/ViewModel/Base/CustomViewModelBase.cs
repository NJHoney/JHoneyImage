using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JHoney_ImageConverter.ViewModel.Base
{
    class CustomViewModelBase:ViewModelBase
    {
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); }
        }
        private bool _isEnabled = true;

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; RaisePropertyChanged("Visibility"); }
        }
        private Visibility _visibility = Visibility.Collapsed;
    }
}
