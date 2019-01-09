using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class ErodeDilate
    {
        public void Erode(string SrcPath, string DstPath)
        {
            Mat SrcImage = new Mat(SrcPath, ImreadModes.Unchanged);
            Mat mask = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3), new Point(1, 1));
            Cv2.Erode(SrcImage, SrcImage, mask);

            SrcImage.SaveImage(DstPath);

            SrcImage.Dispose();
        }
        public void Dilate(string SrcPath, string DstPath)
        {
            Mat SrcImage = new Mat(SrcPath, ImreadModes.Unchanged);
            Mat mask = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3), new Point(1, 1));
            Cv2.Dilate(SrcImage, SrcImage, mask);

            SrcImage.SaveImage(DstPath);

            SrcImage.Dispose();
        }
    }
}
