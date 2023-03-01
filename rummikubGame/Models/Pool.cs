using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace rummikubGame
{
    [Serializable]
    public class Pool
    {
        // consts
        public static int NUMBER_OF_TIMES = 1;
        public static int COLORS_COUNT = 4;
        public static int N = 13; // numbers range

        private Queue<Tile> tilesQueue;
        public Pool()
        {
            // the creation of the pool
            List<Tile> tiles_list = new List<Tile>();
            for (int times = 0; times < NUMBER_OF_TIMES; times++)
            {
                for (int color = 0; color < COLORS_COUNT; color++)
                {
                    for (int n = 1; n <= N; n++)
                    {
                        tiles_list.Add(new Tile(color, n));
                    }
                }
            }

            // add the jokers
            tiles_list.Add(new Tile(1, 0)); // black Joker added
            tiles_list.Add(new Tile(3, 0)); // red Joker added

            // make the list of tiles randomized
            Random rand = new Random();
            var randomized_list = tiles_list.OrderBy(c => rand.Next()).ToList();

            // now insert that list into the queue
            tilesQueue = new Queue<Tile>();
            for(int i=0; i<randomized_list.Count; i++)
            {
                tilesQueue.Enqueue(randomized_list[i]);
            }
        }

        public Tile getTile()
        {
            // if tilesQueue is done check winner and return null
            if (tilesQueue.Count() == 0)
            {   
                // tilesQueue is empty -> tiles are over -> game over -> decide who is the winner(fewer files in hand)
                if (GameTable.computer_player.board.getHandTilesNumber() == GameTable.human_player.board.getHandTilesNumber())
                    MessageBox.Show("Tie!");
                else if (GameTable.computer_player.board.getHandTilesNumber() > GameTable.human_player.board.getHandTilesNumber())
                    MessageBox.Show("You Won!");
                else
                    MessageBox.Show("Computer Won!");
                GameTable.game_over = true;
                GameTable.human_player.board.disableHumanBoard();
                return null;
            }
            GameTable.global_current_pool_size_lbl.Text = getPoolSize() - 1 + " tiles in pool"; // minus 1, because we havent removed any tile yet
            return tilesQueue.Dequeue();
        }

        public void updatePoolSizeLabel()
        {
            GameTable.global_current_pool_size_lbl.Text = getPoolSize() + " tiles in pool";
        }

        public int getPoolSize()
        {
            return tilesQueue.Count();
        }
    }
}
