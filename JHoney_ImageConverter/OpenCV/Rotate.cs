using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class Rotate
    {
        public Mat RotateFromMat(Mat RawImage, double angle)
        {
            Mat dst = new Mat();
            Mat RotatedImage = Cv2.GetRotationMatrix2D(new Point2f(RawImage.Cols / 2, RawImage.Rows / 2), angle, 1);
            Cv2.WarpAffine(RawImage, dst, RotatedImage, RawImage.Size());

            return dst;
        }
    }
}
