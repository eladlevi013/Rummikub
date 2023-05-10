using RummikubGame.Utilities;
using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace rummikubGame
{
    /*
        This class represents tile slot, which where the tiles are located.
        we need them in the game, in order to know where its legal to put a card on.
        and if we didnt put the card in a good location, it will automatically find the 
        closest location and move the card to that location(functionality of that is in the VisualTile class.)
    */

    [Serializable]
    public class Slot
    {
        [NonSerialized]
        private Button slotButton;
        private bool slotState;

        public Slot()
        {
            slotState = Constants.Available;
            slotButton = new Button();
        }

        public Button SlotButton
        {
            get { return slotButton; }
            set { slotButton = value; }
        }

        public bool SlotState
        {
            get { return slotState; }
            set { slotState = value; }
        }
    }
}
