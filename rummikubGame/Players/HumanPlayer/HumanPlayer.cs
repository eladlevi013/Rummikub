using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rummikubGame
{
    [Serializable]
    public class HumanPlayer
    {
        public HumanPlayerBoard board;

        public HumanPlayer()
        {
            board = new HumanPlayerBoard();
        }

        public bool CheckWinner()
        {
            List<List<Tile>> melds = GetMelds();

            // Iterating over arranged melds
            for (int i = 0; i < melds.Count(); i++)
            {
                if (!GameContext.IsLegalMeld(melds[i]))
                    return false;
            }

            return true;
        }

        private List<List<Tile>> GetMelds()
        {
            List<VisualTile> topBoard_tiles = new List<VisualTile>();
            List<VisualTile> bottomBoard_tiles = new List<VisualTile>();
            List<List<VisualTile>> melds = new List<List<VisualTile>>();

            for (int i = 0; i < board._tileButtons.Count; i++)
            {
                if (board._tileButtons[i].SlotLocation[0] == 0)
                    topBoard_tiles.Add(board._tileButtons[i]);
                else
                    bottomBoard_tiles.Add(board._tileButtons[i]);
            }

            // after we have two lists, we will sort them by value and analyze the melds
            topBoard_tiles = topBoard_tiles.OrderBy(card => card.SlotLocation[1]).ToList();
            bottomBoard_tiles = bottomBoard_tiles.OrderBy(card => card.SlotLocation[1]).ToList();

            GetSequencesFromTileList(topBoard_tiles, melds);
            GetSequencesFromTileList(bottomBoard_tiles, melds);

            List<List<Tile>> converted_melds_computer_format = new List<List<Tile>>();
            for (int i = 0; i < melds.Count(); i++)
                converted_melds_computer_format.Add(ConvertTilesButtonListToComputerFormat(melds[i]));

            return converted_melds_computer_format;
        }

        private static void GetSequencesFromTileList(List<VisualTile> tiles_sequence, List<List<VisualTile>> melds)
        {
            List<VisualTile> meld = new List<VisualTile>();
            for (int i = 0; i < tiles_sequence.Count(); i++)
            {
                if (meld.Count() != 0 && meld[meld.Count - 1].SlotLocation[1] + 1 == tiles_sequence[i].SlotLocation[1])
                {
                    meld.Add(tiles_sequence[i]);
                }
                else
                {
                    if (meld.Count != 0)
                        melds.Add(meld);

                    meld = new List<VisualTile>();
                    meld.Add(tiles_sequence[i]);
                }
            }
            if (meld.Count != 0)
                melds.Add(meld);
        }

        public int GetHandTilesNumber()
        {
            List<List<Tile>> melds = GetMelds();
            int tiles_in_sets = 0;

            for (int set_index = 0; set_index < melds.Count(); set_index++)
                if (GameContext.IsLegalMeld(melds[set_index]))
                    tiles_in_sets += melds[set_index].Count();

            return Constants.RummikubTilesInGame - tiles_in_sets;
        }

        public static List<Tile> ConvertTilesButtonListToComputerFormat(List<VisualTile> tiles)
        {
            List<Tile> new_tiles_format = new List<Tile>();
            for (int i = 0; i < tiles.Count(); i++)
                new_tiles_format.Add(tiles[i].TileData);
            return new_tiles_format;
        }
    }
}
