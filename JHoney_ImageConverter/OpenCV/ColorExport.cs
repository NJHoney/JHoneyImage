using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class ColorExport
    {
        public void SingleChannelExport(string SrcPath, string DstPath, int SetChannel)
        {
            Mat SrcImage = new Mat(SrcPath, ImreadModes.Unchanged);

            Mat[] SrcArray = SrcImage.Split();

            Mat DstImage = new Mat(new Size(SrcImage.Width,SrcImage.Height),MatType.CV_8UC3);
            Mat[] DstArray = new Mat[3];
            SrcImage = new Mat(SrcImage.Height, SrcImage.Width, MatType.CV_8UC1, new Scalar(0));

            DstArray[0] = SrcImage;
            DstArray[1] = SrcImage;
            DstArray[2] = SrcImage;
            switch (SetChannel)
            {
                case 0:
                    DstArray[0] = SrcArray[0];
                    break;
                case 1:
                    DstArray[1] = SrcArray[1];
                    break;
                case 2:
                    DstArray[2] = SrcArray[2];
                    break;
            }

            
            Cv2.Merge(DstArray, DstImage);
            //DstImage = DstImage.CvtColor(ColorConversionCodes.GRAY2BGR);



            DstImage.SaveImage(DstPath);

            SrcImage.Dispose();
            DstImage.Dispose();
        }
    }
}
