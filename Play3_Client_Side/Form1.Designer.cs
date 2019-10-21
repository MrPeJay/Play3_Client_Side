namespace Play3_Client_Side
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GameName_Label = new System.Windows.Forms.Label();
            this.Play_Button = new System.Windows.Forms.Button();
            this.EnterName_Label = new System.Windows.Forms.Label();
            this.Name_Input = new System.Windows.Forms.TextBox();
            this.Error_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GameName_Label
            // 
            this.GameName_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.GameName_Label.Font = new System.Drawing.Font("Amethyst", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GameName_Label.ForeColor = System.Drawing.Color.White;
            this.GameName_Label.Location = new System.Drawing.Point(0, 0);
            this.GameName_Label.Name = "GameName_Label";
            this.GameName_Label.Size = new System.Drawing.Size(756, 290);
            this.GameName_Label.TabIndex = 0;
            this.GameName_Label.Text = "PLAY3";
            this.GameName_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Play_Button
            // 
            this.Play_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Play_Button.AutoSize = true;
            this.Play_Button.Location = new System.Drawing.Point(111, 534);
            this.Play_Button.Name = "Play_Button";
            this.Play_Button.Size = new System.Drawing.Size(525, 66);
            this.Play_Button.TabIndex = 1;
            this.Play_Button.Text = "PLAY";
            this.Play_Button.UseVisualStyleBackColor = true;
            this.Play_Button.Click += new System.EventHandler(this.Play_Button_Click);
            // 
            // EnterName_Label
            // 
            this.EnterName_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.EnterName_Label.AutoSize = true;
            this.EnterName_Label.BackColor = System.Drawing.Color.Transparent;
            this.EnterName_Label.Font = new System.Drawing.Font("Century Gothic", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterName_Label.ForeColor = System.Drawing.Color.White;
            this.EnterName_Label.Location = new System.Drawing.Point(209, 282);
            this.EnterName_Label.Name = "EnterName_Label";
            this.EnterName_Label.Size = new System.Drawing.Size(318, 58);
            this.EnterName_Label.TabIndex = 2;
            this.EnterName_Label.Text = "ENTER NAME";
            this.EnterName_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EnterName_Label.Click += new System.EventHandler(this.label1_Click);
            // 
            // Name_Input
            // 
            this.Name_Input.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Name_Input.Font = new System.Drawing.Font("Century Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name_Input.Location = new System.Drawing.Point(233, 411);
            this.Name_Input.Name = "Name_Input";
            this.Name_Input.Size = new System.Drawing.Size(270, 36);
            this.Name_Input.TabIndex = 3;
            this.Name_Input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Name_Input.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Error_Label
            // 
            this.Error_Label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Error_Label.BackColor = System.Drawing.Color.Transparent;
            this.Error_Label.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Error_Label.ForeColor = System.Drawing.Color.Red;
            this.Error_Label.Location = new System.Drawing.Point(125, 369);
            this.Error_Label.Name = "Error_Label";
            this.Error_Label.Size = new System.Drawing.Size(488, 21);
            this.Error_Label.TabIndex = 4;
            this.Error_Label.Text = "Error";
            this.Error_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Error_Label.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(756, 653);
            this.Controls.Add(this.Error_Label);
            this.Controls.Add(this.Name_Input);
            this.Controls.Add(this.EnterName_Label);
            this.Controls.Add(this.Play_Button);
            this.Controls.Add(this.GameName_Label);
            this.MinimumSize = new System.Drawing.Size(750, 700);
            this.Name = "Form1";
            this.Tag = "Map";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyIsDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyIsUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label GameName_Label;
        private System.Windows.Forms.Button Play_Button;
        private System.Windows.Forms.Label EnterName_Label;
        private System.Windows.Forms.TextBox Name_Input;
        private System.Windows.Forms.Label Error_Label;
    }
}

