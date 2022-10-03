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

            // DELETE
            int AFTER_WE_DESTROYED_RANDOMNESS = tiles.Count();
            for (int i = 0; i < 14 - AFTER_WE_DESTROYED_RANDOMNESS; i++)
            {
                tiles.Add(GameTable.pool.getTile());
            }

            tiles = tiles.OrderBy(card => card.getNumber()).ToList();
            firstArrange();
            board = new ComputerBoard(hand, extendedSets);
        }

        public void play()
        {

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
            for(int i=0; i<tiles.Count(); i++)
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
                    extendedSets = extendSets(0, cloned_result, i);
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
            for(int i=0; i<sequences.Count(); i++)
                sum += sequences[i].Count();
            return sum;
        }

        /*
         Create a set class, which contains the data of what type the set is(Group, Run)
         for group we dont need to check the two of the ways to arrange
         */
        public List<List<Tile>> extendSets(int indexOfHandTile, List<List<Tile>> sequences, int number_of_tiles_in_set)
        {
            if (indexOfHandTile >= hand.Count())
            {
                if(getNumberOfTilesInAllSets(sequences) >= number_of_tiles_in_set)
                    return sequences;
                else 
                    return null;
            }
            for (int i=0; i<sequences.Count(); i++)
            {
                if (hand[indexOfHandTile] != null)
                {
                    List<Tile> tempSequenceAddRight = sequences[i].Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                    tempSequenceAddRight.Add(hand[indexOfHandTile]);
                    if (GameTable.isLegalMeld(tempSequenceAddRight) == true)
                    {
                        sequences[i] = tempSequenceAddRight;
                        hand[indexOfHandTile] = null;
                    }
                }
                if (hand[indexOfHandTile] != null)
                {
                    List<Tile> tempSequenceAddLeft = sequences[i].Select(item => item.Clone(item.getColor(), item.getNumber())).ToList();
                    tempSequenceAddLeft.Insert(0, hand[indexOfHandTile]);
                    if (GameTable.isLegalMeld(tempSequenceAddLeft) == true)
                    {
                        sequences[i] = tempSequenceAddLeft;
                        hand[indexOfHandTile] = null;
                    }
                }
            }
            return extendSets(indexOfHandTile + 1, sequences, number_of_tiles_in_set);
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
                for (int i = checkFrom; i < tiles.Count(); i++)
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