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
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.show_computer_tiles_checkbox = new System.Windows.Forms.CheckBox();
            this.game_indicator_lbl = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.computerTiles_groupbox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // board_panel
            // 
            this.board_panel.AllowDrop = true;
            this.board_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("board_panel.BackgroundImage")));
            this.board_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.board_panel.Location = new System.Drawing.Point(12, 367);
            this.board_panel.Name = "board_panel";
            this.board_panel.Size = new System.Drawing.Size(951, 303);
            this.board_panel.TabIndex = 2;
            // 
            // dropped_tiles_btn
            // 
            this.dropped_tiles_btn.BackColor = System.Drawing.Color.Wheat;
            this.dropped_tiles_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dropped_tiles_btn.BackgroundImage")));
            this.dropped_tiles_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dropped_tiles_btn.Location = new System.Drawing.Point(700, 160);
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
            this.pool_btn.Location = new System.Drawing.Point(850, 160);
            this.pool_btn.Name = "pool_btn";
            this.pool_btn.Size = new System.Drawing.Size(102, 135);
            this.pool_btn.TabIndex = 4;
            this.pool_btn.UseVisualStyleBackColor = false;
            this.pool_btn.Click += new System.EventHandler(this.pool_btn_Click);
            // 
            // current_pool_size_lbl
            // 
            this.current_pool_size_lbl.AutoSize = true;
            this.current_pool_size_lbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.current_pool_size_lbl.Location = new System.Drawing.Point(866, 302);
            this.current_pool_size_lbl.Name = "current_pool_size_lbl";
            this.current_pool_size_lbl.Size = new System.Drawing.Size(67, 13);
            this.current_pool_size_lbl.TabIndex = 5;
            this.current_pool_size_lbl.Text = "x tiles in pool";
            // 
            // sort_value_btn
            // 
            this.sort_value_btn.BackColor = System.Drawing.Color.Sienna;
            this.sort_value_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sort_value_btn.BackgroundImage")));
            this.sort_value_btn.Location = new System.Drawing.Point(345, 668);
            this.sort_value_btn.Name = "sort_value_btn";
            this.sort_value_btn.Size = new System.Drawing.Size(147, 29);
            this.sort_value_btn.TabIndex = 8;
            this.sort_value_btn.Text = "Sort By Value";
            this.sort_value_btn.UseVisualStyleBackColor = false;
            this.sort_value_btn.Click += new System.EventHandler(this.sort_value_btn_click);
            // 
            // sort_color_btn
            // 
            this.sort_color_btn.BackColor = System.Drawing.Color.Sienna;
            this.sort_color_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sort_color_btn.BackgroundImage")));
            this.sort_color_btn.Location = new System.Drawing.Point(511, 668);
            this.sort_color_btn.Name = "sort_color_btn";
            this.sort_color_btn.Size = new System.Drawing.Size(147, 29);
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
            this.label1.Location = new System.Drawing.Point(16, 116);
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
            this.label2.Location = new System.Drawing.Point(16, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Hand:";
            // 
            // computerTiles_groupbox
            // 
            this.computerTiles_groupbox.BackColor = System.Drawing.Color.RoyalBlue;
            this.computerTiles_groupbox.Controls.Add(this.label6);
            this.computerTiles_groupbox.Controls.Add(this.label3);
            this.computerTiles_groupbox.Controls.Add(this.label1);
            this.computerTiles_groupbox.Controls.Add(this.label2);
            this.computerTiles_groupbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.computerTiles_groupbox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.computerTiles_groupbox.Location = new System.Drawing.Point(30, 37);
            this.computerTiles_groupbox.Name = "computerTiles_groupbox";
            this.computerTiles_groupbox.Size = new System.Drawing.Size(648, 304);
            this.computerTiles_groupbox.TabIndex = 12;
            this.computerTiles_groupbox.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label6.Location = new System.Drawing.Point(551, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 24);
            this.label6.TabIndex = 13;
            this.label6.Text = "Jokers:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Location = new System.Drawing.Point(414, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "Partial-Sets:";
            // 
            // show_computer_tiles_checkbox
            // 
            this.show_computer_tiles_checkbox.AutoSize = true;
            this.show_computer_tiles_checkbox.Checked = true;
            this.show_computer_tiles_checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.show_computer_tiles_checkbox.Location = new System.Drawing.Point(30, 349);
            this.show_computer_tiles_checkbox.Name = "show_computer_tiles_checkbox";
            this.show_computer_tiles_checkbox.Size = new System.Drawing.Size(126, 17);
            this.show_computer_tiles_checkbox.TabIndex = 12;
            this.show_computer_tiles_checkbox.Text = "Show Computer Tiles";
            this.show_computer_tiles_checkbox.UseVisualStyleBackColor = true;
            this.show_computer_tiles_checkbox.CheckedChanged += new System.EventHandler(this.show_computer_tiles_checkbox_change);
            // 
            // game_indicator_lbl
            // 
            this.game_indicator_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.game_indicator_lbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.game_indicator_lbl.Location = new System.Drawing.Point(684, 60);
            this.game_indicator_lbl.Name = "game_indicator_lbl";
            this.game_indicator_lbl.Size = new System.Drawing.Size(282, 61);
            this.game_indicator_lbl.TabIndex = 13;
            this.game_indicator_lbl.Text = "Game Indicator";
            this.game_indicator_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.game_indicator_lbl.UseMnemonic = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.gameToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(978, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveGameToolStripMenuItem,
            this.loadGameToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.gameToolStripMenuItem.Text = "File";
            // 
            // saveGameToolStripMenuItem
            // 
            this.saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            this.saveGameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveGameToolStripMenuItem.Text = "Save Game";
            this.saveGameToolStripMenuItem.Click += new System.EventHandler(this.saveGameToolStripMenuItem_Click);
            // 
            // loadGameToolStripMenuItem
            // 
            this.loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            this.loadGameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadGameToolStripMenuItem.Text = "Load Game";
            this.loadGameToolStripMenuItem.Click += new System.EventHandler(this.loadGameToolStripMenuItem_Click);
            // 
            // gameToolStripMenuItem1
            // 
            this.gameToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.instructionsToolStripMenuItem});
            this.gameToolStripMenuItem1.Name = "gameToolStripMenuItem1";
            this.gameToolStripMenuItem1.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem1.Text = "Game";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // instructionsToolStripMenuItem
            // 
            this.instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            this.instructionsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.instructionsToolStripMenuItem.Text = "Instructions";
            this.instructionsToolStripMenuItem.Click += new System.EventHandler(this.instructionsToolStripMenuItem_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label4.Location = new System.Drawing.Point(704, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Dropped Tiles";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Location = new System.Drawing.Point(884, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Pool";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // GameTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(978, 715);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.game_indicator_lbl);
            this.Controls.Add(this.show_computer_tiles_checkbox);
            this.Controls.Add(this.sort_value_btn);
            this.Controls.Add(this.sort_color_btn);
            this.Controls.Add(this.current_pool_size_lbl);
            this.Controls.Add(this.pool_btn);
            this.Controls.Add(this.dropped_tiles_btn);
            this.Controls.Add(this.board_panel);
            this.Controls.Add(this.computerTiles_groupbox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "GameTable";
            this.Text = "Rummikub";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.computerTiles_groupbox.ResumeLayout(false);
            this.computerTiles_groupbox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadGameToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem instructionsToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

