using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rummikubGame
{
    public class HumanPlayer
    {
        public string player_name;
        public GraphicalBoard board;

        public HumanPlayer(string player_name)
        {
            this.player_name = player_name;
            board = new GraphicalBoard();
        }

        public string getPlayerName()
        {
            return this.player_name;
        }

        public void setPlayerName(string player_name)
        {
            this.player_name = player_name;
        }
    }
}