using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class Board
    {
        public static int TAG_NUMBER = 0; // every card has tag that indicates the index on the dictionary
        private Slot[,] TileButton_slot;  // 2d array of the slots of the cards
        private Dictionary<int, TileButton> TileButtons; // dictionary of the tiles<index(tag), TileButton(class)>
        public static bool tookCard = false;

        // const values
        const int STARTING_X_LOCATION = 70;
        const int STARTING_Y_LOCATION = 345;
        const int X_SPACE_BETWEEN_TileButtonS = 85;
        const int Y_SPACE_BETWEEN_TileButtonS = 115;
        const int DROPPED_CARD_LOCATION = -1; // when the card is no longer in board

        public Slot[,] getTileButton_slot()
        {
            return TileButton_slot;
        }

        public Dictionary<int, TileButton> getTilesDictionary()
        {
            return TileButtons;
        }

        public Board()
        {
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
                    GameTable.GameTableContext.Controls.Add(TileButton_slot[i, j].getSlotButton());
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
                if (i < 14)
                {
                    TileButton_slot[i / 10, i % 10].changeState(true);
                }

                int[] start_location = { i / 10, i % 10 };
                Tile current_tile_from_pool = GameTable.pool.getTile();
                GenerateNewTile(start_location);

                x_location += X_SPACE_BETWEEN_TileButtonS;
                if (i == 9) { y_location += Y_SPACE_BETWEEN_TileButtonS; x_location = STARTING_X_LOCATION; }
                TAG_NUMBER++;
            }
            tookCard = false;
        }

        public void GenerateNewTile_byClickingPool(int[] slot_location)
        {
            if (tookCard == false && GameTable.HUMAN_PLAYER_TURN == GameTable.current_turn)
            {
                GenerateNewTile(slot_location);
                tookCard = true;
            }
        }

        public void GenerateNewTile(int[] slot_location)
        {
            Tile current_tile_from_pool = GameTable.pool.getTile();
            TileButtons[TAG_NUMBER] = (new TileButton(current_tile_from_pool.getColor(), current_tile_from_pool.getNumber(), slot_location));
            TileButtons[TAG_NUMBER].getTileButton().Size = new Size(75, 100);
            TileButtons[TAG_NUMBER].getTileButton().Location = TileButton_slot[slot_location[0], slot_location[1]].getSlotButton().Location;
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

            GameTable.GameTableContext.Controls.Add(TileButtons[TAG_NUMBER].getTileButton());
            TileButtons[TAG_NUMBER].getTileButton().BringToFront();
            TileButtons[TAG_NUMBER].getTileButton().Tag = TAG_NUMBER;
            TileButton_slot[TileButtons[TAG_NUMBER].getLocation()[0], TileButtons[TAG_NUMBER].getLocation()[1]].changeState(true);
        }

        private void TileButton_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).BringToFront(); // focused TileButton, will be always at top
            for (int i = 0; i < TileButtons.Keys.Count; i++) // iterating over the cards, because there is the option that the card in the dropped tiles
            {
                // it means that the card is in the tiles group that you can interact with
                if (((Button)sender).Tag == TileButtons[TileButtons.Keys.ElementAt(i)].getTileButton().Tag)
                {
                    // we are turning it off, because we want to be able to place to the current place if its the closest location
                    TileButton_slot[TileButtons[(int)((Button)sender).Tag].getLocation()[0], TileButtons[(int)((Button)sender).Tag].getLocation()[1]].changeState(false);
                }
            }
        }

        private void TileButton_MouseEnter(object sender, EventArgs e)
        { 
            ((Button)sender).BackgroundImage = Image.FromFile("bright_tile.png"); // bright effect when the mouse hovers over the tile
        }

        private void TileButton_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).BackgroundImage = Image.FromFile("tile.png"); // in order to make the tile normal after hovering over the card
        }

        private float getDistance(Button moving_card, Button empty_slot)
        {
            // used to find the closet slot to a card
            return (float)Math.Sqrt(Math.Pow(moving_card.Location.X - empty_slot.Location.X, 2) + Math.Pow(moving_card.Location.Y - empty_slot.Location.Y, 2));
        }

        private void TileButton_MouseUp(object sender, MouseEventArgs e)
        {
            Button current_card = (Button)sender; // the card that we dragged with the mouse
            if (TileButtons.ContainsKey((int)current_card.Tag)) // if the card is in our board
            {
                // first, we would like to check if the user wanted to put the TileButton on the drop_TileButton location
                if (getDistance(current_card, GameTable.dropped_tiles) < 100 && GameTable.current_turn == GameTable.HUMAN_PLAYER_TURN && tookCard == true)
                {
                    current_card.Location = new Point(GameTable.dropped_tiles.Location.X + 10, GameTable.dropped_tiles.Location.Y + 18);
                    current_card.Draggable(false);
                    int[] current_location = {DROPPED_CARD_LOCATION, DROPPED_CARD_LOCATION};
                    TileButtons[(int)current_card.Tag].setLocation(current_location);
                    TileButtons.Remove((int)current_card.Tag);

                    // after we dropped a card, it is the end of the turn
                    tookCard = false;
                    GameTable.current_turn = GameTable.COMPUTER_PLAYER_TURN;
                    GameTable.ComputerPlayer.play();
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
            if (checkWinner_humanPlayer() == true)
            {
                MessageBox.Show("You Won!");
            }
        }

        public bool checkWinner_humanPlayer()
        {
            List<TileButton> topBoard_tiles = new List<TileButton>();
            List<TileButton> bottomBoard_tiles = new List<TileButton>();
            List<List<TileButton>> melds = new List<List<TileButton>>();

            for (int i = 0; i < TileButtons.Values.ToList().Count(); i++)
            {
                if (TileButtons.Values.ToList()[i].getLocation()[0] == 0)
                {
                    topBoard_tiles.Add(TileButtons.Values.ToList()[i]);
                }
                else
                {
                    bottomBoard_tiles.Add(TileButtons.Values.ToList()[i]);
                }
            }

            // after we have two lists, we will sort them by value and analyze the melds
            topBoard_tiles = topBoard_tiles.OrderBy(card => card.getLocation()[1]).ToList();
            bottomBoard_tiles = bottomBoard_tiles.OrderBy(card => card.getLocation()[1]).ToList();
            getSequencesFromTileList(topBoard_tiles, melds);
            getSequencesFromTileList(bottomBoard_tiles, melds);

            List<List<Tile>> converted_melds_computer_format = new List<List<Tile>>();
            for(int i=0; i<melds.Count(); i++)
            {
                converted_melds_computer_format.Add(convertTilesButtonListToComputerFormat(melds[i]));
            }

            return GameTable.checkWinner(converted_melds_computer_format);
        }

        private static List<Tile> convertTilesButtonListToComputerFormat(List<TileButton> tiles)
        {
            List<Tile> new_tiles_format = new List<Tile>();
            for(int i=0; i<tiles.Count(); i++)
            {
                new_tiles_format.Add(tiles[i]);
            }
            return new_tiles_format;
        }

        private static void getSequencesFromTileList(List<TileButton> tiles_sequence, List<List<TileButton>> melds)
        {
            List<TileButton> meld = new List<TileButton>();
            for (int i = 0; i < tiles_sequence.Count(); i++) // inserting the melds of the upper board
            {
                if (meld.Count() != 0 && meld[meld.Count - 1].getLocation()[1] + 1 == tiles_sequence[i].getLocation()[1])
                {
                    meld.Add(tiles_sequence[i]);
                }
                else
                {
                    if (meld.Count != 0)
                        melds.Add(meld);
                    meld = new List<TileButton>();
                    meld.Add(tiles_sequence[i]);
                }
            }
            if (meld.Count != 0)
                melds.Add(meld);
        }

        public void ArrangeCardsOnBoard(List<TileButton> sorted_cards)
        {
            int card_index = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
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
    }
}