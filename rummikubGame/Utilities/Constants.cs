using System.Drawing;

namespace rummikubGame.Utilities
{
    public class Constants
    {
        // Global consts
        public const int COMPUTER_PLAYER_TURN = 0;
        public const int HUMAN_PLAYER_TURN = 1;
        public const int RUMMIKUB_TILES_IN_GAME = 14;
        public const int MAX_POSSIBLE_SEQUENCES_NUMBER = 4;
        public const int DROPPED_TILE_LOCATION = -1;
        public const int BLUE_COLOR = 0;
        public const int BLACK_COLOR = 1;
        public const int YELLOW_COLOR = 2;
        public const int RED_COLOR = 3;
        public const int JOKER_NUMBER = 0;

        // Pool related consts
        public static int NUMBER_OF_TIMES = 2;
        public static int COLORS_COUNT = 4;
        public static int N = 13;

        // Slot related consts
        public const bool AVAILABLE = false;
        public const bool ALLOCATED = true;

        // Colors
        public static Color BACKGROUND_COLOR = System.Drawing.ColorTranslator.FromHtml("#383B9A");
        public static Color COMPUTER_BOARD_COLOR = System.Drawing.ColorTranslator.FromHtml("#454691");

        // Saved game file name
        public static string SAVED_GAME_FILE_NAME = "save.rummikub";
    }
}
