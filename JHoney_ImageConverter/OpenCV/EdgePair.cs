using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class EdgePair
    {
        public Mat imgTobinary(Mat rawImage, int threshold, int maxVal)
        {
            if (rawImage.Channels() != 1)
            {
                Cv2.CvtColor(rawImage, rawImage, ColorConversionCodes.RGB2GRAY);
            }
            Cv2.Threshold(rawImage, rawImage, threshold, maxVal, ThresholdTypes.Binary);

            return rawImage;
        }
        public void imgTobinary(string inputImgPath, string outputImgPath, int threshold, int maxVal)
        {
            Mat copyImage = Cv2.ImRead(inputImgPath, ImreadModes.GrayScale);

            Cv2.Threshold(copyImage, copyImage, threshold, maxVal, ThresholdTypes.Binary);

            copyImage.ImWrite(outputImgPath);
        }
    }
}
