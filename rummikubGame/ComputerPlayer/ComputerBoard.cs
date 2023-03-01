using rummikubGame.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class ComputerBoard : Board
    {
        // consts
        const int STARTING_HAND_X_LOCATION_COMPUTER_TILES = 50;
        const int STARTING_HAND_Y_LOCATION_COMPUTER_TILES = 95;
        const int STARTING_SEQUENCES_X_LOCATION_COMPUTER_TILES = 50;
        const int STARTING_SEQUENCES_Y_LOCATION_COMPUTER_TILES = 185;
        const int SECOND_SEQUENCES_X_LOCATION_COMPUTER_TILES = 300;
        const int SECOND_SEQUENCES_Y_LOCATION_COMPUTER_TILES = 170;
        const int X_SPACE_BETWEEN_COMPUTER_TILES = 40;
        const int Y_SPACE_BETWEEN_SEQUENCES = 50;

        // variables of the board
        public List<Tile> hand;
        public List<PartialSet> partial_sets;
        public List<List<Tile>> sequences;
        public List<Tile> jokers;

        [NonSerialized]
        public List<Button> drawn_computer_cards;

        public int getHandTilesNumber()
        {
            return GameTable.RUMMIKUB_TILES_IN_GAME - GameTable.computer_player.getNumberOfTilesInAllSets(sequences);
        }

        public bool checkWinner()
        {
            if (hand.Count() + partial_sets.Count()*2 == 0)
                return true;
            return false;
        }

        public ComputerBoard()
        {
            jokers = new List<Tile>();
            partial_sets = new List<PartialSet>();
            drawn_computer_cards = new List<Button>();
            hand = new List<Tile>();

            string str = "";

            // fills the tiles list
            for (int i = 0; i < GameTable.RUMMIKUB_TILES_IN_GAME; i++)
            {
                // change this
                Tile tile = GameTable.pool.getTile();

                str += "tile: " + i + "num:" + tile.getNumber() + "color: " + tile.getColor() + "\n";

                if (tile.getNumber() == 0)
                {
                    jokers.Add(tile);
                }
                else
                {
                    hand.Add(tile);
                }
            }

            // GameTable.global_current_pool_size_lbl.Text = str;

            /*
            hand.Add(new Tile(GameTable.RED_COLOR, 2));
            hand.Add(new Tile(GameTable.RED_COLOR, 3));
            hand.Add(new Tile(GameTable.RED_COLOR, 4));
            hand.Add(new Tile(GameTable.RED_COLOR, 7));
            hand.Add(new Tile(GameTable.RED_COLOR, 11));

            jokers.Add(new Tile(GameTable.RED_COLOR, 0));

            hand.Add(new Tile(GameTable.BLUE_COLOR,1));
            hand.Add(new Tile(GameTable.BLUE_COLOR, 3));
            hand.Add(new Tile(GameTable.BLUE_COLOR, 5));
            hand.Add(new Tile(GameTable.BLUE_COLOR, 12));
            hand.Add(new Tile(GameTable.BLUE_COLOR, 13));

            hand.Add(new Tile(GameTable.YELLOW_COLOR, 5));
            hand.Add(new Tile(GameTable.YELLOW_COLOR, 13));

            hand.Add(new Tile(GameTable.BLACK_COLOR, 11));
            */
        }

        public void generateBoard()
        {   // draws the tiles of the computer
            int curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES;
            int curr_y_location_drawing_point = STARTING_HAND_Y_LOCATION_COMPUTER_TILES;

            // draws the hand tiles
            for (int i = 0; i < hand.Count(); i++)
            {
                Point tile_location = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                drawSingleComputerCard(hand[i], tile_location);
                curr_x_location_drawing_point += X_SPACE_BETWEEN_COMPUTER_TILES;
            }

            // draws the sequences tiles
            curr_x_location_drawing_point = STARTING_SEQUENCES_X_LOCATION_COMPUTER_TILES;
            curr_y_location_drawing_point = STARTING_SEQUENCES_Y_LOCATION_COMPUTER_TILES;
            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    for (int j = 0; j < sequences[i].Count(); j++)
                    {
                        Point tile_location = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                        drawSingleComputerCard(sequences[i][j], tile_location);
                        curr_x_location_drawing_point += X_SPACE_BETWEEN_COMPUTER_TILES;
                    }
                    if (i >= 2)
                    {
                        if(i == 2)
                        {
                            // if there are alot of sequences continue drawing in another area
                            curr_x_location_drawing_point = SECOND_SEQUENCES_X_LOCATION_COMPUTER_TILES;
                            curr_y_location_drawing_point = SECOND_SEQUENCES_Y_LOCATION_COMPUTER_TILES - Y_SPACE_BETWEEN_SEQUENCES;
                        }
                        else
                        {
                            curr_x_location_drawing_point = SECOND_SEQUENCES_X_LOCATION_COMPUTER_TILES;
                        }
                    }
                    else
                    {   // in any other situation start at the next line
                        curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES;
                    }
                    curr_y_location_drawing_point += Y_SPACE_BETWEEN_SEQUENCES;
                }
            }

            // draw the partial sets
            curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES + 400;
            curr_y_location_drawing_point = STARTING_HAND_Y_LOCATION_COMPUTER_TILES;
            if (partial_sets != null)
            {
                for (int i = 0; i < partial_sets.Count(); i++)
                {
                    Point tile_location1 = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                    drawSingleComputerCard(partial_sets[i].Tile1, tile_location1);

                    curr_x_location_drawing_point += X_SPACE_BETWEEN_COMPUTER_TILES;

                    Point tile_location2 = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                    drawSingleComputerCard(partial_sets[i].Tile2, tile_location2);

                    curr_y_location_drawing_point += Y_SPACE_BETWEEN_SEQUENCES;
                    curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES + 400;

                    if(i == 3)
                    {
                        curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES + 495;
                        curr_y_location_drawing_point = STARTING_HAND_Y_LOCATION_COMPUTER_TILES;
                    }
                }
            }

            curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES + 495 + 40;
            curr_y_location_drawing_point = STARTING_HAND_Y_LOCATION_COMPUTER_TILES;
            if(jokers != null)
            {
                for (int i = 0; i < jokers.Count(); i++)
                {
                    Point tile_location = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                    drawSingleComputerCard(jokers[i], tile_location);
                    curr_x_location_drawing_point += X_SPACE_BETWEEN_COMPUTER_TILES;
                }
            }
        }

        public void drawSingleComputerCard(Tile tile, Point point)
        {
            // draws the given tile at the given location
            Button tileButton = new Button();
            tileButton.Size = new Size(35, 40);
            tileButton.BackgroundImageLayout = ImageLayout.Stretch;
            tileButton.FlatStyle = FlatStyle.Flat;
            tileButton.FlatAppearance.BorderSize = 0;
            // tileButton.Draggable(true);
            tileButton.Location = point;


            if(tile.getNumber() == 0)
            {
                if(tile.getColor() == GameTable.BLACK_COLOR) 
                {
                    tileButton.BackgroundImage = Image.FromFile(GameTable.BLACK_JOKER_PATH);
                }
                else
                {
                    tileButton.BackgroundImage = Image.FromFile(GameTable.RED_JOKER_PATH);
                }
            }
            else
            {
                tileButton.BackgroundImage = Image.FromFile(GameTable.TILE_PATH);
                tileButton.Text = tile.getNumber().ToString();
                if (tile.getColor() == 0) tileButton.ForeColor = (Color.Blue);
                else if (tile.getColor() == 1) tileButton.ForeColor = (Color.Black);
                else if (tile.getColor() == 2) tileButton.ForeColor = (Color.Yellow);
                else tileButton.ForeColor = (Color.Red);
            }


            tileButton.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            drawn_computer_cards.Add(tileButton);
            GameTable.global_gametable_context.Controls.Add(drawn_computer_cards[drawn_computer_cards.Count() - 1]);
            tileButton.BringToFront();
        }

        public void clearBoard()
        {   
            // deletes the visibility of the computer tiles
            for (int i = 0; (drawn_computer_cards.Count != 0) && i < drawn_computer_cards.Count(); i++)
                GameTable.global_gametable_context.Controls.Remove(drawn_computer_cards[i]);
        }

        public void GenerateComputerThrownTile(Tile thrownTile)
        {
            if (GameTable.dropped_tiles_stack.Count() > 1)
                GameTable.dropped_tiles_stack.Peek().setDraggable(false);

            Tile current_tile_from_pool = thrownTile;
            int[] slot_location = { GameTable.DROPPED_TILE_LOCATION, GameTable.DROPPED_TILE_LOCATION };

            TileButton computers_thrown_tile = new TileButton(current_tile_from_pool.getColor(), current_tile_from_pool.getNumber(), slot_location);
            computers_thrown_tile.getTileButton().Location = new Point(GameTable.global_dropped_tiles_btn.Location.X + 10, GameTable.global_dropped_tiles_btn.Location.Y + 18);
            GameTable.human_player.board.TileDesigner(computers_thrown_tile, current_tile_from_pool, true);
            GameTable.dropped_tiles_stack.Push(computers_thrown_tile);
        }
    }
}