using Rummikub;
using rummikubGame.Draggable;
using rummikubGame.Models;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class VisualTile : Button, ISerializable
    {
        [NonSerialized]
        private DraggableComponent _draggable;
        [NonSerialized]
        private BrightnessEffectComponent _brightnessOnHover;
        private VisualTileData _tileData;
        
        public VisualTile(VisualTileData data)
        {
            _tileData = data;
            _draggable = new DraggableComponent(this);
            _brightnessOnHover = new BrightnessEffectComponent(this);

            // Sets mouse events
            MouseDown += TileButton_MouseDown;
            MouseUp += TileButton_MouseUp;
        }

        public VisualTile(int color, int number, int[] slotLocation)
        {
            _tileData = new VisualTileData(color, number, slotLocation);
            _draggable = new DraggableComponent(this);
            _brightnessOnHover = new BrightnessEffectComponent(this);

            // Sets mouse events
            MouseDown += TileButton_MouseDown;
            MouseUp += TileButton_MouseUp;
        }

        public VisualTile(SerializationInfo info, StreamingContext context)
        {
            _tileData = (VisualTileData)info.GetValue("TileData", typeof(VisualTileData));
            _draggable = new DraggableComponent(this);
            _brightnessOnHover = new BrightnessEffectComponent(this);

            // Sets mouse events
            MouseDown += TileButton_MouseDown;
            MouseUp += TileButton_MouseUp;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Serialize the fields you want to persist
            info.AddValue("TileData", _tileData);
        }

        public BrightnessEffectComponent BrightnessOnHover
        {
            get { return _brightnessOnHover; }
            set { _brightnessOnHover = value; }
        }

        public DraggableComponent Draggable
        {
            get { return _draggable; }
            set { _draggable = value; }
        }

        public VisualTileData VisualTileData
        {
            get { return _tileData; }
            set { _tileData = value; }
        }

        public void DisableTile()
        {
            // Removing mouse events
            MouseDown -= TileButton_MouseDown;
            MouseUp -= TileButton_MouseUp;
            Draggable.SetDraggable(false);
        }

        public void TileButton_MouseDown(object sender, MouseEventArgs e)
        {
            VisualTile currTile = (VisualTile)sender;
            currTile.BringToFront();

            // Update the status of the old slot to Available
            if (currTile.VisualTileData.SlotLocation[0] != Constants.DroppedTileLocation
                && currTile.VisualTileData.SlotLocation[1] != Constants.DroppedTileLocation)
                GameContext.HumanPlayer.board.BoardSlots[currTile.VisualTileData.SlotLocation[0], currTile.VisualTileData.SlotLocation[1]].SlotState = Constants.Available;
        }

        public void TileButton_MouseUp(object sender, MouseEventArgs e)
        {   
            VisualTile currTile = (VisualTile)sender;

            if (GameContext.GetDistance(currTile, RummikubGameView.GlobalDroppedTilesBtn) < 100
                && GameContext.CurrentTurn == Constants.HumanPlayerTurn
                && GameContext.HumanPlayer.board.TookCard == true)
            {
                currTile.Location = new Point(RummikubGameView.GlobalDroppedTilesBtn.Location.X
                    + 10, RummikubGameView.GlobalDroppedTilesBtn.Location.Y + 18);
                currTile.VisualTileData.SlotLocation = new int[] { Constants.DroppedTileLocation, Constants.DroppedTileLocation };
                currTile.Draggable.SetDraggable(false);

                // Adding tile to dropped tiles stack
                GameContext.DroppedTilesStack.Push(currTile);
                GameContext.HumanPlayer.board.TileButtons.Remove(currTile);

                // Updating Board
                GameContext.HumanPlayer.board.TookCard = false;
                GameContext.CurrentTurn = Constants.ComputerPlayerTurn;
                RummikubGameView.GlobalGameIndicatorLbl.Text = "Computer's Turn";
                GameContext.ComputerPlayer.ComputerPlay(currTile.VisualTileData.TileData);
            }
            else
            {
                // Find the first empty slot
                float minDistance = float.MaxValue;
                Button firstEmptySlot = null;
                int min_i = -1;
                int min_j = -1;

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        // Check if the current slot is empty
                        if (GameContext.HumanPlayer.board.BoardSlots[i, j].SlotState == Constants.Available)
                        {
                            // Calculate the distance between the current card and the current slot
                            float distance = GameContext.GetDistance(currTile, GameContext.HumanPlayer.board.BoardSlots[i, j].SlotButton);

                            // If this slot is closer than the previous one, update the minimum distance
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                firstEmptySlot = GameContext.HumanPlayer.board.BoardSlots[i, j].SlotButton;
                                min_i = i;
                                min_j = j;
                            }
                        }
                    }
                }

                if (currTile.VisualTileData.SlotLocation[0] == Constants.DroppedTileLocation && currTile.VisualTileData.SlotLocation[1] == Constants.DroppedTileLocation)
                {
                    GameContext.HumanPlayer.board.TookCard = true;
                    GameContext.HumanPlayer.board.TileButtons.Add(currTile);
                    GameContext.DroppedTilesStack.Pop();
                    GameContext.DroppedTilesStack.Peek().DisableTile();
                    RummikubGameView.GlobalGameIndicatorLbl.Text = RummikubGameView.DropTileFromBoardMsg;

                    // Update the location of the focused TileButton to the location of the closest empty slot
                    ControlTransition.Move(currTile, currTile.Location, firstEmptySlot.Location);

                    // Update the status of the old slot to
                    GameContext.HumanPlayer.board.BoardSlots[min_i, min_j].SlotState = Constants.Allocated;
                    currTile.VisualTileData.SlotLocation = new int[] { min_i, min_j };
                }
                else
                {
                    ControlTransition.Move(currTile, currTile.Location, firstEmptySlot.Location);
                    GameContext.HumanPlayer.board.BoardSlots[currTile.VisualTileData.SlotLocation[0], currTile.VisualTileData.SlotLocation[1]].SlotState = Constants.Available;
                    GameContext.HumanPlayer.board.BoardSlots[min_i, min_j].SlotState = Constants.Allocated;
                    currTile.VisualTileData.SlotLocation = new int[] { min_i, min_j };
                }
            }
            // Check Winning every moving tile
            if (GameContext.HumanPlayer.CheckWinner() == true && GameContext.GameOver == false)
            {
                MessageBox.Show("You Won!");
                RummikubGameView.GlobalGameIndicatorLbl.Text = "Game Over - You Won";
                GameContext.GameOver = true;
                GameContext.HumanPlayer.board.DisableBoard();
                GameContext.DroppedTilesStack.Peek().Enabled = false;
            }
        }
    }
}
