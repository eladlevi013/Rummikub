using rummikubGame.Models;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame.Logic
{
    public class GameLogic
    {
        /*
            In this class, we'll have game-logic related functions,
            so the we are thinking of a way making the computer play.
            we'll have functions, such as:
            - extendSets = which extends existing sets.
            - MeldSets = which are actually 2 functions
                - one of the them is just orgenizing the parameters to the real function
                - the real function, which handles the finding sets problem.
            - 
        */


        // Constants
        const List<PartialSet> PartialSetVarPassed = null;

        // -----------------------------------------------------------------
        // Extends existing sequences with remaning tiles in hand
        // Being called while using the recursive function MeldsSets
        // -----------------------------------------------------------------
        public static void ExtendSets(ref List<List<Tile>> sequences, ref List<Tile> hand_tiles)
        {
            // we'll start by finding the not null list of hand sorted by value
            List<Tile> hand_no_null = new List<Tile>();
            for (int i = 0; i < hand_tiles.Count(); i++)
            {
                if (hand_tiles[i] != null)
                {
                    hand_no_null.Add(hand_tiles[i]);
                }
            }
            hand_tiles = hand_no_null;
            hand_tiles = hand_tiles.OrderBy(card => card.Number).ToList();

            /*
                for every hand_tile we'll iterate over all the sequences and we'll
                check if we can append the sequence from the right, if we can
                we'll set tile to null, and after that we'll check from the left.
                every-time we found a matched tile, we'll set it to null.
            */
            for (int hand_tile_index = 0; hand_tile_index < hand_tiles.Count(); hand_tile_index++)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    if (hand_tiles[hand_tile_index] != null)
                    {
                        List<Tile> tempSequenceAddRight = sequences[i].Select(item => item.Clone(item.Color, item.Number)).ToList();
                        tempSequenceAddRight.Add(hand_tiles[hand_tile_index]);
                        if (IsLegalMeld(tempSequenceAddRight) == true)
                        {
                            sequences[i] = tempSequenceAddRight;
                            hand_tiles[hand_tile_index] = null;
                        }
                    }
                    if (hand_tiles[hand_tile_index] != null)
                    {
                        List<Tile> tempSequenceAddLeft = sequences[i].Select(item => item.Clone(item.Color, item.Number)).ToList();
                        tempSequenceAddLeft.Insert(0, hand_tiles[hand_tile_index]);
                        if (IsLegalMeld(tempSequenceAddLeft) == true)
                        {
                            sequences[i] = tempSequenceAddLeft;
                            hand_tiles[hand_tile_index] = null;
                        }
                    }
                }
            }

            // removes the null's from hand
            List<Tile> hand_no_nul = new List<Tile>();
            for (int i = 0; i < hand_tiles.Count(); i++)
            {
                if (hand_tiles[i] != null)
                    hand_no_nul.Add(hand_tiles[i]);
            }
            hand_tiles = hand_no_nul;
        }

        // -----------------------------------------------------------------
        // Overloaded function to meldSets, takes care of the things
        // we need to do before calling the MeldsSets function
        // like splitting the hand into 4 lists by color.
        // -----------------------------------------------------------------
        public static List<List<Tile>> MeldsSets(ref List<Tile> all_tiles, ref List<Tile> jokers)
        {
            // sorting the hand tiles by value
            all_tiles = all_tiles.OrderBy(card => card.Number).ToList();

            // prepering the hand tiles for the MeldsSets function
            List<Tile>[] tiles_lst_color = new List<Tile>[Constants.ColorsCount];

            // classify to Pool.COLORS_COUNT different lists(every color in every array)
            for (int color_index = 0; color_index < Constants.ColorsCount; color_index++)
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

        // -----------------------------------------------------------------
        // Finds the run partial-sets, called in the recursive meldsSets
        // function in the section of the jokers.
        // -----------------------------------------------------------------
        public static List<PartialSet> FindRunPartialSets(List<Tile> hand)
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

        // -----------------------------------------------------------------
        // Duplicates the given Array of list of tiles
        // used in the first meldsSets function.
        // -----------------------------------------------------------------
        public static List<Tile>[] CloneColorSortedHand(List<Tile>[] color_sorted_hand)
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
        // recursive function, called after the first meldsSets.
        // ---------------------------------------------------------
        public static void MeldsSets(List<Tile>[] color_sorted_hand, List<List<Tile>> sequences, List<Tile> jokers, ref List<List<Tile>> best_sequences, ref List<Tile> best_hand)
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

                            if (run_partial_sets[i].Tile2.Number + 2 == hand[j].Number
                            && run_partial_sets[i].Tile2.Color == hand[j].Color)
                            {
                                sequence.Add(run_partial_sets[i].Tile1);
                                sequence.Add(run_partial_sets[i].Tile2);
                                sequence.Add(jokers[0]);
                                sequence.Add(removed_tile);
                            }
                            else if (run_partial_sets[i].Tile1.Number - 2 == hand[j].Number
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
                            List<Tile> temp_joker_best = new List<Tile>(jokers);
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

                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]] == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]])
                    {
                        curr_hand_color_no_duplicates.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]);
                        j--;
                    }
                }

                for (int j = 0; j < curr_hand_color_no_duplicates.Keys.Count() - 2; j++)
                {
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].Number + 1
                        == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].Number &&
                        curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].Number + 1
                        == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]].Number)
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
                if (remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]] == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]])
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

        public static void AddJokersAfterMeldsSets(ref List<PartialSet> partial_set, ref List<List<Tile>> sequences, ref List<Tile> jokers, ref List<Tile> hand)
        {
            JokerCompletePartialSet(ref partial_set, ref sequences, ref jokers);
            JokerCombineSequencesWithHand(ref sequences, ref jokers, ref hand);
            JokerExtendSequenceWithJoker(ref jokers, ref sequences);
        }

        public static void JokerCompletePartialSet(ref List<PartialSet> partial_set, ref List<List<Tile>> sequences, ref List<Tile> jokers)
        {
            List<PartialSet> best_runs_values = new List<PartialSet>();
            List<PartialSet> runs_values = new List<PartialSet>();
            List<PartialSet> groups_values = new List<PartialSet>();

            // classifing partial sets
            if (partial_set.Count() > 0)
            {
                for (int i = 0; i < partial_set.Count(); i++)
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
                for (int i = 0; i < best_runs_values.Count() && jokers.Count() > 0; i++)
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
                    if (runs_values[i].Tile2.Number == 13)
                    {
                        // adding to sequences
                        List<Tile> temp_list = new List<Tile>();
                        temp_list.Add(jokers[0]);
                        temp_list.Add(runs_values[i].Tile1);
                        temp_list.Add(runs_values[i].Tile2);
                        sequences.Add(temp_list);

                        partial_set.Remove(runs_values[i]);
                        jokers.RemoveAt(0);
                    }
                    else
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

        public static void JokerCombineSequencesWithHand(ref List<List<Tile>> sequences, ref List<Tile> unused_jokers, ref List<Tile> hand)
        {
            for (int i = 0; i < sequences.Count() && unused_jokers.Count() > 0; i++)
            {
                for (int j = 0; j < hand.Count() && unused_jokers.Count() > 0; j++)
                {
                    if (IsRun(sequences[i]))
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

        public static void JokerExtendSequenceWithJoker(ref List<Tile> unused_jokers, ref List<List<Tile>> sequences)
        {
            for (int i = 0; i < sequences.Count() && unused_jokers.Count() > 0; i++)
            {
                List<Tile> temp_sequence_from_top = new List<Tile>(sequences[i]);
                temp_sequence_from_top.Add(unused_jokers[0]);

                List<Tile> temp_sequence_from_bottom = new List<Tile>(sequences[i]);
                temp_sequence_from_bottom.Insert(0, unused_jokers[0]);

                // Checking if the sequence can be extended with a joker
                if (IsLegalMeld(temp_sequence_from_top))
                {
                    sequences[i].Add(unused_jokers[0]);
                    unused_jokers.RemoveAt(0);
                    continue;
                }
                else if (IsLegalMeld(temp_sequence_from_bottom))
                {
                    sequences[i].Insert(0, unused_jokers[0]);
                    unused_jokers.RemoveAt(0);
                    continue;
                }
            }
        }

        public static List<PartialSet> CreatePartialSets(ref List<Tile> hand, List<PartialSet> existing_partial_set = PartialSetVarPassed)
        {
            // use the existing partial sets if passed
            if (existing_partial_set == PartialSetVarPassed)
                existing_partial_set = new List<PartialSet>();

            List<PartialSet> currPartialSets = existing_partial_set;
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
                            currPartialSets.Add(partialSet);
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
                            currPartialSets.Add(new PartialSet(tile1, tile2));
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

            return currPartialSets;
        }

        public static void ResolveWinnerOnPoolOver()
        {
            OnGameOver();
            if (GameContext.ComputerPlayer.board.GetHandTilesNumber()
                == GameContext.HumanPlayer.GetHandTilesNumber())
                MessageBox.Show("Tie!");
            else if (GameContext.ComputerPlayer.board.GetHandTilesNumber()
                > GameContext.HumanPlayer.GetHandTilesNumber())
                MessageBox.Show("You Won!");
            else
                MessageBox.Show("Computer Won!");
        }

        public static void OnGameOver()
        {
            GameContext.GameOver = true;
            GameContext.HumanPlayer.board.DisableBoard();
        }

        public static bool IsLegalMeld(List<Tile> meld)
        {
            bool isRun = true;
            if (meld.Count < 3)
                return false;

            // finds the first non-joker tile and uses it to determine the color and value of the run
            int firstNonJokerIndex = 0;
            for (int i = 0; i < meld.Count; i++)
            {
                if (!IsJoker(meld[i]))
                {
                    firstNonJokerIndex = i;
                    break;
                }
            }

            int color = meld[firstNonJokerIndex].Color;
            int value = meld[firstNonJokerIndex].Number;

            for (int i = firstNonJokerIndex + 1; i < meld.Count; i++)
            {
                // if meld number is not equal to the value + the index of the tile in the meld its cant be a run
                if ((meld[i].Number != value + i - firstNonJokerIndex
                    || meld[i].Color != color) && !IsJoker(meld[i]))
                {
                    isRun = false;
                }
                if (IsJoker(meld[i]))
                {
                    // Skip over jokers and continue checking the rest of the tiles
                    continue;
                }
            }
            if (isRun)
            {
                // checking the value of the max
                if (meld[firstNonJokerIndex].Number + ((meld.Count - 1) - firstNonJokerIndex) > 13)
                {
                    return false;
                }
                // checking the value of the min
                if (meld[firstNonJokerIndex].Number - firstNonJokerIndex < 1)
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
                if (meld[i + 1].Number != value && !IsJoker(meld[i + 1]))
                {
                    return false; // its cannot be group
                }
                for (int j = i + 1; j < meld.Count(); j++)
                {
                    if (meld[i].Color == meld[j].Color
                        && !IsJoker(meld[i]) && !IsJoker(meld[j]))
                    {
                        return false;
                    }
                }
            }

            // check if there are too many jokers used
            int numJokers = CountJokers(meld);
            if (numJokers > 2)
            {
                return false; // too many jokers used
            }
            return true;
        }

        private static int CountJokers(List<Tile> meld)
        {
            int count = 0;
            foreach (Tile tile in meld)
            {
                if (IsJoker(tile))
                    count++;
            }
            return count;
        }

        public static bool IsJoker(Tile tile)
        {
            return tile.Number == Constants.JokerNumber;
        }

        public static bool IsRun(List<Tile> sequence)
        {
            // assuming valid sequence(group or run)
            return sequence[0].Color == sequence[1].Color;
        }
    }
}
