using Rummikub;
using rummikubGame.Models;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class ComputerBoard : IBoard
    {
        /*
            This function is used on the ComputerPlayer class,
            in order to serve board related function to the computer player.
            in this class some main function such as:
            - GetHandTilesNumber
            - 
        */

        // Board Constants
        const int StartingHandXLocationComputerTiles = 50;
        const int StartingHandYLocationComputerTiles = 95;
        const int StartingSequencesXLocationComputerTiles = 50;
        const int StartingSequencesYLocationComputerTiles = 185;
        const int SecondSequencesXLocationComputerTiles = 300;
        const int SecondSequencesYLocationComputerTiles = 170;
        const int XSpaceBetweenComputerTiles = 31;
        const int YSpaceBetweenSequences = 50;

        // Board Variables
        public List<Tile> hand;
        public List<PartialSet> partialSets;
        public List<List<Tile>> sequences;
        public List<Tile> unusedJokers;

        [NonSerialized]
        public List<Label> drawnComputerCards;

        public int GetHandTilesNumber()
        {
            return Constants.RummikubTilesInGame 
                - GameContext.ComputerPlayer.board.GetNumberOfTilesInAllSets(sequences);
        }

        public ComputerBoard()
        {
            unusedJokers = new List<Tile>();
            partialSets = new List<PartialSet>();
            drawnComputerCards = new List<Label>();
            hand = new List<Tile>();

            // generating starting tiles
            for (int i = 0; i < Constants.RummikubTilesInGame; i++)
            {
                Tile tile = GameContext.Pool.GetTile();

                if (tile.Number == 0)
                    unusedJokers.Add(tile);
                else
                    hand.Add(tile);
            }
        }

        public List<Tile> GetAllJokers()
        {
            List<Tile> jokers = new List<Tile>(unusedJokers);

            // adding hand to all_tiles
            for (int j = 0; j < hand.Count(); j++)
            {
                if (hand[j] != null && hand[j].Number == 0)
                    jokers.Add(hand[j]);
            }
            // adding sequences to all_tiles
            for (int j = 0; j < sequences.Count(); j++)
            {
                for (int k = 0; k < sequences[j].Count(); k++)
                {
                    if (sequences[j][k].Number == 0)
                        jokers.Add(sequences[j][k]);
                }
            }
            // adding partial sets to all_tiles
            for (int j = 0; j < partialSets.Count(); j++)
            {
                if (partialSets[j].Tile1.Number == 0)
                    jokers.Add((Tile)partialSets[j].Tile1);

                if (partialSets[j].Tile2.Number == 0)
                    jokers.Add((Tile)partialSets[j].Tile2);
            }

            return jokers;
        }

        public void GetAllTiles(ref List<Tile> allTiles, ref List<Tile> jokers)
        {
            jokers = new List<Tile>(unusedJokers);

            // adding hand to all_tiles
            for (int j = 0; j < hand.Count(); j++)
            {
                if (hand[j] != null && hand[j].Number != 0)
                    allTiles.Add(hand[j]);
                else
                    jokers.Add(hand[j]);
            }
            // adding sequences to all_tiles
            for (int j = 0; j < sequences.Count(); j++)
            {
                for (int k = 0; k < sequences[j].Count(); k++)
                {
                    if (sequences[j][k].Number != 0)
                        allTiles.Add(sequences[j][k]);
                    else
                        jokers.Add(sequences[j][k]);
                }
            }
            // adding partial sets to all_tiles
            for (int j = 0; j < partialSets.Count(); j++)
            {
                if (partialSets[j].Tile1.Number != 0)
                    allTiles.Add((Tile)partialSets[j].Tile1);
                else
                    jokers.Add((Tile)partialSets[j].Tile1);

                if (partialSets[j].Tile2.Number != 0)
                    allTiles.Add((Tile)partialSets[j].Tile2);
                else
                    jokers.Add((Tile)partialSets[j].Tile2);
            }
        }

        public int CountJokers()
        {
            // count number of jokers in sequences
            int jokers_in_sequences = unusedJokers.Count();
            for (int i = 0; i < sequences.Count(); i++)
                for (int j = 0; j < sequences[i].Count(); j++)
                    if (sequences[i][j].Number == 0)
                        jokers_in_sequences++;

            return jokers_in_sequences;
        }

        public void GenerateBoard()
        {
            GenerateTilesToBoard();
        }

        public bool CheckWinner()
        {
            if (hand.Count() + 2 * partialSets.Count() == 0)
                return true;
            return false;
        }

        public void ClearBoard()
        {   
            // deletes the visibility of the computer tiles
            for (int i = 0; (drawnComputerCards.Count != 0) && i < drawnComputerCards.Count(); i++)
                RummikubGameView.GlobalRummikubGameViewContext.Controls.Remove(drawnComputerCards[i]);
        }

        public void UpdatingUnusedJokers(ref List<Tile> jokers, ref List<List<Tile>> sequences)
        {
            List<Tile> tiles_to_remove = new List<Tile>();

            for (int i = 0; i < jokers.Count(); i++)
            {
                for (int j = 0; j < sequences.Count(); j++)
                {
                    for (int k = 0; k < sequences[j].Count(); k++)
                    {
                        // if the joker is in the sequence - add it to the values list
                        if (jokers[i].Number == sequences[j][k].Number
                            && jokers[i].Color == sequences[j][k].Color)
                        {
                            tiles_to_remove.Add(jokers[i]);
                        }
                    }
                }
            }

            // removing used jokers
            for (int i = 0; i < tiles_to_remove.Count; i++)
                jokers.Remove(tiles_to_remove[i]);
        }

        public int GetNumberOfTilesInAllSets(List<List<Tile>> sequences)
        {
            int sum = 0;
            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                    sum += sequences[i].Count();
            }
            return sum;
        }

        public void GenerateTilesToBoard()
        {
            int xLocationDrawingPoint = StartingHandXLocationComputerTiles;
            int yLocationDrawingPoint = StartingHandYLocationComputerTiles;

            // draws the hand tiles
            for (int i = 0; i < hand.Count(); i++)
            {
                Point tile_location = new Point(xLocationDrawingPoint, yLocationDrawingPoint);
                GenerateSingleTile(hand[i], tile_location);
                xLocationDrawingPoint += XSpaceBetweenComputerTiles;
            }

            // draws the sequences tiles
            xLocationDrawingPoint = StartingSequencesXLocationComputerTiles;
            yLocationDrawingPoint = StartingSequencesYLocationComputerTiles;

            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    for (int j = 0; j < sequences[i].Count(); j++)
                    {
                        Point tile_location = new Point(xLocationDrawingPoint, yLocationDrawingPoint);
                        GenerateSingleTile(sequences[i][j], tile_location);
                        xLocationDrawingPoint += XSpaceBetweenComputerTiles;
                    }

                    // if there are alot of sequences continue drawing in another area
                    if (i >= 2)
                    {
                        if (i == 2)
                        {
                            // if there are alot of sequences continue drawing in another area
                            xLocationDrawingPoint = SecondSequencesXLocationComputerTiles;
                            yLocationDrawingPoint = SecondSequencesYLocationComputerTiles - YSpaceBetweenSequences;
                        }
                        else
                        {
                            xLocationDrawingPoint = SecondSequencesXLocationComputerTiles;
                        }
                    }
                    else
                    {
                        // in any other situation start at the next line
                        xLocationDrawingPoint = StartingHandXLocationComputerTiles;
                    }
                    yLocationDrawingPoint += YSpaceBetweenSequences;
                }
            }

            // draw the partial sets
            xLocationDrawingPoint = StartingHandXLocationComputerTiles + 400;
            yLocationDrawingPoint = StartingHandYLocationComputerTiles;

            if (partialSets != null)
            {
                for (int i = 0; i < partialSets.Count(); i++)
                {
                    Point tile_location1 = new Point(xLocationDrawingPoint, yLocationDrawingPoint);
                    GenerateSingleTile(partialSets[i].Tile1, tile_location1);

                    xLocationDrawingPoint += XSpaceBetweenComputerTiles;

                    Point tile_location2 = new Point(xLocationDrawingPoint, yLocationDrawingPoint);
                    GenerateSingleTile(partialSets[i].Tile2, tile_location2);

                    yLocationDrawingPoint += YSpaceBetweenSequences;
                    xLocationDrawingPoint = StartingHandXLocationComputerTiles + 400;

                    if (i == 3)
                    {
                        xLocationDrawingPoint = StartingHandXLocationComputerTiles + 495;
                        yLocationDrawingPoint = StartingHandYLocationComputerTiles;
                    }
                }
            }

            xLocationDrawingPoint = StartingHandXLocationComputerTiles + 495 + 40;
            yLocationDrawingPoint = StartingHandYLocationComputerTiles;

            if (unusedJokers != null)
            {
                for (int i = 0; i < unusedJokers.Count(); i++)
                {
                    Point tile_location = new Point(xLocationDrawingPoint, yLocationDrawingPoint);
                    GenerateSingleTile(unusedJokers[i], tile_location);
                    xLocationDrawingPoint += XSpaceBetweenComputerTiles;
                }
            }
        }

        public void GenerateSingleTile(Tile tile, Point point)
        {
            Label tilePictureBox = new Label();
            tilePictureBox.Size = new Size(27, 40);
            tilePictureBox.Location = point;
            tilePictureBox.BackColor = Color.FromArgb(247, 211, 184);
            tilePictureBox.TextAlign = ContentAlignment.MiddleCenter;
            tilePictureBox.Text = tile.Number.ToString();
            tilePictureBox.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);

            if (tile.Color == 0) tilePictureBox.ForeColor = (Color.Blue);
            else if (tile.Color == 1) tilePictureBox.ForeColor = (Color.Black);
            else if (tile.Color == 2) tilePictureBox.ForeColor = (Color.Yellow);
            else tilePictureBox.ForeColor = (Color.Red);

            if (tile.Number == 0)
            {
                tilePictureBox.Text = "J";
                tilePictureBox.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            }

            drawnComputerCards.Add(tilePictureBox);
            RummikubGameView.GlobalRummikubGameViewContext.Controls.Add(drawnComputerCards[drawnComputerCards.Count() - 1]);
            tilePictureBox.BringToFront(); ;
        }
    }
}
