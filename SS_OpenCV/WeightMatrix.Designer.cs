namespace SS_OpenCV
{
    partial class WeightMatrix
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.weight = new System.Windows.Forms.TextBox();
            this.bttmright = new System.Windows.Forms.TextBox();
            this.bottom = new System.Windows.Forms.TextBox();
            this.bttmleft = new System.Windows.Forms.TextBox();
            this.left = new System.Windows.Forms.TextBox();
            this.middle = new System.Windows.Forms.TextBox();
            this.right = new System.Windows.Forms.TextBox();
            this.topleft = new System.Windows.Forms.TextBox();
            this.top = new System.Windows.Forms.TextBox();
            this.topright = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // weight
            // 
            this.weight.Location = new System.Drawing.Point(130, 246);
            this.weight.Name = "weight";
            this.weight.Size = new System.Drawing.Size(60, 20);
            this.weight.TabIndex = 0;
            // 
            // bttmright
            // 
            this.bttmright.Location = new System.Drawing.Point(157, 179);
            this.bttmright.Name = "bttmright";
            this.bttmright.Size = new System.Drawing.Size(30, 20);
            this.bttmright.TabIndex = 1;
            // 
            // bottom
            // 
            this.bottom.Location = new System.Drawing.Point(121, 179);
            this.bottom.Name = "bottom";
            this.bottom.Size = new System.Drawing.Size(30, 20);
            this.bottom.TabIndex = 2;
            // 
            // bttmleft
            // 
            this.bttmleft.Location = new System.Drawing.Point(85, 179);
            this.bttmleft.Name = "bttmleft";
            this.bttmleft.Size = new System.Drawing.Size(30, 20);
            this.bttmleft.TabIndex = 3;
            // 
            // left
            // 
            this.left.Location = new System.Drawing.Point(85, 153);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(30, 20);
            this.left.TabIndex = 4;
            // 
            // middle
            // 
            this.middle.Location = new System.Drawing.Point(121, 153);
            this.middle.Name = "middle";
            this.middle.Size = new System.Drawing.Size(30, 20);
            this.middle.TabIndex = 5;
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(157, 153);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(30, 20);
            this.right.TabIndex = 6;
            // 
            // topleft
            // 
            this.topleft.Location = new System.Drawing.Point(85, 127);
            this.topleft.Name = "topleft";
            this.topleft.Size = new System.Drawing.Size(30, 20);
            this.topleft.TabIndex = 7;
            // 
            // top
            // 
            this.top.Location = new System.Drawing.Point(121, 127);
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(30, 20);
            this.top.TabIndex = 8;
            // 
            // topright
            // 
            this.topright.Location = new System.Drawing.Point(157, 127);
            this.topright.Name = "topright";
            this.topright.Size = new System.Drawing.Size(30, 20);
            this.topright.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(23, 47);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(231, 21);
            this.comboBox1.TabIndex = 10;
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(32, 342);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(60, 20);
            this.ok.TabIndex = 11;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(183, 342);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(60, 20);
            this.cancel.TabIndex = 12;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Weight:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Coefficients:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Filter:";
            // 
            // WeightMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 374);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.topright);
            this.Controls.Add(this.top);
            this.Controls.Add(this.topleft);
            this.Controls.Add(this.right);
            this.Controls.Add(this.middle);
            this.Controls.Add(this.left);
            this.Controls.Add(this.bttmleft);
            this.Controls.Add(this.bottom);
            this.Controls.Add(this.bttmright);
            this.Controls.Add(this.weight);
            this.Name = "WeightMatrix";
            this.Text = "WeightMatrix";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox weight;
        public System.Windows.Forms.TextBox bttmright;
        public System.Windows.Forms.TextBox bottom;
        public System.Windows.Forms.TextBox bttmleft;
        public System.Windows.Forms.TextBox left;
        public System.Windows.Forms.TextBox middle;
        public System.Windows.Forms.TextBox right;
        public System.Windows.Forms.TextBox topleft;
        public System.Windows.Forms.TextBox top;
        public System.Windows.Forms.TextBox topright;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}