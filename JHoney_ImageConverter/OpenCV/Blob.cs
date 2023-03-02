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
            //OpenCvSharp.SimpleBlobDetector
            //Mat src = new Mat(inputPath, ImreadModes.Unchanged);
            //OpenCvSharp.Blob.CvBlob blob = new OpenCvSharp.Blob.CvBlob();
            //OpenCvSharp.Blob.CvBlobs blobs = new OpenCvSharp.Blob.CvBlobs(src);
            //blob = blobs.GreaterBlob();
            //Rect extendedROI = new Rect(blob.Rect.X - 20, blob.Rect.Y - 20, blob.Rect.Width + 40, blob.Rect.Height + 40);
            //Mat dst = src.Clone(extendedROI);
            //dst.SaveImage(outputPath);
            //src.Dispose();
            //dst.Dispose();
        }
        public void TestBlob(string inputPath, string outputPath)
        {
            try
            {
                Mat src = new Mat(inputPath, ImreadModes.Unchanged);
                src = ~src;
                //Mat mask = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3), new Point(1, 1));
                //Cv2.Dilate(src, src, mask);
                //Cv2.Dilate(src, src, mask);
                //Cv2.Dilate(src, src, mask);
                
                //Cv2.Threshold(src, src, 150, 255, ThresholdTypes.Binary);

                ////Cv2.Canny(src, src, 25, 75);
                //src.SaveImage(@"E:\Test\인터노조\실험\1.png");
                //src = ~src;
                //testcode
                
                //Mat mask = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3), new Point(1, 1));
                //

                //OpenCvSharp.Blob.CvBlob blob = new OpenCvSharp.Blob.CvBlob();
                //OpenCvSharp.Blob.CvBlobs blobs = new OpenCvSharp.Blob.CvBlobs(src);
                //var blobList = blobs.Where(x => x.Value.Rect.Width < 800
                //&& x.Value.Rect.Height < 800
                //&& x.Value.Rect.Width > 600
                //&& x.Value.Rect.Height > 600
                //).ToList().OrderByDescending(x=>x.Value.Rect.Width);

                //blob = blobs.GreaterBlob();

                //blob = blobs.Where(x=>x.Value.Rect.Width>200 && x.Value.Rect.Height>150 && x.Value.Rect.Width < 300).First().Value;

                //int newX, newY, newWidth, newHeight;
                //newX = blob.Rect.X-4>=0 ? blob.Rect.X - 4  : 0;
                //newY = blob.Rect.Y - 4 >= 0 ? blob.Rect.Y - 4 : 0;
                //newX = blob.Rect.X + (blob.Rect.Width/2) -184 >= 0 ? blob.Rect.X + (blob.Rect.Width / 2) - 184 : 0;
                //newY = blob.Rect.Y + (blob.Rect.Height / 2) - 184 >= 0 ? blob.Rect.Y + (blob.Rect.Height/ 2) - 184 : 0;
                //newWidth = blob.Rect.Width;
                //newHeight = blob.Rect.Height;
                //newWidth = 368;
                //newHeight = 368;

                //testcode
                //var select = blobList.First();
                //newX = select.Value.Rect.X;
                //newY = select.Value.Rect.Y;
                //newWidth = select.Value.Rect.Width;
                //newHeight = select.Value.Rect.Height;

                //Mat target = new Mat(inputPath, ImreadModes.GrayScale);
                //target = target.Clone(new Rect(newX,newY,newWidth,newHeight));
                //target.SaveImage(outputPath);
                //var k = Cv2.FitEllipse(target);
                //
                //Console.WriteLine(inputPath + " : " + blobs.Count);
                //Rect extendedROI = new Rect(newX, newY, newWidth, newHeight);
                //Mat dst = new Mat(inputPath, ImreadModes.Color);
                ////Mat Label = new Mat(@"E:\Suakit_work\160. 필옵틱스(SDC향)\original" + inputPath.Substring(inputPath.LastIndexOf("\\"), inputPath.Length - inputPath.LastIndexOf("\\")-4) + "_label.png");
                ////Label = Label.Clone(extendedROI);
                ////Label.SaveImage(@"E:\Test\Schaeffler\label" + inputPath.Substring(inputPath.LastIndexOf("\\"), inputPath.Length - inputPath.LastIndexOf("\\") - 4) + "_label.png");
                //dst = dst.Clone(extendedROI);
                //dst.SaveImage(outputPath.Substring(0,outputPath.Length-3) + "png");
                src.Dispose();
                //dst.Dispose();
            }
            catch(Exception e)
            {
                return;
            }
            
        }
    }
}
