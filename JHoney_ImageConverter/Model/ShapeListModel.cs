using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace JHoney_ImageConverter.Model
{
    internal class ShapeListModel: ObservableRecipient
    {

        public Shape Shape
        {
            get { return _shape; }
            set { _shape = value; OnPropertyChanged("Shape"); }
        }
        private Shape _shape;

        public string TypeName = "";

        public string uuid = "";

        public Point Point
        {
            get { return _point; }
            set { _point = value; OnPropertyChanged("Point"); }
        }
        private Point _point;

        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }
        private double _width=0;

        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); }
        }
        private double _height = 0;
    }
}
