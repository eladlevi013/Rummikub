using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rummikubGame.Models
{
    [Serializable]
    public class VisualTileData
    {
        private int[] _slotLocation;
        private Tile _tileData;

        public int[] SlotLocation
        {
            get { return _slotLocation; }
            set { _slotLocation = value; }
        }

        public Tile TileData
        {
            get { return _tileData; }
            set { _tileData = value; }
        }

        public VisualTileData(int color, int number, int[] slotLocation)
        {
            _tileData = new Tile(color, number);
            _slotLocation = slotLocation;
        }
    }
}
