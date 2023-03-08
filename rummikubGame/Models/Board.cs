using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rummikubGame
{
    public interface Board
    {
        void GenerateBoard();
        void ClearBoard();
        bool CheckWinner();
    }
}