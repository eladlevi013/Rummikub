using rummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rummikubGame
{
    [Serializable]
    public class Pool
    {
        private Queue<Tile> tilesQueue;
        
        public Pool()
        {
            tilesQueue = new Queue<Tile>();
            List<Tile> tiles_list = new List<Tile>();
            
            for (int times = 0; times < Constants.NUMBER_OF_TIMES; times++)
            {
                for (int color = 0; color < Constants.COLORS_COUNT; color++)
                {
                    for (int n = 1; n <= Constants.N; n++)
                    {
                        tiles_list.Add(new Tile(color, n));
                    }
                }
            }

            // adding jokers
            tiles_list.Add(new Tile(Constants.BLACK_COLOR, Constants.JOKER_NUMBER));
            tiles_list.Add(new Tile(Constants.RED_COLOR, Constants.JOKER_NUMBER));

            // Shuffeling the tiles
            Random rand = new Random();
            List<Tile> randomized_list = tiles_list.OrderBy(c => rand.Next()).ToList();

            // now insert that list into the queue
            for(int i=0; i<randomized_list.Count; i++)
            {
                tilesQueue.Enqueue(randomized_list[i]);
            }
        }

        public Tile GetTile()
        {
            // if tilesQueue is done check winner and return null
            if (tilesQueue.Count() == 0)
            {
                GameTable.CheckWinnerWhenPoolOver();
                return null;
            }

            // minus 1, because we havent removed any tile yet
            GameTable.global_current_pool_size_lbl.Text = GetPoolSize() - 1 + " tiles in pool";
            return tilesQueue.Dequeue();
        }

        public void UpdatePoolSizeLabel()
        {
            GameTable.global_current_pool_size_lbl.Text = GetPoolSize() + " tiles in pool";
        }

        private int GetPoolSize()
        {
            return tilesQueue.Count();
        }
    }
}
