using Rummikub;
using rummikubGame.Exceptions;
using rummikubGame.Logic;
using rummikubGame.Models;
using rummikubGame.Utilities;
using RummikubGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class ComputerPlayer
    {
        /*
            This class will mainly used for calculations
            the computer player has to take in order to play
            in the optimal way he can in the current turn.
        */

        public ComputerBoard board;
        
        public ComputerPlayer()
        {
            board = new ComputerBoard();
            board.sequences = GameLogic.MeldsSets(ref board.hand, ref board.unusedJokers);
            
            // removing jokers that are in sequences
            board.UpdatingUnusedJokers(ref board.unusedJokers, ref board.sequences);
            board.partialSets = GameLogic.CreatePartialSets(ref board.hand);
            GameLogic.AddJokersAfterMeldsSets(ref board.partialSets, ref board.sequences, ref board.unusedJokers, ref board.hand);

            // takes care of the graphical board of the computer
            if (RummikubGameView.ShowComputerTilesToggle)
                board.GenerateBoard();
        }

        // -----------------------------------------------------------------
        // Tries to replace the given tile with every tile,
        // in order to get the tile that fitted the most.
        // -----------------------------------------------------------------
        public bool BetterArrangementFound(Tile tile_to_replace)
        {
            // vars for finding the best option
            List<Tile> better_option_hand = board.hand;
            List<List<Tile>> better_option_sequences = board.sequences;
            List<PartialSet> better_option_partial_sets = board.partialSets;
            List<Tile> starting_jokers = board.GetAllJokers();

            bool better_option_found = false;
            Tile optimal_dropped_tile = null;

            // replacing every tile in starting_tiles with the given paramter in order to get best result
            for (int i = 0; i < Constants.RummikubTilesInGame - board.CountJokers(); i++)
            {
                // all the tiles that are not jokers
                List<Tile> all_tiles = new List<Tile>();
                List<Tile> jokers = new List<Tile>();

                board.GetAllTiles(ref all_tiles, ref jokers);

                // the tile that is being replaced by the given parameter
                Tile dropped_tile = all_tiles[i];

                // if tile_to_replace is joker - add it to the jokers list
                if (tile_to_replace.Number == 0)
                {
                    jokers.Add(tile_to_replace);
                    all_tiles.RemoveAt(i);
                }
                else
                {
                    all_tiles[i] = tile_to_replace;
                }

                List<List<Tile>> temp_sequences = GameLogic.MeldsSets(ref all_tiles, ref jokers);
                List<Tile> temp_hand = new List<Tile>(all_tiles);

                board.UpdatingUnusedJokers(ref jokers, ref temp_sequences);

                List<PartialSet> temp_partial_set = GameLogic.CreatePartialSets(ref temp_hand);
                GameLogic.AddJokersAfterMeldsSets(ref temp_partial_set, ref temp_sequences, ref jokers, ref temp_hand);

                // check current situation is better than the optimal
                if (board.GetNumberOfTilesInAllSets(better_option_sequences) < board.GetNumberOfTilesInAllSets(temp_sequences))
                {
                    better_option_sequences = temp_sequences;
                    better_option_hand = temp_hand;
                    better_option_partial_sets = temp_partial_set;
                    optimal_dropped_tile = dropped_tile;
                    better_option_found = true;
                }
            }

            // better arrangment found
            if (better_option_found)
            {
                // updates global board variables
                board.sequences = better_option_sequences;
                board.hand = better_option_hand;

                // updating unused jokers
                board.unusedJokers = new List<Tile>(starting_jokers);
                board.UpdatingUnusedJokers(ref board.unusedJokers, ref board.sequences);

                // create partial sets from the given hand
                board.partialSets = better_option_partial_sets;
                //board.partial_sets = new List<PartialSet>();
                //board.partial_sets = CreatePartialSets(ref board.hand);

                GameLogic.AddJokersAfterMeldsSets(ref board.partialSets, ref board.sequences, ref board.unusedJokers, ref board.hand);

                // take the last thrown tile from the dropped tiles stack(graphically)
                if (GameContext.DroppedTilesStack.Count() > 0)
                    RummikubGameView.GlobalRummikubGameViewContext.Controls.Remove(GameContext.DroppedTilesStack.Peek());

                // generate computer thrown tile in Stack
                int[] tile_in_dropped_tiles_location = { Constants.DroppedTileLocation, Constants.DroppedTileLocation };
                VisualTile dropped_tile = new VisualTile(optimal_dropped_tile.Color, optimal_dropped_tile.Number, tile_in_dropped_tiles_location);
                GameContext.GenerateComputerThrownTile(dropped_tile.VisualTileData.TileData);
                
                // returns true because better option found
                return (true);
            }
            // board.partial_sets = CreatePartialSets(ref board.hand);
            return (false);
        }

        // -----------------------------------------------------------------
        // called every time computer have to play, it checks if tile
        // the other player dropped helps the computer for better sequences.
        // if didnt option found take a tile from stack and check again.
        // -----------------------------------------------------------------
        public void ComputerPlay(Tile to_be_replaced)
        {
            try
            {
                // humanPlayer dropped tile, not giving better result 
                if (to_be_replaced != null && !BetterArrangementFound(to_be_replaced))
                {
                    Tile tile = GameContext.Pool.GetTile();
                    if (tile == null) // pool is empty
                        return;

                    // better option with tile from pool
                    if (!BetterArrangementFound(tile))
                    {
                        // if partial set found from dropped tile stack
                        if (board.hand.Count() > 1)
                        {
                            if (tile.Number == 0)
                            {
                                board.unusedJokers.Add(tile);
                            }
                            else
                            {
                                board.hand.Add(tile);
                            }

                            // board.partial_sets = CreatePartialSets(ref board.hand);
                            // AddJokers();

                            if (board.hand.Count() > 0)
                            {
                                Random rnd = new Random();
                                int random_tile_to_drop_index = rnd.Next(board.hand.Count());
                                Tile random_tile_to_drop = board.hand[random_tile_to_drop_index];
                                board.hand.Remove(random_tile_to_drop);
                                GameContext.GenerateComputerThrownTile(random_tile_to_drop);
                            }
                            // hand cannot be 0. if it is, it means that the tile we dropped is not part of a set
                        }
                        else
                        {
                            // if all tiles in partial sets
                            if (board.hand.Count() == 0 && board.partialSets.Count() > 0)
                            {
                                // if tile from pool didnt gave us better result drop random tile from partial sets
                                Random rnd_partial_sets_index = new Random();
                                bool partial_sets_null = true;
                                Tile random_tile_to_drop = null;
                                int random_tile_to_drop_index = 0;

                                while (partial_sets_null)
                                {
                                    random_tile_to_drop_index = rnd_partial_sets_index.Next(board.partialSets.Count());
                                    random_tile_to_drop = (Tile)board.partialSets[random_tile_to_drop_index].Tile1;
                                    if (random_tile_to_drop != null)
                                    {
                                        partial_sets_null = false;
                                    }
                                }

                                if (tile.Number == 0)
                                    board.unusedJokers.Add(tile);
                                else
                                    board.hand.Add(tile);

                                board.hand.Add(board.partialSets[random_tile_to_drop_index].Tile2);
                                board.partialSets.RemoveAt(random_tile_to_drop_index);
                                GameContext.GenerateComputerThrownTile(random_tile_to_drop);
                            }
                            else
                            {
                                // if tile from pool didnt gave us better result drop random tile from hand
                                Random rnd_hand_index = new Random();
                                bool hand_null = true;
                                Tile random_tile_to_drop = null;
                                int random_tile_to_drop_index = 0;

                                while (hand_null && board.hand.Count() > 0)
                                {
                                    random_tile_to_drop_index = rnd_hand_index.Next(board.hand.Count());
                                    random_tile_to_drop = board.hand[random_tile_to_drop_index];
                                    if (random_tile_to_drop != null)
                                    {
                                        hand_null = false;
                                    }
                                }

                                if (board.hand.Count() > 0)
                                    board.hand.RemoveAt(random_tile_to_drop_index);

                                if (tile.Number == 0)
                                    board.unusedJokers.Add(tile);
                                else
                                    board.hand.Add(tile);

                                GameContext.GenerateComputerThrownTile(random_tile_to_drop);
                            }
                        }
                    }
                }

                // updating partial-set after taking tiles
                board.partialSets = GameLogic.CreatePartialSets(ref board.hand, board.partialSets);

                // done in both cases(better option or not)
                GameContext.CurrentTurn = Constants.HumanPlayerTurn;
                RummikubGameView.GlobalGameIndicatorLbl.Text = RummikubGameView.TakeTileFromPoolStackMsg;
                GameContext.HumanPlayer.board.TookCard = false;
                GameContext.ComputerPlayer.board.ClearBoard();

                if (RummikubGameView.ShowComputerTilesToggle)
                    GameContext.ComputerPlayer.board.GenerateBoard();

                // if the game is over, and the computer won
                if (board.CheckWinner() == true && GameContext.GameOver == false)
                {
                    MessageBox.Show("Computer Won!");
                    RummikubGameView.GlobalGameIndicatorLbl.Text = "Game Over - Computer Won";
                    GameContext.HumanPlayer.board.DisableBoard();
                    
                    if (GameContext.DroppedTilesStack.Count > 0)
                        GameContext.DroppedTilesStack.Peek().Enabled = false;
                    GameContext.GameOver = true;
                }
            }
            catch (EmptyPoolException)
            {
                return;
            }
        }
    }
}
