using JHoney_ImageConverter.Model;
using OpenCvSharp;
using OpenCvSharp.Cuda;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.OpenCV
{
    class MatchTemplate
    {
        public ObservableCollection<PatternResultModel> DoMatchTemplateReturnResultList(string rawImagePath, string targetImagePath, double Threshold)
        {
            ObservableCollection<PatternResultModel> ResultList = new ObservableCollection<PatternResultModel>();

            int result_cols, result_rows;

            Mat SrcImage = Cv2.ImRead(rawImagePath, ImreadModes.GrayScale);

            Mat MatchImage = Cv2.ImRead(targetImagePath, ImreadModes.GrayScale);


            int cropWidth = MatchImage.Width;
            int cropHeight = MatchImage.Height;

            result_cols = SrcImage.Cols - MatchImage.Cols + 1;
            result_rows = SrcImage.Rows - MatchImage.Rows + 1;

            Mat ResultImage = new Mat(result_rows, result_cols, MatType.CV_32FC1);


            List<Rect> RectList = new List<Rect>();
            List<double> ScoreList = new List<double>();
            Rect TempRect;
            OpenCvSharp.Point matchLocation;
            TemplateMatchModes match_method = TemplateMatchModes.CCorrNormed;

            // Do the Matching and Normalize
            Cv2.MatchTemplate(SrcImage, MatchImage, ResultImage, match_method);
            Cv2.Normalize(ResultImage, ResultImage, 1, 0, NormTypes.MinMax, -1);



            double minValue, maxValue;//score
            double value;
            int x, y;
            double CurrentThreshold;
            int CurrentCount = 0;
            minValue = 0.1;
            maxValue = 0.9;
            int SelectCount = 0;

            for (y = 0; y < result_rows; y++)
            {
                for (x = 0; x < result_cols; x++)
                {
                    value = ResultImage.At<float>(y, x);

                    if (match_method == TemplateMatchModes.SqDiff || match_method == TemplateMatchModes.SqDiffNormed)
                    {
                        CurrentThreshold = minValue;

                        if (value < Threshold)
                        {
                            matchLocation.X = x;
                            matchLocation.Y = y;
                            TempRect = new Rect(matchLocation.X, matchLocation.Y, MatchImage.Width, MatchImage.Height);
                            ResultList.Add(new PatternResultModel() { RectInfo = TempRect, ScoreInfo = value });
                            //ScoreList.Add(value);
                            //RectList.Add(TempRect);
                            CurrentCount++;
                        }
                    }
                    else
                    {
                        //CurrentThreshold = maxValue;

                        if (value > Threshold)
                        {
                            matchLocation.X = x;
                            matchLocation.Y = y;

                            {
                                TempRect = new Rect(matchLocation.X, matchLocation.Y, MatchImage.Width, MatchImage.Height);
                                ResultList.Add(new PatternResultModel() { RectInfo = TempRect, ScoreInfo = value });
                                //ScoreList.Add(value);
                                //RectList.Add(TempRect);
                                CurrentCount++;
                            }


                        }
                    }
                }

            }
            //ResultList.RectList = RectList;
            //ResultList.ScoreList = ScoreList;
            var k = from Thres in ResultList
                         orderby Thres.ScoreInfo descending select Thres;
            ResultList = new ObservableCollection<PatternResultModel>(k);
            return ResultList;
        }

        public List<Rect> DoMatchTemplateReturnROIList(string rawImagePath, string targetImagePath, double Threshold)
        {

            int result_cols, result_rows;

            Mat SrcImage = Cv2.ImRead(rawImagePath, ImreadModes.GrayScale);
            
            Mat MatchImage = Cv2.ImRead(targetImagePath, ImreadModes.GrayScale);
            

            int cropWidth = MatchImage.Width;
            int cropHeight = MatchImage.Height;

            result_cols = SrcImage.Cols - MatchImage.Cols + 1;
            result_rows = SrcImage.Rows - MatchImage.Rows + 1;

            Mat ResultImage = new Mat(result_rows, result_cols, MatType.CV_32FC1);


            List<Rect> RectList = new List<Rect>();
            List<double> ScoreList = new List<double>();
            Rect TempRect;
            OpenCvSharp.Point matchLocation;
            TemplateMatchModes match_method = TemplateMatchModes.CCorrNormed;

            // Do the Matching and Normalize
            Cv2.MatchTemplate(SrcImage, MatchImage, ResultImage, match_method);
            Cv2.Normalize(ResultImage, ResultImage, 0, 1, NormTypes.MinMax, -1, new Mat());



            double minValue, maxValue;//score
            double value;
            int x, y;
            double CurrentThreshold;
            int CurrentCount = 0;
            minValue = 0.1;
            maxValue = 0.9;
            int SelectCount = 0;

            for (y = 0; y < result_rows; y++)
            {
                for (x = 0; x < result_cols; x++)
                {
                    value = ResultImage.At<float>(y, x);

                    if (match_method == TemplateMatchModes.SqDiff || match_method == TemplateMatchModes.SqDiffNormed)
                    {
                        CurrentThreshold = minValue;

                        if (value < Threshold)
                        {
                            matchLocation.X = x;
                            matchLocation.Y = y;
                            TempRect = new Rect(matchLocation.X, matchLocation.Y, MatchImage.Width, MatchImage.Height);
                            ScoreList.Add(value);
                            RectList.Add(TempRect);
                            CurrentCount++;
                        }
                    }
                    else
                    {
                        CurrentThreshold = maxValue;

                        if (value > Threshold)
                        {
                            matchLocation.X = x;
                            matchLocation.Y = y;

                            if (RectList.Where(var => (Math.Abs(var.X - x) < 10)).Count() > 0)
                            {
                                //var data = from RectX in RectList where Math.Abs(RectX.X - x) < 100 select RectX;
                                SelectCount = RectList.FindIndex(k => (Math.Abs(k.X - x) < 100));
                                if (ScoreList[SelectCount] < value)
                                {
                                    ScoreList.RemoveAt(SelectCount);
                                    RectList.RemoveAt(SelectCount);

                                    TempRect = new Rect(matchLocation.X, matchLocation.Y, MatchImage.Width, MatchImage.Height);
                                    ScoreList.Add(value);
                                    RectList.Add(TempRect);
                                    //CurrentCount--;
                                }
                            }
                            else
                            {
                                TempRect = new Rect(matchLocation.X, matchLocation.Y, MatchImage.Width, MatchImage.Height);
                                ScoreList.Add(value);
                                RectList.Add(TempRect);
                                CurrentCount++;
                            }


                        }
                    }
                }

            }
            return RectList;
        }
        public Rect DoMatchTemplateReturnROI(string rawImagePath, string targetImagePath)
        {
            int result_cols, result_rows;
            Mat SrcImage = Cv2.ImRead(rawImagePath, ImreadModes.GrayScale);
            Mat MatchImage = Cv2.ImRead(targetImagePath, ImreadModes.GrayScale);

            int cropWidth = MatchImage.Width;
            int cropHeight = MatchImage.Height;

            result_cols = SrcImage.Cols - MatchImage.Cols + 1;
            result_rows = SrcImage.Rows - MatchImage.Rows + 1;

            Mat ResultImage = new Mat(result_rows, result_cols, MatType.CV_32FC1);

            // Do the Matching and Normalize
            Cv2.MatchTemplate(SrcImage, MatchImage, ResultImage, TemplateMatchModes.CCorrNormed);
            Cv2.Normalize(ResultImage, ResultImage, 0, 1, NormTypes.MinMax, -1, new Mat());

            double minValue, maxValue;//score
            OpenCvSharp.Point minLocation, maxLocation, matchLocation;
            Cv2.MinMaxLoc(ResultImage, out minValue, out maxValue, out minLocation, out maxLocation, new Mat());
            matchLocation = maxLocation;


            OpenCvSharp.Rect roi = new Rect(matchLocation.X, matchLocation.Y, cropWidth, cropHeight);

            if (roi.Y + roi.Height > SrcImage.Rows)
                roi.Y = roi.Y - ((roi.Y + roi.Width) - SrcImage.Rows);

            if (roi.X + roi.Width > SrcImage.Cols)
                roi.X = roi.X - ((roi.X + roi.Width) - SrcImage.Cols);

            if (roi.X < 0)
                roi.X = 0;

            if (roi.Y < 0)
                roi.Y = 0;

            return roi;
        }
        public void DoMatchTemplateAndSave(string rawImagePath, string targetImagePath, double Theshold, string outputPath)
        {
            int result_cols, result_rows;
            Mat SrcImage = Cv2.ImRead(rawImagePath, ImreadModes.GrayScale);
            Mat MatchImage = Cv2.ImRead(targetImagePath, ImreadModes.GrayScale);

            int cropWidth = MatchImage.Width;
            int cropHeight = MatchImage.Height;

            result_cols = SrcImage.Cols - MatchImage.Cols + 1;
            result_rows = SrcImage.Rows - MatchImage.Rows + 1;

            Mat ResultImage = new Mat(result_rows, result_cols, MatType.CV_32FC1);

            // Do the Matching and Normalize
            Cv2.MatchTemplate(SrcImage, MatchImage, ResultImage, TemplateMatchModes.CCorrNormed);
            Cv2.Normalize(ResultImage, ResultImage, 1, 0, NormTypes.MinMax, -1, new Mat());

            double minValue, maxValue;//score
            OpenCvSharp.Point minLocation, maxLocation, matchLocation;
            Cv2.MinMaxLoc(ResultImage, out minValue, out maxValue, out minLocation, out maxLocation, new Mat());
            matchLocation = maxLocation;


            OpenCvSharp.Rect roi = new Rect(matchLocation.X, matchLocation.Y, cropWidth, cropHeight);

            if (roi.Y + roi.Height > SrcImage.Rows)
                roi.Y = roi.Y - ((roi.Y + roi.Width) - SrcImage.Rows);

            if (roi.X + roi.Width > SrcImage.Cols)
                roi.X = roi.X - ((roi.X + roi.Width) - SrcImage.Cols);

            if (roi.X < 0)
                roi.X = 0;

            if (roi.Y < 0)
                roi.Y = 0;

            SrcImage = SrcImage.Clone(roi);
            SrcImage.SaveImage(outputPath);
        }
    }

}
