using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public class Tile
    {
        private Button tile_button;
        private int color;
        private int number;
        private int[] location;

        public Tile(int color, int number, int[] location)
        {
            this.color = color;
            this.number = number;
            this.location = location;
            tile_button = new Button();
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
    }
}
