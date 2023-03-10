using System.Windows.Forms;

namespace rummikubGame.Draggable.Elements
{
    public class DraggableButton: DraggableComponent
    {        
        public DraggableButton() : base(new Button()) { }

        public Button GetButton()
        {
            return (Button)control;
        }
    }
}
