using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rummikubGame.Models
{
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
    }
}
