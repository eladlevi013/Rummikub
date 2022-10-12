using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace rummikubGame
{
    public class Pool
    {
        private Queue<Tile> tilesQueue;
        public Pool()
        {
            const int NUMBER_OF_TIMES = 1;
            const int COLORS_COUNT = 4;
            const int N = 13;

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
            // tiles_list.Add(new Tile(1, 0)); // black Joker added
            // tiles_list.Add(new Tile(3, 0)); // red Joker added

            Random rand = new Random();
            var randomized_list = tiles_list.OrderBy(c => rand.Next()).ToList();

            tilesQueue = new Queue<Tile>();
            for(int i=0; i<randomized_list.Count; i++)
            {
                tilesQueue.Enqueue(randomized_list[i]);
            }
        }

        public Tile getTile()
        {
            if (tilesQueue.Count() == 0)
            {   // tilesQueue is empty - tiles are over
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

        public int getPoolSize()
        {
            return tilesQueue.Count();
        }
    }
}
