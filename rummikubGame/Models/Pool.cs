using Rummikub;
using rummikubGame.Exceptions;
using rummikubGame.Logic;
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
        /*
            This class purpose, is to serve easy card getting from pool,
            its managing its own cards and all you need to do is make a single
            object of that, and call GetTile function.
        */

        private Queue<Tile> _tilesQueue;
        
        public Pool()
        {
            List<Tile> tilesList = new List<Tile>();
            
            for (int times = 0; times < Constants.NumberOfTimes; times++)
            {
                for (int color = 0; color < Constants.ColorsCount; color++)
                {
                    for (int n = 1; n <= Constants.N; n++)
                    {
                        tilesList.Add(new Tile(color, n));
                    }
                }
            }

            // adding jokers
            tilesList.Add(new Tile(Constants.BlackColor, Constants.JokerNumber));
            tilesList.Add(new Tile(Constants.RedColor, Constants.JokerNumber));

            // Shuffeling the tiles list
            Random rand = new Random();
            List<Tile> randomized_list = tilesList.OrderBy(c => rand.Next()).ToList();

            // now insert that list into the queue
            _tilesQueue = new Queue<Tile>();
            for (int i=0; i < randomized_list.Count; i++)
            {
                _tilesQueue.Enqueue(randomized_list[i]);
            }
        }

        public Tile GetTile()
        {
            // if queue is empty, we need to resolve the winner
            if (_tilesQueue.Count() == 0)
            {
                GameLogic.ResolveWinnerOnPoolOver();
                throw new EmptyPoolException("Pool is empty");
            }

            RummikubGameView.GlobalCurrentPoolSizeLbl.Text = GetPoolSize() - 1 + " tiles in pool";
            return _tilesQueue.Dequeue();
        }

        public void UpdatePoolSizeLabel()
        {
            RummikubGameView.GlobalCurrentPoolSizeLbl.Text = GetPoolSize() + " tiles in pool";
        }

        private int GetPoolSize()
        {
            return _tilesQueue.Count();
        }
    }
}
