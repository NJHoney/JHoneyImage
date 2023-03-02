
using CommunityToolkit.Mvvm.ComponentModel;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace JHoney_ImageConverter.Model
{
    class PatternResultModel:ObservableRecipient
    {
        public Rect RectInfo
        {
            get { return _rectInfo; }
            set { _rectInfo = value; OnPropertyChanged("RectInfo"); }
        }
        private Rect _rectInfo = new Rect();

        public double ScoreInfo
        {
            get { return _scoreInfo; }
            set { _scoreInfo = value; OnPropertyChanged("ScoreInfo"); }
        }
        private double _scoreInfo = 0;


    }
}
