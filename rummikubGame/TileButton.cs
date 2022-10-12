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
        private int[] slot_location;

        public TileButton(int color, int number, int[] slot_location): base(color, number)
        {
            this.slot_location = slot_location;
            tile_button = new Button();
        }

        public Button getTileButton()
        {
            return this.tile_button;
        }

        public int[] getSlotLocation()
        {
            return this.slot_location;
        }

        public void setSlotLocation(int[] slot_location)
        {
            this.slot_location = slot_location;
        }

        public string ToString()
        {
            return "Color: " + this.color + ", Value: " + this.number + ", slot_location: [" + this.slot_location[0] + ", " + this.slot_location[1] + "]";
        }
    }
}
