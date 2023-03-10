using System;

namespace rummikubGame
{
    [Serializable]
    public class HumanPlayer
    {
        public string player_name;
        public PlayerBoard board;

        public HumanPlayer(string player_name)
        {
            this.player_name = player_name;
            board = new PlayerBoard(); // generate the board of the human player
        }

        public string GetPlayerName()
        {
            return this.player_name;
        }

        public void SetPlayerName(string player_name)
        {
            this.player_name = player_name;
        }
    }
}
