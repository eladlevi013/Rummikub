using rummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class PlayerBoard : IBoard
    {
        // Consts
        const int STARTING_X_LOCATION = 70;
        const int STARTING_Y_LOCATION = 410;
        const int X_SPACE_BETWEEN_TileButtons = 85;
        const int Y_SPACE_BETWEEN_TileButtons = 115;
        const int DROPPED_CARD_LOCATION = -1;
        const int RUMMIKUB_CARDS_NUMBER = 14;

        // statics
        public static int TAG_NUMBER = 0;
        public static bool tookCard = false;

        // board elements
        public Slot[,] TileButton_slot { get; set; } // 2d-array of the slots of the cards
        public Dictionary<int, VisualTile> TileButtons;

        // image proccesing vars
        private Image originalBackgroundImage = null;
        private float currentBrightnessLevel = 1.0f;

        public Dictionary<int, VisualTile> GetTilesDictionary()
        {
            return TileButtons;
        }

        public PlayerBoard()
        {
            GenerateBoard();
            GameTable.dropped_tiles_stack = new Stack<VisualTile>();
        }

        public void DisableLastDroppedTile()
        {
            if (GameTable.dropped_tiles_stack.Count > 0)
            {
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().Enabled = false;
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().MouseUp -= new MouseEventHandler(this.TileButton_MouseUp);
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().MouseDown -= new MouseEventHandler(this.TileButton_MouseDown); ;
            }
        }

        public void GenerateNewTileByClickingPool(int[] slot_location)
        {
            if (tookCard == false && Constants.HUMAN_PLAYER_TURN == GameTable.current_turn && GameTable.game_over == false)
            {
                GenerateNewTileToSlotLocation(slot_location);
                tookCard = true; // prevent unlimited tile picking
                GameTable.global_game_indicator_lbl.Text = GameTable.DROP_TILE_FROM_BOARD_MSG;

                // tile in stack won't be interactable after we generated one
                if (GameTable.dropped_tiles_stack != null && GameTable.dropped_tiles_stack.Count() != 0)
                {
                    //GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().MouseUp -= new MouseEventHandler(this.TileButton_MouseUp);
                    //GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().MouseDown -= new MouseEventHandler(this.TileButton_MouseDown);
                    GameTable.dropped_tiles_stack.Peek().TileButton.SetDraggable(false);
                }
            }
        }

        private void GenerateNewTileToSlotLocation(int[] slot_location)
        {
            Tile current_tile_from_pool = GameTable.pool.GetTile();
            if (current_tile_from_pool == null)
                return;

            TileButtons[TAG_NUMBER] = (new VisualTile(current_tile_from_pool.Color, current_tile_from_pool.Number, slot_location));
            // TileButtons[TAG_NUMBER].TileButton.GetButton().Location = TileButton_slot[slot_location[0], slot_location[1]].SlotButton.Location;
            TileButton_slot[TileButtons[TAG_NUMBER].SlotLocation[0], TileButtons[TAG_NUMBER].SlotLocation[1]].SlotState = true;
            TileDesigner(TileButtons[TAG_NUMBER], current_tile_from_pool, true);
            ControlTransition.Move(TileButtons[TAG_NUMBER-1].TileButton.GetButton(), GameTable.global_pool_btn.Location, TileButton_slot[slot_location[0], slot_location[1]].SlotButton.Location);
        }

        public void TileDesigner(VisualTile tile, Tile tile_info, bool tag_update)
        {   
            // creates graphical tile(location should be set outside the function)
            tile.TileButton.GetButton().Size = new Size(75, 100);
            tile.TileButton.GetButton().BackgroundImageLayout = ImageLayout.Stretch;
            tile.TileButton.SetDraggable(true); // usage of the extension
            tile.TileButton.GetButton().FlatStyle = FlatStyle.Flat;
            tile.TileButton.GetButton().FlatAppearance.BorderSize = 0;
            tile.TileButton.GetButton().Font = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
            tile.TileButton.GetButton().MouseUp += new MouseEventHandler(this.TileButton_MouseUp);
            tile.TileButton.GetButton().MouseDown += new MouseEventHandler(this.TileButton_MouseDown);
            tile.TileButton.GetButton().MouseEnter += TileButton_MouseEnter;
            tile.TileButton.GetButton().MouseLeave += TileButton_MouseLeave;

            // if joker
            if (tile.Number == 0)
            {
                if (tile.Color == Constants.BLACK_COLOR)
                {
                    tile.TileButton.GetButton().BackgroundImage = Image.FromFile(GameTable.BLACK_JOKER_PATH);
                }
                else
                {
                    tile.TileButton.GetButton().BackgroundImage = Image.FromFile(GameTable.RED_JOKER_PATH);
                }
            }
            else
            {
                tile.TileButton.GetButton().BackgroundImage = Image.FromFile(GameTable.TILE_PATH);
                tile.TileButton.GetButton().Text = tile_info.Number.ToString();
                if (tile.Color == 0) tile.TileButton.GetButton().ForeColor = (Color.Blue);
                else if (tile.Color == 1) tile.TileButton.GetButton().ForeColor = (Color.Black);
                else if (tile.Color == 2) tile.TileButton.GetButton().ForeColor = (Color.Yellow);
                else tile.TileButton.GetButton().ForeColor = (Color.Red);
            }

            if (tag_update)
                tile.TileButton.GetButton().Tag = TAG_NUMBER++;
            else 
                tile.TileButton.GetButton().Tag = tile.Tag;
            
            tile.Tag = (int)tile.TileButton.GetButton().Tag;
            GameTable.global_gametable_context.Controls.Add(tile.TileButton.GetButton());
            tile.TileButton.GetButton().BringToFront();
        }

        public void TileButton_MouseDown(object sender, MouseEventArgs e)
        {
            Button current_card = (Button)sender;
            current_card.BringToFront();

            // iterating over the cards, because there is the option that the card in the dropped tiles
            for (int i = 0; i < TileButtons.Keys.Count; i++)
            {
                // it means that the card is in the tiles group that you can interact with
                if (current_card.Tag == TileButtons[TileButtons.Keys.ElementAt(i)].TileButton.GetButton().Tag 
                    && TileButtons[(int)current_card.Tag].SlotLocation[0] != -1 
                    && TileButtons[(int)((Button)sender).Tag].SlotLocation[1] != -1)
                {
                    // we'll make it available in order to be able to place to the current place if its the closest location
                    TileButton_slot[TileButtons[(int)((Button)sender).Tag].SlotLocation[0]
                        , TileButtons[(int)((Button)sender).Tag].SlotLocation[1]].SlotState = Constants.AVAILABLE;
                }
            }
        }

        public void SetBackgroundImageBrightness(Button button, float brightnessLevel)
        {
            if (button.BackgroundImage == null)
            {
                return;
            }

            originalBackgroundImage = button.BackgroundImage;


            if (brightnessLevel == currentBrightnessLevel)
            {
                return;
            }

            float[][] matrixItems ={
           new float[] {brightnessLevel, 0, 0, 0, 0},
           new float[] {0, brightnessLevel, 0, 0, 0},
           new float[] {0, 0, brightnessLevel, 0, 0},
           new float[] {0, 0, 0, 1, 0},
           new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Bitmap bmp = new Bitmap(originalBackgroundImage.Width, originalBackgroundImage.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(originalBackgroundImage, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }

            button.BackgroundImage = bmp;
            currentBrightnessLevel = brightnessLevel;
        }

        public void ResetBackgroundImageBrightness(Button button)
        {
            if (button.BackgroundImage == null || originalBackgroundImage == null || currentBrightnessLevel == 1.0f)
            {
                return;
            }

            button.BackgroundImage = originalBackgroundImage;
            currentBrightnessLevel = 1.0f;
        }

        public void TileButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                SetBackgroundImageBrightness(button, 1.2f);
            }
        }

        public void TileButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ResetBackgroundImageBrightness(button);
            }
        }

        private float GetDistance(Button moving_card, Button empty_slot)
        {
            // used to find the closet slot to a card
            return (float)Math.Sqrt(Math.Pow(moving_card.Location.X - empty_slot.Location.X, 2) 
                + Math.Pow(moving_card.Location.Y - empty_slot.Location.Y, 2));
        }

        public void TileButton_MouseUp(object sender, MouseEventArgs e)
        {
            Button current_card = (Button)sender; // the card that we dragged with the mouse
            if (TileButtons.ContainsKey((int)current_card.Tag) || (GameTable.dropped_tiles_stack.Count > 0 
                && (int)GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().Tag == (int)current_card.Tag))
            {
                // first, we would like to check if the user wanted to put the TileButton on the drop_TileButton location
                if (GetDistance(current_card, GameTable.global_dropped_tiles_btn) < 100 
                    && GameTable.current_turn == Constants.HUMAN_PLAYER_TURN 
                    && tookCard == true && TileButtons.ContainsKey((int)current_card.Tag))
                {
                    current_card.Location = new Point(GameTable.global_dropped_tiles_btn.Location.X 
                        + 10, GameTable.global_dropped_tiles_btn.Location.Y + 18);
                    TileButtons[(int)current_card.Tag].TileButton.SetDraggable(false);
                    int[] current_location = {DROPPED_CARD_LOCATION, DROPPED_CARD_LOCATION};
                    TileButtons[(int)current_card.Tag].SlotLocation = current_location;

                    // add the dropped card to stack
                    GameTable.dropped_tiles_stack.Push(TileButtons[(int)current_card.Tag]);

                    // remove the tiles button
                    TileButtons.Remove((int)current_card.Tag);

                    // after we dropped a card, it is the end of the turn
                    tookCard = false;
                    GameTable.current_turn = Constants.COMPUTER_PLAYER_TURN;
                    GameTable.global_game_indicator_lbl.Text = "Computer's Turn";

                    // sleep
                    Thread.Sleep(5);

                    // call the computerPlayer play function in another thread in order to prevent crashes
                    GameTable.computer_player.ComputerPlay(GameTable.dropped_tiles_stack.Peek());
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
                            if (TileButton_slot[i, j].SlotState == Constants.AVAILABLE)
                            {
                                first_empty_slot = TileButton_slot[i, j].SlotButton;
                                found_first_empty_slot = true;
                                min_i = i;
                                min_j = j;
                            }
                        }
                    }

                    // we'll calculate the distance between the card, and the first empty slot
                    float min_distance = GetDistance(current_card, first_empty_slot);

                    // now, we'll calculate the distance between the card and all the empty slots, and we'll find the minimum
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            // if the distance is smaller, and the slot is available
                            if (GetDistance(current_card, TileButton_slot[i, j].SlotButton) < min_distance 
                                && TileButton_slot[i, j].SlotState == Constants.AVAILABLE)
                            {
                                min_distance = GetDistance(current_card, TileButton_slot[i, j].SlotButton);
                                min_i = i;
                                min_j = j;
                            }
                        }
                    }

                    // we made it empty, as we clicked_down to move the card, so now we have to make it non-empty again
                    if(TileButtons.ContainsKey((int)current_card.Tag) 
                        && TileButtons[(int)((Button)sender).Tag].SlotLocation[0] != -1 
                        && TileButtons[(int)((Button)sender).Tag].SlotLocation[1] != -1)
                        TileButton_slot[TileButtons[(int)((Button)sender).Tag].SlotLocation[0], 
                            TileButtons[(int)((Button)sender).Tag].SlotLocation[1]].SlotState = true;

                    // update the location of the focused TileButton, to the location of the minimum distance that we found earlier
                    ControlTransition.Move(current_card, current_card.Location, TileButton_slot[min_i, min_j].SlotButton.Location);

                    // now we need to update the status of the old slot to empty
                    if (TileButtons.ContainsKey((int)current_card.Tag) 
                        && TileButtons[(int)((Button)sender).Tag].SlotLocation[0] != -1 
                        && TileButtons[(int)((Button)sender).Tag].SlotLocation[1] != -1)
                        TileButton_slot[TileButtons[(int)current_card.Tag].SlotLocation[0]
                            , TileButtons[(int)current_card.Tag].SlotLocation[1]].SlotState = false;

                    // we'll change the status of the 'minimum-distance' slot(now contains the card)
                    TileButton_slot[min_i, min_j].SlotState = true;

                    // update the TileButton location(the location of the slot)
                    int[] updated_TileButton_location = { min_i, min_j };
                    if(TileButtons.ContainsKey((int)current_card.Tag))
                        TileButtons[(int)current_card.Tag].SlotLocation = updated_TileButton_location;
                }
            }
            if (CheckWinner() == true && GameTable.game_over == false)
            {
                MessageBox.Show("You Won!");
                GameTable.global_game_indicator_lbl.Text = "Game Over - You Won";
                GameTable.game_over = true;
                GameTable.human_player.board.disableHumanBoard();
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().Enabled = false;
            }
        }
        
        private List<List<Tile>> meldsArrangedByPlayer()
        {
            List<VisualTile> topBoard_tiles = new List<VisualTile>();
            List<VisualTile> bottomBoard_tiles = new List<VisualTile>();
            List<List<VisualTile>> melds = new List<List<VisualTile>>();

            for (int i = 0; i < TileButtons.Values.ToList().Count(); i++)
            {
                if (TileButtons.Values.ToList()[i].SlotLocation[0] == 0)
                    topBoard_tiles.Add(TileButtons.Values.ToList()[i]);
                else
                    bottomBoard_tiles.Add(TileButtons.Values.ToList()[i]);
            }

            // after we have two lists, we will sort them by value and analyze the melds
            topBoard_tiles = topBoard_tiles.OrderBy(card => card.SlotLocation[1]).ToList();
            bottomBoard_tiles = bottomBoard_tiles.OrderBy(card => card.SlotLocation[1]).ToList();

            GetSequencesFromTileList(topBoard_tiles, melds);
            GetSequencesFromTileList(bottomBoard_tiles, melds);

            List<List<Tile>> converted_melds_computer_format = new List<List<Tile>>();
            for (int i = 0; i < melds.Count(); i++)
                converted_melds_computer_format.Add(convertTilesButtonListToComputerFormat(melds[i]));

            return converted_melds_computer_format;
        }

        public int GetHandTilesNumber()
        {
            List<List<Tile>> melds = meldsArrangedByPlayer();
            int tiles_in_sets = 0;

            for(int set_index = 0; set_index < melds.Count(); set_index++)
                if (GameTable.IsLegalMeld(melds[set_index]))
                    tiles_in_sets += melds[set_index].Count();

            return Constants.RUMMIKUB_TILES_IN_GAME - tiles_in_sets;
        }

        public static List<Tile> convertTilesButtonListToComputerFormat(List<VisualTile> tiles)
        {
            List<Tile> new_tiles_format = new List<Tile>();
            for(int i=0; i<tiles.Count(); i++)
                new_tiles_format.Add(tiles[i]);
            return new_tiles_format;
        }

        private static void GetSequencesFromTileList(List<VisualTile> tiles_sequence, List<List<VisualTile>> melds)   
        {
            List<VisualTile> meld = new List<VisualTile>();
            for (int i = 0; i < tiles_sequence.Count(); i++)
            {
                if (meld.Count() != 0 && meld[meld.Count - 1].SlotLocation[1] + 1 == tiles_sequence[i].SlotLocation[1])
                {
                    meld.Add(tiles_sequence[i]);
                }
                else
                {
                    if (meld.Count != 0)
                        melds.Add(meld);

                    meld = new List<VisualTile>();
                    meld.Add(tiles_sequence[i]);
                }
            }
            if (meld.Count != 0)
                melds.Add(meld);
        }

        public void ArrangeCardsOnBoard(List<VisualTile> sorted_cards)
        {
            int card_index = 0;
            for (int i = 0; i < GameTable.HUMAN_PLAYER_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameTable.HUMAN_PLAYER_BOARD_WIDTH; j++)
                {
                    TileButton_slot[i, j].SlotState = Constants.AVAILABLE;
                    if (card_index < sorted_cards.Count())
                    {
                        sorted_cards[card_index].TileButton.GetButton().Location = TileButton_slot[i, j].SlotButton.Location;
                        TileButton_slot[i, j].SlotState = true;
                        int[] location_arr = { i, j };
                        sorted_cards[card_index].SlotLocation = location_arr;
                    }
                    card_index++;
                }
            }
        }

        public void disableHumanBoard()
        {
            for(int i=0; i < TileButtons.Keys.ToList().Count(); i++)
            {
                TileButtons[TileButtons.Keys.ElementAt(i)].TileButton.GetButton().Enabled = false;
            }
            if (GameTable.dropped_tiles_stack.Count > 0)
            {
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().Enabled = false;
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().MouseUp -= new MouseEventHandler(this.TileButton_MouseUp);
                GameTable.dropped_tiles_stack.Peek().TileButton.GetButton().MouseDown -= new MouseEventHandler(this.TileButton_MouseDown); ;
            }
        }

        public void GenerateTiles()
        {
            // slots from loading.
            Slot[,] loaded_slots = TileButton_slot;

            int x_location = STARTING_X_LOCATION;
            int y_location = STARTING_Y_LOCATION;
            TileButton_slot = new Slot[GameTable.HUMAN_PLAYER_BOARD_HEIGHT, GameTable.HUMAN_PLAYER_BOARD_WIDTH];

            // Generating the slots
            for (int i = 0; i < GameTable.HUMAN_PLAYER_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameTable.HUMAN_PLAYER_BOARD_WIDTH; j++)
                {   // Generate a single slotButton
                    TileButton_slot[i, j] = new Slot();
                    TileButton_slot[i, j].SlotButton.BackgroundImage = Image.FromFile(GameTable.SLOT_PATH);
                    TileButton_slot[i, j].SlotButton.BackgroundImageLayout = ImageLayout.Stretch;
                    TileButton_slot[i, j].SlotButton.FlatStyle = FlatStyle.Flat;
                    TileButton_slot[i, j].SlotButton.FlatAppearance.BorderSize = 0;
                    TileButton_slot[i, j].SlotButton.Size = new Size(GameTable.TILE_WIDTH, GameTable.TILE_HEIGHT);
                    TileButton_slot[i, j].SlotButton.Location = new Point(x_location, y_location);
                    TileButton_slot[i, j].SlotState = loaded_slots[i, j].SlotState;
                    GameTable.global_gametable_context.Controls.Add(TileButton_slot[i, j].SlotButton);
                    x_location += X_SPACE_BETWEEN_TileButtons;
                }
                y_location += Y_SPACE_BETWEEN_TileButtons;
                x_location = STARTING_X_LOCATION;
            }

            // loaded tileButtons dictionary
            Dictionary<int, VisualTile> loaded_tile_buttons = TileButtons;

            TileButtons = new Dictionary<int, VisualTile>();

            // generating tiles from dictionary
            for(int i = 0; i < loaded_tile_buttons.Keys.ToList().Count(); i++)
            {
                VisualTile tile = loaded_tile_buttons[loaded_tile_buttons.Keys.ToList()[i]];
                TileButtons[loaded_tile_buttons.Keys.ToList()[i]] = new VisualTile(tile.Color, tile.Number, tile.SlotLocation, tile.Tag);
                TileButtons[TileButtons.Keys.ToList()[i]].TileButton.GetButton().Location 
                    = TileButton_slot[tile.SlotLocation[0], tile.SlotLocation[1]].SlotButton.Location;
                TileDesigner(TileButtons[TileButtons.Keys.ToList()[i]], tile, false);
            }
        }

        public void GenerateBoard()
        {
            int x_location = STARTING_X_LOCATION;
            int y_location = STARTING_Y_LOCATION;
            TileButton_slot = new Slot[GameTable.HUMAN_PLAYER_BOARD_HEIGHT, GameTable.HUMAN_PLAYER_BOARD_WIDTH];
            TileButtons = new Dictionary<int, VisualTile>();

            // Generating the slots
            for (int i = 0; i < GameTable.HUMAN_PLAYER_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < GameTable.HUMAN_PLAYER_BOARD_WIDTH; j++)
                {   // Generate a single slotButton
                    TileButton_slot[i, j] = new Slot();
                    TileButton_slot[i, j].SlotButton.BackgroundImage = Image.FromFile(GameTable.SLOT_PATH);
                    TileButton_slot[i, j].SlotButton.BackgroundImageLayout = ImageLayout.Stretch;
                    TileButton_slot[i, j].SlotButton.FlatStyle = FlatStyle.Flat;
                    TileButton_slot[i, j].SlotButton.FlatAppearance.BorderSize = 0;
                    TileButton_slot[i, j].SlotButton.Size = new Size(GameTable.TILE_WIDTH, GameTable.TILE_HEIGHT);
                    TileButton_slot[i, j].SlotButton.Location = new Point(x_location, y_location);
                    TileButton_slot[i, j].SlotState = Constants.AVAILABLE;
                    GameTable.global_gametable_context.Controls.Add(TileButton_slot[i, j].SlotButton);
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
                TileButton_slot[i / 10, i % 10].SlotState = Constants.ALLOCATED;
            }
        }

        public void ClearBoard()
        {
            for (int i = 0; i < TileButtons.Values.ToList().Count(); i++)
            {
                GameTable.global_gametable_context.Controls.Remove(TileButtons.Values.ToList()[i].TileButton.GetButton());
            }
        }

        public bool CheckWinner()
        {
            List<List<Tile>> melds = meldsArrangedByPlayer();
            return GameTable.CheckWinner(melds);
        }
    }
}
