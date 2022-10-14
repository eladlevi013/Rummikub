using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class PlayerBoard : Board
    {
        // consts
        const int STARTING_X_LOCATION = 70;
        const int STARTING_Y_LOCATION = 395;
        const int X_SPACE_BETWEEN_TileButtons = 85;
        const int Y_SPACE_BETWEEN_TileButtons = 115;
        const int DROPPED_CARD_LOCATION = -1; // when the card is no longer in board
        const int RUMMIKUB_CARDS_NUMBER = 14;

        // statics
        public static int TAG_NUMBER = 0; // every card has tag that indicates the index on the dictionary
        public static bool tookCard = false;

        // board elements
        private Slot[,] TileButton_slot; // 2d-array of the slots of the cards
        private Dictionary<int, TileButton> TileButtons;

        public Slot[,] getTileButtonSlot()
        {
            return TileButton_slot;
        }

        public Dictionary<int, TileButton> getTilesDictionary()
        {
            return TileButtons;
        }

        public PlayerBoard()
        {
            generateBoard();
            GameTable.dropped_tiles_stack = new Stack<TileButton>();
        }

        public void GenerateNewTileByClickingPool(int[] slot_location)
        {
            if (tookCard == false && GameTable.HUMAN_PLAYER_TURN == GameTable.current_turn && GameTable.game_over == false)
            {
                GenerateNewTileToSlotLocation(slot_location);
                tookCard = true; // prevent unlimited tile picking
                GameTable.global_game_indicator_lbl.Text = GameTable.DROP_TILE_FROM_BOARD_MSG;

                // tile in stack won't be interactable after we generated one
                if (GameTable.dropped_tiles_stack != null && GameTable.dropped_tiles_stack.Count() != 0)
                {
                    GameTable.dropped_tiles_stack.Peek().getTileButton().MouseUp -= new MouseEventHandler(this.TileButton_MouseUp);
                    GameTable.dropped_tiles_stack.Peek().getTileButton().MouseDown -= new MouseEventHandler(this.TileButton_MouseDown);
                    GameTable.dropped_tiles_stack.Peek().getTileButton().Draggable(false);
                }
            }
        }

        private void GenerateNewTileToSlotLocation(int[] slot_location)
        {
            Tile current_tile_from_pool = GameTable.pool.getTile();
            if (current_tile_from_pool == null)
                return;

            TileButtons[TAG_NUMBER] = (new TileButton(current_tile_from_pool.getColor(), current_tile_from_pool.getNumber(), slot_location));
            TileButtons[TAG_NUMBER].getTileButton().Location = TileButton_slot[slot_location[0], slot_location[1]].getSlotButton().Location;
            TileButton_slot[TileButtons[TAG_NUMBER].getSlotLocation()[0], TileButtons[TAG_NUMBER].getSlotLocation()[1]].changeState(true);
            TileDesigner(TileButtons[TAG_NUMBER], current_tile_from_pool);
        }

        public void TileDesigner(TileButton tile, Tile tile_info)
        {   // creates graphical tile(location should be set outside the function)
            tile.getTileButton().Size = new Size(75, 100);
            tile.getTileButton().BackgroundImage = Image.FromFile("Tile.png");
            tile.getTileButton().BackgroundImageLayout = ImageLayout.Stretch;
            tile.getTileButton().Draggable(true); // usage of the extension
            tile.getTileButton().FlatStyle = FlatStyle.Flat;
            tile.getTileButton().FlatAppearance.BorderSize = 0;
            tile.getTileButton().Text = tile_info.getNumber().ToString();
            if (tile.getColor() == 0) tile.getTileButton().ForeColor = (Color.Blue);
            else if (tile.getColor() == 1) tile.getTileButton().ForeColor = (Color.Black);
            else if (tile.getColor() == 2) tile.getTileButton().ForeColor = (Color.Yellow);
            else tile.getTileButton().ForeColor = (Color.Red);
            tile.getTileButton().Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
            tile.getTileButton().MouseUp += new MouseEventHandler(this.TileButton_MouseUp);
            tile.getTileButton().MouseDown += new MouseEventHandler(this.TileButton_MouseDown);
            tile.getTileButton().MouseEnter += TileButton_MouseEnter;
            tile.getTileButton().MouseLeave += TileButton_MouseLeave;
            tile.getTileButton().Tag = TAG_NUMBER;
            GameTable.global_gametable_context.Controls.Add(tile.getTileButton());
            tile.getTileButton().BringToFront();
            TAG_NUMBER++;
        }

        private void TileButton_MouseDown(object sender, MouseEventArgs e)
        {
            Button current_card = (Button)sender;
            current_card.BringToFront();
            for (int i = 0; i < TileButtons.Keys.Count; i++) // iterating over the cards, because there is the option that the card in the dropped tiles
            {
                // it means that the card is in the tiles group that you can interact with
                if (current_card.Tag == TileButtons[TileButtons.Keys.ElementAt(i)].getTileButton().Tag && TileButtons[(int)current_card.Tag].getSlotLocation()[0] != -1 && TileButtons[(int)((Button)sender).Tag].getSlotLocation()[1] != -1)
                {
                    // we'll make it available in order to be able to place to the current place if its the closest location
                    TileButton_slot[TileButtons[(int)((Button)sender).Tag].getSlotLocation()[0], TileButtons[(int)((Button)sender).Tag].getSlotLocation()[1]].changeState(Slot.AVAILABLE);
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
            if (TileButtons.ContainsKey((int)current_card.Tag) || (GameTable.dropped_tiles_stack.Count > 0 && (int)GameTable.dropped_tiles_stack.Peek().getTileButton().Tag == (int)current_card.Tag)) // if the card is in our board
            {
                // first, we would like to check if the user wanted to put the TileButton on the drop_TileButton location
                if (getDistance(current_card, GameTable.global_dropped_tiles_btn) < 100 && GameTable.current_turn == GameTable.HUMAN_PLAYER_TURN && tookCard == true && TileButtons.ContainsKey((int)current_card.Tag))
                {
                    current_card.Location = new Point(GameTable.global_dropped_tiles_btn.Location.X + 10, GameTable.global_dropped_tiles_btn.Location.Y + 18);
                    current_card.Draggable(false);
                    int[] current_location = {DROPPED_CARD_LOCATION, DROPPED_CARD_LOCATION};
                    TileButtons[(int)current_card.Tag].setSlotLocation(current_location);

                    // add the dropped card to stack
                    GameTable.dropped_tiles_stack.Push(TileButtons[(int)current_card.Tag]);

                    // remove the tiles button
                    TileButtons.Remove((int)current_card.Tag);

                    // after we dropped a card, it is the end of the turn
                    tookCard = false;
                    GameTable.current_turn = GameTable.COMPUTER_PLAYER_TURN;
                    GameTable.global_game_indicator_lbl.Text = "Computer's Turn";

                    // sleep
                    Thread.Sleep(5);

                    // call the computerPlayer play function in another thread in order to prevent crashes
                    GameTable.computer_player.play(GameTable.dropped_tiles_stack.Peek());
                }
                // otherwise we would like to search the first empty slot to put in the TileButton
                else
                {
                    if (TileButtons.ContainsKey((int)current_card.Tag) == false)
                    {
                        TileButtons[(int)current_card.Tag] = GameTable.dropped_tiles_stack.Peek();
                        tookCard = true;
                        GameTable.global_game_indicator_lbl.Text = GameTable.DROP_TILE_FROM_BOARD_MSG;
                    }

                    // Now we'll search the first empty slot, so we would know what is the the most close 
                    int min_i = 0;
                    int min_j = 0;
                    Button first_empty_slot = null;
                    bool found_first_empty_slot = false;
                    for (int i = 0; i < 2 && found_first_empty_slot == false; i++)
                    {
                        for (int j = 0; j < 10 && found_first_empty_slot == false; j++)
                        {
                            if (TileButton_slot[i, j].getState() == Slot.AVAILABLE)
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
                            if (getDistance(current_card, TileButton_slot[i, j].getSlotButton()) < min_distance && TileButton_slot[i, j].getState() == Slot.AVAILABLE)
                            {
                                min_distance = getDistance(current_card, TileButton_slot[i, j].getSlotButton());
                                min_i = i;
                                min_j = j;
                            }
                        }
                    }

                    // we made it empty, as we clicked_down to move the card, so now we have to make it non-empty again
                    if(TileButtons.ContainsKey((int)current_card.Tag) && TileButtons[(int)((Button)sender).Tag].getSlotLocation()[0] != -1 && TileButtons[(int)((Button)sender).Tag].getSlotLocation()[1] != -1)
                        TileButton_slot[TileButtons[(int)((Button)sender).Tag].getSlotLocation()[0], TileButtons[(int)((Button)sender).Tag].getSlotLocation()[1]].changeState(true);

                    // update the location of the focused TileButton, to the location of the minimum distance that we found earlier
                    current_card.Location = TileButton_slot[min_i, min_j].getSlotButton().Location;

                    // now we need to update the status of the old slot to empty
                    if (TileButtons.ContainsKey((int)current_card.Tag) && TileButtons[(int)((Button)sender).Tag].getSlotLocation()[0] != -1 && TileButtons[(int)((Button)sender).Tag].getSlotLocation()[1] != -1)
                        TileButton_slot[TileButtons[(int)current_card.Tag].getSlotLocation()[0], TileButtons[(int)current_card.Tag].getSlotLocation()[1]].changeState(false);

                    // we'll change the status of the 'minimum-distance' slot(now contains the card)
                    TileButton_slot[min_i, min_j].changeState(true);

                    // update the TileButton location(the location of the slot)
                    int[] updated_TileButton_location = { min_i, min_j };
                    if(TileButtons.ContainsKey((int)current_card.Tag))
                        TileButtons[(int)current_card.Tag].setSlotLocation(updated_TileButton_location);
                }
            }
            if (checkWinner() == true)
            {
                MessageBox.Show("You Won!");
                GameTable.global_game_indicator_lbl.Text = "Game Over - You Won";
            }
        }

        public bool checkWinner()
        {
            List<List<Tile>> melds = meldsArrangedByPlayer();
            return GameTable.checkWinner(melds);
        }
        
        private List<List<Tile>> meldsArrangedByPlayer()
        {
            List<TileButton> topBoard_tiles = new List<TileButton>();
            List<TileButton> bottomBoard_tiles = new List<TileButton>();
            List<List<TileButton>> melds = new List<List<TileButton>>();

            for (int i = 0; i < TileButtons.Values.ToList().Count(); i++)
            {
                if (TileButtons.Values.ToList()[i].getSlotLocation()[0] == 0)
                    topBoard_tiles.Add(TileButtons.Values.ToList()[i]);
                else
                    bottomBoard_tiles.Add(TileButtons.Values.ToList()[i]);
            }

            // after we have two lists, we will sort them by value and analyze the melds
            topBoard_tiles = topBoard_tiles.OrderBy(card => card.getSlotLocation()[1]).ToList();
            bottomBoard_tiles = bottomBoard_tiles.OrderBy(card => card.getSlotLocation()[1]).ToList();

            getSequencesFromTileList(topBoard_tiles, melds);
            getSequencesFromTileList(bottomBoard_tiles, melds);

            List<List<Tile>> converted_melds_computer_format = new List<List<Tile>>();
            for (int i = 0; i < melds.Count(); i++)
                converted_melds_computer_format.Add(convertTilesButtonListToComputerFormat(melds[i]));

            return converted_melds_computer_format;
        }

        public int getHandTilesNumber()
        {
            List<List<Tile>> melds = meldsArrangedByPlayer();
            int tiles_in_sets = 0;

            for(int set_index = 0; set_index < melds.Count(); set_index++)
                if (GameTable.isLegalMeld(melds[set_index]))
                    tiles_in_sets += melds[set_index].Count();

            return GameTable.RUMMIKUB_TILES_IN_GAME - tiles_in_sets;
        }

        private static List<Tile> convertTilesButtonListToComputerFormat(List<TileButton> tiles)
        {
            List<Tile> new_tiles_format = new List<Tile>();
            for(int i=0; i<tiles.Count(); i++)
                new_tiles_format.Add(tiles[i]);
            return new_tiles_format;
        }

        private static void getSequencesFromTileList(List<TileButton> tiles_sequence, List<List<TileButton>> melds)   
        {
            List<TileButton> meld = new List<TileButton>();
            for (int i = 0; i < tiles_sequence.Count(); i++)
            {
                if (meld.Count() != 0 && meld[meld.Count - 1].getSlotLocation()[1] + 1 == tiles_sequence[i].getSlotLocation()[1])
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
            for (int i = 0; i < GameTable.HUMAN_PLAYER_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameTable.HUMAN_PLAYER_BOARD_WIDTH; j++)
                {
                    TileButton_slot[i, j].changeState(Slot.AVAILABLE);
                    if (card_index < sorted_cards.Count())
                    {
                        sorted_cards[card_index].getTileButton().Location = TileButton_slot[i, j].getSlotButton().Location;
                        TileButton_slot[i, j].changeState(true);
                        int[] location_arr = { i, j };
                        sorted_cards[card_index].setSlotLocation(location_arr);
                    }
                    card_index++;
                }
            }
        }

        public void disableHumanBoard()
        {
            for(int i=0; i < TileButtons.Keys.ToList().Count(); i++)
            {
                TileButtons[TileButtons.Keys.ElementAt(i)].getTileButton().Enabled = false;
            }
            if (GameTable.dropped_tiles_stack.Count > 0)
            {
                GameTable.dropped_tiles_stack.Peek().getTileButton().Enabled = false;
                GameTable.dropped_tiles_stack.Peek().getTileButton().MouseUp -= new MouseEventHandler(this.TileButton_MouseUp);
                GameTable.dropped_tiles_stack.Pop().getTileButton().MouseDown -= new MouseEventHandler(this.TileButton_MouseDown); ;
            }
        }

        public void generateBoard()
        {
            int x_location = STARTING_X_LOCATION;
            int y_location = STARTING_Y_LOCATION;
            TileButton_slot = new Slot[GameTable.HUMAN_PLAYER_BOARD_HEIGHT, GameTable.HUMAN_PLAYER_BOARD_WIDTH];
            TileButtons = new Dictionary<int, TileButton>();

            // Generating the slots
            for (int i = 0; i < GameTable.HUMAN_PLAYER_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameTable.HUMAN_PLAYER_BOARD_WIDTH; j++)
                {   // Generate a single slotButton
                    TileButton_slot[i, j] = new Slot();
                    TileButton_slot[i, j].getSlotButton().BackgroundImage = Image.FromFile("slot.png");
                    TileButton_slot[i, j].getSlotButton().BackgroundImageLayout = ImageLayout.Stretch;
                    TileButton_slot[i, j].getSlotButton().FlatStyle = FlatStyle.Flat;
                    TileButton_slot[i, j].getSlotButton().FlatAppearance.BorderSize = 0;
                    TileButton_slot[i, j].getSlotButton().Size = new Size(GameTable.TILE_WIDTH, GameTable.TILE_HEIGHT);
                    TileButton_slot[i, j].getSlotButton().Location = new Point(x_location, y_location);
                    TileButton_slot[i, j].changeState(Slot.AVAILABLE);
                    GameTable.global_gametable_context.Controls.Add(TileButton_slot[i, j].getSlotButton());
                    x_location += X_SPACE_BETWEEN_TileButtons;
                }
                y_location += Y_SPACE_BETWEEN_TileButtons;
                x_location = STARTING_X_LOCATION;
            }

            // Generating the TileButtons
            for (int i = 0; i < RUMMIKUB_CARDS_NUMBER; i++)
            {
                int[] start_location = { i / 10, i % 10 };
                GenerateNewTileToSlotLocation(start_location);
                TileButton_slot[i / 10, i % 10].changeState(Slot.ALLOCATED);
            }
        }
    }
}