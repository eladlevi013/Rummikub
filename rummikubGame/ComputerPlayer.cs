using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class ComputerPlayer
    {
        public ComputerBoard board;

        public ComputerPlayer()
        {
            // it takes care of the graphical representation of the tiles of the computer
            board = new ComputerBoard();

            // arrange given tiles in optimal way
            firstArrange();

            // draw the tiles on the screen
            if (GameTable.global_view_computer_tiles_groupbox.Checked == true)
                board.generateBoard();
        }

        public void firstArrange()
        {
            // sorting the tiles in order to get 1,2,3 
            board.hand = board.hand.OrderBy(card => card.getNumber()).ToList();

            List<Tile> sorted_tiles_no_dup = new List<Tile>(board.hand);

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
            meldsSetsBetter(tiles_lst_color, sequences, ref result, ref board.hand);

            // extendedSets function is being called(makes sequences bigger from hand tiles)
            if (result != null)
            {
                extendSets(ref result, ref board.hand);
                board.sequences = result;
            }
        }

        public bool better_sequences_after_taking_new_tile(Tile new_tile)
        {
            List<Tile> starting_tiles_if_not_better = board.hand;

            bool replced_card_better_result = false;
            List<Tile> optimal_solution_hand = board.hand;
            List<List<Tile>> optimal_solution_sequences = board.sequences;
            Tile optimal_dropped_tile = null;

            // replacing every tile in starting_tiles with the given paramter in order to get better result
            for (int i = 0; i < GameTable.RUMMIKUB_TILES_IN_GAME; i++)
            {
                // we want copy of starting tiles because result function makes some tiles null and we would not like it to be affect the next iteration
                List<Tile> starting_tiles_copy = new List<Tile>();

                // starting_tiles will be a list of all of the tiles
                for (int j = 0; j < board.hand.Count(); j++)
                {
                    // adds the hand cards to the temp_tiles list
                    if (board.hand[j] != null)
                        starting_tiles_copy.Add(board.hand[j]);
                }
                if (board.sequences != null)
                { // adds the tiles in sets to the temp_tiles list
                    for (int j = 0; j < board.sequences.Count(); j++)
                        for (int k = 0; k < board.sequences[j].Count(); k++)
                            starting_tiles_copy.Add(board.sequences[j][k]);
                }
                starting_tiles_copy = starting_tiles_copy.Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();

                List<List<Tile>> temp_extendedSets = new List<List<Tile>>();
                List<Tile> temp_hand = new List<Tile>();
                Tile dropped_tile = starting_tiles_copy[i];

                // replacing starting tile at index i, in order to see if its getting better result
                starting_tiles_copy = starting_tiles_copy.OrderBy(card => card.getNumber()).ToList();
                starting_tiles_copy[i] = new_tile;
                temp_hand = new List<Tile>(board.hand);

                List<Tile> sorted_tiles_no_dup = new List<Tile>(starting_tiles_copy);
                // classify to 4 different lists(every color in every array)
                List<Tile>[] tiles_lst_color = new List<Tile>[4];
                for (int color_index = 0; color_index < 4; color_index++)
                {
                    tiles_lst_color[color_index] = new List<Tile>();
                    for (int k = 0; k < sorted_tiles_no_dup.Count(); k++)
                    {
                        if (sorted_tiles_no_dup[k].getColor() == color_index)
                        {
                            tiles_lst_color[color_index].Add(sorted_tiles_no_dup[k]);
                        }
                    }
                }

                List<List<Tile>> result = new List<List<Tile>>();
                List<List<Tile>> sequences = new List<List<Tile>>();
                meldsSetsBetter(tiles_lst_color, sequences, ref result, ref temp_hand);

                // extendedSets function is being called(makes sequences bigger from hand tiles)
                if (result != null)
                {
                    extendSets(ref result, ref temp_hand);
                    temp_extendedSets = result;
                }

                // check if the current situation is better than the optimal
                if (getNumberOfTilesInAllSets(optimal_solution_sequences) < getNumberOfTilesInAllSets(temp_extendedSets))
                {
                    optimal_solution_sequences = temp_extendedSets;
                    optimal_solution_hand = temp_hand;
                    replced_card_better_result = true;
                    optimal_dropped_tile = dropped_tile;
                }
            }

            /* if we found a better option than the starting point:
                 * 1. replace the global sequences,hand parameters
                 * 2. delete last tile from stack, draw dropped_card on stack
                 * 3. return true */
            if (replced_card_better_result == true)
            {
                board.sequences = optimal_solution_sequences;
                board.hand = optimal_solution_hand;

                if(GameTable.dropped_tiles_stack.Count() > 0)
                    GameTable.global_gametable_context.Controls.Remove(GameTable.dropped_tiles_stack.Peek().getTileButton());

                int[] tile_in_dropped_tiles_location = { GameTable.DROPPED_TILE_LOCATION, GameTable.DROPPED_TILE_LOCATION };
                TileButton dropped_tile = new TileButton(optimal_dropped_tile.getColor(), optimal_dropped_tile.getNumber(), tile_in_dropped_tiles_location);
                board.GenerateComputerThrownTile(dropped_tile);
                return true;
            }
            board.hand = starting_tiles_if_not_better;
            return false;
        }

        public void play(Tile to_be_replaced)
        {
            // didnt find better arrangement
            if(to_be_replaced != null && !better_sequences_after_taking_new_tile(to_be_replaced))
            {
                Tile tile = GameTable.pool.getTile();
                if (tile == null) // pool is empty
                    return;

                // better option with tile from pool
                if (better_sequences_after_taking_new_tile(tile) == false)
                {
                    // if tile from pool didnt gave us better result drop random tile from hand
                    Random rnd_hand_index = new Random();
                    bool hand_null = true;
                    Tile random_tile_to_drop = null;
                    int random_tile_to_drop_index = 0;
                    while (hand_null)
                    {
                        random_tile_to_drop_index = rnd_hand_index.Next(board.hand.Count());
                        random_tile_to_drop = board.hand[random_tile_to_drop_index];
                        if (random_tile_to_drop != null)
                        {
                            hand_null = false;
                        }
                    }
                    board.hand.RemoveAt(random_tile_to_drop_index);
                    board.hand.Add(tile);
                    board.GenerateComputerThrownTile(random_tile_to_drop);
                }
            }

            // done in both cases(better option or not)
            GameTable.current_turn = GameTable.HUMAN_PLAYER_TURN;
            GameTable.global_game_indicator_lbl.Text = GameTable.TAKE_TILE_FROM_POOL_STACK_MSG;
            PlayerBoard.tookCard = false;
            GameTable.computer_player.board.deleteCardsVisibility();

            if (GameTable.global_view_computer_tiles_groupbox.Checked == true)
                GameTable.computer_player.board.generateBoard();

            // if the game is over, and the computer won
            if (board.checkWinner() == true && GameTable.game_over == false)
            {
                MessageBox.Show("Computer Won!");
                GameTable.global_game_indicator_lbl.Text = "Game Over - Computer Won";
                GameTable.human_player.board.disableHumanBoard();

                if(GameTable.dropped_tiles_stack.Count > 0)
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

        /* Create a set class, which contains the data of what type the set is (Group, Run)
         for group we dont need to check the two of the ways to arrange */
        public void extendSets(ref List<List<Tile>> sequences, ref List<Tile> hand_tiles)
        {

            /*  we have to sort the hand_tiles
                here's why:
                    hand: 5,4
                    seq: {{1,2,3}}
               if we wont sort the output will be 1,2,3,4 instead of 1,2,3,4,5 */
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
        }

        public void meldsSetsBetter(List<Tile>[] color_sorted_hand, List<List<Tile>> sequences, ref List<List<Tile>> best_sequences, ref List<Tile> best_hand)
        {
            // if current sequences is better, we would like to replace the global sequences and hand vars
            if (sequences.Count() > best_sequences.Count())
            {
                best_sequences = sequences;
                List<Tile> temp_hand = new List<Tile>();
                temp_hand.AddRange(color_sorted_hand[0]); temp_hand.AddRange(color_sorted_hand[1]); temp_hand.AddRange(color_sorted_hand[2]); temp_hand.AddRange(color_sorted_hand[3]);
                temp_hand = temp_hand.OrderBy(card => card.getNumber()).ToList();
                best_hand = temp_hand;
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
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].getColor() == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]].getColor() && curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].getNumber() == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j - 1]].getNumber())
                    {
                        curr_hand_color_no_duplicates.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]);
                        j--;
                    }
                }

                for (int j = 0; j < curr_hand_color_no_duplicates.Keys.Count() - 2; j++)
                {
                    if (curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]].getNumber() + 1 == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].getNumber() && curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]].getNumber() + 1 == curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]].getNumber())
                    {
                        // add to seq
                        List<List<Tile>> temp_sequences = new List<List<Tile>>(sequences);
                        temp_sequences.Add(new List<Tile>() { curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j]], curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 1]], curr_hand_color_no_duplicates[curr_hand_color_no_duplicates.Keys.ToList()[j + 2]] });

                        Dictionary<int, Tile> temp_curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_clone);
                        curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j]); curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j+1]); curr_hand_color_clone.Remove(curr_hand_color_no_duplicates.Keys.ToList()[j+2]);

                        color_sorted_hand[i] = new List<Tile>(curr_hand_color_clone.Values.ToList());
                        meldsSetsBetter(color_sorted_hand, temp_sequences, ref best_sequences, ref best_hand);

                        // fix what we ruined
                        color_sorted_hand[i] = temp_curr_hand_color_clone.Values.ToList();
                        curr_hand_color_clone = new Dictionary<int, Tile>(curr_hand_color_no_duplicates);
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
                if (remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getColor() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]].getColor() && remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j]].getNumber() == remaning_tiles_dict_no_dup[remaning_tiles_dict_no_dup.Keys.ToList()[j - 1]].getNumber())
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
                    temp_remaning_tiles_dict.Remove(remaning_tiles_dict.Keys.ToList()[j]); temp_remaning_tiles_dict.Remove(remaning_tiles_dict.Keys.ToList()[j+1]); temp_remaning_tiles_dict.Remove(remaning_tiles_dict.Keys.ToList()[j+2]);

                    // classify to 4 different lists(every color in every array)
                    List<Tile> sorted_tiles_no_dup = new List<Tile>(temp_remaning_tiles_dict.Values.ToList());
                    List<Tile>[] tiles_lst_color = new List<Tile>[4];
                    for (int color_index = 0; color_index < 4; color_index++) { tiles_lst_color[color_index] = new List<Tile>(); }
                    for (int i = 0; i < sorted_tiles_no_dup.Count(); i++)
                        tiles_lst_color[sorted_tiles_no_dup[i].getColor()].Add(sorted_tiles_no_dup[i]);

                    meldsSetsBetter(tiles_lst_color, temp_sequences, ref best_sequences, ref best_hand);
                }
            }
            return;
        }
    }
}