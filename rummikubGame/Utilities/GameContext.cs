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

        public static void ResolveWinnerOnPoolOver()
        {
            OnGameOver();
            if (ComputerPlayer.board.GetHandTilesNumber() 
                == HumanPlayer.GetHandTilesNumber())
                MessageBox.Show("Tie!");
            else if (ComputerPlayer.board.GetHandTilesNumber() 
                > HumanPlayer.GetHandTilesNumber())
                MessageBox.Show("You Won!");
            else
                MessageBox.Show("Computer Won!");
        }

        public static void OnGameOver()
        {
            GameOver = true;
            HumanPlayer.board.DisableBoard();
        }

        public static bool IsLegalMeld(List<Tile> meld)
        {
            bool isRun = true;
            if (meld.Count < 3)
                return false;

            // finds the first non-joker tile and uses it to determine the color and value of the run
            int firstNonJokerIndex = 0;
            for (int i = 0; i < meld.Count; i++)
            {
                if (!IsJoker(meld[i]))
                {
                    firstNonJokerIndex = i;
                    break;
                }
            }

            int color = meld[firstNonJokerIndex].Color;
            int value = meld[firstNonJokerIndex].Number;

            for (int i = firstNonJokerIndex + 1; i < meld.Count; i++)
            {
                // if meld number is not equal to the value + the index of the tile in the meld its cant be a run
                if ((meld[i].Number != value + i - firstNonJokerIndex
                    || meld[i].Color != color) && !IsJoker(meld[i]))
                {
                    isRun = false;
                }
                if (IsJoker(meld[i]))
                {
                    // Skip over jokers and continue checking the rest of the tiles
                    continue;
                }
            }
            if (isRun)
            {
                // checking the value of the max
                if (meld[firstNonJokerIndex].Number + ((meld.Count - 1) - firstNonJokerIndex) > 13)
                {
                    return false;
                }
                // checking the value of the min
                if (meld[firstNonJokerIndex].Number - firstNonJokerIndex < 1)
                {
                    return false;
                }
                return true; // 2+ run sequence
            }
            if (meld.Count > 4)
            {
                return false; // group of 4+ cannot exist
            }
            for (int i = 0; i < meld.Count() - 1; i++)
            {
                if (meld[i + 1].Number != value && !IsJoker(meld[i + 1]))
                {
                    return false; // its cannot be group
                }
                for (int j = i + 1; j < meld.Count(); j++)
                {
                    if (meld[i].Color == meld[j].Color 
                        && !IsJoker(meld[i]) && !IsJoker(meld[j]))
                    {
                        return false;
                    }
                }
            }

            // check if there are too many jokers used
            int numJokers = CountJokers(meld);
            if (numJokers > 2)
            {
                return false; // too many jokers used
            }
            return true;
        }

        private static int CountJokers(List<Tile> meld)
        {
            int count = 0;
            foreach (Tile tile in meld)
            {
                if (IsJoker(tile))
                    count++;
            }
            return count;
        }

        public static bool IsJoker(Tile tile)
        {
            return tile.Number == Constants.JokerNumber;
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

        public static bool IsRun(List<Tile> sequence)
        {
            // assuming valid sequence(group or run)
            return sequence[0].Color == sequence[1].Color;
        }

        public static bool HandContains(List<Tile> hand, Tile tile)
        {
            for(int i=0; i < hand.Count; i++)
            {
                if (hand[i] == tile)
                    return true;
            }
            return false;
        }
    }
}
