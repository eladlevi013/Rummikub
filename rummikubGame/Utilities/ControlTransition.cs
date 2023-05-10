using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RummikubGame.Utilities
{
    public static class ControlTransition
    {
        /*
            This class created in order to make it easy to animate
            card from one place to another.
            its used mainly while getting card from pool,
            and auto find the closest while releasing a card.
        */

        public static async void Move(Control control, Point startPoint, Point endPoint)
        {
            int steps = 10; // number of animation steps
            int interval = 10; // interval between animation steps in milliseconds
            float dx = (endPoint.X - startPoint.X) / (float)steps;
            float dy = (endPoint.Y - startPoint.Y) / (float)steps;

            for (int i = 0; i <= steps; i++)
            {
                Point location = new Point((int)Math.Round(startPoint.X + i * dx)
                    , (int)Math.Round(startPoint.Y + i * dy));
                control.Location = location;
                await Task.Delay(interval);
            }
        }

    }
}
