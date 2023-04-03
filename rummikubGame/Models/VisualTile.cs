using Rummikub;
using rummikubGame.Draggable;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class VisualTile : Button
    {
        // Brightness on hover effect constants
        private const float DefaultBrightnessLevel = 1.0f;
        private const float HoverBrightnessLevel = 1.2f;

        // Varibales for brightness hovering effect
        private Image _brightnessImage;
        private Image _originalBackgroundImage;
        private float _currentBrightnessLevel = DefaultBrightnessLevel;

        private int[] _slotLocation;
        private DraggableComponent _draggable;
        public Tile _tileData;

        public VisualTile(int color, int number, int[] slotLocation)
        {
            _tileData = new Tile(color, number);
            _slotLocation = slotLocation;
            _draggable = new DraggableComponent(this);

            // Sets mouse events
            MouseEnter += VisualTile_MouseEnter;
            MouseLeave += VisualTile_MouseLeave;
            MouseDown += TileButton_MouseDown;
            MouseUp += TileButton_MouseUp;
        }

        public int[] SlotLocation
        {
            get { return _slotLocation; }
            set { _slotLocation = value; }
        }

        public DraggableComponent Draggable
        {
            get { return _draggable; }
            set { _draggable = value; }
        }

        public Tile TileData
        {
            get { return _tileData; }
            set { _tileData = value; }
        }

        public void RemoveBrightness()
        {
            if (BackgroundImage == null || _originalBackgroundImage == null 
                || _currentBrightnessLevel == DefaultBrightnessLevel)
            {
                return;
            }
            BackgroundImage = _originalBackgroundImage;
            _currentBrightnessLevel = DefaultBrightnessLevel;
        }

        public void ApplyBrightness()
        {
            _originalBackgroundImage = BackgroundImage;

            if (BackgroundImage == null || HoverBrightnessLevel == _currentBrightnessLevel)
            {
                return;
            }

            if (_brightnessImage == null)
            {
                // Generate brightness image
                float[][] matrixItems ={
                    new float[] { HoverBrightnessLevel, 0, 0, 0, 0},
                    new float[] {0, HoverBrightnessLevel, 0, 0, 0},
                    new float[] {0, 0, HoverBrightnessLevel, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };
                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                Bitmap bmp = new Bitmap(_originalBackgroundImage.Width, _originalBackgroundImage.Height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(_originalBackgroundImage, new Rectangle(0, 0, bmp.Width, bmp.Height),
                        0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }

                _brightnessImage = bmp;
            }
            BackgroundImage = _brightnessImage;
            _currentBrightnessLevel = HoverBrightnessLevel;
        }

        public void DisableTile()
        {
            // Removing mouse events
            MouseEnter -= VisualTile_MouseEnter;
            MouseLeave -= VisualTile_MouseLeave;
            MouseDown -= TileButton_MouseDown;
            MouseUp -= TileButton_MouseUp;
            Draggable.SetDraggable(false);
        }

        public void VisualTile_MouseEnter(object sender, EventArgs e)
        {
            ApplyBrightness();
        }

        public void VisualTile_MouseLeave(object sender, EventArgs e)
        {
            RemoveBrightness();
        }

        public void TileButton_MouseDown(object sender, MouseEventArgs e)
        {
            VisualTile currTile = (VisualTile)sender;
            currTile.BringToFront();

            // Update the status of the old slot to Available
            if (currTile.SlotLocation[0] != Constants.DroppedTileLocation
                && currTile.SlotLocation[1] != Constants.DroppedTileLocation)
                GameContext.HumanPlayer.board.BoardSlots[currTile.SlotLocation[0], currTile.SlotLocation[1]].SlotState = Constants.Available;
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
                currTile.SlotLocation = new int[] { Constants.DroppedTileLocation, Constants.DroppedTileLocation };
                currTile.Draggable.SetDraggable(false);

                // Adding tile to dropped tiles stack
                GameContext.DroppedTilesStack.Push(currTile);
                GameContext.HumanPlayer.board.TileButtons.Remove(currTile);

                // Updating Board
                GameContext.HumanPlayer.board.TookCard = false;
                GameContext.CurrentTurn = Constants.ComputerPlayerTurn;
                RummikubGameView.GlobalGameIndicatorLbl.Text = "Computer's Turn";
                GameContext.ComputerPlayer.ComputerPlay(currTile.TileData);
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

                if (currTile.SlotLocation[0] == Constants.DroppedTileLocation && currTile.SlotLocation[1] == Constants.DroppedTileLocation)
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
                    currTile.SlotLocation = new int[] { min_i, min_j };
                }
                else
                {
                    ControlTransition.Move(currTile, currTile.Location, firstEmptySlot.Location);
                    GameContext.HumanPlayer.board.BoardSlots[currTile.SlotLocation[0], currTile.SlotLocation[1]].SlotState = Constants.Available;
                    GameContext.HumanPlayer.board.BoardSlots[min_i, min_j].SlotState = Constants.Allocated;
                    currTile.SlotLocation = new int[] { min_i, min_j };
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
