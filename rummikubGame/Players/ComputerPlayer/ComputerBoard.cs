using Rummikub;
using rummikubGame.Models;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class ComputerBoard : IBoard
    {
        // consts
        const int STARTING_HAND_X_LOCATION_COMPUTER_TILES = 50;
        const int STARTING_HAND_Y_LOCATION_COMPUTER_TILES = 95;
        const int STARTING_SEQUENCES_X_LOCATION_COMPUTER_TILES = 50;
        const int STARTING_SEQUENCES_Y_LOCATION_COMPUTER_TILES = 185;
        const int SECOND_SEQUENCES_X_LOCATION_COMPUTER_TILES = 300;
        const int SECOND_SEQUENCES_Y_LOCATION_COMPUTER_TILES = 170;
        const int X_SPACE_BETWEEN_COMPUTER_TILES = 31;
        const int Y_SPACE_BETWEEN_SEQUENCES = 50;
        const List<PartialSet> PARTIAL_SET_VAR_PASSED = null;

        // variables of the board
        public List<Tile> hand;
        public List<PartialSet> partial_sets;
        public List<List<Tile>> sequences;
        public List<Tile> unused_jokers;

        [NonSerialized]
        public List<Label> drawn_computer_cards;

        public int GetHandTilesNumber()
        {
            return Constants.RummikubTilesInGame - RummikubGameView.ComputerPlayer.board.GetNumberOfTilesInAllSets(sequences);
        }

        public bool CheckWinner()
        {
            if (hand.Count() + 2*partial_sets.Count() == 0)
                return true;
            return false;
        }

        public ComputerBoard()
        {
            unused_jokers = new List<Tile>();
            partial_sets = new List<PartialSet>();
            drawn_computer_cards = new List<Label>();
            hand = new List<Tile>();

            // fills the tiles list
            for (int i = 0; i < Constants.RummikubTilesInGame; i++)
            {
                // change this
                Tile tile = RummikubGameView.Pool.GetTile();

                if (tile.Number == 0)
                {
                    unused_jokers.Add(tile);
                }
                else
                {
                    hand.Add(tile);
                }
            }
        }

        // ---------------------------------------------------------
        /// <summary>
        /// creates partial sets from the given hand
        /// </summary>
        /// <param></param>
        // ---------------------------------------------------------
        public List<PartialSet> CreatePartialSets(ref List<Tile> hand, List<PartialSet> existing_partial_set = PARTIAL_SET_VAR_PASSED)
        {
            if (existing_partial_set == PARTIAL_SET_VAR_PASSED)
            {
                existing_partial_set = new List<PartialSet>();
            }

            List<PartialSet> partial_sets = existing_partial_set;
            List<int> indexes = new List<int>();

            // find runs
            for (int i = 0; i < hand.Count(); i++)
            {
                for (int j = 0; j < hand.Count(); j++)
                {
                    Tile tile1 = hand[i];
                    Tile tile2 = hand[j];

                    if (i != j && (Math.Abs(tile1.Number - tile2.Number) == 1
                        && tile1.Color == tile2.Color))
                    {
                        if (!indexes.Contains(i) && !indexes.Contains(j))
                        {
                            // create partial set
                            PartialSet partialSet = new PartialSet(tile1, tile2);
                            partialSet.SortPartialSet();
                            partial_sets.Add(partialSet);

                            // add the indexes of the tiles that are in the partial set
                            indexes.Add(i);
                            indexes.Add(j);
                        }
                    }
                }
            }

            // find groups
            for (int i = 0; i < hand.Count(); i++)
            {
                for (int j = 0; j < hand.Count(); j++)
                {
                    Tile tile1 = hand[i];
                    Tile tile2 = hand[j];
                    if (i != j && (tile1.Number == tile2.Number && tile1.Color != tile2.Color) ||
                    ((Math.Abs(tile1.Number - tile2.Number) == 2 || Math.Abs(tile1.Number - tile2.Number) == 1)
                    && tile1.Color == tile2.Color))
                    {
                        if (!indexes.Contains(i) && !indexes.Contains(j))
                        {
                            partial_sets.Add(new PartialSet(tile1, tile2));
                            indexes.Add(i);
                            indexes.Add(j);
                        }
                    }
                }
            }

            // duplicating the hand
            List<Tile> temp_hand = new List<Tile>();
            for (int i = 0; i < hand.Count(); i++)
            {
                temp_hand.Add(hand[i]);
            }

            // remove the tiles that are in partial sets
            for (int i = 0; i < indexes.Count(); i++)
            {
                hand.Remove(temp_hand[indexes[i]]);
            }

            return partial_sets;
        }

        public List<Tile> GetAllJokers()
        {
            List<Tile> jokers = new List<Tile>(unused_jokers);

            // adding hand to all_tiles
            for (int j = 0; j < hand.Count(); j++)
            {
                if (hand[j] != null && hand[j].Number == 0)
                    jokers.Add(hand[j]);
            }

            // adding sequences to all_tiles
            for (int j = 0; j < sequences.Count(); j++)
            {
                for (int k = 0; k < sequences[j].Count(); k++)
                {
                    if (sequences[j][k].Number == 0)
                        jokers.Add(sequences[j][k]);
                }
            }

            // adding partial sets to all_tiles
            for (int j = 0; j < partial_sets.Count(); j++)
            {
                if (partial_sets[j].Tile1.Number == 0)
                    jokers.Add((Tile)partial_sets[j].Tile1);

                if (partial_sets[j].Tile2.Number == 0)
                    jokers.Add((Tile)partial_sets[j].Tile2);
            }

            return jokers;
        }

        public void GetAllTiles(ref List<Tile> all_tiles, ref List<Tile> jokers)
        {
            jokers = new List<Tile>(unused_jokers);

            // adding hand to all_tiles
            for (int j = 0; j < hand.Count(); j++)
            {
                if (hand[j] != null && hand[j].Number != 0)
                    all_tiles.Add(hand[j]);
                else
                    jokers.Add(hand[j]);
            }

            // adding sequences to all_tiles
            for (int j = 0; j < sequences.Count(); j++)
            {
                for (int k = 0; k < sequences[j].Count(); k++)
                {
                    if (sequences[j][k].Number != 0)
                        all_tiles.Add(sequences[j][k]);
                    else
                        jokers.Add(sequences[j][k]);
                }
            }

            // adding partial sets to all_tiles
            for (int j = 0; j < partial_sets.Count(); j++)
            {
                if (partial_sets[j].Tile1.Number != 0)
                    all_tiles.Add((Tile)partial_sets[j].Tile1);
                else
                    jokers.Add((Tile)partial_sets[j].Tile1);

                if (partial_sets[j].Tile2.Number != 0)
                    all_tiles.Add((Tile)partial_sets[j].Tile2);
                else
                    jokers.Add((Tile)partial_sets[j].Tile2);
            }
        }

        public int CountJokers()
        {
            // count number of jokers in sequences
            int jokers_in_sequences = unused_jokers.Count();
            for (int i = 0; i < sequences.Count(); i++)
                for (int j = 0; j < sequences[i].Count(); j++)
                    if (sequences[i][j].Number == 0)
                        jokers_in_sequences++;

            return jokers_in_sequences;
        }

        public void GenerateBoard()
        {   // draws the tiles of the computer
            int curr_x_location_drawing_point = STARTING_HAND_X_LOCATION_COMPUTER_TILES;
            int curr_y_location_drawing_point = STARTING_HAND_Y_LOCATION_COMPUTER_TILES;

            // draws the hand tiles
            for (int i = 0; i < hand.Count(); i++)
            {
                Point tile_location = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                DrawComputerTile(hand[i], tile_location);
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
                        DrawComputerTile(sequences[i][j], tile_location);
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
                    DrawComputerTile(partial_sets[i].Tile1, tile_location1);

                    curr_x_location_drawing_point += X_SPACE_BETWEEN_COMPUTER_TILES;

                    Point tile_location2 = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                    DrawComputerTile(partial_sets[i].Tile2, tile_location2);

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
            if(unused_jokers != null)
            {
                for (int i = 0; i < unused_jokers.Count(); i++)
                {
                    Point tile_location = new Point(curr_x_location_drawing_point, curr_y_location_drawing_point);
                    DrawComputerTile(unused_jokers[i], tile_location);
                    curr_x_location_drawing_point += X_SPACE_BETWEEN_COMPUTER_TILES;
                }
            }
        }

        public bool IsRun(List<Tile> sequence)
        {
            // assuming valid sequence(group or run)
            return sequence[0].Color == sequence[1].Color;
        }

        public void DrawComputerTile(Tile tile, Point point)
        {
            Label tilePictureBox = new Label();
            tilePictureBox.Size = new Size(27, 40);
            tilePictureBox.Location = point;
            tilePictureBox.BackColor = Color.FromArgb(247, 211, 184);
            tilePictureBox.TextAlign = ContentAlignment.MiddleCenter;
            tilePictureBox.Text = tile.Number.ToString();
            tilePictureBox.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            if (tile.Color == 0) tilePictureBox.ForeColor = (Color.Blue);
            else if (tile.Color == 1) tilePictureBox.ForeColor = (Color.Black);
            else if (tile.Color == 2) tilePictureBox.ForeColor = (Color.Yellow);
            else tilePictureBox.ForeColor = (Color.Red);

            if (tile.Number == 0)
            {
                tilePictureBox.Text = "J";
                tilePictureBox.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            }

            drawn_computer_cards.Add(tilePictureBox);
            RummikubGameView.GlobalRummikubGameViewContext.Controls.Add(drawn_computer_cards[drawn_computer_cards.Count() - 1]);
            tilePictureBox.BringToFront();
        }

        public void ClearBoard()
        {   
            // deletes the visibility of the computer tiles
            for (int i = 0; (drawn_computer_cards.Count != 0) && i < drawn_computer_cards.Count(); i++)
                RummikubGameView.GlobalRummikubGameViewContext.Controls.Remove(drawn_computer_cards[i]);
        }

        public void GenerateComputerThrownTile(Tile thrownTile)
        {
            if (RummikubGameView.DroppedTilesStack.Count() > 1)
                RummikubGameView.DroppedTilesStack.Peek().TileButton.SetDraggable(false);

            Tile current_tile_from_pool = thrownTile;
            int[] slot_location = {Constants.DroppedTileLocation, Constants.DroppedTileLocation };

            VisualTile computers_thrown_tile = new VisualTile(current_tile_from_pool.Color, current_tile_from_pool.Number, slot_location);
            computers_thrown_tile.TileButton.GetButton().Location = new Point(RummikubGameView.GlobalDroppedTilesBtn.Location.X + 10, RummikubGameView.GlobalDroppedTilesBtn.Location.Y + 18);
            RummikubGameView.HumanPlayer.board.TileDesigner(computers_thrown_tile, current_tile_from_pool, true);
            RummikubGameView.DroppedTilesStack.Push(computers_thrown_tile);
        }

        // ---------------------------------------------------------
        /// <summary>
        /// deleting from board.jokers the jokers that are in the sequences
        /// </summary>
        // ---------------------------------------------------------
        public void UpdatingUnusedJokers(ref List<Tile> jokers, ref List<List<Tile>> sequences)
        {
            List<Tile> tiles_to_remove = new List<Tile>();

            for (int i = 0; i < jokers.Count(); i++)
            {
                for (int j = 0; j < sequences.Count(); j++)
                {
                    for (int k = 0; k < sequences[j].Count(); k++)
                    {
                        // if the joker is in the sequence - add it to the values list
                        if (jokers[i].Number == sequences[j][k].Number
                            && jokers[i].Color == sequences[j][k].Color)
                        {
                            tiles_to_remove.Add(jokers[i]);
                        }
                    }
                }
            }

            // removing used jokers
            for (int i = 0; i < tiles_to_remove.Count; i++)
                jokers.Remove(tiles_to_remove[i]);
        }

        public int GetNumberOfTilesInAllSets(List<List<Tile>> sequences)
        {
            int sum = 0;
            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                    sum += sequences[i].Count();
            }
            return sum;
        }
    }
}
