using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class ImageConvertInfoModel : ObservableRecipient
    {
        public string ConvertCommandName
        {
            get { return _convertCommandName; }
            set { _convertCommandName = value; OnPropertyChanged("ConvertCommandName"); }
        }
        private string _convertCommandName;

        /// <summary>
        /// Value
        /// </summary>
        public List<string> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; OnPropertyChanged("ParamList"); }
        }
        private List<string> _paramList = new List<string>();

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("IsSelected"); }
        }
        private bool _isSelected = false;
    }
}
