using System.Drawing;

namespace rummikubGame
{
    public interface IBoard
    {
        void GenerateBoard();
        void GenerateTilesToBoard();
        void ClearBoard();
    }
}
