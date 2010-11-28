using System;

namespace MouseNag.InputMonitoring
{
    public interface IInputSource
    {
        event EventHandler<EventArgs> KeyBoardKeyPressed;
        event EventHandler<EventArgs> MouseMoved;
    }
}
