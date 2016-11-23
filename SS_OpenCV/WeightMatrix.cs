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
            foreach (Control t in this.Controls) if(t is TextBox) t.KeyPress += numberTextBox;
        }

        private void numberTextBox(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int idx = comboBox.SelectedIndex;
            switch(idx)
            {
                case 0:
                    topleft.Text = topright.Text = bttmleft.Text = bttmright.Text = "1";
                    top.Text = left.Text = right.Text = bottom.Text = "2";
                    middle.Text = "4";
                    weight.Text = "16";
                    break;
                case 1:
                    topleft.Text = top.Text = topright.Text = left.Text = right.Text = bttmleft.Text = bottom.Text = bttmright.Text = "-1";
                    middle.Text = "9";
                    weight.Text = "1";
                    break;
                case 2:
                    topleft.Text = topright.Text = bttmleft.Text = bttmright.Text = weight.Text = "1";
                    top.Text = left.Text = right.Text = bottom.Text = "-2";
                    middle.Text = "4";
                    break;
                default:
                    foreach (Control t in this.Controls) if (t is TextBox) t.Text = "";
                    break;
            }

            if(idx == 3)
            {
                foreach (Control t in this.Controls) if (t is TextBox) t.Enabled = true;
            }
            else
            {
                foreach (Control t in this.Controls) if (t is TextBox) t.Enabled = false;
            }
        }
    }
}
