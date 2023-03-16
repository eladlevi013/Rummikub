using Rummikub;
using rummikubGame.Exceptions;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
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
            
            for (int times = 0; times < Constants.NumberOfTimes; times++)
            {
                for (int color = 0; color < Constants.ColorsCount; color++)
                {
                    for (int n = 1; n <= Constants.N; n++)
                    {
                        tiles_list.Add(new Tile(color, n));
                    }
                }
            }

            // adding jokers
            tiles_list.Add(new Tile(Constants.BlackColor, Constants.JokerNumber));
            tiles_list.Add(new Tile(Constants.RedColor, Constants.JokerNumber));

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
                RummikubGameView.CheckWinnerWhenPoolOver();
                throw new EmptyPoolException("Pool is empty");
            }

            // minus 1, because we havent removed any tile yet
            RummikubGameView.GlobalCurrentPoolSizeLbl.Text = GetPoolSize() - 1 + " tiles in pool";
            return tilesQueue.Dequeue();
        }

        public void UpdatePoolSizeLabel()
        {
            RummikubGameView.GlobalCurrentPoolSizeLbl.Text = GetPoolSize() + " tiles in pool";
        }

        private int GetPoolSize()
        {
            return tilesQueue.Count();
        }
    }
}
