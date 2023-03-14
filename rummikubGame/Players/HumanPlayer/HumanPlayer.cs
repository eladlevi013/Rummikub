using System;

namespace rummikubGame
{
    [Serializable]
    public class HumanPlayer
    {
        public string player_name;
        public PlayerBoard board;

        public HumanPlayer()
        {
            board = new PlayerBoard(); // generate the board of the human player
        }
    }
}
