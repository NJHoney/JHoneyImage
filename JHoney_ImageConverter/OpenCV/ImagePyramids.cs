using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class ImagePyramids
    {
        public void PyrUp(string SrcPath, string DstPath)
        {
            Mat SrcImage = new Mat(SrcPath, ImreadModes.Unchanged);

            Cv2.PyrUp(SrcImage, SrcImage);

            SrcImage.SaveImage(DstPath);

            SrcImage.Release();
        }
        public void PyrDown(string SrcPath, string DstPath)
        {
            Mat SrcImage = new Mat(SrcPath, ImreadModes.Unchanged);

            Cv2.PyrDown(SrcImage, SrcImage);

            SrcImage.SaveImage(DstPath);

            SrcImage.Release();
        }
    }
}
