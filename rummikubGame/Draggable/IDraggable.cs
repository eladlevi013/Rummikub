using System;
using System.Windows.Forms;

namespace rummikubGame.Models
{
    public interface IDraggable
    {
        /*
            Interface for the draggable component.
            used in the DraggableComponent class.
            and implemented in the DraggableComponent class.
            consists function:
            - SetDraggable
            - StartDragging
            - Dragging_TimerTick
            - StopDragging
        */

        void SetDraggable(bool draggable);
        void StartDragging(object sender, MouseEventArgs e);
        void Dragging_TimerTick(object sender, EventArgs e);
        void StopDragging(object sender, MouseEventArgs e);
    }
}
