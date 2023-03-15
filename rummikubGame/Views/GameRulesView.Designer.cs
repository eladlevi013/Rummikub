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
            this.instructionsGroupbox = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.instructionsRtf = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.closeInstructionsButton = new System.Windows.Forms.Button();
            this.instructionsGroupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // instructionsGroupbox
            // 
            this.instructionsGroupbox.Controls.Add(this.pictureBox2);
            this.instructionsGroupbox.Controls.Add(this.instructionsRtf);
            this.instructionsGroupbox.Controls.Add(this.pictureBox1);
            this.instructionsGroupbox.Location = new System.Drawing.Point(45, 38);
            this.instructionsGroupbox.Name = "instructionsGroupbox";
            this.instructionsGroupbox.Size = new System.Drawing.Size(887, 590);
            this.instructionsGroupbox.TabIndex = 0;
            this.instructionsGroupbox.TabStop = false;
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
            // instructionsRtf
            // 
            this.instructionsRtf.Location = new System.Drawing.Point(32, 95);
            this.instructionsRtf.Name = "instructionsRtf";
            this.instructionsRtf.Size = new System.Drawing.Size(819, 472);
            this.instructionsRtf.TabIndex = 2;
            this.instructionsRtf.Text = "";
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
            // closeInstructionsButton
            // 
            this.closeInstructionsButton.BackColor = System.Drawing.Color.Sienna;
            this.closeInstructionsButton.Location = new System.Drawing.Point(329, 648);
            this.closeInstructionsButton.Name = "closeInstructionsButton";
            this.closeInstructionsButton.Size = new System.Drawing.Size(309, 49);
            this.closeInstructionsButton.TabIndex = 1;
            this.closeInstructionsButton.Text = "Close Instructions";
            this.closeInstructionsButton.UseVisualStyleBackColor = false;
            this.closeInstructionsButton.Click += new System.EventHandler(this.CloseInstructionsButton_Click);
            // 
            // GameRulesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 729);
            this.Controls.Add(this.closeInstructionsButton);
            this.Controls.Add(this.instructionsGroupbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameRulesView";
            this.Text = "GameRules";
            this.Load += new System.EventHandler(this.GameRulesView_Load);
            this.instructionsGroupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox instructionsGroupbox;
        private Button closeInstructionsButton;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private RichTextBox instructionsRtf;
    }
}