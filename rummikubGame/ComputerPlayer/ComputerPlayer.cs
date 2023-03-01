using rummikubGame.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class ComputerPlayer
    {
        public ComputerBoard board;

        public ComputerPlayer()
        {
            board = new ComputerBoard();
            board.sequences = meldsSets(ref board.hand);
            createPartialSets();

            // takes care of the graphical board of the computer
            if (GameTable.global_view_computer_tiles_groupbox.Checked)
                board.generateBoard();
        }

        // ---------------------------------------------------------
        /// <summary>
        /// creates partial sets from the given hand
        /// </summary>
        /// <param></param>
        // ---------------------------------------------------------
        public void createPartialSets()
        {
            List<int> indexes = new List<int>();

            // find runs
            for (int i = 0; i < board.hand.Count(); i++)
            {
                for (int j = 0; j < board.hand.Count(); j++)
                {
                    Tile tile1 = board.hand[i];
                    Tile tile2 = board.hand[j];
                    if (i != j && (Math.Abs(tile1.getNumber() - tile2.getNumber()) == 1 && tile1.getColor() == tile2.getColor()))
                    {
                        if (!indexes.Contains(i) && !indexes.Contains(j))
                        {
                            board.partial_sets.Add(new PartialSet(tile1, tile2));
                            indexes.Add(i);
                            indexes.Add(j);
                        }
                    }
                }
            }

            // find groups
            for (int i = 0; i < board.hand.Count(); i++)
            {
                for (int j = 0; j < board.hand.Count(); j++)
                {
                    Tile tile1 = board.hand[i];
                    Tile tile2 = board.hand[j];
                    if (i != j && (tile1.getNumber() == tile2.getNumber() && tile1.getColor() != tile2.getColor()) ||
                    ((Math.Abs(tile1.getNumber() - tile2.getNumber()) == 2 || Math.Abs(tile1.getNumber() - tile2.getNumber()) == 1)
                    && tile1.getColor() == tile2.getColor()))
                    {
                        if (!indexes.Contains(i) && !indexes.Contains(j))
                        {
                            board.partial_sets.Add(new PartialSet(tile1, tile2));
                            indexes.Add(i);
                            indexes.Add(j);
                        }
                    }
                }
            }

            // duplicating the board.hand
            List<Tile> temp_hand = new List<Tile>();
            for (int i = 0; i < board.hand.Count(); i++)
            {
                temp_hand.Add(board.hand[i]);
            }

            // remove the tiles that are in partial sets
            for (int i = 0; i < indexes.Count(); i++)
            {
                board.hand.Remove(temp_hand[indexes[i]]);
            }
        }

        // ---------------------------------------------------------
        /// <summary>
        /// Tries to replace the given tlie with every hand tile, in order to get the tile that fitted the most.
        /// in that proccess we also know what tile we would like to drop to the stack.
        /// </summary>
        /// <param name="new_tile">the tile that we are replacing with, every hand tile</param>
        // ---------------------------------------------------------
        public bool OptimalTileToReplaceExists(Tile new_tile)
        {
            bool replaced_card_gives_better_result = false;
            List<Tile> optimal_solution_hand = board.hand;
            List<List<Tile>> optimal_solution_sequences = board.sequences;
            Tile optimal_dropped_tile = null;
            List<Tile> optimal_jokers = board.jokers;

            // count number of jokers in sequences
            int jokers_in_sequences = board.jokers.Count();
            for (int i = 0; i < board.sequences.Count(); i++)
                for (int j = 0; j < board.sequences[i].Count(); j++)
                    if (board.sequences[i][j].getNumber() == 0)
                        jokers_in_sequences++;

            int replace_tiles_number = GameTable.RUMMIKUB_TILES_IN_GAME - jokers_in_sequences;

            // replacing every tile in starting_tiles with the given paramter in order to get better result
            for (int i = 0; i < replace_tiles_number; i++)
            {
                List<Tile> starting_tiles = new List<Tile>();

                // starting_tiles will be a list of all of the tiles
                for (int j = 0; j < board.hand.Count(); j++)
                {
                    // adds the hand cards to the temp_tiles list
                    if (board.hand[j] != null && board.hand[j].getNumber() != 0)
                        starting_tiles.Add(board.hand[j]);
                    else
                        board.jokers.Add(board.hand[j]);
                }
                if (board.sequences != null)
                { // adds the tiles in sets to the temp_tiles list
                    for (int j = 0; j < board.sequences.Count(); j++)
                        for (int k = 0; k < board.sequences[j].Count(); k++)
                            if (board.sequences[j][k].getNumber() != 0)
                                starting_tiles.Add(board.sequences[j][k]);
                            else
                                board.jokers.Add(board.sequences[j][k]);
                }
                if (board.partial_sets != null)
                {
                    for (int j = 0; j < board.partial_sets.Count(); j++)
                    {
                        if(board.partial_sets[j].Tile1.getNumber() != 0)
                            starting_tiles.Add((Tile)board.partial_sets[j].Tile1);
                        else
                            board.jokers.Add((Tile)board.partial_sets[j].Tile1);

                        if (board.partial_sets[j].Tile2.getNumber() != 0)
                            starting_tiles.Add((Tile)board.partial_sets[j].Tile2);
                        else
                            board.jokers.Add((Tile)board.partial_sets[j].Tile2);
                    }
                }

                // variables that changes every iteration
                List<List<Tile>> temp_extendedSets = new List<List<Tile>>();
                List<Tile> temp_hand = new List<Tile>();
                Tile curr_dropped_tile = starting_tiles[i];
                // List<Tile> temp_jokers = new List<Tile>();

                starting_tiles[i] = new_tile;

                temp_extendedSets = meldsSets(ref starting_tiles);
                temp_hand = starting_tiles;
                // emp_jokers = new List<Tile>(board.jokers);

                // check current situation is better than the optimal
                if (getNumberOfTilesInAllSets(optimal_solution_sequences) < getNumberOfTilesInAllSets(temp_extendedSets))
                {
                    optimal_solution_sequences = temp_extendedSets;
                    optimal_solution_hand = temp_hand;
                    // optimal_jokers = temp_jokers;

                    replaced_card_gives_better_result = true;
                    optimal_dropped_tile = curr_dropped_tile;
                }
            }

            // better option than starting point
            if (replaced_card_gives_better_result)
            {
                // updates global board variables
                board.sequences = optimal_solution_sequences;
                board.hand = optimal_solution_hand;
                // board.jokers = optimal_jokers;
                board.partial_sets = new List<PartialSet>();

                // for loop over board.jokers
                // board.jokers - best_jokers
                List<Tile> values = new List<Tile>();

                for(int i=0; i<board.jokers.Count(); i++)
                {
                    for(int j=0; j<board.sequences.Count(); j++)
                    {
                        for(int k=0; k < board.sequences[j].Count(); k++)
                        {
                            if (board.jokers[i] == board.sequences[j][k])
                            {
                                values.Add((Tile)board.jokers[i]);
                            }
                        }
                    }
                }
                // removing used jokers
                for (int i = 0; i < values.Count; i++)
                    board.jokers.Remove(values[i]);

                // create partial sets from the given hand
                createPartialSets();

                // take the last thrown tile from the dropped tiles stack(graphically)
                if (GameTable.dropped_tiles_stack.Count() > 0)
                    GameTable.global_gametable_context.Controls.Remove(GameTable.dropped_tiles_stack.Peek().getTileButton());

                // generate computer thrown tile in Stack
                int[] tile_in_dropped_tiles_location = { GameTable.DROPPED_TILE_LOCATION, GameTable.DROPPED_TILE_LOCATION };
                TileButton dropped_tile = new TileButton(optimal_dropped_tile.getColor(), optimal_dropped_tile.getNumber(), tile_in_dropped_tiles_location);
                board.GenerateComputerThrownTile(dropped_tile);
                return (true);
            }
            return (false);
        }

        // ---------------------------------------------------------
        /// being called every time computer have to play, it checks if tile that I dropped helps the computer
        /// in order to get better sequences.
        /// if didnt option found take a tile from stack, and check again.
        /// </summary>
        /// <param name="to_be_replaced"></param>
        // ---------------------------------------------------------
        public void play(Tile to_be_replaced)
        {
            // better arrangement was not found
            if(to_be_replaced != null && !OptimalTileToReplaceExists(to_be_replaced))
            {
                Tile tile = GameTable.pool.getTile();
                if (tile == null) // pool is empty
                    return;

                // better option with tile from pool
                if (!OptimalTileToReplaceExists(tile))
                {
                    // if partial set found from dropped tile stack
                    if (board.hand.Count() > 1)
                    {
                        board.hand.Add(tile);
                        createPartialSets();

                        // AddJokers();

                        if (board.hand.Count() > 0)
                        {
                            Random rnd = new Random();
                            int random_tile_to_drop_index = rnd.Next(board.hand.Count());
                            Tile random_tile_to_drop = board.hand[random_tile_to_drop_index];
                            board.hand.Remove(random_tile_to_drop);
                            board.GenerateComputerThrownTile(random_tile_to_drop);
                        }
                        // hand cannot be 0. if it is, it means that the tile we dropped is not part of a set
                    }
                    else
                    {
                        // if all tiles in partial sets
                        if (board.hand.Count() == 0 && board.partial_sets.Count() > 0)
                        {
                            // if tile from pool didnt gave us better result drop random tile from partial sets
                            Random rnd_partial_sets_index = new Random();
                            bool partial_sets_null = true;
                            Tile random_tile_to_drop = null;
                            int random_tile_to_drop_index = 0;

                            while (partial_sets_null)
                            {
                                random_tile_to_drop_index = rnd_partial_sets_index.Next(board.partial_sets.Count());
                                random_tile_to_drop = (Tile)board.partial_sets[random_tile_to_drop_index].Tile1;
                                if (random_tile_to_drop != null)
                                {
                                    partial_sets_null = false;
                                }
                            }

                            board.hand.Add(tile);
                            board.hand.Add(board.partial_sets[random_tile_to_drop_index].Tile2);
                            board.partial_sets.RemoveAt(random_tile_to_drop_index);
                            board.GenerateComputerThrownTile(random_tile_to_drop);
                        }
                        else
                        {
                            // if tile from pool didnt gave us better result drop random tile from hand
                            Random rnd_hand_index = new Random();
                            bool hand_null = true;
                            Tile random_tile_to_drop = null;
                            int random_tile_to_drop_index = 0;

                            while (hand_null && board.hand.Count() > 0)
                            {
                                random_tile_to_drop_index = rnd_hand_index.Next(board.hand.Count());
                                random_tile_to_drop = board.hand[random_tile_to_drop_index];
                                if (random_tile_to_drop != null)
                                {
                                    hand_null = false;
                                }
                            }

                            if(board.hand.Count() > 0)
                                board.hand.RemoveAt(random_tile_to_drop_index);
                            board.hand.Add(tile);
                            board.GenerateComputerThrownTile(random_tile_to_drop);
                        }
                    }
                }
            }

            // done in both cases(better option or not)
            GameTable.current_turn = GameTable.HUMAN_PLAYER_TURN;
            GameTable.global_game_indicator_lbl.Text = GameTable.TAKE_TILE_FROM_POOL_STACK_MSG;
            PlayerBoard.tookCard = false;
            GameTable.computer_player.board.clearBoard();

            if (GameTable.global_view_computer_tiles_groupbox.Checked == true)
                GameTable.computer_player.board.generateBoard();

            // if the game is over, and the computer won
            if (board.checkWinner() == true && GameTable.game_over == false)
            {
                MessageBox.Show("Computer Won!");
                GameTable.global_game_indicator_lbl.Text = "Game Over - Computer Won";
                GameTable.human_player.board.disableHumanBoard();

                if (GameTable.dropped_tiles_stack.Count > 0)
                    GameTable.dropped_tiles_stack.Peek().getTileButton().Enabled = false;
                GameTable.game_over = true;
            }
        }

        public int getNumberOfTilesInAllSets(List<List<Tile>> sequences)
        {
            int sum = 0;
            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                    sum += sequences[i].Count();
            }
            return sum;
        }

        // ---------------------------------------------------------
        /// <summary>
        /// Create a set class, which contains the data of what type the set is (Group, Run)
        /// for group we dont need to check the two of the ways to arrange
        /// </summary>
        /// <param name="sequences">Current sequences we need to extend</param>
        /// <param name="hand_tiles">Remained tiles that we can extend the sequences with</param>
        // ---------------------------------------------------------
        public void extendSets(ref List<List<Tile>> sequences, ref List<Tile> hand_tiles)
        {
            List<Tile> hand_no_null = new List<Tile>();
            for(int i=0; i<hand_tiles.Count(); i++)
            {
                if (hand_tiles[i] != null)
                {
                    hand_no_null.Add(hand_tiles[i]);
                }
            }
            hand_tiles = hand_no_null;
            hand_tiles = hand_tiles.OrderBy(card => card.getNumber()).ToList();

            for(int hand_tile_index=0; hand_tile_index < hand_tiles.Count(); hand_tile_index++)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    if (hand_tiles[hand_tile_index] != null)
                    {
                        List<Tile> tempSequenceAddRight = sequences[i].Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                        tempSequenceAddRight.Add(hand_tiles[hand_tile_index]);
                        if (GameTable.isLegalMeld(tempSequenceAddRight) == true)
                        {
                            sequences[i] = tempSequenceAddRight;
                            hand_tiles[hand_tile_index] = null;
                        }
                    }
                    if (hand_tiles[hand_tile_index] != null)
                    {
                        List<Tile> tempSequenceAddLeft = sequences[i].Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                        tempSequenceAddLeft.Insert(0, hand_tiles[hand_tile_index]);
                        if (GameTable.isLegalMeld(tempSequenceAddLeft) == true)
                        {
                            sequences[i] = tempSequenceAddLeft;
                            hand_tiles[hand_tile_index] = null;
                        }
                    }
                }
            }

            // removes the null's from hand
            List<Tile> hand_no_nul = new List<Tile>();
            for(int i=0; i<hand_tiles.Count(); i++)
            {
                if(hand_tiles[i] != null)
                    hand_no_nul.Add(hand_tiles[i]);
            }
            hand_tiles = hand_no_nul;
        }

        // ---------------------------------------------------------
        /// <summary>
        /// Overloaded function to meldSets, takes care of the things we need to do before calling
        /// the meldsSets function(like splitting the hand into 4 lists by color).
        /// </summary>
        /// <param name="hand_tiles">Passing the tiles that we need to build sequences from</param>
        // ---------------------------------------------------------
        public List<List<Tile>> meldsSets(ref List<Tile> starting_tiles)
        {
            // sorting the hand tiles
            starting_tiles = starting_tiles.OrderBy(card => card.getNumber()).ToList();

            //// adding jokers to board.jokers and removing from starting_tiles
            //for(int i=0; i<starting_tiles.Count(); i++)
            //{
            //    if (starting_tiles[i].getNumber() == 0)
            //    {
            //        board.jokers.Add(starting_tiles[i]);
            //        starting_tiles.RemoveAt(i);
            //        i--;
            //    }
            //}

            List<Tile> sorted_tiles_no_dup = new List<Tile>(starting_tiles);

            // classify to 4 different lists(every color in every array)
            List<Tile>[] tiles_lst_color = new List<Tile>[4];
            for (int color_index = 0; color_index < 4; color_index++)
            {
                tiles_lst_color[color_index] = new List<Tile>();
                for (int i = 0; i < sorted_tiles_no_dup.Count(); i++)
                {
                    if (sorted_tiles_no_dup[i].getColor() == color_index)
                    {
                        tiles_lst_color[color_index].Add(sorted_tiles_no_dup[i]);
                    }
                }
            }

            List<List<Tile>> result = new List<List<Tile>>();
            List<List<Tile>> sequences = new List<List<Tile>>();
            List<Tile> best_jokers = new List<Tile>();

            // cloning board.jokers
            List<Tile> temp_jokers = new List<Tile>();
            for (int i = 0; i < board.jokers.Count(); i++)
            {
                temp_jokers.Add(board.jokers[i]);
            }

            meldsSets(tiles_lst_color, sequences, temp_jokers, ref result, ref starting_tiles, ref best_jokers);

             return result;
        }

        // ---------------------------------------------------------
        /// <summary>
        /// finds the optimal way arranging the given tiles
        /// </summary>
        /// <param name="color_sorted_hand"></param>
        /// <param name="sequences"></param>
        /// <param name="best_sequences"></param>
        /// <param name="best_hand"></param>
        // ---------------------------------------------------------
        public void meldsSets(List<Tile>[] color_sorted_hand, List<List<Tile>> sequences, List<Tile> jokers, ref List<List<Tile>> best_sequences, ref List<Tile> best_hand, ref List<Tile> best_jokers)
        {
            List<Tile> hand_temp = new List<Tile>();
            hand_temp.AddRange(color_sorted_hand[0]); hand_temp.AddRange(color_sorted_hand[1]); hand_temp.AddRange(color_sorted_hand[2]); hand_temp.AddRange(color_sorted_hand[3]);
            hand_temp = hand_temp.OrderBy(card => card.getNumber()).ToList();

            List<List<Tile>> sequences_temp = new List<List<Tile>>();
            for (int i = 0; i < sequences.Count; i++)
            {
                sequences_temp.Add(new List<Tile>(sequences[i]));
            }

            if (jokers.Count() > 0)
            {
                // jokers part
                // find run partialSet
                List<PartialSet> runPartialSets = new List<PartialSet>();
                List<int> indexes = new List<int>();

                // find runs
                for (int i = 0; i < hand_temp.Count(); i++)
                {
                    for (int j = 0; j < hand_temp.Count(); j++)
                    {
                        Tile tile1 = hand_temp[i];
                        Tile tile2 = hand_temp[j];
                        if (i != j && (Math.Abs(tile1.getNumber() - tile2.getNumber()) == 1 && tile1.getColor() == tile2.getColor()))
                        {
                            if (!indexes.Contains(i) && !indexes.Contains(j))
                            {
                                runPartialSets.Add(new PartialSet(tile1, tile2));
                                indexes.Add(i);
                                indexes.Add(j);
                            }
                        }
                    }
                }
            

                // duplicating the board.hand
                //List<Tile> temp = new List<Tile>();
                //for (int i = 0; i < hand_temp.Count(); i++)
                //{
                //    temp.Add(hand_temp[i]);
                //}

                //// remove the tiles that are in partial sets
                //for (int i = 0; i < indexes.Count(); i++)
                //{
                //    hand_temp.Remove(temp[indexes[i]]);
                //}

                List<Tile> hand_temp_complete = new List<Tile>(hand_temp);
                List<Tile>[] color_sorted_hand_complete = new List<Tile>[4];

                for (int i = 0; i < color_sorted_hand.Length; i++)
                {
                    color_sorted_hand_complete[i] = new List<Tile>();

                    for (int j = 0; j < color_sorted_hand[i].Count(); j++)
                    {
                        color_sorted_hand_complete[i].Add(color_sorted_hand[i][j]);
                    }
                }



                // if one of the partialSets has a tile that we are looking for
                for (int i = 0; i < runPartialSets.Count(); i++)
                {
                    // check if tile_temp exists in hand_temp
                    for (int j = 0; j < hand_temp.Count(); j++)
                    {
                        color_sorted_hand = new List<Tile>[4];

                        for (int k = 0; k < color_sorted_hand_complete.Length; k++)
                        {
                            color_sorted_hand[k] = new List<Tile>();

                            for (int l = 0; l < color_sorted_hand_complete[k].Count(); l++)
                            {
                                color_sorted_hand[k].Add(color_sorted_hand_complete[k][l]);
                            }
                        }

                        hand_temp = new List<Tile>(hand_temp_complete);

                        if (runPartialSets[i].Tile2.getNumber() + 2 == hand_temp[j].getNumber() && runPartialSets[i].Tile2.getColor() == hand_temp[j].getColor())
                        {
                            // add to sequences
                            List<Tile> sequence = new List<Tile>();
                            sequence.Add(runPartialSets[i].Tile1);
                            sequence.Add(runPartialSets[i].Tile2);
                            sequence.Add(jokers[0]);
                            sequence.Add(hand_temp[j]);

                            // remove tiles we used
                            Tile temp_tile = hand_temp[j];

                            hand_temp.Remove(runPartialSets[i].Tile1);  
                            hand_temp.Remove(runPartialSets[i].Tile2);
                            hand_temp.Remove(temp_tile);

                            // remove tiles we used from color_sorted_hand
                            color_sorted_hand[runPartialSets[i].Tile1.getColor()].Remove(runPartialSets[i].Tile1);
                            color_sorted_hand[runPartialSets[i].Tile2.getColor()].Remove(runPartialSets[i].Tile2);
                            color_sorted_hand[temp_tile.getColor()].Remove(temp_tile);

                            // remove jokers to the next function call
                            List<Tile> temp_joker_best =  new  List<Tile>(jokers);
                            temp_joker_best.Remove(jokers[0]);

                            // add to sequences_temp the new sequence(need to be reset because of the recursion in the loop)
                            sequences_temp = new List<List<Tile>>();
                            for (int k = 0; k < sequences.Count; k++)
                            {
                                sequences_temp.Add(new List<Tile>(sequences[k]));
                            }
                            sequences_temp.Add(sequence);

                            meldsSets(color_sorted_hand, sequences_temp, temp_joker_best, ref best_sequences, ref best_hand, ref best_jokers);
                        }
                        else if (runPartialSets[i].Tile1.getNumber() - 2 == hand_temp[j].getNumber() && runPartialSets[i].Tile1.getColor() == hand_temp[j].getColor())
                        {
                            // add to sequences
                            List<Tile> sequence = new List<Tile>();
                            sequence.Add(hand_temp[j]);
                            sequence.Add(jokers[0]);
                            sequence.Add(runPartialSets[i].Tile1);
                            sequence.Add(runPartialSets[i].Tile2);

                            // remove tiles we used
                            Tile temp_tile = hand_temp[j];

                            hand_temp.Remove(runPartialSets[i].Tile1);
                            hand_temp.Remove(runPartialSets[i].Tile2);
                            hand_temp.Remove(temp_tile);

                            // remove tiles we used from color_sorted_hand
                            color_sorted_hand[runPartialSets[i].Tile1.getColor()].Remove(runPartialSets[i].Tile1);
                            color_sorted_hand[runPartialSets[i].Tile2.getColor()].Remove(runPartialSets[i].Tile2);
                            color_sorted_hand[temp_tile.getColor()].Remove(temp_tile);

                            // remove jokers to the next function call
                            List<Tile> temp_joker_best = new List<Tile>(jokers);
                            temp_joker_best.Remove(jokers[0]);

                            // add to sequences_temp the new sequence(need to be reset because of the recursion in the loop)
                            sequences_temp = new List<List<Tile>>();
                            for (int k = 0; k < sequences.Count; k++)
                            {
                                sequences_temp.Add(new List<Tile>(sequences[k]));
                            }
                            sequences_temp.Add(sequence);

                            meldsSets(color_sorted_hand, sequences_temp, temp_joker_best, ref best_sequences, ref best_hand, ref best_jokers);
                        }
                    }
                }
            }








            extendSets(ref sequences_temp, ref hand_temp);
            extendSets(ref best_sequences, ref best_hand);

            // if current sequences is better, we would like to replace the global sequences and hand vars
            if (hand_temp.Count() < best_hand.Count())
            {
                best_sequences = sequences_temp;

                //List<Tile> temp_hand = new List<Tile>();
                //temp_hand.AddRange(color_sorted_hand[0]); temp_hand.AddRange(color_sorted_hand[1]); temp_hand.AddRange(color_sorted_hand[2]); temp_hand.AddRange(color_sorted_hand[3]);
                //temp_hand = temp_hand.OrderBy(card => card.getNumber()).ToList();

                best_hand = hand_temp;

                best_jokers = new List<Tile>();
                // update best_jokers
                for (int i = 0; i < best_sequences.Count(); i++)
                {
                    for (int j = 0; j < best_sequences[i].Count(); j++)
                    {
                        if (best_sequences[i][j].getNumber() == 0)
                        {
                            best_jokers.Add(best_sequences[i][j]);
                        }
                    }
                }
            }
            else if (hand_temp.Count() == best_hand.Count())
            {
                if (sequences.Count() > best_sequences.Count())
                {
                    best_sequences = sequences;
                    List<Tile> temp_hand = new List<Tile>();
                    temp_hand.AddRange(color_sorted_hand[0]); temp_hand.AddRange(color_sorted_hand[1]); temp_hand.AddRange(color_sorted_hand[2]); temp_hand.AddRange(color_sorted_hand[3]);
                    temp_hand = temp_hand.OrderBy(card => card.getNumber()).ToList();
                    best_hand = temp_hand;

                    // update best_jokers
                    for(int i=0; i<best_sequences.Count(); i++)
                    {
                        for(int j=0; j < best_sequences[i].Count(); j++)
                        {
                            if (best_sequences[i][j].getNumber() == 0)
                            {
                                best_jokers.Add(best_sequences[i][j]);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Dictionary<int, Tile> curr_hand_color_no_duplicates = new Dictionary<int, Tile>();
                for (int k = 0; k < color_sorted_hand[i].Count(); k++) curr_hand_color_no_duplicates[k] = color_sorted_hand[i][k];

                // thats the dictionary we we'll use later to recover
                Dictionary<int, Tile> curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_no_duplicates);

                // now we'll remove the duplicates
                for (int j = 1; j < curr_hand_color_no_duplicates.Count(); j++)
                {
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].getColor() == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]].getColor() &&
                        curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].getNumber() == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]].getNumber())
                    {
                        curr_hand_color_no_duplicates.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]);
                        j--;
                    }
                }

                for (int j = 0; j < curr_hand_color_no_duplicates.Keys.Count() - 2; j++)
                {
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].getNumber() + 1 == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].getNumber() &&
                        curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].getNumber() + 1 == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]].getNumber())
                    {
                        // add to seq
                        List<List<Tile>> temp_sequences = new List<List<Tile>>(sequences);
                        temp_sequences.Add(new List<Tile>() { curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]], curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]], curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]] });

                        Dictionary<int, Tile> temp_curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_clone);
                        temp_curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]); temp_curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j + 1]); temp_curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j + 2]);

                        color_sorted_hand[i] = new List<Tile>(temp_curr_hand_color_clone.Values.ToList());
                        meldsSets(color_sorted_hand, temp_sequences, jokers, ref best_sequences, ref best_hand, ref best_jokers);

                        // fix what we ruined
                        color_sorted_hand[i] = temp_curr_hand_color_clone.Values.ToList();
                        curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_clone);
                    }
                }
            }

            // adding all the remaning tiles to a one sorted long list
            List<Tile> remaning_tiles = new List<Tile>();
            remaning_tiles.AddRange(color_sorted_hand[0]); remaning_tiles.AddRange(color_sorted_hand[1]); remaning_tiles.AddRange(color_sorted_hand[2]); remaning_tiles.AddRange(color_sorted_hand[3]);
            remaning_tiles = remaning_tiles.OrderBy(card => card.getNumber()).ToList();

            Dictionary<int, Tile> remaning_tiles_dict_no_dup = new Dictionary<int, Tile>();
            for (int i = 0; i < remaning_tiles.Count(); i++) remaning_tiles_dict_no_dup[i] = remaning_tiles[i];

            Dictionary<int, Tile> remaning_tiles_dict = new Dictionary<int, Tile>(remaning_tiles_dict_no_dup);

            // now we'll remove the duplicates
            for (int j = 1; j < remaning_tiles_dict_no_dup.Count(); j++)
            {
                if (remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getColor() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]].getColor() &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getNumber() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]].getNumber())
                {
                    remaning_tiles_dict_no_dup.Remove(remaning_tiles_dict_no_dup.Keys.ToList()[j]);
                    j--;
                }
            }

            for (int j = 0; j < remaning_tiles_dict_no_dup.Keys.Count() - 2; j++)
            {
                if (remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getNumber() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].getNumber() &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].getNumber() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].getNumber() &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getNumber() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].getNumber() &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getColor() != remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].getColor() &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].getColor() != remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].getColor() &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getColor() != remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].getColor())
                {
                    List<List<Tile>> temp_sequences = new List<List<Tile>>(sequences);
                    temp_sequences.Add(new List<Tile>() { remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]], remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]], remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]] });

                    Dictionary<int, Tile> temp_remaning_tiles_dict = new Dictionary<int, Tile>(remaning_tiles_dict);
                    temp_remaning_tiles_dict.Remove(remaning_tiles_dict_no_dup.Keys.ToList()[j]); temp_remaning_tiles_dict.Remove(remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]); temp_remaning_tiles_dict.Remove(remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]);

                    // classify to 4 different lists(every color in every array)
                    List<Tile> sorted_tiles_no_dup = new List<Tile>(temp_remaning_tiles_dict.Values.ToList());
                    List<Tile>[] tiles_lst_color = new List<Tile>[4];
                    for (int color_index = 0; color_index < 4; color_index++) { tiles_lst_color[color_index] = new List<Tile>(); }
                    for (int i = 0; i < sorted_tiles_no_dup.Count(); i++)
                        tiles_lst_color[sorted_tiles_no_dup[i].getColor()].Add(sorted_tiles_no_dup[i]);
                        
                    meldsSets(tiles_lst_color, temp_sequences, jokers, ref best_sequences, ref best_hand, ref best_jokers);
                }
            }
            return;
        }

        // ------------------------------------------------ jokers ------------------------------------------------
        public void AddJokers()
        {
            JokerPartialSetCombine();
            JokerCombineParitalSetWithHand();
            JokerCompletePartialSet();
            JokerCombineSequencesWithHand();
            JokerExtendSequenceWithJoker();
        }

        public bool isPartialSetConsecutiveRun(PartialSet partial_set)
        {
            // color is same, and the numbers are consecutive
            return partial_set.Tile1.getColor() == partial_set.Tile2.getColor()
                && (partial_set.Tile2.getNumber() - partial_set.Tile1.getNumber()) == 1;
        }

        public void extendSequenceWithParitalSetTiles(ref List<Tile> sequence, PartialSet partial_set)
        {
            sequence.Add(partial_set.Tile1);
            sequence.Add(partial_set.Tile2);
        }

        public void JokerPartialSetCombine()
        {
            // list of only partial sets that are runs
            List<PartialSet> partial_sets_runs = new List<PartialSet>();
            for(int i=0; i < board.partial_sets.Count() && board.jokers.Count() > 0; i++)
            {
                if (isPartialSetConsecutiveRun(board.partial_sets[i]))
                    partial_sets_runs.Add(board.partial_sets[i]);
            }

            // iterating over this list for each partial set that is a run, we'll try to combine it with the jokers
            for (int i = 0; i < partial_sets_runs.Count() && board.jokers.Count() > 0; i++)
            {
                for(int j= i+1; j < partial_sets_runs.Count() && board.jokers.Count() > 0; j++)
                {
                    // partialSets i,j have the same color
                    if (partial_sets_runs[i].Tile1.getColor() == partial_sets_runs[j].Tile1.getColor())
                    {
                        if (partial_sets_runs[i].Tile1.getNumber() - partial_sets_runs[j].Tile2.getNumber() == 2)
                        {
                            List<Tile> sequences = new List<Tile>();
                            extendSequenceWithParitalSetTiles(ref sequences, partial_sets_runs[j]);
                            sequences.Add(board.jokers[0]);
                            extendSequenceWithParitalSetTiles(ref sequences, partial_sets_runs[i]);
                            board.sequences.Add(sequences);
                            // removing the used tiles
                            board.jokers.RemoveAt(0);
                            board.partial_sets.Remove(partial_sets_runs[i]);
                            board.partial_sets.Remove(partial_sets_runs[j]);
                        }
                        else if (partial_sets_runs[j].Tile1.getNumber() - partial_sets_runs[i].Tile2.getNumber() == 2)
                        {
                            List<Tile> sequences = new List<Tile>();
                            extendSequenceWithParitalSetTiles(ref sequences, partial_sets_runs[i]);
                            sequences.Add(board.jokers[0]);
                            extendSequenceWithParitalSetTiles(ref sequences, partial_sets_runs[j]);
                            board.sequences.Add(sequences);
                            // removing the used tiles
                            board.jokers.RemoveAt(0);
                            board.partial_sets.Remove(partial_sets_runs[i]);
                            board.partial_sets.Remove(partial_sets_runs[j]);
                        }
                    }
                }
            }
        }

        public void JokerCombineParitalSetWithHand()
        {

        }

        public void JokerCompletePartialSet()
        {

        }

        public void JokerCombineSequencesWithHand()
        {

        }

        public void JokerExtendSequenceWithJoker()
        {

        }
    }
}