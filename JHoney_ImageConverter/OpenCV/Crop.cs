using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class Crop
    {
        public void cropImageSave(string inputImgPath, string outputImgPath, int startX, int startY, int cropWidth, int cropHeight)
        {
            Mat rawImage = Cv2.ImRead(inputImgPath, ImreadModes.Unchanged);
            if(rawImage.Type().Channels==512)
            {
                rawImage = Cv2.ImRead(inputImgPath, ImreadModes.GrayScale);
            }
            Mat croppedImage;
            Rect cropROI;
            cropROI.X = startX;
            cropROI.Y = startY;
            cropROI.Width = cropWidth;
            cropROI.Height = cropHeight;

            croppedImage = rawImage.Clone(cropROI);

            croppedImage.ImWrite(outputImgPath);

            rawImage.Dispose();
            croppedImage.Dispose();
        }
    }
}
