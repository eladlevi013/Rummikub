namespace rummikubGame
{
    public interface IBoard
    {
        /*
            Both of the boards of the human-player and the computer-player
            are implementing this interface, in order to server a uniform
            class sturcture in those classes to make it more easy to understand.
            which forces those class to implement those important methods
            that should be in any board, such as:
                - generatingBoard,
                - generating-tiles
                - clear-board.
        */

        void GenerateBoard();
        void GenerateTilesToBoard();
        void ClearBoard();
    }
}
