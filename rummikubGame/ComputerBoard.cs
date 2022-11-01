using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class ComputerBoard : Board
    {
        // variables of the board
        public List<Tile> hand;
        public List<List<Tile>> sequences;
        private List<Button> drawn_computer_cards;

        public int getHandTilesNumber()
        {
            return GameTable.RUMMIKUB_TILES_IN_GAME - GameTable.computer_player.getNumberOfTilesInAllSets(sequences);
        }

        public bool checkWinner()
        {
            // first we'll find the number of tiles in hand
            int hand_counter = 0;
            for(int hand_index =0; hand_index < hand.Count(); hand_index++)
            {
                if (hand[hand_index] != null)
                    hand_counter += 1;
            }
            if (hand_counter == 0)
                return true;
            return false;
        }

        public ComputerBoard()
        {
            drawn_computer_cards = new List<Button>();
            hand = new List<Tile>();

            /*
            hand.Add(new Tile(3, 3));
            hand.Add(new Tile(3, 4));
            hand.Add(new Tile(3, 5));
            hand.Add(new Tile(3, 6));

            hand.Add(new Tile(2, 3));
            hand.Add(new Tile(2, 4));
            hand.Add(new Tile(2, 5));

            hand.Add(new Tile(0, 3));
            hand.Add(new Tile(0, 4));
            hand.Add(new Tile(0, 5));

            hand.Add(new Tile(1, 9));
            hand.Add(new Tile(1, 10));
            hand.Add(new Tile(1, 11));
            hand.Add(new Tile(1, 12));
            */

            /*
            hand.Add(new Tile(3, 4));
            hand.Add(new Tile(3, 5));
            hand.Add(new Tile(3, 6));
            hand.Add(new Tile(3, 7));
            hand.Add(new Tile(2, 4));
            hand.Add(new Tile(0, 4));
            */

            // double group
            /*
            hand.Add(new Tile(0, 13));
            hand.Add(new Tile(2, 13));
            hand.Add(new Tile(3, 13));
            hand.Add(new Tile(0, 13));
            hand.Add(new Tile(2, 13));
            hand.Add(new Tile(3, 13));
            */

            // double run
            /*
            hand.Add(new Tile(3, 4));
            hand.Add(new Tile(3, 5));
            hand.Add(new Tile(3, 6));
            hand.Add(new Tile(3, 4));
            hand.Add(new Tile(3, 5));
            hand.Add(new Tile(3, 6));
            */
            /*
            hand.Add(new Tile(1, 1));
            hand.Add(new Tile(1, 2));
            hand.Add(new Tile(1, 3));
            hand.Add(new Tile(1, 4));
            hand.Add(new Tile(1, 5));
            hand.Add(new Tile(1, 6));
            hand.Add(new Tile(0, 6));
            hand.Add(new Tile(2, 6));
            hand.Add(new Tile(1, 8));
            hand.Add(new Tile(1, 9));
            hand.Add(new Tile(1, 10));
            hand.Add(new Tile(1, 11));
            hand.Add(new Tile(3, 11));
            hand.Add(new Tile(0, 11));
            */

            // fills the tiles list
            for (int i = 0; i < GameTable.RUMMIKUB_TILES_IN_GAME; i++)
                hand.Add(GameTable.pool.getTile());
        }

        public void generateBoard()
        {   // draws the tiles of the computer
            int starting_x_computer_tiles = 50;
            int starting_y_computer_tiles = 80;

            // draws the hand tiles
            for (int i = 0; i < hand.Count(); i++)
            {
                Point tile_location = new Point(starting_x_computer_tiles, starting_y_computer_tiles);
                if (hand[i] != null)
                {
                    drawSingleComputerCard(hand[i], tile_location);
                    starting_x_computer_tiles += 40;
                }
            }

            // draws the sequences tiles
            starting_x_computer_tiles = 50;
            starting_y_computer_tiles = 170;
            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    for (int j = 0; j < sequences[i].Count(); j++)
                    {
                        Point tile_location = new Point(starting_x_computer_tiles, starting_y_computer_tiles);
                        drawSingleComputerCard(sequences[i][j], tile_location);
                        starting_x_computer_tiles += 40;
                    }
                    if (i == 2)
                    {   // if there are alot of sequences continue drawing in another area
                        starting_x_computer_tiles = 300;
                        starting_y_computer_tiles = 120;
                    }
                    else
                    {   // in any other situation start at the next line
                        starting_x_computer_tiles = 50;
                    }
                    starting_y_computer_tiles += 50;
                }
            }
        }

        public void drawSingleComputerCard(Tile tile, Point point)
        {   // draws the given tile at the given location
            Button tileButton = new Button();
            tileButton.Size = new Size(35, 40);
            tileButton.BackgroundImage = Image.FromFile("Tile.png");
            tileButton.BackgroundImageLayout = ImageLayout.Stretch;
            tileButton.Draggable(true); // usage of the extension
            tileButton.FlatStyle = FlatStyle.Flat;
            tileButton.FlatAppearance.BorderSize = 0;
            tileButton.Text = tile.getNumber().ToString();
            tileButton.Location = point;
            if (tile.getColor() == 0) tileButton.ForeColor = (Color.Blue);
            else if (tile.getColor() == 1) tileButton.ForeColor = (Color.Black);
            else if (tile.getColor() == 2) tileButton.ForeColor = (Color.Yellow);
            else tileButton.ForeColor = (Color.Red);
            tileButton.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            drawn_computer_cards.Add(tileButton);
            GameTable.global_gametable_context.Controls.Add(drawn_computer_cards[drawn_computer_cards.Count() - 1]);
            tileButton.BringToFront();
        }

        public void deleteCardsVisibility()
        {   // deletes the visibility of the computer tiles
            for (int i = 0; (drawn_computer_cards.Count != 0) && i < drawn_computer_cards.Count(); i++)
                GameTable.global_gametable_context.Controls.Remove(drawn_computer_cards[i]);
        }

        public void GenerateComputerThrownTile(Tile thrownTile)
        {
            if (GameTable.dropped_tiles_stack.Count() > 1)
                GameTable.dropped_tiles_stack.Peek().getTileButton().Draggable(false);

            Tile current_tile_from_pool = thrownTile;
            int[] slot_location = { GameTable.DROPPED_TILE_LOCATION, GameTable.DROPPED_TILE_LOCATION };

            TileButton computers_thrown_tile = new TileButton(current_tile_from_pool.getColor(), current_tile_from_pool.getNumber(), slot_location);
            computers_thrown_tile.getTileButton().Location = new Point(GameTable.global_dropped_tiles_btn.Location.X + 10, GameTable.global_dropped_tiles_btn.Location.Y + 18);
            GameTable.human_player.board.TileDesigner(computers_thrown_tile, current_tile_from_pool);
            GameTable.dropped_tiles_stack.Push(computers_thrown_tile);
        }
    }
}