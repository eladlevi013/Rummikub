using System;

namespace rummikubGame.Models
{
    /*
        This class represents PartialSet, which is a set of two cards,
        which are missed another single card in order to be a valid set.
        an example of that can be, {1,2}, {1,3}, {2,2} and so on
        (of course the colors match accordingly to the rules of the game.)
        we use that class mainly in the Logic of the game while finding optimal sets.
    */

    [Serializable]
    public class PartialSet
    {
        private Tile tile1 = null;
        private Tile tile2 = null;

        public PartialSet(Tile tile1, Tile tile2)
        {
            this.tile1 = tile1;
            this.tile2 = tile2;
        }

        public Tile Tile1
        {
            get { return tile1; }
            set { tile1 = value; }
        }

        public Tile Tile2
        {
            get { return tile2; }
            set { tile2 = value; }
        }

        public void SortPartialSet()
        {
            if (tile1.Number > tile2.Number)
            {
                SwapTiles();
            }
        }

        private void SwapTiles()
        {
            Tile temp = tile1;
            tile1 = tile2;
            tile2 = temp;
        }
    }
}
