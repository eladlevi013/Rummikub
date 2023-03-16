using RummikubGame.Utilities;
using System;
using System.Windows.Forms;

namespace rummikubGame
{
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
