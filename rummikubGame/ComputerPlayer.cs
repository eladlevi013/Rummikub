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
            board.starting_tiles = board.starting_tiles.OrderBy(card => card.getNumber()).ToList();

            bool best_sets_number_found = false; 
            List<List<Tile>> result = null;

            // result will be melds of 3 tiles
            List<List<Tile>> legalSets = new List<List<Tile>>(); legalSets.Add(new List<Tile>()); // meldSets parameter
            for (int i = GameTable.MAX_POSSIBLE_SEQUENCES_NUMBER; i >= 1 && !best_sets_number_found; i--)
            {
                result = meldsSets(board.starting_tiles, ref legalSets, 0, i);
                // result = meldsSetsBetter(board.starting_tiles);
                if (result == null)
                {
                    legalSets = new List<List<Tile>>();
                    legalSets.Add(new List<Tile>());
                }
                else
                {
                    best_sets_number_found = true;
                }
            }

            // sets the hand(cards that are not in any sequence)
            for (int i = 0; i < board.starting_tiles.Count(); i++)
            {
                if (board.starting_tiles[i] != null)
                    board.hand.Add(board.starting_tiles[i]);
            }
;
            // extendedSets function is being called(makes sequences bigger from hand tiles)
            if (result != null)
            {
                extendSets(ref result, ref board.hand);
                board.sequences = result;
            }
        }

        public bool better_sequences_after_taking_new_tile(Tile new_tile)
        {
            bool replced_card_better_result = false;
            List<Tile> optimal_solution_hand = board.hand;
            List<List<Tile>> optimal_solution_sequences = board.sequences;
            List<Tile> starting_tiles = new List<Tile>();
            Tile optimal_dropped_tile = null;

            // starting_tiles will be a list of all of the tiles
            for (int j = 0; j < board.hand.Count(); j++)
            { 
                // adds the hand cards to the temp_tiles list
                if (board.hand[j] != null)
                    starting_tiles.Add(board.hand[j]);
            }
            if (board.sequences != null)
            { // adds the tiles in sets to the temp_tiles list
                for (int j = 0; j < board.sequences.Count(); j++)
                    for (int k = 0; k < board.sequences[j].Count(); k++)
                        starting_tiles.Add(board.sequences[j][k]);
            }

            // replacing every tile in starting_tiles with the given paramter in order to get better result
            for (int i = 0; i < GameTable.RUMMIKUB_TILES_IN_GAME; i++)
            {
                // we want copy of starting tiles because result function makes some tiles null and we would not like it to be affect the next iteration
                List<Tile> starting_tiles_copy = starting_tiles.Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                List<List<Tile>> temp_extendedSets = new List<List<Tile>>();
                List<Tile> temp_hand = new List<Tile>();
                Tile dropped_tile = starting_tiles_copy[i];

                // replacing starting tile at index i, in order to see if its getting better result
                starting_tiles_copy[i] = new_tile;

                // Similar proccess as firstArrange
                List<List<Tile>> legalSets = new List<List<Tile>>(); legalSets.Add(new List<Tile>());
                bool best_sets_number_found = false;
                List<List<Tile>> result = null;
                for (int j = GameTable.RUMMIKUB_TILES_IN_GAME; j >= 1 && !best_sets_number_found; j--)
                {
                    result = meldsSets(starting_tiles_copy,  ref legalSets, 0, j);
                    // result = meldsSetsBetter(board.starting_tiles);
                    if (result == null)
                    {
                        legalSets = new List<List<Tile>>();
                        legalSets.Add(new List<Tile>());
                    }
                    else
                    {
                        best_sets_number_found = true;
                    }
                }

                // add to hand
                for (int j = 0; j < starting_tiles_copy.Count(); j++)
                {
                    if (starting_tiles_copy[j] != null)
                        temp_hand.Add(starting_tiles_copy[j]);
                }
;
                // makes sure to pick the best extended sequences
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
                 * 3. return true
            */
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
            return false;
        }

        public void play(Tile to_be_replaced)
        {
            // didnt find better arrangement
            if(!better_sequences_after_taking_new_tile(to_be_replaced))
            {
                Tile tile = GameTable.pool.getTile();
                if (tile == null) // pool is empty
                    return;

                // better option with tile from pool
                if (better_sequences_after_taking_new_tile(tile) == true)
                {
                    board.GenerateComputerThrownTile(GameTable.dropped_tiles_stack.Pop());
                }
                else
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
            if (board.checkWinner() == true)
            {
                MessageBox.Show("Computer Won!");
                GameTable.global_game_indicator_lbl.Text = "Game Over - Computer Won";
                GameTable.human_player.board.disableHumanBoard();
                GameTable.dropped_tiles_stack.Peek().getTileButton().Enabled = false;
                GameTable.game_over = true;
            }
        }

        public List<List<Tile>> CloneSets(List<List<Tile>> sets)
        {
            List<List<Tile>> cloned_sets = new List<List<Tile>>();
            for(int i=0; i<sets.Count(); i++)
            {
                List<Tile> set = new List<Tile>();
                cloned_sets.Add(set);
                for(int j=0; j < sets[i].Count(); j++)
                {
                    cloned_sets[i].Add(sets[i][j]);
                }
            }
            return cloned_sets;
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

        /*
         Create a set class, which contains the data of what type the set is(Group, Run)
         for group we dont need to check the two of the ways to arrange
         */
        public void extendSets(ref List<List<Tile>> sequences, ref List<Tile> hand_tiles)
        {
            // we have to sort the hand_tiles
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

        public static bool canBeLegal(List<Tile> set)
        {
            if (set.Count() < 2)
                return true;
            if (set.Count() == 2)
            {
                Tile t1 = set[0];
                Tile t2 = set[1];
                if (t1.getNumber() + 1 == t2.getNumber() && t1.getColor() == t2.getColor())
                    return true;
                if (t1.getNumber() == t2.getNumber() && t1.getColor() != t2.getColor())
                    return true;
                return false;
            }
            return GameTable.isLegalMeld(set);
        }

        public List<List<Tile>> meldsSetsBetter(List<Tile> sorted_tiles)
        {
            List<Tile> sorted_tiles_no_dup = new List<Tile>(sorted_tiles);

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

            List<List<Tile>> sequences = new List<List<Tile>>();
            for (int i = 0; i < 4; i++)
            {
                bool meld_found = false;
                do
                {
                    meld_found = false;
                    Dictionary<int, Tile> current_color_lst = new Dictionary<int, Tile>();
                    for (int k = 0; k < tiles_lst_color[i].Count(); k++) current_color_lst[k] = tiles_lst_color[i][k];

                    // now we'll remove the duplicates
                    for (int j = 1; j < current_color_lst.Count(); j++)
                    {
                        if (current_color_lst[current_color_lst.Keys.ToList()[j]].getColor() == current_color_lst[current_color_lst.Keys.ToList()[j - 1]].getColor() && current_color_lst[current_color_lst.Keys.ToList()[j]].getNumber() == current_color_lst[current_color_lst.Keys.ToList()[j - 1]].getNumber())
                        {
                            current_color_lst.Remove(current_color_lst.Keys.ToList()[j]);
                            j--;
                        }
                    }

                    for(int j=0; j< current_color_lst.Keys.Count()-2; j++)
                    {
                        if (current_color_lst[current_color_lst.Keys.ToList()[j]].getNumber() + 1 == current_color_lst[current_color_lst.Keys.ToList()[j+1]].getNumber() && current_color_lst[current_color_lst.Keys.ToList()[j+1]].getNumber() + 1 == current_color_lst[current_color_lst.Keys.ToList()[j+2]].getNumber())
                        {
                            sequences.Add(new List<Tile>() { current_color_lst[j], current_color_lst[j + 1], current_color_lst[j + 2] });
                            current_color_lst.Remove(current_color_lst.Keys.ToList()[j]); current_color_lst.Remove(current_color_lst.Keys.ToList()[j]); current_color_lst.Remove(current_color_lst.Keys.ToList()[j]);
                            tiles_lst_color[i].RemoveAt(j); tiles_lst_color[i].RemoveAt(j); tiles_lst_color[i].RemoveAt(j);
                            meld_found = true;
                        }
                    }
                }
                while (meld_found);
            }
            return sequences;
        }

        public List<List<Tile>> meldsSets(List<Tile> tiles, ref List<List<Tile>> sets, int meldStart, int maxSets)
        {
            List<Tile> currSet = sets[sets.Count() - 1];
            if (GameTable.isLegalMeld(currSet))
            {
                if (sets.Count() >= maxSets)
                    return sets;
                sets.Add(new List<Tile>());
                List<List<Tile>> result = meldsSets(tiles, ref sets, meldStart + 1, maxSets);
                if (result == null)
                {
                    sets.RemoveAt(sets.Count() - 1);
                }
                else
                {
                    return result;
                }
            }
            if (canBeLegal(currSet))
            {
                for (int i = 0; i < tiles.Count(); i++)
                {
                    Tile t = tiles[i];
                    // && !t.isJoker()
                    if (t != null)
                    {
                        tiles[i] = null;
                        currSet.Add(t);
                        List<List<Tile>> result = meldsSets(tiles, ref sets, meldStart, maxSets);
                        if (result == null)
                        {
                            currSet.RemoveAt(currSet.Count() - 1);
                            tiles[i] = t;
                            meldStart++;
                        }
                        else
                            return result;
                    }
                }
            }
            return null;
        }
    }
}