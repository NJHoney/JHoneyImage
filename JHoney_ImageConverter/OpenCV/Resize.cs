using ControlzEx.Standard;
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
        public Mat ResizeFromMat(Mat RawImage, int width, int height)
        {
            Mat dst = new Mat();
            Cv2.Resize(RawImage, dst, new Size(width, height), 0, 0, InterpolationFlags.Cubic);

            return dst;
        }
        public void ResizeFromPath(string inputImgPath, string outputImgPath, int Width, int Height)
        {
            Mat rawImage = Cv2.ImRead(inputImgPath, ImreadModes.Unchanged);
            Mat croppedImage;

            croppedImage = rawImage.Resize(new Size(Width, Height), 0, 0, InterpolationFlags.Cubic);

            croppedImage.ImWrite(outputImgPath);

            rawImage.Dispose();
            croppedImage.Dispose();
        }

        public Mat Resize_RatioFromMat(Mat src, int width, int height, int paddingValue)
        {           
            int srcWidth = src.Width;
            int srcHeight = src.Height;
            double srcRatio = (double)srcWidth / srcHeight;

            // 조정할 크기와 종횡비 가져오기
            double dstRatio = (double)width / height;

            // 종횡비에 따라 크기 계산
            int dstWidth = 0;
            int dstHeight = 0;
            int startX = 0, startY = 0;
            if (srcRatio > dstRatio)
            {
                // 입력 이미지가 더 가로로 길 경우
                dstWidth = width;
                dstHeight = (int)(dstWidth / srcRatio);
                startY = (height - dstHeight) / 2;
            }
            else
            {
                // 입력 이미지가 더 세로로 길 경우
                dstHeight = height;
                dstWidth = (int)(dstHeight * srcRatio);
                startX = (width - dstWidth) / 2;
            }

            // 이미지 크기 조정
            Mat dst = new Mat(new Size(width, height), src.Type(), new Scalar(255, 255, 255));

            // 이미지 크기 조정
            Mat resized = new Mat();
            Cv2.Resize(src, resized, new Size(dstWidth, dstHeight), interpolation: InterpolationFlags.Cubic);

            // 출력 이미지에 조정된 이미지 삽입
            resized.CopyTo(dst[new Rect(startX, startY, dstWidth, dstHeight)]);


            return dst;
        }
    }
}
