using Rummikub;
using rummikubGame.Exceptions;
using rummikubGame.Logic;
using rummikubGame.Models;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class HumanPlayerBoard : IBoard, ISerializable
    {
        // PlayerBoard Constants
        const int StartingXDrawingLocation = 70;
        const int StartingYDrawingLocation = 410;
        const int XSpaceBetweenVisualTileButtons = 85;
        const int YSpaceBetweenVisualTileButtons = 115;

        // board elements
        public Slot[,] _boardSlots = null;
        public List<VisualTile> _tileButtons = null;
        public bool _tookCard = false;

        public List<VisualTile> TileButtons
        {
            get { return _tileButtons; }
            set { _tileButtons = value; }
        }

        public Slot[,] BoardSlots
        {
            get { return _boardSlots; }
            set { _boardSlots = value; }
        }

        public bool TookCard
        {
            get { return _tookCard; }
            set { _tookCard = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TileButtons", ConvertVisualTileToSerealilzableFormat());
        }

        public HumanPlayerBoard(SerializationInfo info, StreamingContext context)
        {
            List<VisualTileData> tileButtonsData = (List<VisualTileData>)info.GetValue("TileButtons", typeof(List<VisualTileData>));
            TileButtons = new List<VisualTile>();

            // Building the tiles from the data
            for (int i = 0; i < tileButtonsData.Count(); i++)
            {
                TileButtons.Add(GenerateSingleTile(tileButtonsData[i].TileData, tileButtonsData[i].SlotLocation));
            }

            GenerateBoardSlots();

            // Arrange the tiles on the board
            for (int i = 0; i < TileButtons.Count; i++)
            {
                int[] tileSlotLocation = _tileButtons[i].VisualTileData.SlotLocation;
                BoardSlots[tileSlotLocation[0], tileSlotLocation[1]].SlotState = Constants.Allocated;
                _tileButtons[i].Location = BoardSlots[tileSlotLocation[0], tileSlotLocation[1]].SlotButton.Location;
            }
        }

        public HumanPlayerBoard()
        {
            GenerateBoard();
        }

        public List<VisualTileData> ConvertVisualTileToSerealilzableFormat()
        {
            List<VisualTileData> result = new List<VisualTileData>();

            for(int i = 0; i < TileButtons.Count(); i++)
            {
                result.Add(TileButtons[i].VisualTileData);
            }

            return result;
        }

        public void ArrangeCardsOnBoard(List<VisualTile> sortedCards)
        {
            int index = 0;

            for (int i = 0; i < RummikubGameView.HumanPlayerBoardHeight; i++)
            {
                for (int j = 0; j < RummikubGameView.HumanPlayerBoardWidth; j++)
                {
                    BoardSlots[i, j].SlotState = Constants.Available;
                    if (index < sortedCards.Count())
                    {
                        sortedCards[index].Location = BoardSlots[i, j].SlotButton.Location;
                        BoardSlots[i, j].SlotState = true;
                        int[] location_arr = { i, j };
                        sortedCards[index].VisualTileData.SlotLocation = location_arr;
                    }
                    index++;
                }
            }
        }

        public void GenerateBoard()
        {
            GenerateBoardSlots();
            GenerateTilesToBoard();
        }

        public void GenerateBoardSlots()
        {
            int x_location = StartingXDrawingLocation;
            int y_location = StartingYDrawingLocation;
            BoardSlots = new Slot[RummikubGameView.HumanPlayerBoardHeight, RummikubGameView.HumanPlayerBoardWidth];

            // Generating the slots
            for (int i = 0; i < RummikubGameView.HumanPlayerBoardHeight; i++)
            {
                for (int j = 0; j < RummikubGameView.HumanPlayerBoardWidth; j++)
                {
                    // Generate a single slotButton
                    BoardSlots[i, j] = new Slot();
                    try
                    {
                        BoardSlots[i, j].SlotButton.BackgroundImage = Image.FromFile(RummikubGameView.SlotPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        (RummikubGameView.GlobalRummikubGameViewContext).Close();
                    }
                    BoardSlots[i, j].SlotButton.BackgroundImageLayout = ImageLayout.Stretch;
                    BoardSlots[i, j].SlotButton.FlatStyle = FlatStyle.Flat;
                    BoardSlots[i, j].SlotButton.FlatAppearance.BorderSize = 0;
                    BoardSlots[i, j].SlotButton.Size = new Size(RummikubGameView.TileWidth, RummikubGameView.TileHeight);
                    BoardSlots[i, j].SlotButton.Location = new Point(x_location, y_location);
                    BoardSlots[i, j].SlotState = Constants.Available;
                    RummikubGameView.GlobalRummikubGameViewContext.Controls.Add(BoardSlots[i, j].SlotButton);
                    x_location += XSpaceBetweenVisualTileButtons;
                }
                y_location += YSpaceBetweenVisualTileButtons;
                x_location = StartingXDrawingLocation;
            }
        }

        public void GenerateTilesToBoard()
        {
            TileButtons = new List<VisualTile>();

            // Generating the TileButtons
            for (int i = 0; i < Constants.RummikubTilesInGame; i++)
            {
                int[] start_location = { i / 10, i % 10 };
                GenerateSingleTileToBoard(start_location, false);
                BoardSlots[i / 10, i % 10].SlotState = Constants.Allocated;
            }
        }

        public void GenerateSingleTileToBoard(int[] slotLocation, bool animation)
        {
            try
            {
                Tile poolTile = GameContext.Pool.GetTile();
                VisualTile visualTile = GenerateSingleTile(poolTile, slotLocation);

                // Adding tile to board
                TileButtons.Add(visualTile);
                BoardSlots[visualTile.VisualTileData.SlotLocation[0], visualTile.VisualTileData.SlotLocation[1]].SlotState = Constants.Allocated;

                // Moving tile to slot location
                if (animation)
                    ControlTransition.Move(visualTile
                        , RummikubGameView.GlobalPoolBtn.Location, BoardSlots[slotLocation[0]
                        , slotLocation[1]].SlotButton.Location);
                else
                    visualTile.Location = BoardSlots[slotLocation[0], slotLocation[1]].SlotButton.Location;
            }
            catch (EmptyPoolException)
            {
                return;
            }
        }

        public VisualTile GenerateSingleTile(Tile tile, int[] slotLocation)
        {
            VisualTile visualTile = new VisualTile(tile.Color, tile.Number, slotLocation);

            // Setting the tile's design
            visualTile.Size = new Size(75, 100);
            visualTile.BackgroundImageLayout = ImageLayout.Stretch;
            visualTile.FlatStyle = FlatStyle.Flat;
            visualTile.FlatAppearance.BorderSize = 0;
            visualTile.Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
            visualTile.Draggable.SetDraggable(true);

            if (GameLogic.IsJoker(tile))
            {
                if (tile.Color == Constants.BlackColor)
                {
                    try
                    {
                        visualTile.BackgroundImage = Image.FromFile(RummikubGameView.BlackJokerPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        (RummikubGameView.GlobalRummikubGameViewContext).Close();
                    }
                }
                else
                {
                    try
                    {
                        visualTile.BackgroundImage = Image.FromFile(RummikubGameView.RedJokerPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                        (RummikubGameView.GlobalRummikubGameViewContext).Close();
                    }
                }
            }
            else
            {
                try
                {
                    visualTile.BackgroundImage = Image.FromFile(RummikubGameView.TilePath);
                    visualTile.Text = tile.Number.ToString();

                    if (tile.Color == 0) visualTile.ForeColor = (Color.Blue);
                    else if (tile.Color == 1) visualTile.ForeColor = (Color.Black);
                    else if (tile.Color == 2) visualTile.ForeColor = (Color.Yellow);
                    else visualTile.ForeColor = (Color.Red);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    (RummikubGameView.GlobalRummikubGameViewContext).Close();
                }
            }

            RummikubGameView.GlobalRummikubGameViewContext.Controls.Add(visualTile);
            visualTile.BringToFront();

            return visualTile;
        }

        public void ClearBoard()
        {
            // Iterating over the tiles and removing them from the board
            for (int i = 0; i < TileButtons.Count(); i++)
            {
                RummikubGameView.GlobalRummikubGameViewContext.Controls.Remove(TileButtons[i]);
            }
        }

        public void DisableBoard()
        {
            for (int i = 0; i < TileButtons.Count(); i++)
            {
                TileButtons[i].DisableTile();
            }
        }

        public  void hint()
        {
            List<Tile> all_tiles = new List<Tile>();
            List<Tile> jokers = new List<Tile>();

            // separating the jokers from the tiles
            for(int i=0; i<_tileButtons.Count; i++)
            {
                if (_tileButtons[i].VisualTileData.TileData.Number == 0)
                {
                    jokers.Add(_tileButtons[i].VisualTileData.TileData);
                }
                else
                {
                    all_tiles.Add(_tileButtons[i].VisualTileData.TileData);
                }
            }

            // get sequences and partial sets
            List<List<Tile>> sequences = GameLogic.MeldsSets(ref all_tiles, ref jokers);
            List<PartialSet> parial_sets = GameLogic.CreatePartialSets(ref all_tiles);

            // find the not nessesary tiles and highlight it
            if(all_tiles.Count > 0)
            {
                for (int i = 0; i < _tileButtons.Count; i++)
                {
                    if (_tileButtons[i].VisualTileData.TileData == all_tiles[0])
                    {
                        _tileButtons[i].BrightnessOnHover.BrightnessTime(3);
                    }
                }
            }
        }
    }
}
