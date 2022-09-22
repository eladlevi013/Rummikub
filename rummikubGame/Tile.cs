using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace rummikubGame
{
    public class Tile
    {
        private Button tile_button;
        private bool is_dropped;
        private int color;
        private int number;
        private int[] location;

        public Tile(int color, int number, int[] location)
        {
            is_dropped = false;
            this.color = color;
            this.number = number;
            this.location = location;
            tile_button = new Button();
        }

        public void setDropped(bool dropped)
        {
            this.is_dropped = dropped;
        }

        public bool isDropped()
        {
            return this.is_dropped;
        }

        public Button getTileButton()
        {
            return this.tile_button;
        }

        public int getColor()
        {
            return this.color;
        }

        public int getNumber()
        {
            return this.number;
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
