namespace rummikubGame
{
    public interface IBoard
    {
        void GenerateBoard();
        void ClearBoard();
        bool CheckWinner();
    }
}
