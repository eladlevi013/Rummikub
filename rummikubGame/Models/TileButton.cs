using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class TileButton: Tile
    {
        // dragging element
        public static bool currently_dragging = false;
        private Point dragStart;

        public bool draggable;
        public int tag;
        [NonSerialized]
        public Button tile_button; // button element of the tile
        private int[] slot_location; // location of the slot of the card

        public TileButton(int color, int number, int[] slot_location) : base(color, number)
        {
            this.draggable = false;
            this.slot_location = slot_location;
            this.tile_button = new Button();
        }

        public TileButton(int color, int number, int[] slot_location, int tag): base(color, number)
        {
            this.tag = tag;
            this.slot_location = slot_location;
            this.tile_button = new Button();
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

        public void setDraggable(bool draggable)
        {
            this.draggable = draggable;
            if(draggable)
            {
                this.tile_button.MouseDown += Button_MouseDown;
                this.tile_button.MouseMove += Button_MouseMove;
                this.tile_button.MouseUp += Button_MouseUp;
            }
            else
            {
                this.tile_button.MouseDown -= Button_MouseDown;
                this.tile_button.MouseMove -= Button_MouseMove;
                this.tile_button.MouseUp -= Button_MouseUp;
            }
        }

        // dragging element
        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            currently_dragging = true;
            dragStart = e.Location;
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (currently_dragging)
            {
                int deltaX = e.X - dragStart.X;
                int deltaY = e.Y - dragStart.Y;
                Button button = sender as Button;
                button.Location = new Point(button.Location.X + deltaX, button.Location.Y + deltaY);
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            currently_dragging = false;
        }
    }
}