using System.Drawing;
using System.Windows.Forms;

namespace Rummikub.Views
{
    partial class GameRulesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameRulesView));
            this.instructions_groupbox = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.instructions_rtf = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.close_instructions_btn = new System.Windows.Forms.Button();
            this.instructions_groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // instructions_groupbox
            // 
            this.instructions_groupbox.Controls.Add(this.pictureBox2);
            this.instructions_groupbox.Controls.Add(this.instructions_rtf);
            this.instructions_groupbox.Controls.Add(this.pictureBox1);
            this.instructions_groupbox.Location = new System.Drawing.Point(45, 38);
            this.instructions_groupbox.Name = "instructions_groupbox";
            this.instructions_groupbox.Size = new System.Drawing.Size(887, 590);
            this.instructions_groupbox.TabIndex = 0;
            this.instructions_groupbox.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(24, 450);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(250, 114);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // instructions_rtf
            // 
            this.instructions_rtf.Location = new System.Drawing.Point(32, 95);
            this.instructions_rtf.Name = "instructions_rtf";
            this.instructions_rtf.Size = new System.Drawing.Size(819, 472);
            this.instructions_rtf.TabIndex = 2;
            this.instructions_rtf.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(319, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(264, 62);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // close_instructions_btn
            // 
            this.close_instructions_btn.BackColor = System.Drawing.Color.Sienna;
            this.close_instructions_btn.Location = new System.Drawing.Point(329, 648);
            this.close_instructions_btn.Name = "close_instructions_btn";
            this.close_instructions_btn.Size = new System.Drawing.Size(309, 49);
            this.close_instructions_btn.TabIndex = 1;
            this.close_instructions_btn.Text = "Close Instructions";
            this.close_instructions_btn.UseVisualStyleBackColor = false;
            this.close_instructions_btn.Click += new System.EventHandler(this.close_instructions_btn_Click);
            // 
            // GameRulesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 729);
            this.Controls.Add(this.close_instructions_btn);
            this.Controls.Add(this.instructions_groupbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameRulesView";
            this.Text = "GameRules";
            this.Load += new System.EventHandler(this.GameRulesView_Load);
            this.instructions_groupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox instructions_groupbox;
        private Button close_instructions_btn;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private RichTextBox instructions_rtf;
    }
}