using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    [Serializable]
    public class Slot
    {
        // consts
        public const bool AVAILABLE = false;
        public const bool ALLOCATED = true;

        [NonSerialized]
        private Button slot_button; // the actual button of the slot
        private bool slot_state; // false-empty, true-not empty
        public Slot()
        {
            this.slot_state = AVAILABLE;
            this.slot_button = new Button();
        }

        public void changeState(bool state)
        {
            this.slot_state = state;
        }

        public bool getState()
        {
            return this.slot_state;
        }

        public Button getSlotButton()
        {
            return this.slot_button;
        }

        public string ToString()
        {
            return "State: " + (this.slot_state) ;
        }
    }
}