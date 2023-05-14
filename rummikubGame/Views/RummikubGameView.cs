using Rummikub.Views;
using rummikubGame;
using rummikubGame.Draggable;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Rummikub
{
    public partial class RummikubGameView : Form 
    {
        // Show computer tiles toggle
        public static bool ShowComputerTilesToggle = true;

        // Assets path constants
        public static string AssetsPath = ConfigurationManager.AppSettings["AssetsPath"];
        public static string SlotPath = Path.Combine(AssetsPath, ConfigurationManager.AppSettings["SlotPath"]);
        public static string TilePath = Path.Combine(AssetsPath, ConfigurationManager.AppSettings["TilePath"]);
        public static string BrightTilePath = Path.Combine(AssetsPath, ConfigurationManager.AppSettings["BrightTilePath"]);
        public static string BlackJokerPath = Path.Combine(AssetsPath, ConfigurationManager.AppSettings["BlackJokerPath"]);
        public static string RedJokerPath = Path.Combine(AssetsPath, ConfigurationManager.AppSettings["RedJokerPath"]);

        // Graphical constants
        public const int HumanPlayerBoardHeight = 2;
        public const int HumanPlayerBoardWidth = 10;
        public const int TileWidth = 75;
        public const int TileHeight = 100;

        // Game indicator messages
        public static string TakeTileFromPoolStackMsg = "Your turn - take tile from pool/stack";
        public static string DropTileFromBoardMsg = "Your turn - drop tile from board";

        // UI global elements - needs to be accessed outside this class
        public static Label GlobalGameIndicatorLbl;
        public static Label GlobalCurrentPoolSizeLbl;
        public static Form GlobalRummikubGameViewContext; // Used in order to add buttons from other classes
        public static Button GlobalDroppedTilesBtn; // Dropped tiles button, used in the mouseUp
        public static Button GlobalPoolBtn; // Pool/stack button, used in the mouseUp
        public static GroupBox GlobalPoolDropGroupbox;

        public RummikubGameView()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void StartGameObjectCreation()
        {
            GameContext.DroppedTilesStack = new Stack<VisualTile>(); // empty dropped tiles
            GameContext.Pool = new Pool(); // generate rummikub tiles
            GameContext.ComputerPlayer = new ComputerPlayer();
            GameContext.HumanPlayer = new HumanPlayer();
        }

        public void StartGameSetTurn()
        {
            Random rnd = new Random();
            GameContext.CurrentTurn = rnd.Next(0, 2);
            if (GameContext.CurrentTurn == Constants.ComputerPlayerTurn)
            {
                game_indicator_lbl.Text = "Computer's turn";
                GameContext.ComputerPlayer.ComputerPlay(null);
            }
            else
            {
                game_indicator_lbl.Text = "Your turn";
            }

            DraggableComponent d = new DraggableComponent(game_indicator_lbl);
            d.SetDraggable(true);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool bHandled = false;
            switch (keyData)
            {
                case Keys.F5:
                    StartNewGame();
                    bHandled = true;
                    break;
            }
            return bHandled;
        }

        private void RummikubGameView_Load(object sender, EventArgs e)
        {
            // update global variables
            GlobalPoolBtn = pool_btn;
            GlobalCurrentPoolSizeLbl = current_pool_size_lbl;
            GlobalRummikubGameViewContext = this; // updates the RummikubGameView context
            GlobalDroppedTilesBtn = dropped_tiles_btn; // updates the dropped_tiles variable, so it'll be accessed outside that class
            GlobalGameIndicatorLbl = game_indicator_lbl;

            StartGameObjectCreation();

            // change the style of the drop_TileButtons_location
            GlobalDroppedTilesBtn.FlatStyle = FlatStyle.Flat;
            GlobalDroppedTilesBtn.FlatAppearance.BorderSize = 0;
            GlobalDroppedTilesBtn.BackColor = Constants.BackgroundColor;

            // set background color
            computerTiles_groupbox.BackColor = Constants.ComputerBoardColor;
            board_panel.BackColor = Constants.BackgroundColor;
            this.BackColor = Constants.BackgroundColor;

            // sets design of the pool_btn
            pool_btn.BackColor = Constants.BackgroundColor;
            pool_btn.FlatStyle = FlatStyle.Flat;
            pool_btn.FlatAppearance.BorderSize = 0;

            // set button flat design
            sort_color_btn.FlatStyle = FlatStyle.Flat;
            sort_color_btn.FlatAppearance.BorderSize = 0;
            sort_color_btn.BackColor = Constants.MainButtonsColor;
            sort_value_btn.FlatStyle = FlatStyle.Flat;
            sort_value_btn.FlatAppearance.BorderSize = 0;
            sort_value_btn.BackColor = Constants.MainButtonsColor;

            // this will send back the panel(the board)
            board_panel.SendToBack();

            // sets hint background
            hint_btn.BackColor = Constants.BackgroundColor;
            hint_btn.FlatStyle = FlatStyle.Flat;
            hint_btn.FlatAppearance.BorderSize = 0;

            StartGameSetTurn();

            // if the game is over, and the computer won
            if (GameContext.ComputerPlayer.board.CheckWinner() == true && GameContext.GameOver == false)
            {
                MessageBox.Show("Computer Won!");
                RummikubGameView.GlobalGameIndicatorLbl.Text = "Game Over - Computer Won";
                GameContext.HumanPlayer.board.DisableBoard();

                if (GameContext.DroppedTilesStack.Count > 0)
                    GameContext.DroppedTilesStack.Peek().Enabled = false;
                GameContext.GameOver = true;
            }
        }

        private void PoolBtn_Click(object sender, EventArgs e)
        {
            // generate a card to the last-empty place in the board
            bool found_last_empty_location = false;
            for (int i = HumanPlayerBoardHeight - 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = HumanPlayerBoardWidth - 1; j >= 0 && !found_last_empty_location; j--)
                {
                    if (GameContext.HumanPlayer.board.BoardSlots[i, j].SlotState == Constants.Available)
                    {
                        int[] location_arr = { i, j }; // last empty place in board
                        GameContext.PoolOnClick(location_arr); // generate tile in that location
                        found_last_empty_location = true; // skip future iterations
                    }
                }
            }
        }

        private void SortValueBtn_Click(object sender, EventArgs e)
        {
            List<VisualTile> sorted_cards = GameContext.HumanPlayer.board.TileButtons;
            sorted_cards = sorted_cards.OrderBy(card => card.VisualTileData.TileData.Number).ToList();
            GameContext.HumanPlayer.board.ArrangeCardsOnBoard(sorted_cards);
        }

        private void SortColorBtn_Click(object sender, EventArgs e)
        {
            // getting the tiles of the user
            List<VisualTile> tiles = GameContext.HumanPlayer.board.TileButtons;
            List<VisualTile>[] colors_lst = new List<VisualTile>[Constants.ColorsCount];

            // initializing the lists
            for(int i=0; i<colors_lst.Length; i++)
            {
                colors_lst[i] = new List<VisualTile>();
            }

            // Classiying the hand to N colors;
            for(int i=0; i< tiles.Count; i++)
            {
                colors_lst[tiles[i].VisualTileData.TileData.Color].Add(tiles[i]);
            }
            
            // Sorting every array by number
            for(int i=0; i<colors_lst.Length; i++)
            {
                colors_lst[i] = colors_lst[i].OrderBy(card => card.VisualTileData.TileData.Number).ToList();
            }

            // merging the arrays
            List<VisualTile> sorted_tiles = new List<VisualTile>();
            
            for(int i=0; i<colors_lst.Length; i++)
            {
                sorted_tiles.AddRange(colors_lst[i]);
            }

            GameContext.HumanPlayer.board.ArrangeCardsOnBoard(sorted_tiles);
        }

        private void ClearAllTilesFromScreen()
        {
            // Clearning the boards
            GameContext.HumanPlayer.board.ClearBoard();
            GameContext.ComputerPlayer.board.ClearBoard();

            // Clearing dropped tiles
            while (GameContext.DroppedTilesStack.Count > 0)
            {
                RummikubGameView.GlobalRummikubGameViewContext.Controls.Remove(GameContext.DroppedTilesStack.Peek());
                GameContext.DroppedTilesStack.Pop();
            }
        }

        public void StartNewGame()
        {
            ClearAllTilesFromScreen();

            // Sets more vars
            GameContext.GameOver = false;
            GlobalDroppedTilesBtn.Enabled = true;

            // reset dragging var
            DraggableComponent.IsCurrentlyDragging = false;

            // Sets the game to start
            StartGameObjectCreation();
            StartGameSetTurn();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private void SaveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SaveFileDialog = new SaveFileDialog();
                SaveFileDialog.Filter = "Rummikub File|*.rummikub";
                SaveFileDialog.Title = "save";

                if (SaveFileDialog.ShowDialog() == DialogResult.OK &&
                    SaveFileDialog.FileName != "")
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(SaveFileDialog.FileName, FileMode.Create);
                    formatter.Serialize(stream, GameContext.HumanPlayer);
                    formatter.Serialize(stream, GameContext.ComputerPlayer);

                    // saving game info
                    formatter.Serialize(stream, GameContext.CurrentTurn);
                    formatter.Serialize(stream, GameContext.GameOver);
                    formatter.Serialize(stream, GameContext.Pool);
                    formatter.Serialize(stream, GameContext.DroppedTilesStack);
                    formatter.Serialize(stream, GlobalGameIndicatorLbl.Text);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.Filter = "Rummikub File|*.rummikub";
                OpenFileDialog.Title = "save";

                if (OpenFileDialog.ShowDialog() == DialogResult.OK &&
                    OpenFileDialog.FileName != "")
                {
                    // Clearning the boards
                    ClearAllTilesFromScreen();

                    // loading game info from binary file called save.rummikub
                    BinaryFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(OpenFileDialog.FileName, FileMode.Open);
                    GameContext.HumanPlayer = (HumanPlayer)formatter.Deserialize(stream);
                    GameContext.ComputerPlayer = (ComputerPlayer)formatter.Deserialize(stream);

                    GameContext.CurrentTurn = (int)formatter.Deserialize(stream);
                    GameContext.GameOver = (bool)formatter.Deserialize(stream);
                    GameContext.Pool = (Pool)formatter.Deserialize(stream);
                    GameContext.DroppedTilesStack = (Stack<VisualTile>)formatter.Deserialize(stream);
                    GlobalGameIndicatorLbl.Text = (string)formatter.Deserialize(stream);
                    stream.Close();

                    // fix dropped tiles stack
                    Stack<VisualTile> temp_dropped_tiles = GameContext.DroppedTilesStack;
                    Stack<VisualTile> revered_dropped_tiles = new Stack<VisualTile>();
                    while (temp_dropped_tiles.Count > 0)
                    {
                        revered_dropped_tiles.Push(temp_dropped_tiles.Pop());
                    }

                    while (revered_dropped_tiles.Count > 1)
                    {
                        GameContext.GenerateComputerThrownTile(revered_dropped_tiles.Pop().VisualTileData.TileData);
                        if(GameContext.DroppedTilesStack.Count() > 0)
                            GameContext.DroppedTilesStack.Peek().DisableTile();
                    }
                    if (revered_dropped_tiles.Count > 0 && revered_dropped_tiles.Peek() != null)
                    {
                        GameContext.GenerateComputerThrownTile(revered_dropped_tiles.Pop().VisualTileData.TileData);
                        if (GameContext.HumanPlayer.board.TookCard == true)
                            if (GameContext.DroppedTilesStack.Count() > 0)
                                GameContext.DroppedTilesStack.Peek().DisableTile();
                    }

                    // fix to the computer player board
                    GameContext.ComputerPlayer.board.drawnComputerCards = new List<Label>();

                    GameContext.ComputerPlayer.board.GenerateBoard();

                    // changing the labels
                    GameContext.Pool.UpdatePoolSizeLabel();

                    // checking if game over
                    if (GameContext.GameOver)
                        GameContext.HumanPlayer.board.DisableBoard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                StartNewGame();
            }
        }

        private void InstructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameRulesView gameRules_form = new GameRulesView();
            gameRules_form.StartPosition = FormStartPosition.Manual;
            gameRules_form.Location = this.Location;
            gameRules_form.Size = this.Size;
            gameRules_form.Icon = this.Icon;
            gameRules_form.ShowDialog();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowComputerTilesToggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Checked == true)
            {
                computerTiles_groupbox.Visible = false;
                GameContext.ComputerPlayer.board.ClearBoard();
                ShowComputerTilesToggle = false;
                ((ToolStripMenuItem)sender).Checked = false;
            }
            else
            {
                computerTiles_groupbox.Visible = true;
                GameContext.ComputerPlayer.board.GenerateBoard();
                ShowComputerTilesToggle = true;
                ((ToolStripMenuItem)sender).Checked = true;
            }
        }

        private void hint_btn_Click(object sender, EventArgs e)
        {
            GameContext.HumanPlayer.board.hint(); ;
        }
    }
}
