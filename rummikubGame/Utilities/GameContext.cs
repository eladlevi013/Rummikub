using Rummikub;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame.Utilities
{
    public class GameContext
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

        public static void PoolOnClick(int[] slotLocation)
        {
            if (!HumanPlayer.board.TookCard && !GameOver
                && CurrentTurn == Constants.HumanPlayerTurn)
            {
                // Make tile in stack non-interactable after generating new tile
                if (DroppedTilesStack != null && DroppedTilesStack.Any())
                {
                    DroppedTilesStack.Peek().DisableTile();
                }

                HumanPlayer.board.TookCard = true;
                RummikubGameView.GlobalGameIndicatorLbl.Text = RummikubGameView.DropTileFromBoardMsg;
                HumanPlayer.board.GenerateSingleTileToBoard(slotLocation, true);
            }
        }

        public static float GetDistance(Button tile, Button slot)
        {
            // Used find the closet slot to a card
            return (float)Math.Sqrt(Math.Pow(tile.Location.X - slot.Location.X, 2)
                + Math.Pow(tile.Location.Y - slot.Location.Y, 2));
        }

        public static void GenerateComputerThrownTile(Tile thrownTile)
        {
            if (DroppedTilesStack.Count() > 1)
                DroppedTilesStack.Peek().Draggable.SetDraggable(false);

            int[] slotLocation = { Constants.DroppedTileLocation, Constants.DroppedTileLocation };
            VisualTile visualThrownTile = HumanPlayer.board.GenerateSingleTile(thrownTile, slotLocation);
            visualThrownTile.Location
                = new Point(RummikubGameView.GlobalDroppedTilesBtn.Location.X + 10, RummikubGameView.GlobalDroppedTilesBtn.Location.Y + 18);
            DroppedTilesStack.Push(visualThrownTile);

        }
    }
}
