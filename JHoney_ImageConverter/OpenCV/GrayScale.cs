using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class GrayScale
    {
        public void imgToGrayscale(string inputImgPath, string outputImgPath)
        {
            Mat copyImage = Cv2.ImRead(inputImgPath, ImreadModes.GrayScale);

            copyImage.ImWrite(outputImgPath);

            copyImage.Dispose();
        }
    }
}
