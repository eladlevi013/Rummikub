using System.Drawing;

namespace RummikubGame.Utilities
{
    public class Constants
    {
        // Global consts
        public const int ComputerPlayerTurn = 0;
        public const int HumanPlayerTurn = 1;
        public const int RummikubTilesInGame = 14;
        public const int MaxPossibleSequencesNumber = 4;
        public const int DroppedTileLocation = -1;
        public const int BlueColor = 0;
        public const int BlackColor = 1;
        public const int YellowColor = 2;
        public const int RedColor = 3;
        public const int JokerNumber = 0;

        // Pool related consts
        public const int NumberOfTimes = 2;
        public const int ColorsCount = 4;
        public const int N = 13;

        // Slot related consts
        public const bool Available = false;
        public const bool Allocated = true;

        // Colors
        public static Color BackgroundColor = ColorTranslator.FromHtml("#383B9A");
        public static Color ComputerBoardColor = ColorTranslator.FromHtml("#454691");
        public static Color MainButtonsColor = ColorTranslator.FromHtml("#98734a");

        // Saved game file name
        public static string SavedGameFileName = "save.rummikub";
    }
}
