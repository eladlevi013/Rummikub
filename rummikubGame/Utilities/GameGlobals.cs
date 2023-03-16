using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rummikubGame;
using rummikubGame.Models;

namespace RummikubGame.Utilities
{
    public static class GameGlobals
    {
        // Private fields
        private static int _currentTurn;
        private static bool _gameOver = false;
        private static Pool _pool;
        private static Stack<VisualTile> _droppedTilesStack;
        private static HumanPlayer _humanPlayer;
        private static ComputerPlayer _computerPlayer;

        // Public properties
        public static int CurrentTurn
        {
            get { return _currentTurn; }
            set { _currentTurn = value; }
        }

        public static bool GameOver
        {
            get { return _gameOver; }
            set { _gameOver = value; }
        }

        public static Pool Pool
        {
            get { return _pool; }
            set { _pool = value; }
        }

        public static Stack<VisualTile> DroppedTilesStack
        {
            get { return _droppedTilesStack; }
            set { _droppedTilesStack = value; }
        }

        public static HumanPlayer HumanPlayer
        {
            get { return _humanPlayer; }
            set { _humanPlayer = value; }
        }

        public static ComputerPlayer ComputerPlayer
        {
            get { return _computerPlayer; }
            set { _computerPlayer = value; }
        }
    }
}
