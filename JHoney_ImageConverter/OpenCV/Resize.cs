using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class Resize
    {
        public Mat ResizeFromPath(Mat RawImage, int width, int height)
        {
            Mat dst = new Mat();
            Cv2.Resize(RawImage, dst, new Size(width, height), 0, 0, InterpolationFlags.Linear);

            return dst;
        }
        public void ResizeFromPath(string inputImgPath, string outputImgPath, int Width, int Height)
        {
            Mat rawImage = Cv2.ImRead(inputImgPath, ImreadModes.Unchanged);
            Mat croppedImage;

            croppedImage = rawImage.Resize(new Size(Width, Height));

            croppedImage.ImWrite(outputImgPath);

            rawImage.Dispose();
            croppedImage.Dispose();
        }
    }
}
