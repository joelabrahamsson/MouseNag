using System;
using MouseNag.InputMonitoring;

namespace MouseNag.Presentation
{
    public class MouseNagPresenter
    {
        private INag nagger;
        private bool currentlyUsingMouse;
        private bool currentlyUsingKeyBoard;

        public MouseNagPresenter(IInputSource inputSource, INag nagger)
        {
            this.nagger = nagger;
            inputSource.MouseMoved += MouseMoved;
            inputSource.KeyBoardKeyPressed += KeyBoardKeyPressed;
        }

        void KeyBoardKeyPressed(object sender, EventArgs e)
        {
            currentlyUsingMouse = false;
            currentlyUsingKeyBoard = true;
        }

        void MouseMoved(object sender, EventArgs e)
        {
            bool wasUsingMouseAlready = currentlyUsingMouse;
            
            currentlyUsingMouse = true;

            if (!wasUsingMouseAlready && currentlyUsingKeyBoard)
                nagger.Nag();

            currentlyUsingKeyBoard = false;
        }
    }
}
