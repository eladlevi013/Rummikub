using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace rummikubGame
{
    public partial class GameTable : Form
    {
        // consts
        public const int COMPUTER_PLAYER_TURN = 0;
        public const int HUMAN_PLAYER_TURN = 1;
        public const int RUMMIKUB_TILES_IN_GAME = 14;
        public const int MAX_POSSIBLE_SEQUENCES_NUMBER = 4;
        public const int DROPPED_TILE_LOCATION = -1;
        public const int BLUE_COLOR = 0;
        public const int BLACK_COLOR = 1;
        public const int YELLOW_COLOR = 2;
        public const int RED_COLOR = 3;

        // joker consts
        public const int JOKER_NUMBER = 0;

        // graphical consts
        public const int HUMAN_PLAYER_BOARD_HEIGHT = 2;
        public const int HUMAN_PLAYER_BOARD_WIDTH = 10;
        public const int TILE_WIDTH = 75;
        public const int TILE_HEIGHT = 100;

        // game indicator messages
        public static String TAKE_TILE_FROM_POOL_STACK_MSG = "Your turn - take tile from pool/stack";
        public static String DROP_TILE_FROM_BOARD_MSG = "Your turn - drop tile from board";

        // players
        public static HumanPlayer human_player; // human-player
        public static ComputerPlayer computer_player; // computer-player

        // UI global elements - needs to be accessed outside this class
        public static Label global_game_indicator_lbl;
        public static Label global_current_pool_size_lbl;
        public static Form global_gametable_context; // used in order to add buttons from other classes
        public static Button global_dropped_tiles_btn; // dropped_tiles button, used in the mouseUp
        public static CheckBox global_view_computer_tiles_groupbox; // groupBox show computer tiles

        // global variables
        public static int current_turn; // indicates who should play now
        public static bool game_over = false; // game is running or over
        public static Pool pool; // the pool of cards
        public static Stack<TileButton> dropped_tiles_stack; // the stack of dropped cards

        public GameTable()
        {
            InitializeComponent();
        }

        public void startGameObjectCreation()
        {
            // create objects
            dropped_tiles_stack = new Stack<TileButton>(); // empty dropped tiles
            pool = new Pool(); // generate rummikub tiles
            human_player = new HumanPlayer("Player Default Name");
            computer_player = new ComputerPlayer();
        }

        public void startGameSetTurn()
        {
            // Sets the starting player and start the game
            Random rnd = new Random();
            current_turn = rnd.Next(0, 2);
            if (current_turn == COMPUTER_PLAYER_TURN)
            {
                game_indicator_lbl.Text = "Computer's turn";
                computer_player.play(null);
            }
            else
            {
                game_indicator_lbl.Text = "Your turn";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // update global variables
            global_view_computer_tiles_groupbox = show_computer_tiles_checkbox;
            global_current_pool_size_lbl = current_pool_size_lbl;
            global_gametable_context = this; // updates the gameTable context
            global_dropped_tiles_btn = dropped_tiles_btn; // updates the dropped_tiles variable, so it'll be accessed outside that class
            global_game_indicator_lbl = game_indicator_lbl;

            startGameObjectCreation();

            // change the style of the drop_TileButtons_location
            global_dropped_tiles_btn.FlatStyle = FlatStyle.Flat;
            global_dropped_tiles_btn.FlatAppearance.BorderSize = 0;
            global_dropped_tiles_btn.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");

            // set background color
            computerTiles_groupbox.BackColor = System.Drawing.ColorTranslator.FromHtml("#454691"); // chancing back color of groupbox
            board_panel.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");

            // sets design of the pool_btn
            pool_btn.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");
            pool_btn.FlatStyle = FlatStyle.Flat;
            pool_btn.FlatAppearance.BorderSize = 0;

            // set button flat design
            sort_color_btn.FlatStyle = FlatStyle.Flat;
            sort_color_btn.FlatAppearance.BorderSize = 0;
            sort_value_btn.FlatStyle = FlatStyle.Flat;
            sort_value_btn.FlatAppearance.BorderSize = 0;

            // this will send back the panel(the board)
            board_panel.SendToBack();

            startGameSetTurn();

            // if the game is over, and the computer won
            if (computer_player.board.checkWinner() == true && GameTable.game_over == false)
            {
                MessageBox.Show("Computer Won!");
                GameTable.global_game_indicator_lbl.Text = "Game Over - Computer Won";
                GameTable.human_player.board.disableHumanBoard();

                if (GameTable.dropped_tiles_stack.Count > 0)
                    GameTable.dropped_tiles_stack.Peek().getTileButton().Enabled = false;
                GameTable.game_over = true;
            }
        }

        public static bool checkWinner(List<List<Tile>> melds) 
        {
            // if all melds are good, user won
            for (int i = 0; i < melds.Count(); i++)
            {
                if (!GameTable.isLegalMeld(melds[i]))
                    return false;
            }
            return true;
        }

        /*
        public static bool isLegalMeld(List<Tile> meld)
        {
            if (meld.Count() < 3)
                return false;

            bool isRun = true;
            int color = meld[0].getColor();
            int value = meld[0].getNumber();

            for (int i = 1; i < meld.Count(); i++)
            {
                if (meld[i].getNumber() != value + i || meld[i].getColor() != color)
                {
                    isRun = false; break;
                }
            }
            if (isRun) return true; // 2+ run sequence

            if (meld.Count() > 4) return false; // group of 4+ cannot be exists
            for (int i = 0; i < meld.Count() - 1; i++)
            {
                if (meld[i + 1].getNumber() != value) return false; // its cannot be group
                for (int j = i + 1; j < meld.Count(); j++)
                {
                    if (meld[i].getColor() == meld[j].getColor())
                        return false;
                }
            }
            return true;
        }
        */

        public static bool isLegalMeld(List<Tile> meld)
        {
            if (meld.Count < 3)
                return false;

            bool isRun = true;

            // finds the first non-joker tile and uses it to determine the color and value of the run
            int first_non_joker_index = 0;
            for(int i = 0; i < meld.Count; i++)
            {
                if (!isJoker(meld[i]))
                {
                    first_non_joker_index = i;
                    break;
                }
            }

            int color = meld[first_non_joker_index].getColor();
            int value = meld[first_non_joker_index].getNumber();

            for (int i = first_non_joker_index + 1; i < meld.Count; i++)
            {
                // if meld number is not equal to the value + the index of the tile in the meld its cant be a run
                if (meld[i].getNumber() != value + i - first_non_joker_index
                    && meld[i].getColor() != color && !isJoker(meld[i]))
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
                global_game_indicator_lbl.Text = "good run.";
                return true; // 2+ run sequence
            }

            if (meld.Count > 4)
                return false; // group of 4+ cannot exist

            for (int i = 0; i < meld.Count() - 1; i++)
            {
                if (meld[i + 1].getNumber() != value && !isJoker(meld[i + 1])) 
                    return false; // its cannot be group
                for (int j = i + 1; j < meld.Count(); j++)
                {
                    if (meld[i].getColor() == meld[j].getColor() && !isJoker(meld[i]) && !isJoker(meld[j]))
                    {
                        global_game_indicator_lbl.Text = "bad.";
                        return false;
                    }
                }
            }

            // check if there are too many jokers used
            int numJokers = countJokers(meld);
            if (numJokers > 2)
                return false; // too many jokers used

            global_game_indicator_lbl.Text = "good group.";
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

        private static int countDistinctColors(List<Tile> meld)
        {
            HashSet<int> colors = new HashSet<int>();
            foreach (Tile tile in meld)
            {
                if (!isJoker(tile))
                    colors.Add(tile.getColor());
            }
            return colors.Count;
        }

        private static bool isJoker(Tile tile)
        {
            return tile.getNumber() == JOKER_NUMBER;
        }

        private void pool_btn_Click(object sender, EventArgs e)
        {
            // generate a card to the last-empty place in the board
            bool found_last_empty_location = false;
            for (int i = HUMAN_PLAYER_BOARD_HEIGHT - 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = HUMAN_PLAYER_BOARD_WIDTH - 1; j >= 0 && !found_last_empty_location; j--)
                {
                    if (human_player.board.getTileButtonSlot()[i, j].getState() == Slot.AVAILABLE)
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
            List<TileButton> sorted_cards = human_player.board.getTilesDictionary().Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.getNumber()).ToList();
            human_player.board.ArrangeCardsOnBoard(sorted_cards);
        }

        private void sort_color_btn_click(object sender, EventArgs e)
        {
            List<TileButton> sorted_cards = human_player.board.getTilesDictionary().Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.getColor()).ToList();
            human_player.board.ArrangeCardsOnBoard(sorted_cards);
        }

        private void show_computer_tiles_checkbox_change(object sender, EventArgs e)
        {   // if to delete or create the graphical representation of the computer tiles
            if (show_computer_tiles_checkbox.Checked == false)
            {
                computerTiles_groupbox.Visible = false;
                computer_player.board.clearBoard();
            }
            else
            {
                computerTiles_groupbox.Visible = true;
                computer_player.board.generateBoard();
            }
        }

        private void clearAllTilesFromScreen()
        {
            // Clearning the boards
            human_player.board.clearBoard();
            computer_player.board.clearBoard();

            // Clearing dropped tiles
            while (GameTable.dropped_tiles_stack.Count > 0)
            {
                GameTable.global_gametable_context.Controls.Remove(GameTable.dropped_tiles_stack.Peek().getTileButton());
                GameTable.dropped_tiles_stack.Pop();
            }
        }

        public void startNewGame()
        {
            clearAllTilesFromScreen();

            // Sets more vars
            GameTable.game_over = false;
            GameTable.global_dropped_tiles_btn.Enabled = true;
            PlayerBoard.tookCard = false;

            // Sets the game to start
            startGameObjectCreation();
            startGameSetTurn();
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
                Stream stream = new FileStream("save.rummikub", FileMode.Create);
                formatter.Serialize(stream, human_player);
                formatter.Serialize(stream, computer_player);

                // saving game info
                formatter.Serialize(stream, GameTable.current_turn);
                formatter.Serialize(stream, GameTable.game_over);
                formatter.Serialize(stream, GameTable.pool);
                formatter.Serialize(stream, GameTable.dropped_tiles_stack);
                formatter.Serialize(stream, PlayerBoard.tookCard);
                formatter.Serialize(stream, PlayerBoard.TAG_NUMBER);
                formatter.Serialize(stream, GameTable.global_game_indicator_lbl.Text);
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
                Stream stream = new FileStream("save.rummikub", FileMode.Open);
                human_player = (HumanPlayer)formatter.Deserialize(stream);
                computer_player = (ComputerPlayer)formatter.Deserialize(stream);

                GameTable.current_turn = (int)formatter.Deserialize(stream);
                GameTable.game_over = (bool)formatter.Deserialize(stream);
                GameTable.pool = (Pool)formatter.Deserialize(stream);
                GameTable.dropped_tiles_stack = (Stack<TileButton>)formatter.Deserialize(stream);
                PlayerBoard.tookCard = (bool)formatter.Deserialize(stream);
                PlayerBoard.TAG_NUMBER = (int)formatter.Deserialize(stream);
                GameTable.global_game_indicator_lbl.Text = (string)formatter.Deserialize(stream);
                stream.Close();

                // fix dropped tiles stack
                Stack<TileButton> temp_dropped_tiles = dropped_tiles_stack;
                Stack<TileButton> revered_dropped_tiles = new Stack<TileButton>();
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
                computer_player.board.drawn_computer_cards = new List<Button>();

                human_player.board.generateTiles();
                computer_player.board.generateBoard();

                // changing the labels
                pool.updatePoolSizeLabel();

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
    }
}