using rummikubGame.Draggable;
using rummikubGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using rummikubGame.Utilities;
using Rummikub.Views;
using System.IO;

namespace Rummikub
{
    public partial class RummikubGameView : Form
    {
        // Assets path consts
        public static String ASSETS_PATH = ConfigurationManager.AppSettings["AssetsPath"];
        public static String SLOT_PATH = Path.Combine(ASSETS_PATH, ConfigurationManager.AppSettings["SlotPath"]);
        public static String TILE_PATH = Path.Combine(ASSETS_PATH, ConfigurationManager.AppSettings["TilePath"]);
        public static String BRIGHT_TILE_PATH = Path.Combine(ASSETS_PATH, ConfigurationManager.AppSettings["BrightTilePath"]);
        public static String BLACK_JOKER_PATH = Path.Combine(ASSETS_PATH, ConfigurationManager.AppSettings["BlackJokerPath"]);
        public static String RED_JOKER_PATH = Path.Combine(ASSETS_PATH, ConfigurationManager.AppSettings["RedJokerPath"]);

        // Graphical consts
        public const int HUMAN_PLAYER_BOARD_HEIGHT = 2;
        public const int HUMAN_PLAYER_BOARD_WIDTH = 10;
        public const int TILE_WIDTH = 75;
        public const int TILE_HEIGHT = 100;

        // Game indicator messages
        public static String TAKE_TILE_FROM_POOL_STACK_MSG = "Your turn - take tile from pool/stack";
        public static String DROP_TILE_FROM_BOARD_MSG = "Your turn - drop tile from board";

        // players
        public static HumanPlayer human_player;
        public static ComputerPlayer computer_player;

        // UI global elements - needs to be accessed outside this class
        public static Label global_game_indicator_lbl;
        public static Label global_current_pool_size_lbl;
        public static Form global_RummikubGameView_context; // used in order to add buttons from other classes
        public static Button global_dropped_tiles_btn; // dropped_tiles button, used in the mouseUp
        public static Button global_pool_btn; // pool_stack button, used in the mouseUp
        public static CheckBox global_view_computer_tiles_groupbox; // groupBox show computer tiles

        // global variables
        public static int current_turn;
        public static bool game_over = false;
        public static Pool pool;
        public static Stack<VisualTile> dropped_tiles_stack;

        public RummikubGameView()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void StartGameObjectCreation()
        {
            dropped_tiles_stack = new Stack<VisualTile>(); // empty dropped tiles
            pool = new Pool(); // generate rummikub tiles
            computer_player = new ComputerPlayer();
            human_player = new HumanPlayer("Player Default Name");
        }

        public void StartGameSetTurn()
        {
            Random rnd = new Random();
            current_turn = rnd.Next(0, 2);
            if (current_turn == Constants.COMPUTER_PLAYER_TURN)
            {
                game_indicator_lbl.Text = "Computer's turn";
                computer_player.ComputerPlay(null);
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
            global_view_computer_tiles_groupbox = show_computer_tiles_checkbox;
            global_current_pool_size_lbl = current_pool_size_lbl;
            global_RummikubGameView_context = this; // updates the RummikubGameView context
            global_dropped_tiles_btn = dropped_tiles_btn; // updates the dropped_tiles variable, so it'll be accessed outside that class
            global_game_indicator_lbl = game_indicator_lbl;
            global_pool_btn = pool_btn;

            StartGameObjectCreation();

            // change the style of the drop_TileButtons_location
            global_dropped_tiles_btn.FlatStyle = FlatStyle.Flat;
            global_dropped_tiles_btn.FlatAppearance.BorderSize = 0;
            global_dropped_tiles_btn.BackColor = Constants.BACKGROUND_COLOR;

            // set background color
            computerTiles_groupbox.BackColor = Constants.COMPUTER_BOARD_COLOR;
            board_panel.BackColor = Constants.BACKGROUND_COLOR;
            this.BackColor = Constants.BACKGROUND_COLOR;

            // sets design of the pool_btn
            pool_btn.BackColor = Constants.BACKGROUND_COLOR;
            pool_btn.FlatStyle = FlatStyle.Flat;
            pool_btn.FlatAppearance.BorderSize = 0;

            // set button flat design
            sort_color_btn.FlatStyle = FlatStyle.Flat;
            sort_color_btn.FlatAppearance.BorderSize = 0;
            sort_color_btn.BackColor = Constants.MAIN_BUTTONS_COLOR;
            sort_value_btn.FlatStyle = FlatStyle.Flat;
            sort_value_btn.FlatAppearance.BorderSize = 0;
            sort_value_btn.BackColor = Constants.MAIN_BUTTONS_COLOR;

            // this will send back the panel(the board)
            board_panel.SendToBack();

            StartGameSetTurn();

            // if the game is over, and the computer won
            if (computer_player.board.CheckWinner() == true && RummikubGameView.game_over == false)
            {
                MessageBox.Show("Computer Won!");
                RummikubGameView.global_game_indicator_lbl.Text = "Game Over - Computer Won";
                RummikubGameView.human_player.board.disableHumanBoard();

                if (RummikubGameView.dropped_tiles_stack.Count > 0)
                    RummikubGameView.dropped_tiles_stack.Peek().TileButton.GetButton().Enabled = false;
                RummikubGameView.game_over = true;
            }
        }

        public static void CheckWinnerWhenPoolOver()
        {
            // tilesQueue is empty -> tiles are over -> game over -> decide who is the winner(fewer files in hand)
            if (RummikubGameView.computer_player.board.getHandTilesNumber() == RummikubGameView.human_player.board.GetHandTilesNumber())
                MessageBox.Show("Tie!");
            else if (RummikubGameView.computer_player.board.getHandTilesNumber() > RummikubGameView.human_player.board.GetHandTilesNumber())
                MessageBox.Show("You Won!");
            else
                MessageBox.Show("Computer Won!");
            RummikubGameView.game_over = true;
            RummikubGameView.human_player.board.disableHumanBoard();
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
            return tile.Number == Constants.JOKER_NUMBER;
        }

        private void pool_btn_Click(object sender, EventArgs e)
        {
            // generate a card to the last-empty place in the board
            bool found_last_empty_location = false;
            for (int i = HUMAN_PLAYER_BOARD_HEIGHT - 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = HUMAN_PLAYER_BOARD_WIDTH - 1; j >= 0 && !found_last_empty_location; j--)
                {
                    if (human_player.board.TileButton_slot[i, j].SlotState == Constants.AVAILABLE)
                    {
                        int[] location_arr = { i, j }; // last empty place in board
                        human_player.board.GenerateNewTileByClickingPool(location_arr); // generate tile in that location
                        found_last_empty_location = true; // skip future iterations
                    }
                }
            }
        }

        private void sort_value_btn_click(object sender, EventArgs e)
        {
            List<VisualTile> sorted_cards = human_player.board.GetTilesDictionary().Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.Number).ToList();
            human_player.board.ArrangeCardsOnBoard(sorted_cards);
        }

        private void sort_color_btn_click(object sender, EventArgs e)
        {
            List<VisualTile> sorted_cards = human_player.board.GetTilesDictionary().Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.Color).ToList();
            human_player.board.ArrangeCardsOnBoard(sorted_cards);
        }

        private void show_computer_tiles_checkbox_change(object sender, EventArgs e)
        {   // if to delete or create the graphical representation of the computer tiles
            if (show_computer_tiles_checkbox.Checked == false)
            {
                computerTiles_groupbox.Visible = false;
                computer_player.board.ClearBoard();
            }
            else
            {
                computerTiles_groupbox.Visible = true;
                computer_player.board.GenerateBoard();
            }
        }

        private void clearAllTilesFromScreen()
        {
            // Clearning the boards
            human_player.board.ClearBoard();
            computer_player.board.ClearBoard();

            // Clearing dropped tiles
            while (RummikubGameView.dropped_tiles_stack.Count > 0)
            {
                RummikubGameView.global_RummikubGameView_context.Controls.Remove(RummikubGameView.dropped_tiles_stack.Peek().TileButton.GetButton());
                RummikubGameView.dropped_tiles_stack.Pop();
            }
        }

        public void startNewGame()
        {
            clearAllTilesFromScreen();

            // Sets more vars
            RummikubGameView.game_over = false;
            RummikubGameView.global_dropped_tiles_btn.Enabled = true;
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
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Constants.SAVED_GAME_FILE_NAME, FileMode.Create);
                formatter.Serialize(stream, human_player);
                formatter.Serialize(stream, computer_player);

                // saving game info
                formatter.Serialize(stream, RummikubGameView.current_turn);
                formatter.Serialize(stream, RummikubGameView.game_over);
                formatter.Serialize(stream, RummikubGameView.pool);
                formatter.Serialize(stream, RummikubGameView.dropped_tiles_stack);
                formatter.Serialize(stream, PlayerBoard.tookCard);
                formatter.Serialize(stream, PlayerBoard.TAG_NUMBER);
                formatter.Serialize(stream, RummikubGameView.global_game_indicator_lbl.Text);
                stream.Close();
                // MessageBox.Show("Game saved successfully!");
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
                // Clearning the boards
                clearAllTilesFromScreen();

                // loading game info from binary file called save.rummikub
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Constants.SAVED_GAME_FILE_NAME, FileMode.Open);
                human_player = (HumanPlayer)formatter.Deserialize(stream);
                computer_player = (ComputerPlayer)formatter.Deserialize(stream);

                RummikubGameView.current_turn = (int)formatter.Deserialize(stream);
                RummikubGameView.game_over = (bool)formatter.Deserialize(stream);
                RummikubGameView.pool = (Pool)formatter.Deserialize(stream);
                RummikubGameView.dropped_tiles_stack = (Stack<VisualTile>)formatter.Deserialize(stream);
                PlayerBoard.tookCard = (bool)formatter.Deserialize(stream);
                PlayerBoard.TAG_NUMBER = (int)formatter.Deserialize(stream);
                RummikubGameView.global_game_indicator_lbl.Text = (string)formatter.Deserialize(stream);
                stream.Close();

                // fix dropped tiles stack
                Stack<VisualTile> temp_dropped_tiles = dropped_tiles_stack;
                Stack<VisualTile> revered_dropped_tiles = new Stack<VisualTile>();
                while (temp_dropped_tiles.Count > 0)
                {
                    revered_dropped_tiles.Push(temp_dropped_tiles.Pop());
                }

                while (revered_dropped_tiles.Count > 1)
                {
                    computer_player.board.GenerateComputerThrownTile(revered_dropped_tiles.Pop());
                    human_player.board.DisableLastDroppedTile();
                }
                if (revered_dropped_tiles.Count > 0 && revered_dropped_tiles.Peek() != null)
                {
                    computer_player.board.GenerateComputerThrownTile(revered_dropped_tiles.Pop());
                    if (PlayerBoard.tookCard == true)
                        human_player.board.DisableLastDroppedTile();
                }

                // fix to the computer player board
                computer_player.board.drawn_computer_cards = new List<Label>();

                human_player.board.GenerateTiles();
                computer_player.board.GenerateBoard();

                // changing the labels
                pool.UpdatePoolSizeLabel();

                // checking if game over
                if (game_over)
                    human_player.board.disableHumanBoard();
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
    }
}
