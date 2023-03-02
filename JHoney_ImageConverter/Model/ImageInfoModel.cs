using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class ImageInfoModel:ObservableRecipient
    {
        public int Image__Width
        {
            get { return _info_Width; }
            set { _info_Width = value; OnPropertyChanged("Image__Width"); }
        }
        private int _info_Width = 0;
        public int Image__Height
        {
            get { return _info_Height; }
            set { _info_Height = value; OnPropertyChanged("Image__Height"); }
        }
        private int _info_Height = 0;

        public int Channel
        {
            get { return _imageInChannel; }
            set { _imageInChannel = value; OnPropertyChanged("Channel"); }
        }
        private int _imageInChannel = 0;
        public int Channel__B
        {
            get { return _imageInChannel_B; }
            set { _imageInChannel_B = value; OnPropertyChanged("Channel__B"); }
        }
        private int _imageInChannel_B = 0;
        public int Channel__G
        {
            get { return _imageInChannel_G; }
            set { _imageInChannel_G = value; OnPropertyChanged("Channel__G"); }
        }
        private int _imageInChannel_G = 0;
        public int Channel__R
        {
            get { return _imageInChannel_R; }
            set { _imageInChannel_R = value; OnPropertyChanged("Channel__R"); }
        }
        private int _imageInChannel_R = 0;
        public int Channel__A
        {
            get { return _imageInChannel_A; }
            set { _imageInChannel_A = value; OnPropertyChanged("Channel__A"); }
        }
        private int _imageInChannel_A = 0;

        public int Mouse__X
        {
            get { return _imageInMouse_X; }
            set { _imageInMouse_X = value; OnPropertyChanged("Mouse__X"); }
        }
        private int _imageInMouse_X = 0;
        public int Mouse__Y
        {
            get { return _imageInMouse_Y; }
            set { _imageInMouse_Y = value; OnPropertyChanged("Mouse__Y"); }
        }
        private int _imageInMouse_Y = 0;

        public double Scale
        {
            get { return _scale; }
            set { _scale = value; OnPropertyChanged("Scale"); }
        }
        private double _scale = 1.0f;
    }
}
