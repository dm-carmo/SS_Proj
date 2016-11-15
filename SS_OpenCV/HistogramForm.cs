using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SS_OpenCV
{
    public partial class HistogramForm : Form
    {
        public HistogramForm(int[] intensity, int[] blue, int[] green, int[] red)
        {
            InitializeComponent();
            DataPointCollection list1 = chart1.Series[0].Points;
            DataPointCollection list2 = chart1.Series[1].Points;
            DataPointCollection list3 = chart1.Series[2].Points;
            DataPointCollection list4 = chart1.Series[3].Points;
            for (int i = 0; i < 256; i++)
            {
                list1.AddXY(i, intensity[i]);
                list2.AddXY(i, blue[i]);
                list3.AddXY(i, green[i]);
                list4.AddXY(i, red[i]);
            }
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Numero Pixeis";
            chart1.ResumeLayout();
        }
    }
}
