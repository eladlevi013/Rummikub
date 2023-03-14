using System.Drawing;
using System.Windows.Forms;

namespace Rummikub
{
    partial class RummikubGameView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RummikubGameView));
            this.sort_value_btn = new System.Windows.Forms.Button();
            this.computerTiles_groupbox = new System.Windows.Forms.GroupBox();
            this.sequences_lbl = new System.Windows.Forms.Label();
            this.partial_sets_lbl = new System.Windows.Forms.Label();
            this.hand_tiles_lbl = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instructionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showComputerTilesToggleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sort_color_btn = new System.Windows.Forms.Button();
            this.board_panel = new System.Windows.Forms.Panel();
            this.pool_btn = new System.Windows.Forms.Button();
            this.dropped_tiles_btn = new System.Windows.Forms.Button();
            this.pool_lbl = new System.Windows.Forms.Label();
            this.dropped_tiles_lbl = new System.Windows.Forms.Label();
            this.game_indicator_lbl = new System.Windows.Forms.Label();
            this.current_pool_size_lbl = new System.Windows.Forms.Label();
            this.computerTiles_groupbox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sort_value_btn
            // 
            this.sort_value_btn.BackColor = System.Drawing.Color.Sienna;
            this.sort_value_btn.Location = new System.Drawing.Point(502, 674);
            this.sort_value_btn.Name = "sort_value_btn";
            this.sort_value_btn.Size = new System.Drawing.Size(148, 32);
            this.sort_value_btn.TabIndex = 8;
            this.sort_value_btn.Text = "Sort By Value";
            this.sort_value_btn.UseVisualStyleBackColor = true;
            this.sort_value_btn.Click += new System.EventHandler(this.sort_value_btn_click);
            // 
            // computerTiles_groupbox
            // 
            this.computerTiles_groupbox.Controls.Add(this.sequences_lbl);
            this.computerTiles_groupbox.Controls.Add(this.partial_sets_lbl);
            this.computerTiles_groupbox.Controls.Add(this.hand_tiles_lbl);
            this.computerTiles_groupbox.Location = new System.Drawing.Point(18, 39);
            this.computerTiles_groupbox.Name = "computerTiles_groupbox";
            this.computerTiles_groupbox.Size = new System.Drawing.Size(606, 324);
            this.computerTiles_groupbox.TabIndex = 1;
            this.computerTiles_groupbox.TabStop = false;
            // 
            // sequences_lbl
            // 
            this.sequences_lbl.AutoSize = true;
            this.sequences_lbl.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.sequences_lbl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.sequences_lbl.Location = new System.Drawing.Point(14, 108);
            this.sequences_lbl.Name = "sequences_lbl";
            this.sequences_lbl.Size = new System.Drawing.Size(100, 25);
            this.sequences_lbl.TabIndex = 11;
            this.sequences_lbl.Text = "Sequences:";
            // 
            // partial_sets_lbl
            // 
            this.partial_sets_lbl.AutoSize = true;
            this.partial_sets_lbl.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.partial_sets_lbl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.partial_sets_lbl.Location = new System.Drawing.Point(407, 20);
            this.partial_sets_lbl.Name = "partial_sets_lbl";
            this.partial_sets_lbl.Size = new System.Drawing.Size(101, 25);
            this.partial_sets_lbl.TabIndex = 10;
            this.partial_sets_lbl.Text = "Partial Sets:";
            // 
            // hand_tiles_lbl
            // 
            this.hand_tiles_lbl.AutoSize = true;
            this.hand_tiles_lbl.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.hand_tiles_lbl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.hand_tiles_lbl.Location = new System.Drawing.Point(16, 16);
            this.hand_tiles_lbl.Name = "hand_tiles_lbl";
            this.hand_tiles_lbl.Size = new System.Drawing.Size(59, 25);
            this.hand_tiles_lbl.TabIndex = 8;
            this.hand_tiles_lbl.Text = "Hand:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(967, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveGameToolStripMenuItem,
            this.loadGameToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveGameToolStripMenuItem
            // 
            this.saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            this.saveGameToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.saveGameToolStripMenuItem.Text = "Save Game";
            this.saveGameToolStripMenuItem.Click += new System.EventHandler(this.saveGameToolStripMenuItem_Click);
            // 
            // loadGameToolStripMenuItem
            // 
            this.loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            this.loadGameToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.loadGameToolStripMenuItem.Text = "Load Game";
            this.loadGameToolStripMenuItem.Click += new System.EventHandler(this.loadGameToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.instructionsToolStripMenuItem,
            this.showComputerTilesToggleToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // instructionsToolStripMenuItem
            // 
            this.instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            this.instructionsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.instructionsToolStripMenuItem.Text = "Instructions";
            this.instructionsToolStripMenuItem.Click += new System.EventHandler(this.instructionsToolStripMenuItem_Click);
            // 
            // showComputerTilesToggleToolStripMenuItem
            // 
            this.showComputerTilesToggleToolStripMenuItem.Checked = true;
            this.showComputerTilesToggleToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showComputerTilesToggleToolStripMenuItem.Name = "showComputerTilesToggleToolStripMenuItem";
            this.showComputerTilesToggleToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showComputerTilesToggleToolStripMenuItem.Text = "Show Computer Tiles Toggle";
            this.showComputerTilesToggleToolStripMenuItem.Click += new System.EventHandler(this.showComputerTilesToggleToolStripMenuItem_Click);
            // 
            // sort_color_btn
            // 
            this.sort_color_btn.BackColor = System.Drawing.Color.Sienna;
            this.sort_color_btn.Location = new System.Drawing.Point(335, 674);
            this.sort_color_btn.Name = "sort_color_btn";
            this.sort_color_btn.Size = new System.Drawing.Size(148, 32);
            this.sort_color_btn.TabIndex = 9;
            this.sort_color_btn.Text = "Sort By Color";
            this.sort_color_btn.UseVisualStyleBackColor = false;
            this.sort_color_btn.Click += new System.EventHandler(this.sort_color_btn_click);
            // 
            // board_panel
            // 
            this.board_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("board_panel.BackgroundImage")));
            this.board_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.board_panel.Location = new System.Drawing.Point(12, 352);
            this.board_panel.Name = "board_panel";
            this.board_panel.Size = new System.Drawing.Size(951, 334);
            this.board_panel.TabIndex = 0;
            // 
            // pool_btn
            // 
            this.pool_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pool_btn.BackgroundImage")));
            this.pool_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pool_btn.Location = new System.Drawing.Point(815, 156);
            this.pool_btn.Name = "pool_btn";
            this.pool_btn.Size = new System.Drawing.Size(99, 135);
            this.pool_btn.TabIndex = 3;
            this.pool_btn.UseVisualStyleBackColor = true;
            this.pool_btn.Click += new System.EventHandler(this.pool_btn_Click);
            // 
            // dropped_tiles_btn
            // 
            this.dropped_tiles_btn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dropped_tiles_btn.BackgroundImage")));
            this.dropped_tiles_btn.Location = new System.Drawing.Point(678, 156);
            this.dropped_tiles_btn.Name = "dropped_tiles_btn";
            this.dropped_tiles_btn.Size = new System.Drawing.Size(99, 135);
            this.dropped_tiles_btn.TabIndex = 2;
            this.dropped_tiles_btn.UseVisualStyleBackColor = true;
            // 
            // pool_lbl
            // 
            this.pool_lbl.AutoSize = true;
            this.pool_lbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pool_lbl.Location = new System.Drawing.Point(844, 136);
            this.pool_lbl.Name = "pool_lbl";
            this.pool_lbl.Size = new System.Drawing.Size(53, 13);
            this.pool_lbl.TabIndex = 5;
            this.pool_lbl.Text = "Pool Tiles";
            // 
            // dropped_tiles_lbl
            // 
            this.dropped_tiles_lbl.AutoSize = true;
            this.dropped_tiles_lbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dropped_tiles_lbl.Location = new System.Drawing.Point(694, 136);
            this.dropped_tiles_lbl.Name = "dropped_tiles_lbl";
            this.dropped_tiles_lbl.Size = new System.Drawing.Size(73, 13);
            this.dropped_tiles_lbl.TabIndex = 4;
            this.dropped_tiles_lbl.Text = "Dropped Tiles";
            // 
            // game_indicator_lbl
            // 
            this.game_indicator_lbl.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.game_indicator_lbl.ForeColor = System.Drawing.Color.White;
            this.game_indicator_lbl.Location = new System.Drawing.Point(694, 63);
            this.game_indicator_lbl.Name = "game_indicator_lbl";
            this.game_indicator_lbl.Size = new System.Drawing.Size(216, 50);
            this.game_indicator_lbl.TabIndex = 7;
            this.game_indicator_lbl.Text = "GAME INDICATOR";
            this.game_indicator_lbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // current_pool_size_lbl
            // 
            this.current_pool_size_lbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.current_pool_size_lbl.Location = new System.Drawing.Point(813, 294);
            this.current_pool_size_lbl.Name = "current_pool_size_lbl";
            this.current_pool_size_lbl.Size = new System.Drawing.Size(101, 21);
            this.current_pool_size_lbl.TabIndex = 6;
            this.current_pool_size_lbl.Text = "Pool Size";
            this.current_pool_size_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RummikubGameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 721);
            this.Controls.Add(this.current_pool_size_lbl);
            this.Controls.Add(this.sort_color_btn);
            this.Controls.Add(this.sort_value_btn);
            this.Controls.Add(this.dropped_tiles_btn);
            this.Controls.Add(this.computerTiles_groupbox);
            this.Controls.Add(this.game_indicator_lbl);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dropped_tiles_lbl);
            this.Controls.Add(this.board_panel);
            this.Controls.Add(this.pool_lbl);
            this.Controls.Add(this.pool_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "RummikubGameView";
            this.Text = "RummikubGameView";
            this.Load += new System.EventHandler(this.RummikubGameView_Load);
            this.computerTiles_groupbox.ResumeLayout(false);
            this.computerTiles_groupbox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private GroupBox computerTiles_groupbox;
        private Label sequences_lbl;
        private Label partial_sets_lbl;
        private Label hand_tiles_lbl;
        private Button sort_value_btn;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveGameToolStripMenuItem;
        private ToolStripMenuItem loadGameToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem instructionsToolStripMenuItem;
        private Button sort_color_btn;
        private Panel board_panel;
        private ToolStripMenuItem showComputerTilesToggleToolStripMenuItem;
        private Label game_indicator_lbl;
        private Label current_pool_size_lbl;
        private Button dropped_tiles_btn;
        private Label pool_lbl;
        private Label dropped_tiles_lbl;
        private Button pool_btn;
    }
}