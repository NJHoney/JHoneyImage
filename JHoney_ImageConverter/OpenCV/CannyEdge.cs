using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class CannyEdge
    {
        public Mat cannyToImage(Mat RawImage, int threshold1)
        {
            if(RawImage.Channels()!=1)
            {
                RawImage = RawImage.CvtColor(ColorConversionCodes.BGR2GRAY);
            }
            Cv2.Canny(RawImage, RawImage, threshold1, threshold1 * 3);

            return RawImage;
        }
        public void cannyToImage(string inputPath, string outputPath, int threshold1)
        {
            Mat copyImage = Cv2.ImRead(inputPath, ImreadModes.Grayscale);
            Cv2.Canny(copyImage, copyImage, threshold1, threshold1 * 3);
            copyImage.ImWrite(outputPath);
        }
    }
}
