using System.Reflection;
using System.Windows.Forms;

namespace rummikubGame.Draggable.Elements
{
    public class DraggableButton : DraggableComponent
    {        
        public DraggableButton() : base(new Button()) 
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(control, true, null);
        }

        public Button GetButton()
        {
            return (Button)control;
        }
    }
}
