using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class TileButton: Tile
    {
        private Button tile_button;
        private int[] location;

        public TileButton(int color, int number, int[] location): base(color, number)
        {
            this.location = location;
            tile_button = new Button();
        }

        public Button getTileButton()
        {
            return this.tile_button;
        }

        public int[] getLocation()
        {
            return this.location;
        }

        public void setLocation(int[] location)
        {
            this.location = location;
        }

        public string ToString()
        {
            return "Color: " + this.color + ", Value: " + this.number + ", location: [" + this.location[0] + ", " + this.location[1] + "]";
        }
    }
}
