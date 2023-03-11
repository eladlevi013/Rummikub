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
            instructions_groupbox = new GroupBox();
            pictureBox2 = new PictureBox();
            instructions_rtf = new RichTextBox();
            pictureBox1 = new PictureBox();
            close_instructions_btn = new Button();
            instructions_groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // instructions_groupbox
            // 
            instructions_groupbox.Controls.Add(pictureBox2);
            instructions_groupbox.Controls.Add(instructions_rtf);
            instructions_groupbox.Controls.Add(pictureBox1);
            instructions_groupbox.Location = new Point(52, 44);
            instructions_groupbox.Name = "instructions_groupbox";
            instructions_groupbox.Size = new Size(863, 578);
            instructions_groupbox.TabIndex = 0;
            instructions_groupbox.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(37, 452);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(259, 105);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // instructions_rtf
            // 
            instructions_rtf.Location = new Point(37, 110);
            instructions_rtf.Name = "instructions_rtf";
            instructions_rtf.Size = new Size(812, 447);
            instructions_rtf.TabIndex = 2;
            instructions_rtf.Text = "";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(282, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(308, 71);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // close_instructions_btn
            // 
            close_instructions_btn.BackColor = Color.Sienna;
            close_instructions_btn.Location = new Point(313, 640);
            close_instructions_btn.Name = "close_instructions_btn";
            close_instructions_btn.Size = new Size(361, 57);
            close_instructions_btn.TabIndex = 1;
            close_instructions_btn.Text = "Close Instructions";
            close_instructions_btn.UseVisualStyleBackColor = false;
            close_instructions_btn.Click += close_instructions_btn_Click;
            // 
            // GameRulesView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(977, 725);
            Controls.Add(close_instructions_btn);
            Controls.Add(instructions_groupbox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "GameRulesView";
            Text = "GameRules";
            Load += GameRulesView_Load;
            instructions_groupbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox instructions_groupbox;
        private Button close_instructions_btn;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private RichTextBox instructions_rtf;
    }
}