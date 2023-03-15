using Rummikub.Views;
using rummikubGame;
using rummikubGame.Draggable;
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

        // Players
        public static HumanPlayer HumanPlayer;
        public static ComputerPlayer ComputerPlayer;

        // UI global elements - needs to be accessed outside this class
        public static Label GlobalGameIndicatorLbl;
        public static Label GlobalCurrentPoolSizeLbl;
        public static Form GlobalRummikubGameViewContext; // Used in order to add buttons from other classes
        public static Button GlobalDroppedTilesBtn; // Dropped tiles button, used in the mouseUp
        public static Button GlobalPoolBtn; // Pool/stack button, used in the mouseUp
        public static GroupBox GlobalPoolDropGroupbox;

        // Global variables
        public static int CurrentTurn;
        public static bool GameOver = false;
        public static Pool Pool;
        public static Stack<VisualTile> DroppedTilesStack;

        public RummikubGameView()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void StartGameObjectCreation()
        {
            DroppedTilesStack = new Stack<VisualTile>(); // empty dropped tiles
            Pool = new Pool(); // generate rummikub tiles
            ComputerPlayer = new ComputerPlayer();
            HumanPlayer = new HumanPlayer();
        }

        public void StartGameSetTurn()
        {
            Random rnd = new Random();
            CurrentTurn = rnd.Next(0, 2);
            if (CurrentTurn == Constants.ComputerPlayerTurn)
            {
                game_indicator_lbl.Text = "Computer's turn";
                ComputerPlayer.ComputerPlay(null);
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
                    startNewGame();
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

            StartGameSetTurn();

            // if the game is over, and the computer won
            if (ComputerPlayer.board.CheckWinner() == true && GameOver == false)
            {
                MessageBox.Show("Computer Won!");
                RummikubGameView.GlobalGameIndicatorLbl.Text = "Game Over - Computer Won";
                RummikubGameView.HumanPlayer.board.disableHumanBoard();

                if (RummikubGameView.DroppedTilesStack.Count > 0)
                    RummikubGameView.DroppedTilesStack.Peek().TileButton.GetButton().Enabled = false;
                GameOver = true;
            }
        }

        public static void CheckWinnerWhenPoolOver()
        {
            // tilesQueue is empty -> tiles are over -> game over -> decide who is the winner(fewer files in hand)
            if (RummikubGameView.ComputerPlayer.board.getHandTilesNumber() == RummikubGameView.HumanPlayer.board.GetHandTilesNumber())
                MessageBox.Show("Tie!");
            else if (RummikubGameView.ComputerPlayer.board.getHandTilesNumber() > RummikubGameView.HumanPlayer.board.GetHandTilesNumber())
                MessageBox.Show("You Won!");
            else
                MessageBox.Show("Computer Won!");
            GameOver = true;
            RummikubGameView.HumanPlayer.board.disableHumanBoard();
        }

        public static bool CheckWinner(List<List<Tile>> melds)
        {
            // if all melds are good, user won
            for (int i = 0; i < melds.Count(); i++)
            {
                if (!RummikubGameView.IsLegalMeld(melds[i]))
                    return false;
            }
            return true;
        }

        public static bool IsLegalMeld(List<Tile> meld)
        {
            if (meld.Count < 3)
            {
                return false;
            }

            bool isRun = true;

            // finds the first non-joker tile and uses it to determine the color and value of the run
            int first_non_joker_index = 0;
            for (int i = 0; i < meld.Count; i++)
            {
                if (!isJoker(meld[i]))
                {
                    first_non_joker_index = i;
                    break;
                }
            }

            int color = meld[first_non_joker_index].Color;
            int value = meld[first_non_joker_index].Number;

            for (int i = first_non_joker_index + 1; i < meld.Count; i++)
            {
                // if meld number is not equal to the value + the index of the tile in the meld its cant be a run
                if ((meld[i].Number != value + i - first_non_joker_index
                    || meld[i].Color != color) && !isJoker(meld[i]))
                {
                    isRun = false;
                }
                if (isJoker(meld[i]))
                {
                    // Skip over jokers and continue checking the rest of the tiles
                    continue;
                }
            }

            if (isRun)
            {
                // checking the value of the max
                if (meld[first_non_joker_index].Number + ((meld.Count - 1) - first_non_joker_index) > 13)
                {
                    return false;
                }

                // checking the value of the min
                if (meld[first_non_joker_index].Number - first_non_joker_index < 1)
                {
                    return false;
                }

                return true; // 2+ run sequence
            }

            if (meld.Count > 4)
            {
                return false; // group of 4+ cannot exist
            }
            for (int i = 0; i < meld.Count() - 1; i++)
            {
                if (meld[i + 1].Number != value && !isJoker(meld[i + 1]))
                {
                    return false; // its cannot be group
                }
                for (int j = i + 1; j < meld.Count(); j++)
                {
                    if (meld[i].Color == meld[j].Color && !isJoker(meld[i]) && !isJoker(meld[j]))
                    {
                        return false;
                    }
                }
            }

            // check if there are too many jokers used
            int numJokers = countJokers(meld);
            if (numJokers > 2)
            {
                return false; // too many jokers used
            }

            return true;
        }

        private static int countJokers(List<Tile> meld)
        {
            int count = 0;
            foreach (Tile tile in meld)
            {
                if (isJoker(tile))
                    count++;
            }
            return count;
        }

        private static bool isJoker(Tile tile)
        {
            return tile.Number == Constants.JokerNumber;
        }

        private void pool_btn_Click(object sender, EventArgs e)
        {
            // generate a card to the last-empty place in the board
            bool found_last_empty_location = false;
            for (int i = HumanPlayerBoardHeight - 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = HumanPlayerBoardWidth - 1; j >= 0 && !found_last_empty_location; j--)
                {
                    if (HumanPlayer.board.TileButton_slot[i, j].SlotState == Constants.Available)
                    {
                        int[] location_arr = { i, j }; // last empty place in board
                        HumanPlayer.board.GenerateNewTileByClickingPool(location_arr); // generate tile in that location
                        found_last_empty_location = true; // skip future iterations
                    }
                }
            }
        }

        private void sort_value_btn_click(object sender, EventArgs e)
        {
            List<VisualTile> sorted_cards = HumanPlayer.board.GetTilesDictionary().Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.Number).ToList();
            HumanPlayer.board.ArrangeCardsOnBoard(sorted_cards);
        }

        private void sort_color_btn_click(object sender, EventArgs e)
        {
            // getting the tiles of the user
            List<VisualTile> tiles = HumanPlayer.board.GetTilesDictionary().Values.ToList();
            List<VisualTile>[] colors_lst = new List<VisualTile>[Constants.ColorsCount];

            // initializing the lists
            for(int i=0; i<colors_lst.Length; i++)
            {
                colors_lst[i] = new List<VisualTile>();
            }

            // Classiying the hand to N colors;
            for(int i=0; i< tiles.Count; i++)
            {
                colors_lst[tiles[i].Color].Add(tiles[i]);
            }
            
            // Sorting every array by number
            for(int i=0; i<colors_lst.Length; i++)
            {
                colors_lst[i] = colors_lst[i].OrderBy(card => card.Number).ToList();
            }

            // merging the arrays
            List<VisualTile> sorted_tiles = new List<VisualTile>();
            
            for(int i=0; i<colors_lst.Length; i++)
            {
                sorted_tiles.AddRange(colors_lst[i]);
            }

            HumanPlayer.board.ArrangeCardsOnBoard(sorted_tiles);
        }

        private void clearAllTilesFromScreen()
        {
            // Clearning the boards
            HumanPlayer.board.ClearBoard();
            ComputerPlayer.board.ClearBoard();

            // Clearing dropped tiles
            while (RummikubGameView.DroppedTilesStack.Count > 0)
            {
                RummikubGameView.GlobalRummikubGameViewContext.Controls.Remove(RummikubGameView.DroppedTilesStack.Peek().TileButton.GetButton());
                RummikubGameView.DroppedTilesStack.Pop();
            }
        }

        public void startNewGame()
        {
            clearAllTilesFromScreen();

            // Sets more vars
            RummikubGameView.GameOver = false;
            RummikubGameView.GlobalDroppedTilesBtn.Enabled = true;
            PlayerBoard.tookCard = false;

            // reset dragging var
            DraggableComponent.IsCurrentlyDragging = false;

            // Sets the game to start
            StartGameObjectCreation();
            StartGameSetTurn();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startNewGame();
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
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
                    formatter.Serialize(stream, HumanPlayer);
                    formatter.Serialize(stream, ComputerPlayer);

                    // saving game info
                    formatter.Serialize(stream, RummikubGameView.CurrentTurn);
                    formatter.Serialize(stream, RummikubGameView.GameOver);
                    formatter.Serialize(stream, RummikubGameView.Pool);
                    formatter.Serialize(stream, RummikubGameView.DroppedTilesStack);
                    formatter.Serialize(stream, PlayerBoard.tookCard);
                    formatter.Serialize(stream, PlayerBoard.TAG_NUMBER);
                    formatter.Serialize(stream, RummikubGameView.GlobalGameIndicatorLbl.Text);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
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
                    clearAllTilesFromScreen();

                    // loading game info from binary file called save.rummikub
                    BinaryFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(Constants.SavedGameFileName, FileMode.Open);
                    HumanPlayer = (HumanPlayer)formatter.Deserialize(stream);
                    ComputerPlayer = (ComputerPlayer)formatter.Deserialize(stream);

                    RummikubGameView.CurrentTurn = (int)formatter.Deserialize(stream);
                    RummikubGameView.GameOver = (bool)formatter.Deserialize(stream);
                    RummikubGameView.Pool = (Pool)formatter.Deserialize(stream);
                    RummikubGameView.DroppedTilesStack = (Stack<VisualTile>)formatter.Deserialize(stream);
                    PlayerBoard.tookCard = (bool)formatter.Deserialize(stream);
                    PlayerBoard.TAG_NUMBER = (int)formatter.Deserialize(stream);
                    RummikubGameView.GlobalGameIndicatorLbl.Text = (string)formatter.Deserialize(stream);
                    stream.Close();

                    // fix dropped tiles stack
                    Stack<VisualTile> temp_dropped_tiles = DroppedTilesStack;
                    Stack<VisualTile> revered_dropped_tiles = new Stack<VisualTile>();
                    while (temp_dropped_tiles.Count > 0)
                    {
                        revered_dropped_tiles.Push(temp_dropped_tiles.Pop());
                    }

                    while (revered_dropped_tiles.Count > 1)
                    {
                        ComputerPlayer.board.GenerateComputerThrownTile(revered_dropped_tiles.Pop());
                        HumanPlayer.board.DisableLastDroppedTile();
                    }
                    if (revered_dropped_tiles.Count > 0 && revered_dropped_tiles.Peek() != null)
                    {
                        ComputerPlayer.board.GenerateComputerThrownTile(revered_dropped_tiles.Pop());
                        if (PlayerBoard.tookCard == true)
                            HumanPlayer.board.DisableLastDroppedTile();
                    }

                    // fix to the computer player board
                    ComputerPlayer.board.drawn_computer_cards = new List<Label>();

                    HumanPlayer.board.GenerateTiles();
                    ComputerPlayer.board.GenerateBoard();

                    // changing the labels
                    Pool.UpdatePoolSizeLabel();

                    // checking if game over
                    if (GameOver)
                        HumanPlayer.board.disableHumanBoard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                startNewGame();
            }
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameRulesView gameRules_form = new GameRulesView();
            gameRules_form.StartPosition = FormStartPosition.Manual;
            gameRules_form.Location = this.Location;
            gameRules_form.Size = this.Size;
            gameRules_form.Icon = this.Icon;
            gameRules_form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showComputerTilesToggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Checked == true)
            {
                computerTiles_groupbox.Visible = false;
                ComputerPlayer.board.ClearBoard();
                ShowComputerTilesToggle = false;
                ((ToolStripMenuItem)sender).Checked = false;

                // center the groupbox

                // pool_drop_groupbox.Left = (this.ClientSize.Width - pool_drop_groupbox.Width) / 2;
            }
            else
            {
                computerTiles_groupbox.Visible = true;
                ComputerPlayer.board.GenerateBoard();
                ShowComputerTilesToggle = true;
                ((ToolStripMenuItem)sender).Checked = true;
            }
        }
    }
}
