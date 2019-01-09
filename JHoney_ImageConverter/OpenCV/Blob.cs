using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class Blob
    {
        public void GetBlob(string inputPath, string outputPath)
        {
            Mat src = new Mat(inputPath, ImreadModes.Unchanged);
            OpenCvSharp.Blob.CvBlob blob = new OpenCvSharp.Blob.CvBlob();
            OpenCvSharp.Blob.CvBlobs blobs = new OpenCvSharp.Blob.CvBlobs(src);
            blob = blobs.GreaterBlob();
            Rect extendedROI = new Rect(blob.Rect.X - 20, blob.Rect.Y - 20, blob.Rect.Width + 40, blob.Rect.Height + 40);
            Mat dst = src.Clone(extendedROI);
            dst.SaveImage(outputPath);
            src.Dispose();
            dst.Dispose();
        }
        public void TestBlob(string inputPath, string outputPath)
        {
            try
            {
                Mat src = new Mat(inputPath, ImreadModes.GrayScale);
                Cv2.Threshold(src, src, 50, 255, ThresholdTypes.Binary);
                OpenCvSharp.Blob.CvBlob blob = new OpenCvSharp.Blob.CvBlob();
                OpenCvSharp.Blob.CvBlobs blobs = new OpenCvSharp.Blob.CvBlobs(src);
                blob = blobs.GreaterBlob();
                int newX, newY, newWidth, newHeight;
                newX = blob.Rect.X;
                newY = blob.Rect.Y;
                if ((src.Width - blob.Rect.X - 10) > (blob.Rect.Width + 20))
                {
                    newWidth = blob.Rect.Width + 20;
                }
                else
                {
                    newWidth = src.Width - blob.Rect.X - 10;
                }
                if ((src.Height - blob.Rect.Y - 10) > (blob.Rect.Height + 20))
                {
                    newHeight = blob.Rect.Height + 20;
                }
                else
                {
                    newHeight = src.Height - blob.Rect.Y - 10;
                }

                Rect extendedROI = new Rect(newX, newY, newWidth, newHeight);
                Mat dst = new Mat(inputPath);
                Mat Label = new Mat(@"E:\Test\Schaeffler\label" + inputPath.Substring(inputPath.LastIndexOf("\\"), inputPath.Length - inputPath.LastIndexOf("\\")-4) + "_label.png");
                Label = Label.Clone(extendedROI);
                Label.SaveImage(@"E:\Test\Schaeffler\label" + inputPath.Substring(inputPath.LastIndexOf("\\"), inputPath.Length - inputPath.LastIndexOf("\\") - 4) + "_label.png");
                dst = dst.Clone(extendedROI);
                dst.SaveImage(outputPath);
                src.Dispose();
                dst.Dispose();
            }
            catch(Exception e)
            {
                return;
            }
            
        }
    }
}
