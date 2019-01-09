using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class ChaangeImageBits
    {
        public void ChangeImageBits(string inputImgPath, string outputImgPath, int Bits, string Extension = "png")
        {
            Mat rawImage = Cv2.ImRead(inputImgPath, ImreadModes.Unchanged);
            Mat DstImage = rawImage.Clone();
            Mat[] Merged = new Mat[3];
            //string temp = rawImage.ToString();
            ////temp = temp.Substring(temp.IndexOf("*", 1), temp.IndexOf(",") - temp.IndexOf("*", 1));
            ////temp = temp.Substring(temp.IndexOf("*", 2) + 1, temp.Length - temp.IndexOf("*", 2) - 1);

            MatType CurrentType = MatType.MakeType(rawImage.Depth(), rawImage.Channels());

            switch (Bits)
            {
                case 1:
                    if(CurrentType==MatType.CV_8UC1)
                    {

                    }
                    else
                    {
                        DstImage.ConvertTo(rawImage, MatType.CV_8UC1);
                        Cv2.CvtColor(rawImage, DstImage, ColorConversionCodes.BGR2GRAY);
                    }
                    
                    break;
                case 3:
                    if(CurrentType == MatType.CV_8UC1)
                    {
                        Merged[0] = rawImage;
                        Merged[1] = rawImage;
                        Merged[2] = rawImage;
                        Cv2.Merge(Merged, DstImage);
                    }
                    else
                    {
                        DstImage.ConvertTo(rawImage, MatType.CV_8UC3);
                    }
                    
                    break;
                case 4:
                    DstImage.ConvertTo(rawImage, MatType.CV_8UC4);
                    break;
            }

            //DstImage.SaveImage(outputImgPath.Substring(0, outputImgPath.LastIndexOf(".")) + "." + Extension);
            DstImage.SaveImage(outputImgPath);
            DstImage.Dispose();
            rawImage.Dispose();
            //if(temp=="CV_8UC1")
            //{
            //    DstImage = rawImage.CvtColor(ColorConversionCodes.GRAY2RGB);
            //    DstImage.SaveImage(outputImgPath.Substring(0, outputImgPath.LastIndexOf(".")) + ".bmp");
            //    DstImage.Dispose();
            //}

            //rawImage.ConvertTo(rawImage, MatType.CV_8UC3);
            //rawImage.CvtColor(ColorConversionCodes.GRAY2RGB);
            //croppedImage.ImWrite(outputImgPath);

        }
    }
}
