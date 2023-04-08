using System;
using System.Drawing;
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
        private Point _dragStart;
        private Control _control;
        private bool _draggableEnabled;
        private Timer _moveTimer;

        public DraggableComponent(Control control)
        {
            _control = control;
            _draggableEnabled = false;

            // sets the timer
            _moveTimer = new Timer();
            _moveTimer.Interval = 1;
            _moveTimer.Tick += MoveTimer_Tick;
        }

        public Control Control
        {
            get { return _control; }
            set { _control = value; }
        }

        public bool DraggableEnabled
        {
            get { return _draggableEnabled; }
            set { _draggableEnabled = value; }
        }

        public void SetDraggable(bool draggable)
        {
            _draggableEnabled = draggable;

            // if draggable is true, add the event handlers
            if (draggable)
            {
                _control.MouseDown += StartDragging;
                _control.MouseUp += StopDragging;
            }
            else
            {
                _control.MouseDown -= StartDragging;
                _control.MouseUp -= StopDragging;
            }
        }

        public void StartDragging(object sender, MouseEventArgs e)
        {
            IsCurrentlyDragging = true;
            _dragStart = e.Location;

            _moveTimer.Start();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            if (IsCurrentlyDragging)
            {
                Point screenPos = Cursor.Position;
                Point clientPos = _control.PointToClient(screenPos);
                int deltaX = clientPos.X - _dragStart.X;
                int deltaY = clientPos.Y - _dragStart.Y;
                _control.Location = new Point(_control.Location.X + deltaX, _control.Location.Y + deltaY);
            }
        }


        public void StopDragging(object sender, MouseEventArgs e)
        {
            IsCurrentlyDragging = false;

            // Stopping timer
            _moveTimer.Stop();
            _moveTimer.Dispose();
        }
    }
}
