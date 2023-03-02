﻿
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JHoney_ImageConverter.ViewModel.Base
{
    class CustomViewModelBase:ObservableRecipient
    {
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private bool _isEnabled = true;

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; OnPropertyChanged("Visibility"); }
        }
        private Visibility _visibility = Visibility.Collapsed;
    }
}
