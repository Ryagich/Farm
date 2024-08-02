namespace Code.Input
{
    public class InputHandler
    {
        public bool ControlPressed { get; private set; }

        private InputHandler(InputKeys keys)
        {
            keys.Control.action.started += _ => ControlPressed = true;
            keys.Control.action.canceled += _ => ControlPressed = false;
        }
    }
}