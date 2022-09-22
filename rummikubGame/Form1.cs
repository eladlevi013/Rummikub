using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public partial class Form1 : Form
    {
        const int STARTING_X_LOCATION = 70;
        const int STARTING_Y_LOCATION = 345;

        const int X_SPACE_BETWEEN_TILES = 85;
        const int Y_SPACE_BETWEEN_TILES = 115;

        private Slot[,] tile_slot; 
        private List<Tile> tiles; 

        public Form1()
        {
            InitializeComponent();
        }

        private float getDistance(Button moving_card, Button empty_slot)
        {
            // used to find the closet slot to a card
            return (float)Math.Sqrt(Math.Pow(moving_card.Location.X - empty_slot.Location.X, 2) + Math.Pow(moving_card.Location.Y - empty_slot.Location.Y, 2));
        }

        private void tile_MouseDown(object sender, MouseEventArgs e)
        {
            // focused tile, will be always at top
            ((Button)sender).BringToFront();

            for(int i=0; i<tiles.Count; i++)
            {
                if(((Button)sender).Tag == tiles[i].getTileButton().Tag)
                {
                    // we are turning it off, because we want to be able to place to the current place if its the closest location
                    tile_slot[tiles[(int)((Button)sender).Tag].getLocation()[0], tiles[(int)((Button)sender).Tag].getLocation()[1]].changeState(false);
                }
            }
        }

        private void tile_MouseUp(object sender, MouseEventArgs e)
        {
            // the card that we dragged with the mouse
            Button current_card = (Button)sender;

            // first, we would like to check if the user wanted to put the tile on the drop_tile location
            if (getDistance(current_card, dropped_tiles_btn) < 100)
            {
                current_card.Location = new Point(dropped_tiles_btn.Location.X+10, dropped_tiles_btn.Location.Y+18);
                current_card.Draggable(false);
                int[] current_location = { -1, -1 };
                tiles[(int)current_card.Tag].setLocation(current_location);
                tiles.Remove(tiles[(int)current_card.Tag]);
            }
            // otherwise we would like to search the first empty slot to put in the tile
            else
            {
                // Now we'll search the first empty slot, so we would know what is the the most close 
                int min_i = 0;
                int min_j = 0;
                Button first_empty_slot = null;
                bool found_first_empty_slot = false;
                for (int i = 0; i < 2 && found_first_empty_slot == false; i++)
                {
                    for (int j = 0; j < 10 && found_first_empty_slot == false; j++)
                    {
                        if (tile_slot[i, j].getState() == false)
                        {
                            first_empty_slot = tile_slot[i, j].getSlotButton(); 
                            found_first_empty_slot = true;
                            min_i = i;
                            min_j = j;
                        }
                    }
                }

                // we'll calculate the distance between the card, and the first empty slot
                float min_distance = getDistance(current_card, first_empty_slot);

                // now, we'll calculate the distance between the card and all the empty slots, and we'll find the minimum
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        // if the distance is smaller, and the slot is available
                        if (getDistance(current_card, tile_slot[i, j].getSlotButton()) < min_distance && tile_slot[i, j].getState() == false)
                        {
                            min_distance = getDistance(current_card, tile_slot[i, j].getSlotButton());
                            min_i = i;
                            min_j = j;
                        }
                    }
                }

                // we made it empty, as we clicked_down to move the card, so now we have to make it non-empty again
                //tile_slot[tiles[(int)((Button)sender).Tag].getLocation()[0], tiles[(int)((Button)sender).Tag].getLocation()[1]].changeState(true);

                // update the location of the focused tile, to the location of the minimum distance that we found earlier
                current_card.Location = tile_slot[min_i, min_j].getSlotButton().Location;

                // now we need to update the status of the old slot to empty
                tile_slot[tiles[(int)current_card.Tag].getLocation()[0], tiles[(int)current_card.Tag].getLocation()[1]].changeState(false);

                // we'll change the status of the 'minimum-distance' slot(now contains the card)
                tile_slot[min_i, min_j].changeState(true);

                // update the tile location(the location of the slot)
                int[] updated_tile_location = { min_i, min_j };
                tiles[(int)current_card.Tag].setLocation(updated_tile_location);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // change the style of the drop_tiles_location
            dropped_tiles_btn.FlatStyle = FlatStyle.Flat;
            dropped_tiles_btn.FlatAppearance.BorderSize = 0;
            dropped_tiles_btn.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");

            // set round corners of the board
            board.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");

            // set background color
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");

            // set background color of pool btn
            pool_btn.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");
            pool_btn.FlatStyle = FlatStyle.Flat;
            pool_btn.FlatAppearance.BorderSize = 0;

            Random rnd = new Random();

            // Generating the slots
            int x_location = STARTING_X_LOCATION;
            int y_location = STARTING_Y_LOCATION;
            tile_slot = new Slot[2, 10];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tile_slot[i, j] = new Slot();
                    tile_slot[i, j].getSlotButton().BackgroundImage = Image.FromFile("slot.png");
                    tile_slot[i, j].getSlotButton().BackgroundImageLayout = ImageLayout.Stretch;
                    tile_slot[i, j].getSlotButton().FlatStyle = FlatStyle.Flat;
                    tile_slot[i, j].getSlotButton().FlatAppearance.BorderSize = 0;
                    tile_slot[i, j].getSlotButton().Size = new Size(75, 100);
                    tile_slot[i, j].getSlotButton().Location = new Point(x_location, y_location);
                    tile_slot[i, j].changeState(false); // slot is available
                    Controls.Add(tile_slot[i, j].getSlotButton());
                    x_location += X_SPACE_BETWEEN_TILES;
                }
                y_location += Y_SPACE_BETWEEN_TILES;
                x_location = STARTING_X_LOCATION;
            }

            // Generating the tiles
            x_location = STARTING_X_LOCATION;
            y_location = STARTING_Y_LOCATION;
            tiles = new List<Tile>();

            for (int i = 0; i < 14; i++)
            {
                // change the slots current state to 'not-empty'
                if (i < 14) {
                    tile_slot[i / 10, i % 10].changeState(true);
                }

                int[] start_location = {i/10, i%10};
                tiles.Add(new Tile(rnd.Next(4), rnd.Next(1, 14), start_location));
                tiles[i].getTileButton().Size = new Size(75, 100);
                tiles[i].getTileButton().Location = new Point(x_location, y_location);
                tiles[i].getTileButton().BackgroundImage = Image.FromFile("tile.png");
                tiles[i].getTileButton().BackgroundImageLayout = ImageLayout.Stretch;
                tiles[i].getTileButton().Draggable(true); // usage of the extension
                tiles[i].getTileButton().FlatStyle = FlatStyle.Flat;
                tiles[i].getTileButton().FlatAppearance.BorderSize = 0;
                tiles[i].getTileButton().Text = rnd.Next(1, 14).ToString();

                if (tiles[i].getColor() == 0)
                    tiles[i].getTileButton().ForeColor = (Color.Blue);
                else if (tiles[i].getColor() == 1)
                    tiles[i].getTileButton().ForeColor = (Color.Black);
                else if (tiles[i].getColor() == 2)
                    tiles[i].getTileButton().ForeColor = (Color.Yellow);
                else
                    tiles[i].getTileButton().ForeColor = (Color.Red);

                tiles[i].getTileButton().Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
                tiles[i].getTileButton().MouseUp += new MouseEventHandler(this.tile_MouseUp);
                tiles[i].getTileButton().MouseDown += new MouseEventHandler(this.tile_MouseDown);
                Controls.Add(tiles[i].getTileButton());
                tiles[i].getTileButton().BringToFront();
                tiles[i].getTileButton().Tag = i;

                x_location += X_SPACE_BETWEEN_TILES;
                if (i == 9) { y_location += Y_SPACE_BETWEEN_TILES; x_location = STARTING_X_LOCATION; }
            }

            // this will send back the panel(the board)
            board.SendToBack();
        }

        private Slot[,] GetTile_slot()
        {
            return tile_slot;
        }

        private void pool_btn_Click(object sender, EventArgs e)
        {
            bool found_last_empty_location = false;
            for (int i = 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = 9; j >= 0 && !found_last_empty_location; j--)
                {
                    if (tile_slot[i, j].getState() == false)
                    {
                        tile_slot[i, j].getSlotButton().Text = "EMPTY :)";
                        found_last_empty_location = true;
                    }

                }
            }

            string test = "";
            for (int i = 0; i < tiles.Count; i++)
            {
                test += tiles[i].ToString() + "\n";
            }
            data_indicator_2.Text = test;

            string test1 = "";
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    test1 += "[" + i + ", " + j + "]: " + tile_slot[i, j].ToString() + "\n";
                }
            }
            data_indicator.Text = test1;
        }
    }
}