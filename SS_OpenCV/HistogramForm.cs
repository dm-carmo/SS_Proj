using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SS_OpenCV
{
    public partial class HistogramForm : Form
    {
        public HistogramForm(int[,] hists)
        {
            InitializeComponent();
            if (hists.GetLength(0) == 4) initChartIBGR(hists);
            else if (hists.GetLength(0) == 3) initChartBGR(hists);
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Numero Pixeis";
            chart1.ResumeLayout();
        }

        public HistogramForm(int[] intensity)
        {
            InitializeComponent();
            foreach (Series s in chart1.Series) s.ChartType = SeriesChartType.Column;
            DataPointCollection list1 = chart1.Series[0].Points;
            for (int i = 0; i < 256; i++) list1.AddXY(i, intensity[i]);
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Numero Pixeis";
            chart1.ResumeLayout();
        }

        private void initChartIBGR(int[,] hists)
        {
            DataPointCollection list1 = chart1.Series[0].Points;
            DataPointCollection list2 = chart1.Series[1].Points;
            DataPointCollection list3 = chart1.Series[2].Points;
            DataPointCollection list4 = chart1.Series[3].Points;
            for (int i = 0; i < 256; i++)
            {
                list1.AddXY(i, hists[0, i]);
                list2.AddXY(i, hists[1, i]);
                list3.AddXY(i, hists[2, i]);
                list4.AddXY(i, hists[3, i]);
            }
        }

        private void initChartBGR(int[,] hists)
        {
            DataPointCollection list2 = chart1.Series[1].Points;
            DataPointCollection list3 = chart1.Series[2].Points;
            DataPointCollection list4 = chart1.Series[3].Points;
            for (int i = 0; i < 256; i++)
            {
                list2.AddXY(i, hists[0, i]);
                list3.AddXY(i, hists[1, i]);
                list4.AddXY(i, hists[2, i]);
            }
        }
    }
}
