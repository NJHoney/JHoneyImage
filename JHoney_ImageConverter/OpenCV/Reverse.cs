using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class Reverse
    {
        public void ImgReverse(string inputRawImgPath, string outputPath)
        {
            Mat copyImage = Cv2.ImRead(inputRawImgPath, ImreadModes.Unchanged);
            copyImage = ~copyImage;
            copyImage.ImWrite(outputPath);

            copyImage.Dispose();
        }
    }
}
