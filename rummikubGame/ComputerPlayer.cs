using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class ComputerPlayer
    {
        public List<Tile> tiles;
        public List<Tile> hand;
        public List<List<Tile>> extendedSets;
        public ComputerBoard board;

        public ComputerPlayer()
        {
            tiles = new List<Tile>();
            hand = new List<Tile>();

            // adds the random tiles
            int AFTER_WE_DESTROYED_RANDOMNESS = tiles.Count();
            for (int i = 0; i < GameTable.RUMMIKUB_TILES_IN_GAME - AFTER_WE_DESTROYED_RANDOMNESS; i++)
                tiles.Add(GameTable.pool.getTile());

            // first arrange those random tiles in optimal way
            firstArrange();

            // it takes care of the graphical representation of the tiles of the computer
            board = new ComputerBoard(hand, extendedSets);
        }

        public void firstArrange()
        {
            List<List<Tile>> legalSets = new List<List<Tile>>();
            legalSets.Add(new List<Tile>());

            // result gonna be with melds of 3
            bool best_sets_number_found = false; List<List<Tile>> result = null;
            for (int i = GameTable.MAX_POSSIBLE_SEQUENCES_NUMBER; i >= 1 && !best_sets_number_found; i--)
            {
                result = meldsSets(tiles, legalSets, 0, i);
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
            for (int i = 0; i < tiles.Count(); i++)
            {
                if (tiles[i] != null)
                    hand.Add(tiles[i]);
            }
;
            // extendedSets function is being called(makes sequences bigger from hand tiles)
            if (result != null)
            {
                bool best_extended_sets_found = false;
                for (int i = GameTable.RUMMIKUB_TILES_IN_GAME - hand.Count(); i >= 0 && !best_extended_sets_found; i--)
                {
                    List<List<Tile>> cloned_result = CloneSets(result);
                    extendedSets = extendSets(0, cloned_result, i, hand);
                    if (extendedSets != null)
                    {
                        best_extended_sets_found = true;
                    }
                }
            }
        }

        public bool better_sequences_after_taking_new_tile(Tile to_be_replaced)
        {
            bool replced_card_better_result = false;
            List<Tile> optimal_solution_hand = hand;
            List<List<Tile>> optimal_solution_sequences = extendedSets;
            List<Tile> temp_tiles = new List<Tile>();
            Tile optimal_dropped_tile = null;

            /* 
                filling the temp_tiles list
            */
            for (int j = 0; j < hand.Count(); j++)
            { // adds the hand cards to the temp_tiles list
                if (hand[j] != null)
                    temp_tiles.Add(hand[j]);
            }
            if (extendedSets != null)
            { // adds the tiles in sets to the temp_tiles list
                for (int j = 0; j < extendedSets.Count(); j++)
                    for (int k = 0; k < extendedSets[j].Count(); k++)
                        temp_tiles.Add(extendedSets[j][k]);
            }

            /*
            now we'll replace 
            */
            for (int i = 0; i < 14; i++)
            {
                List<List<Tile>> temp_extendedSets = new List<List<Tile>>();
                List<Tile> temp_hand = new List<Tile>();
                List<Tile> temp_tiles_copy = temp_tiles.Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                Tile dropped_tile = temp_tiles_copy[i];

                temp_tiles_copy[i] = to_be_replaced;

                // basically first arrange
                List<List<Tile>> legalSets = new List<List<Tile>>();
                legalSets.Add(new List<Tile>());

                bool best_sets_number_found = false;
                List<List<Tile>> result = null;

                // we would like to stay with the most melds
                for (int j = 4; j >= 1 && !best_sets_number_found; j--)
                {
                    result = meldsSets(temp_tiles_copy, legalSets, 0, j);
                    if (result == null)
                    {
                        legalSets = new List<List<Tile>>();
                        legalSets.Add(new List<Tile>());
                    }
                    else
                        best_sets_number_found = true;
                }

                // add to hand
                for (int j = 0; j < temp_tiles_copy.Count(); j++)
                {
                    if (temp_tiles_copy[j] != null)
                        temp_hand.Add(temp_tiles[j]);
                }
;
                // makes sure to pick the best extended sequences
                if (result != null)
                {
                    bool best_extended_sets_found = false;
                    for (int j = 14 - temp_hand.Count(); j >= 0 && !best_extended_sets_found; j--)
                    {
                        List<List<Tile>> cloned_result = CloneSets(result);
                        temp_extendedSets = extendSets(0, cloned_result, j, temp_hand);
                        if (temp_extendedSets != null)
                        {
                            best_extended_sets_found = true;
                        }
                    }
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

            if(replced_card_better_result == true)
            {
                extendedSets = optimal_solution_sequences;
                hand = optimal_solution_hand;

                int[] tile_in_dropped_tiles_location = { -1, -1 };
                TileButton dropped_tile = new TileButton(optimal_dropped_tile.getColor(), optimal_dropped_tile.getNumber(), tile_in_dropped_tiles_location);
                GameTable.dropped_tiles_stack.Push(dropped_tile);
                return true;
            }
            return false;
        }

        public void play(Tile to_be_replaced)
        {
            if (to_be_replaced != null && better_sequences_after_taking_new_tile(to_be_replaced) == true) {
                board.setHand(hand);
                board.setSequences(extendedSets);

                TileButton popped_tile = GameTable.dropped_tiles_stack.Pop();
                GameTable.GameTableContext.Controls.Remove(GameTable.dropped_tiles_stack.Peek().getTileButton());
                GameTable.dropped_tiles_stack.Push(popped_tile);
                GameTable.humanPlayer.board.GenerateComputerThrownTile(GameTable.dropped_tiles_stack.Pop());
            }
            else // didnt find any better option
            {
                Random rnd_hand_index = new Random();
                bool hand_null = true;
                Tile random_tile_to_drop = null;
                int random_tile_to_drop_index = 0;
                while (hand_null)
                {
                    random_tile_to_drop_index = rnd_hand_index.Next(hand.Count());
                    random_tile_to_drop = hand[random_tile_to_drop_index];
                    if (random_tile_to_drop != null)
                    {
                        hand_null = false;
                    }
                }
                hand.RemoveAt(random_tile_to_drop_index);


                GameTable.humanPlayer.board.GenerateComputerThrownTile(random_tile_to_drop);

                Tile tile = GameTable.pool.getTile();
                hand.Add(tile);
                GameTable.ComputerPlayer.board.generateBoard();

                Tile tile_from_pool = tile;
                if (better_sequences_after_taking_new_tile(tile_from_pool) == true)
                {
                    board.setHand(hand);
                    board.setSequences(extendedSets);
                    
                    GameTable.humanPlayer.board.GenerateComputerThrownTile(GameTable.dropped_tiles_stack.Pop());
                }
            }
            GameTable.current_turn = GameTable.HUMAN_PLAYER_TURN;
            GameTable.game_indicator.Text = "Your turn";
            PlayerBoard.tookCard = false;
            GameTable.ComputerPlayer.board.deleteCards();

            if (GameTable.showComputerTilesGroupbox.Checked == true)
                GameTable.ComputerPlayer.board.generateBoard();

            // if the game is over, and the computer won
            if (board.checkWinner() == true)
            {
                MessageBox.Show("Computer Won!");
                GameTable.game_indicator.Text = "Game Over - Computer Won";
                GameTable.humanPlayer.board.disableHumanBoard();
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
        public List<List<Tile>> extendSets(int indexOfHandTile, List<List<Tile>> sequences, int number_of_tiles_in_set, List<Tile> hand_tiles)
        {
            if (indexOfHandTile >= hand_tiles.Count())
            {
                if(getNumberOfTilesInAllSets(sequences) >= number_of_tiles_in_set)
                    return sequences;
                else 
                    return null;
            }
            for (int i=0; i<sequences.Count(); i++)
            {
                if (hand_tiles[indexOfHandTile] != null)
                {
                    List<Tile> tempSequenceAddRight = sequences[i].Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                    tempSequenceAddRight.Add(hand_tiles[indexOfHandTile]);
                    if (GameTable.isLegalMeld(tempSequenceAddRight) == true)
                    {
                        sequences[i] = tempSequenceAddRight;
                        hand_tiles[indexOfHandTile] = null;
                    }
                }
                if (hand_tiles[indexOfHandTile] != null)
                {
                    List<Tile> tempSequenceAddLeft = sequences[i].Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                    tempSequenceAddLeft.Insert(0, hand_tiles[indexOfHandTile]);
                    if (GameTable.isLegalMeld(tempSequenceAddLeft) == true)
                    {
                        sequences[i] = tempSequenceAddLeft;
                        hand_tiles[indexOfHandTile] = null;
                    }
                }
            }
            return extendSets(indexOfHandTile + 1, sequences, number_of_tiles_in_set, hand_tiles);
        }

        public List<List<Tile>> meldsSets(List<Tile> tiles, List<List<Tile>> sets, int meldStart, int maxSets)
        {
            List<Tile> currSet = sets[sets.Count() - 1];
            if (GameTable.isLegalMeld(currSet))
            {
                if (sets.Count() >= maxSets)
                    return sets;
                sets.Add(new List<Tile>());
                List<List<Tile>> result = meldsSets(tiles, sets, meldStart + 1, maxSets);
                if (result == null)
                {
                    sets.RemoveAt(sets.Count() - 1);
                }
                else
                {
                    return result;
                }
            }
            if (GameTable.canBeLegal(currSet))
            {
                for (int i = 0; i < tiles.Count(); i++)
                {
                    Tile t = tiles[i];
                    // && !t.isJoker()
                    if (t != null)
                    {
                        tiles[i] = null;
                        currSet.Add(t);
                        List<List<Tile>> result = meldsSets(tiles, sets, meldStart, maxSets);
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