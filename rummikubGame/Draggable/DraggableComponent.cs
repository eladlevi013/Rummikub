using Rummikub;
using rummikubGame.Models;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame.Draggable
{
    public class DraggableComponent
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

        Timer moveTimer;

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
                control.MouseUp += StopDragging;
            }
            else
            {
                control.MouseDown -= StartDragging;
                control.MouseUp -= StopDragging;
            }
        }

        public void StartDragging(object sender, MouseEventArgs e)
        {
            IsCurrentlyDragging = true;
            dragStart = e.Location;

            moveTimer = new Timer();
            moveTimer.Interval = 10; // update every 10 milliseconds
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            if (IsCurrentlyDragging)
            {
                Point screenPos = Cursor.Position;
                Point clientPos = control.PointToClient(screenPos);
                int deltaX = clientPos.X - dragStart.X;
                int deltaY = clientPos.Y - dragStart.Y;
                control.Location = new Point(control.Location.X + deltaX, control.Location.Y + deltaY);
            }
        }


        public void StopDragging(object sender, MouseEventArgs e)
        {
            IsCurrentlyDragging = false;

            moveTimer.Stop();
            moveTimer.Dispose();
        }

    }
}
