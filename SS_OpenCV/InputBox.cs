using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class InputBox : Form
    {

        public InputBox(string _title, KeyPressEventHandler eventHandler)
        {
            InitializeComponent();

            this.Text = _title;
            this.ValueTextBox.KeyPress += eventHandler;
        }
    }
}