using Rummikub;
using rummikubGame.Models;
using rummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class ComputerPlayer
    {
        // -----------------------------------------------------------------
        // This class will mainly used for calculations
        // the computer player has to take in order to play
        // in the optimal way he can in the current turn.
        // -----------------------------------------------------------------

        public ComputerBoard board;
        
        public ComputerPlayer()
        {
            board = new ComputerBoard();
            board.sequences = MeldsSets(ref board.hand, ref board.unused_jokers);
            
            // removing jokers that are in sequences
            board.UpdatingUnusedJokers(ref board.unused_jokers, ref board.sequences);
            board.partial_sets = board.CreatePartialSets(ref board.hand);
            AddJokersAfterMeldsSets(ref board.partial_sets, ref board.sequences, ref board.unused_jokers, ref board.hand);

            // takes care of the graphical board of the computer
            if (RummikubGameView.global_view_computer_tiles_groupbox.Checked)
                board.GenerateBoard();
        }

        // -----------------------------------------------------------------
        // Tries to replace the given tile with every tile,
        // in order to get the tile that fitted the most.
        // -----------------------------------------------------------------
        public bool BetterArrangementFound(Tile tile_to_replace)
        {
            // vars for finding the best option
            List<Tile> better_option_hand = board.hand;
            List<List<Tile>> better_option_sequences = board.sequences;
            List<PartialSet> better_option_partial_sets = board.partial_sets;
            List<Tile> starting_jokers = board.GetAllJokers();

            bool better_option_found = false;
            Tile optimal_dropped_tile = null;

            // replacing every tile in starting_tiles with the given paramter in order to get best result
            for (int i = 0; i < Constants.RUMMIKUB_TILES_IN_GAME - board.CountJokers(); i++)
            {
                // all the tiles that are not jokers
                List<Tile> all_tiles = new List<Tile>();
                List<Tile> jokers = new List<Tile>();

                board.GetAllTiles(ref all_tiles, ref jokers);

                // the tile that is being replaced by the given parameter
                Tile dropped_tile = all_tiles[i];

                // if tile_to_replace is joker - add it to the jokers list
                if (tile_to_replace.Number == 0)
                {
                    jokers.Add(tile_to_replace);
                    all_tiles.RemoveAt(i);
                }
                else
                {
                    all_tiles[i] = tile_to_replace;
                }

                List<List<Tile>> temp_sequences = MeldsSets(ref all_tiles, ref jokers);
                List<Tile> temp_hand = new List<Tile>(all_tiles);

                board.UpdatingUnusedJokers(ref jokers, ref temp_sequences);

                List<PartialSet> temp_partial_set = board.CreatePartialSets(ref temp_hand);
                AddJokersAfterMeldsSets(ref temp_partial_set, ref temp_sequences, ref jokers, ref temp_hand);

                // check current situation is better than the optimal
                if (board.getNumberOfTilesInAllSets(better_option_sequences) < board.getNumberOfTilesInAllSets(temp_sequences))
                {
                    better_option_sequences = temp_sequences;
                    better_option_hand = temp_hand;
                    better_option_partial_sets = temp_partial_set;
                    optimal_dropped_tile = dropped_tile;
                    better_option_found = true;
                }
            }

            // better arrangment found
            if (better_option_found)
            {
                // updates global board variables
                board.sequences = better_option_sequences;
                board.hand = better_option_hand;

                // updating unused jokers
                board.unused_jokers = new List<Tile>(starting_jokers);
                board.UpdatingUnusedJokers(ref board.unused_jokers, ref board.sequences);

                // create partial sets from the given hand
                board.partial_sets = better_option_partial_sets;
                //board.partial_sets = new List<PartialSet>();
                //board.partial_sets = CreatePartialSets(ref board.hand);

                AddJokersAfterMeldsSets(ref board.partial_sets, ref board.sequences, ref board.unused_jokers, ref board.hand);

                // take the last thrown tile from the dropped tiles stack(graphically)
                if (RummikubGameView.dropped_tiles_stack.Count() > 0)
                    RummikubGameView.global_RummikubGameView_context.Controls.Remove(RummikubGameView.dropped_tiles_stack.Peek().TileButton.GetButton());

                // generate computer thrown tile in Stack
                int[] tile_in_dropped_tiles_location = { Constants.DROPPED_TILE_LOCATION, Constants.DROPPED_TILE_LOCATION };
                VisualTile dropped_tile = new VisualTile(optimal_dropped_tile.Color, optimal_dropped_tile.Number, tile_in_dropped_tiles_location);
                board.GenerateComputerThrownTile(dropped_tile);
                
                // returns true because better option found
                return (true);
            }
            // board.partial_sets = CreatePartialSets(ref board.hand);
            return (false);
        }

        // -----------------------------------------------------------------
        // called every time computer have to play, it checks if tile
        // the other player dropped helps the computer for better sequences.
        // if didnt option found take a tile from stack and check again.
        // -----------------------------------------------------------------
        public void ComputerPlay(Tile to_be_replaced)
        {
            // humanPlayer dropped tile, not giving better result 
            if(to_be_replaced != null && !BetterArrangementFound(to_be_replaced))
            {
                Tile tile = RummikubGameView.pool.GetTile();
                if (tile == null) // pool is empty
                    return;

                // better option with tile from pool
                if (!BetterArrangementFound(tile))
                {
                    // if partial set found from dropped tile stack
                    if (board.hand.Count() > 1)
                    {
                        if(tile.Number == 0)
                        {
                            board.unused_jokers.Add(tile);
                        }
                        else
                        {
                            board.hand.Add(tile);
                        }

                        // board.partial_sets = CreatePartialSets(ref board.hand);
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

                            if(tile.Number == 0)
                                board.unused_jokers.Add(tile);
                            else
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

                            if (tile.Number == 0)
                                board.unused_jokers.Add(tile);
                            else
                                board.hand.Add(tile);

                            board.GenerateComputerThrownTile(random_tile_to_drop);
                        }
                    }
                }
            }

            // updating partial-set after taking tiles
            board.partial_sets = board.CreatePartialSets(ref board.hand, board.partial_sets);

            // done in both cases(better option or not)
            RummikubGameView.current_turn = Constants.HUMAN_PLAYER_TURN;
            RummikubGameView.global_game_indicator_lbl.Text = RummikubGameView.TAKE_TILE_FROM_POOL_STACK_MSG;
            PlayerBoard.tookCard = false;
            RummikubGameView.computer_player.board.ClearBoard();

            if (RummikubGameView.global_view_computer_tiles_groupbox.Checked == true)
                RummikubGameView.computer_player.board.GenerateBoard();

            // if the game is over, and the computer won
            if (board.CheckWinner() == true && RummikubGameView.game_over == false)
            {
                MessageBox.Show("Computer Won!");
                RummikubGameView.global_game_indicator_lbl.Text = "Game Over - Computer Won";
                RummikubGameView.human_player.board.disableHumanBoard();

                if (RummikubGameView.dropped_tiles_stack.Count > 0)
                    RummikubGameView.dropped_tiles_stack.Peek().TileButton.GetButton().Enabled = false;
                RummikubGameView.game_over = true;
            }
        }

        // -----------------------------------------------------------------
        // Extends existing sequences with remaning tiles in hand
        // Being called on the recursive function MeldsSets
        // -----------------------------------------------------------------
        public void ExtendSets(ref List<List<Tile>> sequences, ref List<Tile> hand_tiles)
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
            hand_tiles = hand_tiles.OrderBy(card => card.Number).ToList();

            for(int hand_tile_index=0; hand_tile_index < hand_tiles.Count(); hand_tile_index++)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    if (hand_tiles[hand_tile_index] != null)
                    {
                        List<Tile> tempSequenceAddRight = sequences[i].Select(item => item.Clone(item.Color, item.Number)).ToList();
                        tempSequenceAddRight.Add(hand_tiles[hand_tile_index]);
                        if (RummikubGameView.IsLegalMeld(tempSequenceAddRight) == true)
                        {
                            sequences[i] = tempSequenceAddRight;
                            hand_tiles[hand_tile_index] = null;
                        }
                    }
                    if (hand_tiles[hand_tile_index] != null)
                    {
                        List<Tile> tempSequenceAddLeft = sequences[i].Select(item => item.Clone(item.Color, item.Number)).ToList();
                        tempSequenceAddLeft.Insert(0, hand_tiles[hand_tile_index]);
                        if (RummikubGameView.IsLegalMeld(tempSequenceAddLeft) == true)
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

        // -----------------------------------------------------------------
        // Overloaded function to meldSets, takes care of the things
        // we need to do before calling the MeldsSets function
        // like splitting the hand into 4 lists by color.
        // -----------------------------------------------------------------
        public List<List<Tile>> MeldsSets(ref List<Tile> all_tiles, ref List<Tile> jokers)
        {
            // sorting the hand tiles
            all_tiles = all_tiles.OrderBy(card => card.Number).ToList();

            // prepering the hand tiles for the MeldsSets function
            List<Tile>[] tiles_lst_color = new List<Tile>[Constants.COLORS_COUNT];

            // classify to Pool.COLORS_COUNT different lists(every color in every array)
            for (int color_index = 0; color_index < Constants.COLORS_COUNT; color_index++)
            {
                tiles_lst_color[color_index] = new List<Tile>();
                for (int i = 0; i < all_tiles.Count(); i++)
                {
                    if (all_tiles[i].Color == color_index)
                    {
                        tiles_lst_color[color_index].Add(all_tiles[i]);
                    }
                }
            }

            List<List<Tile>> result = new List<List<Tile>>();
            List<List<Tile>> sequences = new List<List<Tile>>();

            MeldsSets(tiles_lst_color, sequences, jokers, ref result, ref all_tiles);

             return (result);
        }

        public List<PartialSet> FindRunPartialSets(List<Tile> hand)
        {
            List<PartialSet> run_partial_sets = new List<PartialSet>();
            List<int> indexes = new List<int>();
            // find runs
            for (int i = 0; i < hand.Count(); i++)
            {
                for (int j = 0; j < hand.Count(); j++)
                {
                    Tile tile1 = hand[i];
                    Tile tile2 = hand[j];
                    if (i != j && (Math.Abs(tile1.Number - tile2.Number) == 1 && tile1.Color == tile2.Color))
                    {
                        if (!indexes.Contains(i) && !indexes.Contains(j))
                        {
                            PartialSet partial_set = new PartialSet(tile1, tile2);
                            partial_set.SortPartialSet();
                            run_partial_sets.Add(partial_set);
                            indexes.Add(i);
                            indexes.Add(j);
                        }
                    }
                }
            }
            return run_partial_sets;
        }

        public List<Tile>[] CloneColorSortedHand(List<Tile>[] color_sorted_hand)
        {
            List<Tile>[] color_sorted_hand_temp = new List<Tile>[4];

            for (int k = 0; k < color_sorted_hand.Length; k++)
            {
                color_sorted_hand_temp[k] = new List<Tile>();

                for (int l = 0; l < color_sorted_hand[k].Count(); l++)
                {
                    color_sorted_hand_temp[k].Add(color_sorted_hand[k][l]);
                }
            }

            return color_sorted_hand_temp;
        }

        // ---------------------------------------------------------
        // finds the optimal way arranging the given tiles
        // ---------------------------------------------------------
        public void MeldsSets(List<Tile>[] color_sorted_hand, List<List<Tile>> sequences, List<Tile> jokers, ref List<List<Tile>> best_sequences, ref List<Tile> best_hand)
        {
            List<Tile> hand = new List<Tile>();
            hand.AddRange(color_sorted_hand[0]); hand.AddRange(color_sorted_hand[1]); hand.AddRange(color_sorted_hand[2]); hand.AddRange(color_sorted_hand[3]);
            hand = hand.OrderBy(card => card.Number).ToList();

            List<List<Tile>> sequences_temp = new List<List<Tile>>();
            for (int i = 0; i < sequences.Count; i++)
            {
                sequences_temp.Add(new List<Tile>(sequences[i]));
            }

            // jokers
            if (jokers.Count() > 0)
            {
                List<PartialSet> run_partial_sets = FindRunPartialSets(hand);

                // iterating over all the partial sets while searching for a way to meld the jokers
                for (int i = 0; i < run_partial_sets.Count(); i++)
                {
                    for (int j = 0; j < hand.Count(); j++)
                    {
                        if ((run_partial_sets[i].Tile2.Number + 2 == hand[j].Number 
                            && run_partial_sets[i].Tile2.Color == hand[j].Color)
                            || (run_partial_sets[i].Tile1.Number - 2 == hand[j].Number
                            && run_partial_sets[i].Tile1.Color == hand[j].Color))
                        {
                            // Clone original color sorted hand
                            List<Tile>[] color_sorted_hand_temp = CloneColorSortedHand(color_sorted_hand);

                            // save the best hand to drop
                            Tile removed_tile = hand[j];

                            // create sequence
                            List<Tile> sequence = new List<Tile>();

                            if(run_partial_sets[i].Tile2.Number + 2 == hand[j].Number
                            && run_partial_sets[i].Tile2.Color == hand[j].Color)
                            {
                                sequence.Add(run_partial_sets[i].Tile1);
                                sequence.Add(run_partial_sets[i].Tile2);
                                sequence.Add(jokers[0]);
                                sequence.Add(removed_tile);
                            }
                            else if(run_partial_sets[i].Tile1.Number - 2 == hand[j].Number
                            && run_partial_sets[i].Tile1.Color == hand[j].Color)
                            {
                                sequence.Add(removed_tile);
                                sequence.Add(jokers[0]);
                                sequence.Add(run_partial_sets[i].Tile1);
                                sequence.Add(run_partial_sets[i].Tile2);
                            }

                            // add sequence to sequences
                            sequences_temp = sequences.GetRange(0, sequences.Count);
                            sequences_temp.Add(sequence);

                            // remove tiles we used from color_sorted_hand
                            color_sorted_hand_temp[run_partial_sets[i].Tile1.Color].Remove(run_partial_sets[i].Tile1);
                            color_sorted_hand_temp[run_partial_sets[i].Tile2.Color].Remove(run_partial_sets[i].Tile2);
                            color_sorted_hand_temp[removed_tile.Color].Remove(removed_tile);

                            // remove jokers to the next function call
                            List<Tile> temp_joker_best =  new List<Tile>(jokers);
                            temp_joker_best.Remove(jokers[0]);

                            MeldsSets(color_sorted_hand_temp, sequences_temp, temp_joker_best, ref best_sequences, ref best_hand);
                        }
                    }
                }
            }

            ExtendSets(ref sequences_temp, ref hand);
            ExtendSets(ref best_sequences, ref best_hand);

            // if current sequences is better, we would like to replace the global sequences and hand vars
            if (hand.Count() < best_hand.Count())
            {
                best_sequences = sequences_temp;
                best_hand = hand;
            }
            else if (hand.Count() == best_hand.Count())
            {
                if (sequences.Count() > best_sequences.Count())
                {
                    best_sequences = sequences;
                    List<Tile> temp_hand = new List<Tile>();
                    temp_hand.AddRange(color_sorted_hand[0]); temp_hand.AddRange(color_sorted_hand[1]); 
                    temp_hand.AddRange(color_sorted_hand[2]); temp_hand.AddRange(color_sorted_hand[3]);
                    temp_hand = temp_hand.OrderBy(card => card.Number).ToList();
                    best_hand = temp_hand;
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
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].Color == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]].Color &&
                        curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].Number == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]].Number)
                    {
                        curr_hand_color_no_duplicates.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]);
                        j--;
                    }
                }

                for (int j = 0; j < curr_hand_color_no_duplicates.Keys.Count() - 2; j++)
                {
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].Number + 1 == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].Number &&
                        curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].Number + 1 == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]].Number)
                    {
                        // add to seq
                        List<List<Tile>> temp_sequences = new List<List<Tile>>(sequences);
                        temp_sequences.Add(new List<Tile>() { curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]], curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]], curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]] });

                        Dictionary<int, Tile> temp_curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_clone);
                        temp_curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]); temp_curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j + 1]); temp_curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j + 2]);

                        color_sorted_hand[i] = new List<Tile>(temp_curr_hand_color_clone.Values.ToList());
                        MeldsSets(color_sorted_hand, temp_sequences, jokers, ref best_sequences, ref best_hand);

                        // fix what we ruined
                        color_sorted_hand[i] = temp_curr_hand_color_clone.Values.ToList();
                        curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_clone);
                    }
                }
            }

            // adding all the remaning tiles to a one sorted long list
            List<Tile> remaning_tiles = new List<Tile>();
            remaning_tiles.AddRange(color_sorted_hand[0]); remaning_tiles.AddRange(color_sorted_hand[1]); remaning_tiles.AddRange(color_sorted_hand[2]); remaning_tiles.AddRange(color_sorted_hand[3]);
            remaning_tiles = remaning_tiles.OrderBy(card => card.Number).ToList();

            Dictionary<int, Tile> remaning_tiles_dict_no_dup = new Dictionary<int, Tile>();
            for (int i = 0; i < remaning_tiles.Count(); i++) remaning_tiles_dict_no_dup[i] = remaning_tiles[i];

            Dictionary<int, Tile> remaning_tiles_dict = new Dictionary<int, Tile>(remaning_tiles_dict_no_dup);

            // now we'll remove the duplicates
            for (int j = 1; j < remaning_tiles_dict_no_dup.Count(); j++)
            {
                if (remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].Color == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]].Color &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].Number == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]].Number)
                {
                    remaning_tiles_dict_no_dup.Remove(remaning_tiles_dict_no_dup.Keys.ToList()[j]);
                    j--;
                }
            }

            for (int j = 0; j < remaning_tiles_dict_no_dup.Keys.Count() - 2; j++)
            {
                if (remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].Number == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].Number &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].Number == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].Number &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].Number == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].Number &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].Color != remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].Color &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 1]].Color != remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].Color &&
                    remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].Color != remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j + 2]].Color)
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
                        tiles_lst_color[sorted_tiles_no_dup[i].Color].Add(sorted_tiles_no_dup[i]);
                        
                    MeldsSets(tiles_lst_color, temp_sequences, jokers, ref best_sequences, ref best_hand);
                }
            }
            return;
        }

        public void AddJokersAfterMeldsSets(ref List<PartialSet> partial_set, ref List<List<Tile>> sequences, ref List<Tile> jokers, ref List<Tile> hand)
        {
           JokerCompletePartialSet(ref partial_set, ref sequences, ref jokers);
           JokerCombineSequencesWithHand(ref sequences, ref jokers, ref hand);
           JokerExtendSequenceWithJoker(ref jokers, ref sequences);
        }

        public void JokerCompletePartialSet(ref List<PartialSet> partial_set, ref List<List<Tile>> sequences, ref List<Tile> jokers)
        {
            List<PartialSet> best_runs_values = new List<PartialSet>();
            List<PartialSet> runs_values = new List<PartialSet>();
            List<PartialSet> groups_values = new List<PartialSet>();

            // classifing partial sets
            if (partial_set.Count() > 0)
            {
                for(int i=0; i< partial_set.Count(); i++)
                {
                    if (partial_set[i].Tile2.Number - partial_set[i].Tile1.Number == 2)
                    {
                        best_runs_values.Add(partial_set[i]);
                    }
                    else if (partial_set[i].Tile2.Number - partial_set[i].Tile1.Number == 1)
                    {
                        runs_values.Add(partial_set[i]);
                    }
                    else
                    {
                        groups_values.Add(partial_set[i]);
                    }
                }

                // Removing partial sets that can be completed with jokers
                for(int i=0; i < best_runs_values.Count() && jokers.Count() > 0; i++)
                {
                    // adding to sequences
                    List<Tile> temp_list = new List<Tile>();
                    temp_list.Add(best_runs_values[i].Tile1);
                    temp_list.Add(jokers[0]);
                    temp_list.Add(best_runs_values[i].Tile2);
                    sequences.Add(temp_list);

                    partial_set.Remove(best_runs_values[i]);
                    jokers.RemoveAt(0);
                }

                // Removing partial sets that can be completed with jokers
                for (int i = 0; i < runs_values.Count() && jokers.Count() > 0; i++)
                {
                    // adding to sequences
                    List<Tile> temp_list = new List<Tile>();
                    temp_list.Add(runs_values[i].Tile1);
                    temp_list.Add(runs_values[i].Tile2);
                    temp_list.Add(jokers[0]);
                    sequences.Add(temp_list);

                    partial_set.Remove(runs_values[i]);
                    jokers.RemoveAt(0);
                }

                // Removing partial sets that can be completed with jokers
                for (int i = 0; i < groups_values.Count() && jokers.Count() > 0; i++)
                {
                    // adding to sequences
                    List<Tile> temp_list = new List<Tile>();
                    temp_list.Add(groups_values[i].Tile1);
                    temp_list.Add(jokers[0]);
                    temp_list.Add(groups_values[i].Tile2);
                    sequences.Add(temp_list);

                    partial_set.Remove(groups_values[i]);
                    jokers.RemoveAt(0);
                }
            }
        }

        public void JokerCombineSequencesWithHand(ref List<List<Tile>> sequences, ref List<Tile> unused_jokers, ref List<Tile> hand)
        {
            for(int i=0; i< sequences.Count() && unused_jokers.Count() > 0; i++)
            {
                for(int j=0; j< hand.Count() && unused_jokers.Count() > 0; j++)
                {
                    if (board.IsRun(sequences[i]))
                    {
                        if (sequences[i][sequences[i].Count() - 1].Number + 2 == hand[j].Number
                            && sequences[i][sequences[i].Count() - 1].Color == hand[j].Color)
                        {
                            sequences[i].Add(unused_jokers[0]);
                            sequences[i].Add(hand[j]);

                            // Removing used joker and tile from hand
                            unused_jokers.RemoveAt(0);
                            hand.RemoveAt(j);
                            j--;
                        }
                        else if (sequences[i][0].Number - 2 == hand[j].Number
                            && sequences[i][0].Color == hand[j].Color)
                        {
                            sequences[i].Insert(0, unused_jokers[0]);
                            sequences[i].Insert(0, hand[j]);

                            // Removing used joker and tile from hand
                            unused_jokers.RemoveAt(0);
                            hand.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
        }

        public void JokerExtendSequenceWithJoker(ref List<Tile> unused_jokers, ref List<List<Tile>> sequences)
        {
            for(int i=0; i < sequences.Count() && unused_jokers.Count() > 0; i++)
            {
                sequences[i].Add(unused_jokers[0]);
                unused_jokers.RemoveAt(0);
            }
        }
    }
}
