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
            board_panel = new Panel();
            computerTiles_groupbox = new GroupBox();
            sequences_lbl = new Label();
            partial_sets_lbl = new Label();
            hand_tiles_lbl = new Label();
            dropped_tiles_btn = new Button();
            pool_btn = new Button();
            dropped_tiles_lbl = new Label();
            pool_lbl = new Label();
            current_pool_size_lbl = new Label();
            game_indicator_lbl = new Label();
            sort_value_btn = new Button();
            sort_color_btn = new Button();
            show_computer_tiles_checkbox = new CheckBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveGameToolStripMenuItem = new ToolStripMenuItem();
            loadGameToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            gameToolStripMenuItem = new ToolStripMenuItem();
            resetToolStripMenuItem = new ToolStripMenuItem();
            instructionsToolStripMenuItem = new ToolStripMenuItem();
            computerTiles_groupbox.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // board_panel
            // 
            board_panel.BackgroundImage = (Image)resources.GetObject("board_panel.BackgroundImage");
            board_panel.BackgroundImageLayout = ImageLayout.Center;
            board_panel.Location = new Point(12, 367);
            board_panel.Name = "board_panel";
            board_panel.Size = new Size(951, 303);
            board_panel.TabIndex = 0;
            // 
            // computerTiles_groupbox
            // 
            computerTiles_groupbox.Controls.Add(sequences_lbl);
            computerTiles_groupbox.Controls.Add(partial_sets_lbl);
            computerTiles_groupbox.Controls.Add(hand_tiles_lbl);
            computerTiles_groupbox.Location = new Point(21, 45);
            computerTiles_groupbox.Name = "computerTiles_groupbox";
            computerTiles_groupbox.Size = new Size(604, 298);
            computerTiles_groupbox.TabIndex = 1;
            computerTiles_groupbox.TabStop = false;
            // 
            // sequences_lbl
            // 
            sequences_lbl.AutoSize = true;
            sequences_lbl.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            sequences_lbl.ForeColor = SystemColors.ControlLightLight;
            sequences_lbl.Location = new Point(9, 110);
            sequences_lbl.Name = "sequences_lbl";
            sequences_lbl.Size = new Size(100, 25);
            sequences_lbl.TabIndex = 11;
            sequences_lbl.Text = "Sequences:";
            // 
            // partial_sets_lbl
            // 
            partial_sets_lbl.AutoSize = true;
            partial_sets_lbl.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            partial_sets_lbl.ForeColor = SystemColors.ControlLightLight;
            partial_sets_lbl.Location = new Point(399, 20);
            partial_sets_lbl.Name = "partial_sets_lbl";
            partial_sets_lbl.Size = new Size(101, 25);
            partial_sets_lbl.TabIndex = 10;
            partial_sets_lbl.Text = "Partial Sets:";
            // 
            // hand_tiles_lbl
            // 
            hand_tiles_lbl.AutoSize = true;
            hand_tiles_lbl.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            hand_tiles_lbl.ForeColor = SystemColors.ControlLightLight;
            hand_tiles_lbl.Location = new Point(9, 18);
            hand_tiles_lbl.Name = "hand_tiles_lbl";
            hand_tiles_lbl.Size = new Size(59, 25);
            hand_tiles_lbl.TabIndex = 8;
            hand_tiles_lbl.Text = "Hand:";
            // 
            // dropped_tiles_btn
            // 
            dropped_tiles_btn.BackgroundImage = (Image)resources.GetObject("dropped_tiles_btn.BackgroundImage");
            dropped_tiles_btn.Location = new Point(700, 160);
            dropped_tiles_btn.Name = "dropped_tiles_btn";
            dropped_tiles_btn.Size = new Size(98, 135);
            dropped_tiles_btn.TabIndex = 2;
            dropped_tiles_btn.UseVisualStyleBackColor = true;
            // 
            // pool_btn
            // 
            pool_btn.BackgroundImage = (Image)resources.GetObject("pool_btn.BackgroundImage");
            pool_btn.BackgroundImageLayout = ImageLayout.Stretch;
            pool_btn.Location = new Point(850, 160);
            pool_btn.Name = "pool_btn";
            pool_btn.Size = new Size(102, 135);
            pool_btn.TabIndex = 3;
            pool_btn.UseVisualStyleBackColor = true;
            pool_btn.Click += pool_btn_Click;
            // 
            // dropped_tiles_lbl
            // 
            dropped_tiles_lbl.AutoSize = true;
            dropped_tiles_lbl.Location = new Point(710, 140);
            dropped_tiles_lbl.Name = "dropped_tiles_lbl";
            dropped_tiles_lbl.Size = new Size(79, 15);
            dropped_tiles_lbl.TabIndex = 4;
            dropped_tiles_lbl.Text = "Dropped Tiles";
            // 
            // pool_lbl
            // 
            pool_lbl.AutoSize = true;
            pool_lbl.Location = new Point(871, 140);
            pool_lbl.Name = "pool_lbl";
            pool_lbl.Size = new Size(57, 15);
            pool_lbl.TabIndex = 5;
            pool_lbl.Text = "Pool Tiles";
            // 
            // current_pool_size_lbl
            // 
            current_pool_size_lbl.AutoSize = true;
            current_pool_size_lbl.Location = new Point(858, 299);
            current_pool_size_lbl.Name = "current_pool_size_lbl";
            current_pool_size_lbl.Size = new Size(54, 15);
            current_pool_size_lbl.TabIndex = 6;
            current_pool_size_lbl.Text = "Pool Size";
            current_pool_size_lbl.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // game_indicator_lbl
            // 
            game_indicator_lbl.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
            game_indicator_lbl.ForeColor = Color.White;
            game_indicator_lbl.Location = new Point(700, 52);
            game_indicator_lbl.Name = "game_indicator_lbl";
            game_indicator_lbl.Size = new Size(252, 58);
            game_indicator_lbl.TabIndex = 7;
            game_indicator_lbl.Text = "GAME INDICATOR";
            game_indicator_lbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // sort_value_btn
            // 
            sort_value_btn.BackColor = Color.Sienna;
            sort_value_btn.Location = new Point(345, 668);
            sort_value_btn.Name = "sort_value_btn";
            sort_value_btn.Size = new Size(147, 29);
            sort_value_btn.TabIndex = 8;
            sort_value_btn.Text = "Sort By Value";
            sort_value_btn.UseVisualStyleBackColor = true;
            sort_value_btn.Click += sort_value_btn_click;
            // 
            // sort_color_btn
            // 
            sort_color_btn.BackColor = Color.Sienna;
            sort_color_btn.Location = new Point(511, 668);
            sort_color_btn.Name = "sort_color_btn";
            sort_color_btn.Size = new Size(147, 29);
            sort_color_btn.TabIndex = 9;
            sort_color_btn.Text = "Sort By Color";
            sort_color_btn.UseVisualStyleBackColor = false;
            sort_color_btn.Click += sort_color_btn_click;
            // 
            // show_computer_tiles_checkbox
            // 
            show_computer_tiles_checkbox.AutoSize = true;
            show_computer_tiles_checkbox.Checked = true;
            show_computer_tiles_checkbox.CheckState = CheckState.Checked;
            show_computer_tiles_checkbox.Location = new Point(21, 352);
            show_computer_tiles_checkbox.Name = "show_computer_tiles_checkbox";
            show_computer_tiles_checkbox.Size = new Size(138, 19);
            show_computer_tiles_checkbox.TabIndex = 10;
            show_computer_tiles_checkbox.Text = "Show Computer Tiles";
            show_computer_tiles_checkbox.UseVisualStyleBackColor = true;
            show_computer_tiles_checkbox.CheckedChanged += show_computer_tiles_checkbox_change;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gameToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(978, 24);
            menuStrip1.TabIndex = 11;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveGameToolStripMenuItem, loadGameToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveGameToolStripMenuItem
            // 
            saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            saveGameToolStripMenuItem.Size = new Size(134, 22);
            saveGameToolStripMenuItem.Text = "Save Game";
            saveGameToolStripMenuItem.Click += saveGameToolStripMenuItem_Click;
            // 
            // loadGameToolStripMenuItem
            // 
            loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            loadGameToolStripMenuItem.Size = new Size(134, 22);
            loadGameToolStripMenuItem.Text = "Load Game";
            loadGameToolStripMenuItem.Click += loadGameToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(134, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // gameToolStripMenuItem
            // 
            gameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { resetToolStripMenuItem, instructionsToolStripMenuItem });
            gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            gameToolStripMenuItem.Size = new Size(50, 20);
            gameToolStripMenuItem.Text = "Game";
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(136, 22);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // instructionsToolStripMenuItem
            // 
            instructionsToolStripMenuItem.Name = "instructionsToolStripMenuItem";
            instructionsToolStripMenuItem.Size = new Size(136, 22);
            instructionsToolStripMenuItem.Text = "Instructions";
            instructionsToolStripMenuItem.Click += instructionsToolStripMenuItem_Click;
            // 
            // RummikubGameView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 715);
            Controls.Add(show_computer_tiles_checkbox);
            Controls.Add(sort_color_btn);
            Controls.Add(sort_value_btn);
            Controls.Add(game_indicator_lbl);
            Controls.Add(current_pool_size_lbl);
            Controls.Add(pool_lbl);
            Controls.Add(dropped_tiles_lbl);
            Controls.Add(pool_btn);
            Controls.Add(dropped_tiles_btn);
            Controls.Add(computerTiles_groupbox);
            Controls.Add(board_panel);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "RummikubGameView";
            Text = "RummikubGameView";
            Load += RummikubGameView_Load;
            computerTiles_groupbox.ResumeLayout(false);
            computerTiles_groupbox.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel board_panel;
        private GroupBox computerTiles_groupbox;
        private Button dropped_tiles_btn;
        private Button pool_btn;
        private Label dropped_tiles_lbl;
        private Label pool_lbl;
        private Label current_pool_size_lbl;
        private Label game_indicator_lbl;
        private Label sequences_lbl;
        private Label partial_sets_lbl;
        private Label hand_tiles_lbl;
        private Button sort_value_btn;
        private Button sort_color_btn;
        private CheckBox show_computer_tiles_checkbox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveGameToolStripMenuItem;
        private ToolStripMenuItem loadGameToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private ToolStripMenuItem instructionsToolStripMenuItem;
    }
}