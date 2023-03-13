using rummikubGame.Draggable.Elements;
using System;

namespace rummikubGame
{
    [Serializable]
    public class VisualTile : Tile
    {
        [NonSerialized]
        private DraggableButton tileButton;
        private int tag;
        private int[] slotLocation { get; set; }

        public VisualTile(int color, int number, int[] slot_location) : base(color, number)
        {
            this.slotLocation = slot_location;
            this.tileButton = new DraggableButton();
        }

        public VisualTile(int color, int number, int[] slot_location, int tag): base(color, number)
        {
            this.tag = tag;
            this.slotLocation = slot_location;
            this.tileButton = new DraggableButton();
        }

        public DraggableButton TileButton
        {
            get { return tileButton; }
            set { tileButton = value; }
        }

        public int Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public int[] SlotLocation
        {
            get { return slotLocation; }
            set { slotLocation = value; }
        }
    }
}
