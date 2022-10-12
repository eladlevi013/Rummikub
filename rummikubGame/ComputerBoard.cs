﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class ComputerBoard : Board
    {
        private List<Tile> hand;
        private List<List<Tile>> sequences;
        private List<Button> drawn_computer_cards;

        public int getHandTilesNumber()
        {
            return GameTable.RUMMIKUB_TILES_IN_GAME - GameTable.computer_player.getNumberOfTilesInAllSets(sequences);
        }

        public bool checkWinner()
        {
            // first we'll find the number of tiles in hand
            int hand_counter = 0;
            for(int hand_index =0; hand_index < hand.Count(); hand_index++)
            {
                if (hand[hand_index] != null)
                    hand_counter += 1;
            }
            if (hand_counter == 0)
                return true;
            return false;
        }

        public void setHand(List<Tile> updated_hand)
        {
            this.hand = updated_hand;
        }

        public void setSequences(List<List<Tile>> updated_sequences)
        {
            this.sequences = updated_sequences;
        }

        public ComputerBoard(List<Tile> hand, List<List<Tile>> sequences)
        {
            this.hand = hand;
            this.sequences = sequences;
            drawn_computer_cards = new List<Button>();
            if(GameTable.global_view_computer_tiles_groupbox.Checked == true)
                generateBoard();
        }

        public void generateBoard()
        {
            int starting_x_computer_tiles = 50;
            int starting_y_computer_tiles = 80;

            for (int i = 0; i < hand.Count(); i++)
            {
                Point tile_location = new Point(starting_x_computer_tiles, starting_y_computer_tiles);
                if (hand[i] != null)
                {
                    drawSingleComputerCard(hand[i], tile_location);
                    starting_x_computer_tiles += 40;
                }
            }

            starting_x_computer_tiles = 50;
            starting_y_computer_tiles = 170;
            if (sequences != null)
            {
                for (int i = 0; i < sequences.Count(); i++)
                {
                    for (int j = 0; j < sequences[i].Count(); j++)
                    {
                        Point tile_location = new Point(starting_x_computer_tiles, starting_y_computer_tiles);
                        drawSingleComputerCard(sequences[i][j], tile_location);
                        starting_x_computer_tiles += 40;
                    }

                    if (i == 2)
                    {
                        starting_x_computer_tiles = 250;
                        starting_y_computer_tiles = 120;
                    }
                    else
                    {
                        starting_x_computer_tiles = 50;
                    }
                    starting_y_computer_tiles += 50;
                }
            }
        }

        public void drawSingleComputerCard(Tile tile, Point point)
        {
            Button tileButton = new Button();
            tileButton.Size = new Size(35, 40);
            tileButton.BackgroundImage = Image.FromFile("Tile.png");
            tileButton.BackgroundImageLayout = ImageLayout.Stretch;
            tileButton.Draggable(true); // usage of the extension
            tileButton.FlatStyle = FlatStyle.Flat;
            tileButton.FlatAppearance.BorderSize = 0;
            tileButton.Text = tile.getNumber().ToString();
            tileButton.Location = point;

            if (tile.getColor() == 0)
                tileButton.ForeColor = (Color.Blue);
            else if (tile.getColor() == 1)
                tileButton.ForeColor = (Color.Black);
            else if (tile.getColor() == 2)
                tileButton.ForeColor = (Color.Yellow);
            else
                tileButton.ForeColor = (Color.Red);

            tileButton.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            drawn_computer_cards.Add(tileButton);
            GameTable.global_gametable_context.Controls.Add(drawn_computer_cards[drawn_computer_cards.Count() - 1]);
            tileButton.BringToFront();
        }

        public void deleteCards()
        {
            for (int i = 0; (drawn_computer_cards.Count != 0) && i < drawn_computer_cards.Count(); i++)
                GameTable.global_gametable_context.Controls.Remove(drawn_computer_cards[i]);
        }
    }
}