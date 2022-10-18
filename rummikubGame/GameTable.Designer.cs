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
            this.current_pool_size_lbl = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.sort_value_btn = new System.Windows.Forms.Button();
            this.sort_color_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.computerTiles_groupbox = new System.Windows.Forms.GroupBox();
            this.show_computer_tiles_checkbox = new System.Windows.Forms.CheckBox();
            this.game_indicator_lbl = new System.Windows.Forms.Label();
            this.computerTiles_groupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // board_panel
            // 
            this.board_panel.AllowDrop = true;
            this.board_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("board_panel.BackgroundImage")));
            this.board_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.board_panel.Location = new System.Drawing.Point(16, 436);
            this.board_panel.Margin = new System.Windows.Forms.Padding(4);
            this.board_panel.Name = "board_panel";
            this.board_panel.Size = new System.Drawing.Size(1268, 373);
            this.board_panel.TabIndex = 2;
            // 
            // dropped_tiles_btn
            // 
            this.dropped_tiles_btn.BackColor = System.Drawing.Color.Wheat;
            this.dropped_tiles_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dropped_tiles_btn.BackgroundImage")));
            this.dropped_tiles_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dropped_tiles_btn.Location = new System.Drawing.Point(892, 155);
            this.dropped_tiles_btn.Margin = new System.Windows.Forms.Padding(4);
            this.dropped_tiles_btn.Name = "dropped_tiles_btn";
            this.dropped_tiles_btn.Size = new System.Drawing.Size(131, 166);
            this.dropped_tiles_btn.TabIndex = 3;
            this.dropped_tiles_btn.UseVisualStyleBackColor = false;
            // 
            // pool_btn
            // 
            this.pool_btn.BackColor = System.Drawing.Color.Wheat;
            this.pool_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pool_btn.BackgroundImage")));
            this.pool_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pool_btn.Location = new System.Drawing.Point(1092, 155);
            this.pool_btn.Margin = new System.Windows.Forms.Padding(4);
            this.pool_btn.Name = "pool_btn";
            this.pool_btn.Size = new System.Drawing.Size(136, 166);
            this.pool_btn.TabIndex = 4;
            this.pool_btn.UseVisualStyleBackColor = false;
            this.pool_btn.Click += new System.EventHandler(this.pool_btn_Click);
            // 
            // current_pool_size_lbl
            // 
            this.current_pool_size_lbl.AutoSize = true;
            this.current_pool_size_lbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.current_pool_size_lbl.Location = new System.Drawing.Point(1113, 330);
            this.current_pool_size_lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.current_pool_size_lbl.Name = "current_pool_size_lbl";
            this.current_pool_size_lbl.Size = new System.Drawing.Size(83, 16);
            this.current_pool_size_lbl.TabIndex = 5;
            this.current_pool_size_lbl.Text = "x tiles in pool";
            // 
            // sort_value_btn
            // 
            this.sort_value_btn.BackColor = System.Drawing.Color.Sienna;
            this.sort_value_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sort_value_btn.BackgroundImage")));
            this.sort_value_btn.Location = new System.Drawing.Point(444, 806);
            this.sort_value_btn.Margin = new System.Windows.Forms.Padding(4);
            this.sort_value_btn.Name = "sort_value_btn";
            this.sort_value_btn.Size = new System.Drawing.Size(196, 36);
            this.sort_value_btn.TabIndex = 8;
            this.sort_value_btn.Text = "Sort By Value";
            this.sort_value_btn.UseVisualStyleBackColor = false;
            this.sort_value_btn.Click += new System.EventHandler(this.sort_value_btn_click);
            // 
            // sort_color_btn
            // 
            this.sort_color_btn.BackColor = System.Drawing.Color.Sienna;
            this.sort_color_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sort_color_btn.BackgroundImage")));
            this.sort_color_btn.Location = new System.Drawing.Point(665, 806);
            this.sort_color_btn.Margin = new System.Windows.Forms.Padding(4);
            this.sort_color_btn.Name = "sort_color_btn";
            this.sort_color_btn.Size = new System.Drawing.Size(196, 36);
            this.sort_color_btn.TabIndex = 9;
            this.sort_color_btn.Text = "Sort By Color";
            this.sort_color_btn.UseVisualStyleBackColor = false;
            this.sort_color_btn.Click += new System.EventHandler(this.sort_color_btn_click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(21, 140);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sequences:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(21, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 29);
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
            this.computerTiles_groupbox.Location = new System.Drawing.Point(40, 30);
            this.computerTiles_groupbox.Margin = new System.Windows.Forms.Padding(4);
            this.computerTiles_groupbox.Name = "computerTiles_groupbox";
            this.computerTiles_groupbox.Padding = new System.Windows.Forms.Padding(4);
            this.computerTiles_groupbox.Size = new System.Drawing.Size(785, 374);
            this.computerTiles_groupbox.TabIndex = 12;
            this.computerTiles_groupbox.TabStop = false;
            // 
            // show_computer_tiles_checkbox
            // 
            this.show_computer_tiles_checkbox.AutoSize = true;
            this.show_computer_tiles_checkbox.Checked = true;
            this.show_computer_tiles_checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.show_computer_tiles_checkbox.Location = new System.Drawing.Point(40, 414);
            this.show_computer_tiles_checkbox.Margin = new System.Windows.Forms.Padding(4);
            this.show_computer_tiles_checkbox.Name = "show_computer_tiles_checkbox";
            this.show_computer_tiles_checkbox.Size = new System.Drawing.Size(156, 20);
            this.show_computer_tiles_checkbox.TabIndex = 12;
            this.show_computer_tiles_checkbox.Text = "Show Computer Tiles";
            this.show_computer_tiles_checkbox.UseVisualStyleBackColor = true;
            this.show_computer_tiles_checkbox.CheckedChanged += new System.EventHandler(this.show_computer_tiles_checkbox_change);
            // 
            // game_indicator_lbl
            // 
            this.game_indicator_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.game_indicator_lbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.game_indicator_lbl.Location = new System.Drawing.Point(833, 58);
            this.game_indicator_lbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.game_indicator_lbl.Name = "game_indicator_lbl";
            this.game_indicator_lbl.Size = new System.Drawing.Size(455, 75);
            this.game_indicator_lbl.TabIndex = 13;
            this.game_indicator_lbl.Text = "Game Indicator";
            this.game_indicator_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.game_indicator_lbl.UseMnemonic = false;
            // 
            // GameTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(1304, 859);
            this.Controls.Add(this.game_indicator_lbl);
            this.Controls.Add(this.show_computer_tiles_checkbox);
            this.Controls.Add(this.sort_value_btn);
            this.Controls.Add(this.sort_color_btn);
            this.Controls.Add(this.current_pool_size_lbl);
            this.Controls.Add(this.pool_btn);
            this.Controls.Add(this.dropped_tiles_btn);
            this.Controls.Add(this.board_panel);
            this.Controls.Add(this.computerTiles_groupbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
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
        private System.Windows.Forms.Label current_pool_size_lbl;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button sort_value_btn;
        private System.Windows.Forms.Button sort_color_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox computerTiles_groupbox;
        private System.Windows.Forms.CheckBox show_computer_tiles_checkbox;
        private System.Windows.Forms.Label game_indicator_lbl;
    }
}

