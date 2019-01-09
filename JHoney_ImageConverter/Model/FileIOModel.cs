using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JHoney_ImageConverter.Model
{
    class FileIOModel:ViewModelBase
    {
        public FileIOModel() { }
        public FileIOModel(string FileFullPath)
        {
            FileName_Full = FileFullPath;
            MakeProperty();
        }
        #region ---［ 프로퍼티 ］---------------------------------------------------------------------
        public BitmapImage ThumbnailBitmapImage
        {
            get { return _thumbnailBitmapImage; }
            set { _thumbnailBitmapImage = value; RaisePropertyChanged("ThumbnailBitmapImage"); }
        }
        private BitmapImage _thumbnailBitmapImage = new BitmapImage();

        public string ThumbnailPath
        {
            get { return _thumbnailPath; }
            set { _thumbnailPath = value; RaisePropertyChanged("ThumbnailPath"); }
        }
        private string _thumbnailPath = "";


        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged("IsSelected"); }
        }
        private bool _isSelected = false;


        /// <summary>
        /// Path+SafeFileName : C:\temp\temp.jpg
        /// </summary>
        public string FileName_Full
        {
            get { return _fileNameFull; }
            set { _fileNameFull = value; RaisePropertyChanged("FileNameFull"); }
        }
        private string _fileNameFull = "";

        /// <summary>
        /// Only Path : C:\temp\
        /// </summary>
        public string FileName_Path
        {
            get { return _fileNamePath; }
            set { _fileNamePath = value; RaisePropertyChanged("FileNamePath"); }
        }
        private string _fileNamePath = "";

        /// <summary>
        /// Only FileName + Extension : temp.jpg
        /// </summary>
        public string FileName_Safe
        {
            get { return _fileNameSafe; }
            set { _fileNameSafe = value; RaisePropertyChanged("FileNameSafe"); }
        }
        private string _fileNameSafe = "";

        /// <summary>
        /// Only FileName + Extension : temp
        /// </summary>
        public string FileName_OnlyName
        {
            get { return _fileName_OnlyName; }
            set { _fileName_OnlyName = value; RaisePropertyChanged("FileName_OnlyName"); }
        }
        private string _fileName_OnlyName = "";

        /// <summary>
        /// Extension : jpg
        /// </summary>
        public string FileName_Extension
        {
            get { return _fileName_Extension; }
            set { _fileName_Extension = value; RaisePropertyChanged("FileName_Extension"); }
        }
        private string _fileName_Extension = "";
        #endregion ---------------------------------------------------------------------------------

        #region ---［ Private 내부로직 ］---------------------------------------------------------------------
        private string GetOnlyPath(string FullName)
        {
            string OnlyPath = "";

            OnlyPath = FullName.Substring
                (
                0,
                FullName.LastIndexOf("\\") + 1
                );

            return OnlyPath;
        }

        private string GetSafeFileName(string FullName)
        {
            string SafeFileName = "";

            SafeFileName = FullName.Substring
                (
                FullName.LastIndexOf("\\") + 1,
                FullName.Length - FullName.LastIndexOf("\\") - 1
                );

            return SafeFileName;
        }

        private string GetOnlyName(string FullName)
        {
            string OnlyName = "";

            OnlyName = FullName.Substring
                (
                FullName.LastIndexOf("\\") + 1,
                FullName.LastIndexOf(".") - FullName.LastIndexOf("\\") - 1
                );

            return OnlyName;
        }

        private string GetExtension(string FullName)
        {
            string Extension = "";

            Extension = FullName.Substring
                (
                FullName.LastIndexOf(".") + 1,
                FullName.Length - FullName.LastIndexOf(".") - 1
                );

            return Extension;
        }
        #endregion ---------------------------------------------------------------------------------


        public void MakeProperty()
        {
            FileName_Path = GetOnlyPath(FileName_Full);
            FileName_Safe = GetSafeFileName(FileName_Full);
            FileName_OnlyName = GetOnlyName(FileName_Full);
            FileName_Extension = GetExtension(FileName_Full);
        }
    }
}
