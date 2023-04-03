using System.Windows.Forms;

namespace rummikubGame.Models
{
    public interface IDraggable
    {
        void SetDraggable(bool draggable);
        void StartDragging(object sender, MouseEventArgs e);
        void Dragging(object sender, MouseEventArgs e);
        void StopDragging(object sender, MouseEventArgs e);
    }
}