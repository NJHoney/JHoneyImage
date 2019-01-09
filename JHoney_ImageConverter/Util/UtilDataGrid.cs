using JHoney_ImageConverter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace JHoney_ImageConverter.Util
{
    class UtilDataGrid
    {
        DataTable dt1 = new DataTable();

        public void SetDataGrid1(DataGrid datagrid, ObservableCollection<PatternResultModel> RectList)
        {
            datagrid.Columns.Clear();
            dt1.Columns.Clear();
            dt1.Rows.Clear();


            //Make Columns
            dt1.Columns.Add("No");
            dt1.Columns.Add("X");
            dt1.Columns.Add("Y");
            dt1.Columns.Add("Width");
            dt1.Columns.Add("Height");
            dt1.Columns.Add("Prob");

            int CountColumn = 6;

            string[] TempRow = new string[CountColumn];
            //Input Score - ResultScoreArr[ImageCount,ClassCount]
            for (int iLoofCount = 0; iLoofCount < RectList.Count; iLoofCount++)//Increase Row
            {
                //RowInputData.Clear();
                TempRow[0] = (iLoofCount + 1).ToString();
                TempRow[1] = RectList[iLoofCount].RectInfo.X.ToString();
                TempRow[2] = RectList[iLoofCount].RectInfo.Y.ToString();
                TempRow[3] = RectList[iLoofCount].RectInfo.Width.ToString();
                TempRow[4] = RectList[iLoofCount].RectInfo.Height.ToString();
                TempRow[5] = RectList[iLoofCount].ScoreInfo.ToString();

                dt1.Rows.Add(TempRow);
            }
            datagrid.ItemsSource = dt1.AsDataView();
        }
    }
}
