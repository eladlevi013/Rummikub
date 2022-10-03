using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            tiles.Add(new Tile(1, 9));
            tiles.Add(new Tile(1, 10));
            tiles.Add(new Tile(1, 11));
            tiles.Add(new Tile(1, 12));

            tiles.Add(new Tile(3, 3));
            tiles.Add(new Tile(3, 4));
            tiles.Add(new Tile(3, 5));

            // DELETE
            int AFTER_WE_DESTROYED_RANDOMNESS = tiles.Count();
            for (int i = 0; i < 14 - AFTER_WE_DESTROYED_RANDOMNESS; i++)
            {
                tiles.Add(GameTable.pool.getTile());
            }


            // DELETE

            tiles = tiles.OrderBy(card => card.getNumber()).ToList();
            
            firstArrange();
            board = new ComputerBoard(hand, extendedSets);
        }

        public void play()
        {
            // int optimal_solution_score = getNumberOfTilesInAllSets(extendedSets);
            List<Tile> optimal_solution_hand = hand;
            List<List<Tile>> optimal_solution_sequences = extendedSets;

            List<Tile> temp_tiles = new List<Tile>();
            for (int j = 0; j < hand.Count(); j++)
            {
                if (hand[j] != null)
                    temp_tiles.Add(hand[j]);
            }
            if (extendedSets != null)
            {
                for (int j = 0; j < extendedSets.Count(); j++)
                    for (int k = 0; k < extendedSets[j].Count(); k++)
                        temp_tiles.Add(extendedSets[j][k]);
            }

            for (int i=0; i < 14; i++)
            {
                List<List<Tile>> temp_extendedSets = new List<List<Tile>>();
                List<Tile> temp_hand = new List<Tile>();
                List<Tile> temp_tiles_copy = temp_tiles.Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();

                temp_tiles_copy[i] = GameTable.dropped_tiles_stack.Peek();

                // basically first arrange
                List<List<Tile>> legalSets = new List<List<Tile>>();
                legalSets.Add(new List<Tile>());

                bool best_sets_number_found = false;
                List<List<Tile>> result = null;

                // we would like to stay with the most melds
                for (int j = 4; j >= 1 && !best_sets_number_found; j--)
                {
                    result = meldsSets(temp_tiles_copy, legalSets, 0, 0, j);
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
                if(getNumberOfTilesInAllSets(optimal_solution_sequences) < getNumberOfTilesInAllSets(temp_extendedSets))
                {
                    optimal_solution_sequences = temp_extendedSets;
                    optimal_solution_hand = temp_hand;
                }
            }

            extendedSets = optimal_solution_sequences;
            hand = optimal_solution_hand;

            board.setHand(hand);
            board.setSequences(extendedSets);
            GameTable.current_turn = GameTable.HUMAN_PLAYER_TURN;
            GameTable.ComputerPlayer.board.deleteCards();
            GameTable.ComputerPlayer.board.generateBoard();
        }

        public void firstArrange()
        {
            List<List<Tile>> legalSets = new List<List<Tile>>();
            legalSets.Add(new List<Tile>());

            bool best_sets_number_found = false;
            List<List<Tile>> result = null;

            // we would like to stay with the most melds
            for (int i = 4; i >= 1 && !best_sets_number_found; i--)
            {
                result = meldsSets(tiles, legalSets, 0, 0, i);
            }

            // add to hand
            for (int i=0; i<tiles.Count(); i++)
            {
                if (tiles[i] != null)
                    hand.Add(tiles[i]);
            }
;
            // makes sure to pick the best extended sequences
            if (result != null)
            {
                bool best_extended_sets_found = false;
                for (int i = 14 - hand.Count(); i >= 0 && !best_extended_sets_found; i--)
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

        public List<List<Tile>> meldsSets(List<Tile> tiles, List<List<Tile>> sets, int meldStart, int checkFrom, int maxSets)
        {
            List<Tile> currSet = sets[sets.Count() - 1];
            if (GameTable.isLegalMeld(currSet))
            {
                if (sets.Count() >= maxSets)
                    return sets;
                sets.Add(new List<Tile>());
                List<List<Tile>> result = meldsSets(tiles, sets, meldStart + 1, meldStart + 1, maxSets);
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
                        List<List<Tile>> result = meldsSets(tiles, sets, meldStart, i + 1, maxSets);
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