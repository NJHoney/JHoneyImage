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
            Mat[] Merged = new Mat[4];
            Mat mask = rawImage.Clone();
            Mat resImg = new Mat(rawImage.Size(), MatType.CV_8UC4);
            //mask = ~mask;
            Cv2.Threshold(mask, mask, 1, 255, ThresholdTypes.Binary);
            //string temp = rawImage.ToString();
            ////temp = temp.Substring(temp.IndexOf("*", 1), temp.IndexOf(",") - temp.IndexOf("*", 1));
            ////temp = temp.Substring(temp.IndexOf("*", 2) + 1, temp.Length - temp.IndexOf("*", 2) - 1);
            //mask.SaveImage(@"E:\Test\인터노조\Transparent\1.png");
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
                    //DstImage.ConvertTo(rawImage, MatType.CV_8UC4);
                    Mat newa = new Mat(384, 455, MatType.CV_8UC1, new Scalar(255));
                    
                    Merged[0] = DstImage;
                    Merged[1] = DstImage;
                    Merged[2] = DstImage;
                    Merged[3] = mask;

                    Cv2.Merge(Merged, DstImage);
                    break;
            }

            //test
            //resImg.SaveImage(@"E:\Test\인터노조\Transparent\2.png");
            //resImg.SaveImage(outputImgPath);
            //resImg.Dispose();
            //mask.Dispose();
            //

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
