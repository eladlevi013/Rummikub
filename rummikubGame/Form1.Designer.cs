namespace rummikubGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.board = new System.Windows.Forms.Panel();
            this.dropped_tiles_btn = new System.Windows.Forms.Button();
            this.pool_btn = new System.Windows.Forms.Button();
            this.current_pool_size = new System.Windows.Forms.Label();
            this.data_indicator = new System.Windows.Forms.Label();
            this.data_indicator_2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SortByValue_btn = new System.Windows.Forms.Button();
            this.SortByColor_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // board
            // 
            this.board.AllowDrop = true;
            this.board.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("board.BackgroundImage")));
            this.board.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.board.Location = new System.Drawing.Point(12, 300);
            this.board.Name = "board";
            this.board.Size = new System.Drawing.Size(951, 303);
            this.board.TabIndex = 2;
            // 
            // dropped_tiles_btn
            // 
            this.dropped_tiles_btn.BackColor = System.Drawing.Color.Wheat;
            this.dropped_tiles_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dropped_tiles_btn.BackgroundImage")));
            this.dropped_tiles_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dropped_tiles_btn.Location = new System.Drawing.Point(416, 94);
            this.dropped_tiles_btn.Name = "dropped_tiles_btn";
            this.dropped_tiles_btn.Size = new System.Drawing.Size(98, 135);
            this.dropped_tiles_btn.TabIndex = 3;
            this.dropped_tiles_btn.UseVisualStyleBackColor = false;
            // 
            // pool_btn
            // 
            this.pool_btn.BackColor = System.Drawing.Color.Wheat;
            this.pool_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pool_btn.BackgroundImage")));
            this.pool_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pool_btn.Location = new System.Drawing.Point(748, 94);
            this.pool_btn.Name = "pool_btn";
            this.pool_btn.Size = new System.Drawing.Size(102, 135);
            this.pool_btn.TabIndex = 4;
            this.pool_btn.UseVisualStyleBackColor = false;
            this.pool_btn.Click += new System.EventHandler(this.pool_btn_Click);
            // 
            // current_pool_size
            // 
            this.current_pool_size.AutoSize = true;
            this.current_pool_size.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.current_pool_size.Location = new System.Drawing.Point(760, 236);
            this.current_pool_size.Name = "current_pool_size";
            this.current_pool_size.Size = new System.Drawing.Size(67, 13);
            this.current_pool_size.TabIndex = 5;
            this.current_pool_size.Text = "x tiles in pool";
            // 
            // data_indicator
            // 
            this.data_indicator.AutoSize = true;
            this.data_indicator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.data_indicator.Location = new System.Drawing.Point(12, 13);
            this.data_indicator.Name = "data_indicator";
            this.data_indicator.Size = new System.Drawing.Size(74, 13);
            this.data_indicator.TabIndex = 6;
            this.data_indicator.Text = "data_indicator";
            // 
            // data_indicator_2
            // 
            this.data_indicator_2.AutoSize = true;
            this.data_indicator_2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.data_indicator_2.Location = new System.Drawing.Point(124, 13);
            this.data_indicator_2.Name = "data_indicator_2";
            this.data_indicator_2.Size = new System.Drawing.Size(35, 13);
            this.data_indicator_2.TabIndex = 7;
            this.data_indicator_2.Text = "label2";
            // 
            // SortByValue_btn
            // 
            this.SortByValue_btn.BackColor = System.Drawing.Color.Sienna;
            this.SortByValue_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SortByValue_btn.BackgroundImage")));
            this.SortByValue_btn.Location = new System.Drawing.Point(333, 601);
            this.SortByValue_btn.Name = "SortByValue_btn";
            this.SortByValue_btn.Size = new System.Drawing.Size(147, 29);
            this.SortByValue_btn.TabIndex = 8;
            this.SortByValue_btn.Text = "Sort By Value";
            this.SortByValue_btn.UseVisualStyleBackColor = false;
            this.SortByValue_btn.Click += new System.EventHandler(this.SortByValue_btn_Click);
            // 
            // SortByColor_btn
            // 
            this.SortByColor_btn.BackColor = System.Drawing.Color.Sienna;
            this.SortByColor_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SortByColor_btn.BackgroundImage")));
            this.SortByColor_btn.Location = new System.Drawing.Point(499, 601);
            this.SortByColor_btn.Name = "SortByColor_btn";
            this.SortByColor_btn.Size = new System.Drawing.Size(147, 29);
            this.SortByColor_btn.TabIndex = 9;
            this.SortByColor_btn.Text = "Sort By Color";
            this.SortByColor_btn.UseVisualStyleBackColor = false;
            this.SortByColor_btn.Click += new System.EventHandler(this.SortByColor_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(975, 641);
            this.Controls.Add(this.SortByValue_btn);
            this.Controls.Add(this.SortByColor_btn);
            this.Controls.Add(this.data_indicator_2);
            this.Controls.Add(this.data_indicator);
            this.Controls.Add(this.current_pool_size);
            this.Controls.Add(this.pool_btn);
            this.Controls.Add(this.dropped_tiles_btn);
            this.Controls.Add(this.board);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Rummikub";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel board;
        private System.Windows.Forms.Button dropped_tiles_btn;
        private System.Windows.Forms.Button pool_btn;
        private System.Windows.Forms.Label current_pool_size;
        private System.Windows.Forms.Label data_indicator;
        private System.Windows.Forms.Label data_indicator_2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button SortByValue_btn;
        private System.Windows.Forms.Button SortByColor_btn;
    }
}

