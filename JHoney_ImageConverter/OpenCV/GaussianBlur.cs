using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class GaussianBlur
    {
        public Mat gaussianToImg(Mat rawImage, int maskSize)
        {
            if (maskSize % 2 == 0 && maskSize > 0)
            {
                maskSize--;
            }
            else if (maskSize == 0)
            {
                maskSize = 1;
            }
            Cv2.GaussianBlur(rawImage, rawImage, new Size(maskSize, maskSize), 0);

            return rawImage;
        }
    }
}
