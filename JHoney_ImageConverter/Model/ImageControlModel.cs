using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JHoney_ImageConverter.Model
{
    class ImageControlModel:ViewModelBase
    {
        public ImageControlModel()
        {
            Image.Stretch = Stretch.None;
            ImageBrush.Stretch = Stretch.None;
        }

        #region ---［ Image ］---------------------------------------------------------------------

        public Image Image
        {
            get { return _image; }
            set { _image = value; RaisePropertyChanged("Image"); }
        }
        private Image _image = new Image();

        public double ImageScaleX
        {
            get { return _imageScaleX; }
            set { _imageScaleX = value; RaisePropertyChanged("ImageScaleX"); }
        }
        private double _imageScaleX = 1;

        public double ImageScaleY
        {
            get { return _imageScaleY; }
            set { _imageScaleY = value; RaisePropertyChanged("ImageScaleY"); }
        }
        private double _imageScaleY = 1;

        public double Image_XDPI
        {
            get { return _image_XDPI; }
            set { _image_XDPI = value; RaisePropertyChanged("Image_XDPI"); }
        }
        private double _image_XDPI = 96;

        public double Image_YDPI
        {
            get { return _image_YDPI; }
            set { _image_YDPI = value; RaisePropertyChanged("Image_YDPI"); }
        }
        private double _image_YDPI = 96;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; ImageSourceUpdate(_imagePath, "Image"); RaisePropertyChanged("ImagePath"); }
        }
        private string _imagePath = "";
        #endregion ---------------------------------------------------------------------------------

        #region ---［ ImageBrush ］---------------------------------------------------------------------

        public ImageBrush ImageBrush
        {
            get { return _imageBrush; }
            set { _imageBrush = value; RaisePropertyChanged("ImageBrush"); }
        }
        private ImageBrush _imageBrush = new ImageBrush();

        public double ImageBrushScaleX
        {
            get { return _imageBrushScaleX; }
            set { _imageBrushScaleX = value; RaisePropertyChanged("ImageBrushScaleX"); }
        }
        private double _imageBrushScaleX = 1;

        public double ImageBrushScaleY
        {
            get { return _imageBrushScaleY; }
            set { _imageBrushScaleY = value; RaisePropertyChanged("ImageBrushScaleY"); }
        }
        private double _imageBrushScaleY = 1;

        public double ImageBrush_XDPI
        {
            get { return _imageBrush_XDPI; }
            set { _imageBrush_XDPI = value; RaisePropertyChanged("ImageBrush_XDPI"); }
        }
        private double _imageBrush_XDPI = 96;

        public double ImageBrush_YDPI
        {
            get { return _imageBrush_YDPI; }
            set { _imageBrush_YDPI = value; RaisePropertyChanged("ImageBrush_YDPI"); }
        }
        private double _imageBrush_YDPI = 96;

        public string ImageBrushPath
        {
            get { return _imageBrushPath; }
            set { _imageBrushPath = value; ImageSourceUpdate(_imageBrushPath, "ImageBrush"); RaisePropertyChanged("ImageBrushPath"); }
        }
        private string _imageBrushPath = "";
        #endregion ---------------------------------------------------------------------------------

        public void ImageSourceUpdate(string ImagePath, string Target)
        {
            if (ImagePath == "" || ImagePath == null)
            {
                return;
            }
            BitmapImage b = new BitmapImage();
            b.UriSource = null;
            var stream = File.OpenRead(ImagePath);
            b.BeginInit();
            b.CacheOption = BitmapCacheOption.OnLoad;
            b.StreamSource = stream;
            b.EndInit();
            stream.Close();
            stream.Dispose();

            switch (Target)
            {
                case "Image":
                    Image.Source = b;
                    Image_XDPI = b.DpiX;
                    Image_YDPI = b.DpiY;
                    break;
                case "ImageBrush":
                    ImageBrush.ImageSource = b;
                    ImageBrush_XDPI = b.DpiX;
                    ImageBrush_YDPI = b.DpiY;
                    break;
            }

        }
    }
}
