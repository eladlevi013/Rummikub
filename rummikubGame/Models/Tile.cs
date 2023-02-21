using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace rummikubGame
{
    [Serializable]
    public class Tile
    {
        protected int color;
        protected int number;

        public Tile Clone(int color, int number)
        {
            return new Tile(color, number);
        }

        public Tile(int color, int number)
        {
            this.color = color;
            this.number = number;
        }

        public int getColor()
        {
            return this.color;
        }

        public int getNumber()
        {
            return this.number;
        }

        public string ToString()
        {
            return "Color: " + this.color + ", Value: " + this.number;
        }
    }
}
