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
    public partial class DoubleInputBox : Form
    {
        public DoubleInputBox(string label1, string label2, string title, KeyPressEventHandler text1, KeyPressEventHandler text2)
        {
            InitializeComponent();
            this.label1.Text = label1;
            this.label2.Text = label2;
            this.Text = title;
            this.textBox1.KeyPress += text1;
            this.textBox2.KeyPress += text2;
        }
    }
}
