using rummikubGame.Models;
using System.Drawing;
using System.Windows.Forms;

namespace rummikubGame.Draggable
{
    public class DraggableComponent : IDraggable
    {
        /*
        The main target of the this class, converting any control
        element(which are components with visual representation).
        to draggable elements.
        */

        public static bool IsCurrentlyDragging = false;
        protected Point dragStart;
        protected Control control;
        protected bool draggableEnabled;

        public DraggableComponent(Control control)
        {
            this.control = control;
            draggableEnabled = false;
        }

        public Control Control
        {
            get { return control; }
            set { control = value; }
        }
        
        public bool DraggableEnabled
        {
            get { return draggableEnabled; }
            set { draggableEnabled = value; }
        }

        public virtual void SetDraggable(bool draggable)
        {
            draggableEnabled = draggable;
            if (draggable)
            {
                control.MouseDown += StartDragging;
                control.MouseMove += Dragging;
                control.MouseUp += StopDragging;
            }
            else
            {
                control.MouseDown -= StartDragging;
                control.MouseMove -= Dragging;
                control.MouseUp -= StopDragging;
            }
        }

        public void StartDragging(object sender, MouseEventArgs e)
        {
            IsCurrentlyDragging = true;
            dragStart = e.Location;
        }

        public void Dragging(object sender, MouseEventArgs e)
        {
            if (IsCurrentlyDragging)
            {
                int deltaX = e.X - dragStart.X;
                int deltaY = e.Y - dragStart.Y;
                Control control = sender as Control;
                control.Location = new Point(control.Location.X 
                    + deltaX, control.Location.Y + deltaY);
            }
        }

        public void StopDragging(object sender, MouseEventArgs e)
        {
            IsCurrentlyDragging = false;
        }
    }
}
