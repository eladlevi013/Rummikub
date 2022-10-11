using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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
        public const int HUMAN_PLAYER_BOARD_HEIGHT = 2;
        public const int HUMAN_PLAYER_BOARD_WIDTH = 10;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            // update global variables
            global_view_computer_tiles_groupbox = show_computer_tiles_checkbox;
            global_current_pool_size_lbl = current_pool_size_lbl;
            global_gametable_context = this; // updates the gameTable context
            global_dropped_tiles_btn = dropped_tiles_btn; // updates the dropped_tiles variable, so it'll be accessed outside that class
            global_game_indicator_lbl = game_indicator_lbl;

            // create objects
            dropped_tiles_stack = new Stack<TileButton>(); // empty dropped tiles
            pool = new Pool(); // generate rummikub tiles
            human_player = new HumanPlayer("Player Default Name"); 
            computer_player = new ComputerPlayer();

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

            // Sets the starting player and start the game
            Random rnd = new Random();
            current_turn = rnd.Next(0, 2);
            if (current_turn == COMPUTER_PLAYER_TURN)
            {
                computer_player.play(null);
                game_indicator_lbl.Text = "Computer's turn";
            }
            else
            {
                game_indicator_lbl.Text = "Your turn";
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

        public static bool isLegalMeld(List<Tile> meld)
        {
            /*
             Legal meld, can be:
                - group(same number - different colors)
                - run(same color - different numbers(accending order))
             */

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

        private void pool_btn_Click(object sender, EventArgs e)
        {
            // generate a card to the last-empty place in the board
            bool found_last_empty_location = false;
            for (int i = HUMAN_PLAYER_BOARD_HEIGHT - 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = HUMAN_PLAYER_BOARD_WIDTH - 1; j >= 0 && !found_last_empty_location; j--)
                {
                    if (human_player.board.getTileButton_slot()[i, j].getState() == false)
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
                computer_player.board.deleteCards();
            }
            else
            {
                computerTiles_groupbox.Visible = true;
                computer_player.board.generateBoard();
            }
        }
    }
}