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
        public static bool operator ==(Tile t1, Tile t2)

        {
            if (ReferenceEquals(t1, t2)) return true;
            if (ReferenceEquals(t1, null)) return false;
            if (ReferenceEquals(t2, null)) return false;
            return t1.number == t2.number && t1.color == t2.color;

        }

        public static bool operator !=(Tile t1, Tile t2)
        {
            return !(t1 == t2);
        }
    }
}
