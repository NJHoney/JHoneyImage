
using CommunityToolkit.Mvvm.ComponentModel;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JHoney_ImageConverter.Model
{
    class ImageControlModel: ObservableRecipient
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
            set { _image = value; OnPropertyChanged("Image"); }
        }
        private Image _image = new Image();

        public double ImageScaleX
        {
            get { return _imageScaleX; }
            set { _imageScaleX = value; OnPropertyChanged("ImageScaleX"); }
        }
        private double _imageScaleX = 1;

        public double ImageScaleY
        {
            get { return _imageScaleY; }
            set { _imageScaleY = value; OnPropertyChanged("ImageScaleY"); }
        }
        private double _imageScaleY = 1;

        public double Image_XDPI
        {
            get { return _image_XDPI; }
            set { _image_XDPI = value; OnPropertyChanged("Image_XDPI"); }
        }
        private double _image_XDPI = 96;

        public double Image_YDPI
        {
            get { return _image_YDPI; }
            set { _image_YDPI = value; OnPropertyChanged("Image_YDPI"); }
        }
        private double _image_YDPI = 96;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; ImageSourceUpdate(_imagePath, "Image"); OnPropertyChanged("ImagePath"); }
        }
        private string _imagePath = "";
        #endregion ---------------------------------------------------------------------------------

        #region ---［ ImageBrush ］---------------------------------------------------------------------

        public ImageBrush ImageBrush
        {
            get { return _imageBrush; }
            set { _imageBrush = value; OnPropertyChanged("ImageBrush"); }
        }
        private ImageBrush _imageBrush = new ImageBrush();

        public double ImageBrushScaleX
        {
            get { return _imageBrushScaleX; }
            set { _imageBrushScaleX = value; OnPropertyChanged("ImageBrushScaleX"); }
        }
        private double _imageBrushScaleX = 1;

        public double ImageBrushScaleY
        {
            get { return _imageBrushScaleY; }
            set { _imageBrushScaleY = value; OnPropertyChanged("ImageBrushScaleY"); }
        }
        private double _imageBrushScaleY = 1;

        public double ImageBrush_XDPI
        {
            get { return _imageBrush_XDPI; }
            set { _imageBrush_XDPI = value; OnPropertyChanged("ImageBrush_XDPI"); }
        }
        private double _imageBrush_XDPI = 96;

        public double ImageBrush_YDPI
        {
            get { return _imageBrush_YDPI; }
            set { _imageBrush_YDPI = value; OnPropertyChanged("ImageBrush_YDPI"); }
        }
        private double _imageBrush_YDPI = 96;

        public string ImageBrushPath
        {
            get { return _imageBrushPath; }
            set { _imageBrushPath = value; ImageSourceUpdate(_imageBrushPath, "ImageBrush"); OnPropertyChanged("ImageBrushPath"); }
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
        
        public void ImageSourceUpdate(Mat MatImage, string Target)
        {
            if (MatImage == null)
            {
                return;
            }
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            MemoryStream ms = new MemoryStream();
            OpenCvSharp.Extensions.BitmapConverter.ToBitmap(MatImage).Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            b.StreamSource=ms;
            b.EndInit();
            

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
