﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Model
{
    class ImageInfoModel:ObservableObject
    {
        public int Image__Width
        {
            get { return _info_Width; }
            set { _info_Width = value; RaisePropertyChanged("Image__Width"); }
        }
        private int _info_Width = 0;
        public int Image__Height
        {
            get { return _info_Height; }
            set { _info_Height = value; RaisePropertyChanged("Image__Height"); }
        }
        private int _info_Height = 0;

        public int Channel
        {
            get { return _imageInChannel; }
            set { _imageInChannel = value; RaisePropertyChanged("Channel"); }
        }
        private int _imageInChannel = 0;
        public int Channel__B
        {
            get { return _imageInChannel_B; }
            set { _imageInChannel_B = value; RaisePropertyChanged("Channel__B"); }
        }
        private int _imageInChannel_B = 0;
        public int Channel__G
        {
            get { return _imageInChannel_G; }
            set { _imageInChannel_G = value; RaisePropertyChanged("Channel__G"); }
        }
        private int _imageInChannel_G = 0;
        public int Channel__R
        {
            get { return _imageInChannel_R; }
            set { _imageInChannel_R = value; RaisePropertyChanged("Channel__R"); }
        }
        private int _imageInChannel_R = 0;
        public int Channel__A
        {
            get { return _imageInChannel_A; }
            set { _imageInChannel_A = value; RaisePropertyChanged("Channel__A"); }
        }
        private int _imageInChannel_A = 0;

        public int Mouse__X
        {
            get { return _imageInMouse_X; }
            set { _imageInMouse_X = value; RaisePropertyChanged("Mouse__X"); }
        }
        private int _imageInMouse_X = 0;
        public int Mouse__Y
        {
            get { return _imageInMouse_Y; }
            set { _imageInMouse_Y = value; RaisePropertyChanged("Mouse__Y"); }
        }
        private int _imageInMouse_Y = 0;

        public double Scale
        {
            get { return _scale; }
            set { _scale = value; RaisePropertyChanged("Scale"); }
        }
        private double _scale = 1.0f;
    }
}
