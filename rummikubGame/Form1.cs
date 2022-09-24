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
        static int TAG_NUMBER = 0;
        Pool pool;

        const int STARTING_X_LOCATION = 70;
        const int STARTING_Y_LOCATION = 345;

        const int X_SPACE_BETWEEN_TileButtonS = 85;
        const int Y_SPACE_BETWEEN_TileButtonS = 115;

        const int DROPPED_CARD_LOCATION = -1;

        private Slot[,] TileButton_slot; 
        private Dictionary<int, TileButton> TileButtons;

        public Form1()
        {
            InitializeComponent();
        }

        private float getDistance(Button moving_card, Button empty_slot)
        {
            // used to find the closet slot to a card
            return (float)Math.Sqrt(Math.Pow(moving_card.Location.X - empty_slot.Location.X, 2) + Math.Pow(moving_card.Location.Y - empty_slot.Location.Y, 2));
        }

        private void TileButton_MouseDown(object sender, MouseEventArgs e)
        {
            // focused TileButton, will be always at top
            ((Button)sender).BringToFront();

            // we are iterating over the cards, because there is the option that the card in the dropped tiles
            for(int i=0; i<TileButtons.Keys.Count; i++)
            {
                // it means that the card is in the tiles group that you can interact with
                if(((Button)sender).Tag == TileButtons[TileButtons.Keys.ElementAt(i)].getTileButton().Tag)
                {
                    // we are turning it off, because we want to be able to place to the current place if its the closest location
                    TileButton_slot[TileButtons[(int)((Button)sender).Tag].getLocation()[0], TileButtons[(int)((Button)sender).Tag].getLocation()[1]].changeState(false);
                }
            }
        }

        private void TileButton_MouseEnter(object sender, EventArgs e)
        {
            // bright effect when the mouse hovers over the tile
            ((Button)sender).BackgroundImage = Image.FromFile("bright_tile.png");
        }

        private void TileButton_MouseLeave(object sender, EventArgs e)
        {
            // in order to make the tile normal after hovering over the card
            ((Button)sender).BackgroundImage = Image.FromFile("tile.png");
        }

        private void TileButton_MouseUp(object sender, MouseEventArgs e)
        {
            // the card that we dragged with the mouse
            Button current_card = (Button)sender;

            // if the card is in our board
            if (TileButtons.ContainsKey((int)current_card.Tag))
            {
                // first, we would like to check if the user wanted to put the TileButton on the drop_TileButton location
                if (getDistance(current_card, dropped_tiles_btn) < 100)
                {
                    current_card.Location = new Point(dropped_tiles_btn.Location.X + 10, dropped_tiles_btn.Location.Y + 18);
                    current_card.Draggable(false);
                    int[] current_location = {DROPPED_CARD_LOCATION, DROPPED_CARD_LOCATION};
                    TileButtons[(int)current_card.Tag].setLocation(current_location);
                    TileButtons.Remove((int)current_card.Tag);
                }
                // otherwise we would like to search the first empty slot to put in the TileButton
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
                            if (TileButton_slot[i, j].getState() == false)
                            {
                                first_empty_slot = TileButton_slot[i, j].getSlotButton();
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
                            if (getDistance(current_card, TileButton_slot[i, j].getSlotButton()) < min_distance && TileButton_slot[i, j].getState() == false)
                            {
                                min_distance = getDistance(current_card, TileButton_slot[i, j].getSlotButton());
                                min_i = i;
                                min_j = j;
                            }
                        }
                    }

                    // we made it empty, as we clicked_down to move the card, so now we have to make it non-empty again
                    TileButton_slot[TileButtons[(int)((Button)sender).Tag].getLocation()[0], TileButtons[(int)((Button)sender).Tag].getLocation()[1]].changeState(true);

                    // update the location of the focused TileButton, to the location of the minimum distance that we found earlier
                    current_card.Location = TileButton_slot[min_i, min_j].getSlotButton().Location;

                    // now we need to update the status of the old slot to empty
                    TileButton_slot[TileButtons[(int)current_card.Tag].getLocation()[0], TileButtons[(int)current_card.Tag].getLocation()[1]].changeState(false);

                    // we'll change the status of the 'minimum-distance' slot(now contains the card)
                    TileButton_slot[min_i, min_j].changeState(true);

                    // update the TileButton location(the location of the slot)
                    int[] updated_TileButton_location = { min_i, min_j };
                    TileButtons[(int)current_card.Tag].setLocation(updated_TileButton_location);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Generate pool cards
            pool = new Pool();

            // change the style of the drop_TileButtons_location
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

            // set button flat design
            SortByColor_btn.FlatStyle = FlatStyle.Flat;
            SortByColor_btn.FlatAppearance.BorderSize = 0;
            SortByValue_btn.FlatStyle = FlatStyle.Flat;
            SortByValue_btn.FlatAppearance.BorderSize = 0;

            Random rnd = new Random();

            // Generating the slots
            int x_location = STARTING_X_LOCATION;
            int y_location = STARTING_Y_LOCATION;
            TileButton_slot = new Slot[2, 10];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    TileButton_slot[i, j] = new Slot();
                    TileButton_slot[i, j].getSlotButton().BackgroundImage = Image.FromFile("slot.png");
                    TileButton_slot[i, j].getSlotButton().BackgroundImageLayout = ImageLayout.Stretch;
                    TileButton_slot[i, j].getSlotButton().FlatStyle = FlatStyle.Flat;
                    TileButton_slot[i, j].getSlotButton().FlatAppearance.BorderSize = 0;
                    TileButton_slot[i, j].getSlotButton().Size = new Size(75, 100);
                    TileButton_slot[i, j].getSlotButton().Location = new Point(x_location, y_location);
                    TileButton_slot[i, j].changeState(false); // slot is available
                    Controls.Add(TileButton_slot[i, j].getSlotButton());
                    x_location += X_SPACE_BETWEEN_TileButtonS;
                }
                y_location += Y_SPACE_BETWEEN_TileButtonS;
                x_location = STARTING_X_LOCATION;
            }

            // Generating the TileButtons
            x_location = STARTING_X_LOCATION;
            y_location = STARTING_Y_LOCATION;
            TileButtons = new Dictionary<int, TileButton>();

            for (int i = 0; i < 14; i++)
            {
                // change the slots current state to 'not-empty'
                if (i < 14) {
                    TileButton_slot[i / 10, i % 10].changeState(true);
                }

                int[] start_location = {i/10, i%10};
                Tile current_tile_from_pool = pool.getTile();
                TileButtons[TAG_NUMBER] = (new TileButton(current_tile_from_pool.getColor(), current_tile_from_pool.getNumber(), start_location));
                TileButtons[TAG_NUMBER].getTileButton().Size = new Size(75, 100);
                TileButtons[TAG_NUMBER].getTileButton().Location = new Point(x_location, y_location);
                TileButtons[TAG_NUMBER].getTileButton().BackgroundImage = Image.FromFile("Tile.png");
                TileButtons[TAG_NUMBER].getTileButton().BackgroundImageLayout = ImageLayout.Stretch;
                TileButtons[TAG_NUMBER].getTileButton().Draggable(true); // usage of the extension
                TileButtons[TAG_NUMBER].getTileButton().FlatStyle = FlatStyle.Flat;
                TileButtons[TAG_NUMBER].getTileButton().FlatAppearance.BorderSize = 0;
                TileButtons[TAG_NUMBER].getTileButton().Text = current_tile_from_pool.getNumber().ToString();

                if (TileButtons[TAG_NUMBER].getColor() == 0)
                    TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Blue);
                else if (TileButtons[TAG_NUMBER].getColor() == 1)
                    TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Black);
                else if (TileButtons[TAG_NUMBER].getColor() == 2)
                    TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Yellow);
                else
                    TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Red);

                TileButtons[TAG_NUMBER].getTileButton().Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
                TileButtons[TAG_NUMBER].getTileButton().MouseUp += new MouseEventHandler(this.TileButton_MouseUp);
                TileButtons[TAG_NUMBER].getTileButton().MouseDown += new MouseEventHandler(this.TileButton_MouseDown);
                TileButtons[TAG_NUMBER].getTileButton().MouseEnter += TileButton_MouseEnter;
                TileButtons[TAG_NUMBER].getTileButton().MouseLeave += TileButton_MouseLeave;

                Controls.Add(TileButtons[TAG_NUMBER].getTileButton());
                TileButtons[TAG_NUMBER].getTileButton().BringToFront();
                TileButtons[TAG_NUMBER].getTileButton().Tag = TAG_NUMBER;

                x_location += X_SPACE_BETWEEN_TileButtonS;
                if (i == 9) { y_location += Y_SPACE_BETWEEN_TileButtonS; x_location = STARTING_X_LOCATION; }
                TAG_NUMBER++;
            }

            // this will send back the panel(the board)
            board.SendToBack();

            // updates the current tiles in the queue
            current_pool_size.Text = pool.getPoolSize() + " tiles in pool";
        }

        private void pool_btn_Click(object sender, EventArgs e)
        {
            bool found_last_empty_location = false;
            for (int i = 1; i >= 0 && !found_last_empty_location; i--)
            {
                for (int j = 9; j >= 0 && !found_last_empty_location; j--)
                {
                    if (TileButton_slot[i, j].getState() == false)
                    {
                        Tile tile_from_pool = pool.getTile();
                        int[] location_arr = { i, j };
                        TileButton new_tile = new TileButton(tile_from_pool.getColor(), tile_from_pool.getNumber(), location_arr);
                        new_tile.getTileButton().Tag = TAG_NUMBER;
                        found_last_empty_location = true;
                        TileButtons[TAG_NUMBER] = new_tile;

                        TileButtons[TAG_NUMBER].getTileButton().Size = new Size(75, 100);
                        TileButtons[TAG_NUMBER].getTileButton().Location = TileButton_slot[i, j].getSlotButton().Location;
                        TileButtons[TAG_NUMBER].getTileButton().BackgroundImage = Image.FromFile("Tile.png");
                        TileButtons[TAG_NUMBER].getTileButton().BackgroundImageLayout = ImageLayout.Stretch;
                        TileButtons[TAG_NUMBER].getTileButton().Draggable(true); // usage of the extension
                        TileButtons[TAG_NUMBER].getTileButton().FlatStyle = FlatStyle.Flat;
                        TileButtons[TAG_NUMBER].getTileButton().FlatAppearance.BorderSize = 0;
                        TileButtons[TAG_NUMBER].getTileButton().Text = new_tile.getNumber().ToString();
                        TileButtons[TAG_NUMBER].getTileButton().MouseEnter += TileButton_MouseEnter;
                        TileButtons[TAG_NUMBER].getTileButton().MouseLeave += TileButton_MouseLeave;

                        if (TileButtons[TAG_NUMBER].getColor() == 0)
                            TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Blue);
                        else if (TileButtons[TAG_NUMBER].getColor() == 1)
                            TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Black);
                        else if (TileButtons[TAG_NUMBER].getColor() == 2)
                            TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Yellow);
                        else
                            TileButtons[TAG_NUMBER].getTileButton().ForeColor = (Color.Red);

                        TileButtons[TAG_NUMBER].getTileButton().Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
                        TileButtons[TAG_NUMBER].getTileButton().MouseUp += new MouseEventHandler(this.TileButton_MouseUp);
                        TileButtons[TAG_NUMBER].getTileButton().MouseDown += new MouseEventHandler(this.TileButton_MouseDown);
                        Controls.Add(TileButtons[TAG_NUMBER].getTileButton());
                        TileButtons[TAG_NUMBER].getTileButton().BringToFront();
                        TileButtons[TAG_NUMBER].getTileButton().Tag = TAG_NUMBER;
                        TileButton_slot[TileButtons[TAG_NUMBER].getLocation()[0], TileButtons[TAG_NUMBER].getLocation()[1]].changeState(true);

                        // updates the current tiles in the queue
                        current_pool_size.Text = pool.getPoolSize() + " tiles in pool";
                        TAG_NUMBER++;
                    }

                }
            }
             
            string test = "";
            for (int i = 0; i < TileButtons.Keys.Count; i++)
            {
                test += "index: " + TileButtons.Keys.ElementAt(i) + ", " + TileButtons[TileButtons.Keys.ElementAt(i)].ToString() + "\n";
            }
            data_indicator_2.Text = test;

            string test1 = "";
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    test1 += "[" + i + ", " + j + "]: " + TileButton_slot[i, j].ToString() + "\n";
                }
            }
            data_indicator.Text = test1;
            
        }

        private void ArrangeCardsOnBoard(List<TileButton> sorted_cards)
        {
            int card_index = 0;
            for(int i=0; i<2; i++)
            {
                for(int j=0; j<10; j++)
                {
                    TileButton_slot[i, j].changeState(false);
                    if (card_index < sorted_cards.Count())
                    {
                        sorted_cards[card_index].getTileButton().Location = TileButton_slot[i, j].getSlotButton().Location;
                        TileButton_slot[i, j].changeState(true);
                        int[] location_arr = { i, j };
                        sorted_cards[card_index].setLocation(location_arr);
                    }
                    card_index++;
                }
            }
        }

        private void SortByValue_btn_Click(object sender, EventArgs e)
        {
            List<TileButton> sorted_cards = TileButtons.Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.getNumber()).ToList();
            ArrangeCardsOnBoard(sorted_cards);
        }

        private void SortByColor_btn_Click(object sender, EventArgs e)
        {
            List<TileButton> sorted_cards = TileButtons.Values.ToList();
            sorted_cards = sorted_cards.OrderBy(card => card.getColor()).ToList();
            ArrangeCardsOnBoard(sorted_cards);
        }
    }
}