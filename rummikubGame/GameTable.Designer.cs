namespace rummikubGame
{
    partial class GameTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameTable));
            this.board_panel = new System.Windows.Forms.Panel();
            this.dropped_tiles_btn = new System.Windows.Forms.Button();
            this.pool_btn = new System.Windows.Forms.Button();
            this.current_pool_size = new System.Windows.Forms.Label();
            this.data_indicator = new System.Windows.Forms.Label();
            this.data_indicator_2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SortByValue_btn = new System.Windows.Forms.Button();
            this.SortByColor_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.computerTiles_groupbox = new System.Windows.Forms.GroupBox();
            this.show_computer_tiles_checkbox = new System.Windows.Forms.CheckBox();
            this.computerTiles_groupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // board_panel
            // 
            this.board_panel.AllowDrop = true;
            this.board_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("board_panel.BackgroundImage")));
            this.board_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.board_panel.Location = new System.Drawing.Point(12, 354);
            this.board_panel.Name = "board_panel";
            this.board_panel.Size = new System.Drawing.Size(951, 303);
            this.board_panel.TabIndex = 2;
            // 
            // dropped_tiles_btn
            // 
            this.dropped_tiles_btn.BackColor = System.Drawing.Color.Wheat;
            this.dropped_tiles_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dropped_tiles_btn.BackgroundImage")));
            this.dropped_tiles_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dropped_tiles_btn.Location = new System.Drawing.Point(662, 126);
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
            this.pool_btn.Location = new System.Drawing.Point(819, 126);
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
            this.current_pool_size.Location = new System.Drawing.Point(835, 268);
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
            this.data_indicator.Size = new System.Drawing.Size(0, 13);
            this.data_indicator.TabIndex = 6;
            // 
            // data_indicator_2
            // 
            this.data_indicator_2.AutoSize = true;
            this.data_indicator_2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.data_indicator_2.Location = new System.Drawing.Point(124, 13);
            this.data_indicator_2.Name = "data_indicator_2";
            this.data_indicator_2.Size = new System.Drawing.Size(0, 13);
            this.data_indicator_2.TabIndex = 7;
            // 
            // SortByValue_btn
            // 
            this.SortByValue_btn.BackColor = System.Drawing.Color.Sienna;
            this.SortByValue_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SortByValue_btn.BackgroundImage")));
            this.SortByValue_btn.Location = new System.Drawing.Point(333, 655);
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
            this.SortByColor_btn.Location = new System.Drawing.Point(499, 655);
            this.SortByColor_btn.Name = "SortByColor_btn";
            this.SortByColor_btn.Size = new System.Drawing.Size(147, 29);
            this.SortByColor_btn.TabIndex = 9;
            this.SortByColor_btn.Text = "Sort By Color";
            this.SortByColor_btn.UseVisualStyleBackColor = false;
            this.SortByColor_btn.Click += new System.EventHandler(this.SortByColor_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(16, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sequences:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(16, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Hand:";
            // 
            // computerTiles_groupbox
            // 
            this.computerTiles_groupbox.BackColor = System.Drawing.Color.RoyalBlue;
            this.computerTiles_groupbox.Controls.Add(this.label1);
            this.computerTiles_groupbox.Controls.Add(this.label2);
            this.computerTiles_groupbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.computerTiles_groupbox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.computerTiles_groupbox.Location = new System.Drawing.Point(30, 24);
            this.computerTiles_groupbox.Name = "computerTiles_groupbox";
            this.computerTiles_groupbox.Size = new System.Drawing.Size(585, 304);
            this.computerTiles_groupbox.TabIndex = 12;
            this.computerTiles_groupbox.TabStop = false;
            // 
            // show_computer_tiles_checkbox
            // 
            this.show_computer_tiles_checkbox.AutoSize = true;
            this.show_computer_tiles_checkbox.Checked = true;
            this.show_computer_tiles_checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.show_computer_tiles_checkbox.Location = new System.Drawing.Point(30, 336);
            this.show_computer_tiles_checkbox.Name = "show_computer_tiles_checkbox";
            this.show_computer_tiles_checkbox.Size = new System.Drawing.Size(126, 17);
            this.show_computer_tiles_checkbox.TabIndex = 12;
            this.show_computer_tiles_checkbox.Text = "Show Computer Tiles";
            this.show_computer_tiles_checkbox.UseVisualStyleBackColor = true;
            this.show_computer_tiles_checkbox.CheckedChanged += new System.EventHandler(this.show_computer_tiles_checkbox_CheckedChanged);
            // 
            // GameTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(976, 695);
            this.Controls.Add(this.show_computer_tiles_checkbox);
            this.Controls.Add(this.SortByValue_btn);
            this.Controls.Add(this.SortByColor_btn);
            this.Controls.Add(this.data_indicator_2);
            this.Controls.Add(this.data_indicator);
            this.Controls.Add(this.current_pool_size);
            this.Controls.Add(this.pool_btn);
            this.Controls.Add(this.dropped_tiles_btn);
            this.Controls.Add(this.board_panel);
            this.Controls.Add(this.computerTiles_groupbox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GameTable";
            this.Text = "Rummikub";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.computerTiles_groupbox.ResumeLayout(false);
            this.computerTiles_groupbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel board_panel;
        private System.Windows.Forms.Button dropped_tiles_btn;
        private System.Windows.Forms.Button pool_btn;
        private System.Windows.Forms.Label current_pool_size;
        private System.Windows.Forms.Label data_indicator;
        private System.Windows.Forms.Label data_indicator_2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button SortByValue_btn;
        private System.Windows.Forms.Button SortByColor_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox computerTiles_groupbox;
        private System.Windows.Forms.CheckBox show_computer_tiles_checkbox;
    }
}

