using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class WeightMatrix : Form
    {
        public WeightMatrix()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 3;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int idx = comboBox.SelectedIndex;
            switch(idx)
            {
                case 0:
                    topleft.Text = "1";
                    top.Text = "2";
                    topright.Text = "1";
                    left.Text = "2";
                    middle.Text = "4";
                    right.Text = "2";
                    bttmleft.Text = "1";
                    bottom.Text = "2";
                    bttmright.Text = "1";
                    weight.Text = "16";
                    break;
                case 1:
                    topleft.Text = "-1";
                    top.Text = "-1";
                    topright.Text = "-1";
                    left.Text = "-1";
                    middle.Text = "9";
                    right.Text = "-1";
                    bttmleft.Text = "-1";
                    bottom.Text = "-1";
                    bttmright.Text = "-1";
                    weight.Text = "1";
                    break;
                case 2:
                    topleft.Text = "1";
                    top.Text = "-2";
                    topright.Text = "1";
                    left.Text = "-2";
                    middle.Text = "4";
                    right.Text = "-2";
                    bttmleft.Text = "1";
                    bottom.Text = "-2";
                    bttmright.Text = "1";
                    weight.Text = "1";
                    break;
                default:
                    topleft.Text = "";
                    top.Text = "";
                    topright.Text = "";
                    left.Text = "";
                    middle.Text = "";
                    right.Text = "";
                    bttmleft.Text = "";
                    bottom.Text = "";
                    bttmright.Text = "";
                    weight.Text = "";
                    break;
            }

            if(idx == 3)
            {
                topleft.Enabled = true;
                top.Enabled = true;
                topright.Enabled = true;
                left.Enabled = true;
                middle.Enabled = true;
                right.Enabled = true;
                bttmleft.Enabled = true;
                bottom.Enabled = true;
                bttmright.Enabled = true;
                weight.Enabled = true;
            }
            else
            {
                topleft.Enabled = false;
                top.Enabled = false;
                topright.Enabled = false;
                left.Enabled = false;
                middle.Enabled = false;
                right.Enabled = false;
                bttmleft.Enabled = false;
                bottom.Enabled = false;
                bttmright.Enabled = false;
                weight.Enabled = false;
            }
        }
    }
}
