namespace rummikubGame
{
    partial class GameRules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameRules));
            this.instructions_groupbox = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.instructions_rtf = new System.Windows.Forms.RichTextBox();
            this.close_instructions_btn = new System.Windows.Forms.Button();
            this.instructions_groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // instructions_groupbox
            // 
            this.instructions_groupbox.Controls.Add(this.pictureBox2);
            this.instructions_groupbox.Controls.Add(this.pictureBox1);
            this.instructions_groupbox.Controls.Add(this.instructions_rtf);
            this.instructions_groupbox.Location = new System.Drawing.Point(36, 33);
            this.instructions_groupbox.Name = "instructions_groupbox";
            this.instructions_groupbox.Size = new System.Drawing.Size(903, 583);
            this.instructions_groupbox.TabIndex = 0;
            this.instructions_groupbox.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(39, 427);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(247, 115);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(269, 54);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(370, 91);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // instructions_rtf
            // 
            this.instructions_rtf.BackColor = System.Drawing.SystemColors.HotTrack;
            this.instructions_rtf.Location = new System.Drawing.Point(16, 146);
            this.instructions_rtf.Name = "instructions_rtf";
            this.instructions_rtf.ReadOnly = true;
            this.instructions_rtf.Size = new System.Drawing.Size(869, 412);
            this.instructions_rtf.TabIndex = 1;
            this.instructions_rtf.Text = "Focus";
            // 
            // close_instructions_btn
            // 
            this.close_instructions_btn.BackColor = System.Drawing.Color.Sienna;
            this.close_instructions_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("close_instructions_btn.BackgroundImage")));
            this.close_instructions_btn.Location = new System.Drawing.Point(327, 640);
            this.close_instructions_btn.Name = "close_instructions_btn";
            this.close_instructions_btn.Size = new System.Drawing.Size(348, 51);
            this.close_instructions_btn.TabIndex = 10;
            this.close_instructions_btn.Text = "Close Instructions";
            this.close_instructions_btn.UseVisualStyleBackColor = false;
            this.close_instructions_btn.Click += new System.EventHandler(this.CloseInstructions_ButtonClick);
            // 
            // GameRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(975, 711);
            this.Controls.Add(this.close_instructions_btn);
            this.Controls.Add(this.instructions_groupbox);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GameRules";
            this.Text = "GameRules";
            this.Load += new System.EventHandler(this.GameRules_Load);
            this.instructions_groupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox instructions_groupbox;
        private System.Windows.Forms.Button close_instructions_btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox instructions_rtf;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}