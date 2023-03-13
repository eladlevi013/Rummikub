using System;

namespace rummikubGame
{
    [Serializable]
    public class Tile
    {
        protected int color;
        protected int number;

        public Tile(int color, int number)
        {
            this.color = color;
            this.number = number;
        }

        public Tile Clone(int color, int number)
        {
            return new Tile(color, number);
        }

        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
    }
}
