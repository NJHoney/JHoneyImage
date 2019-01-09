using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class MedianBlur
    {
        public Mat imgToMedian(Mat rawImage, int kSize)
        {
            if (kSize % 2 == 0 && kSize > 0)
            {
                kSize--;
            }
            else if (kSize == 0)
            {
                kSize = 1;
            }
            Cv2.MedianBlur(rawImage, rawImage, kSize);

            return rawImage;
        }
    }
}
