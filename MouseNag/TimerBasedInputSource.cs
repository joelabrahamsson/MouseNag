using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MouseNag
{
    public interface INag
    {
        void Nag();
    }

    public class SystemInput : ISystemInput
    {
        MousePosition mousePosition = new MousePosition();
        InputTime inputTime = new InputTime();
        public Point GetCursorPosition()
        {
            return mousePosition.GetCoordinates();
        }

        public uint GetLastInputTime()
        {
            return inputTime.GetLastInputTime();
        }
    }

    public class MousePosition
    {
        public Point GetCoordinates()
        {
            POINTAPI position = new POINTAPI();
            GetCursorPos(ref position);

            return new Point(position.X, position.Y);
        }

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(ref POINTAPI lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTAPI
        {
            public int X;
            public int Y;
        }
    }

    [DefaultPropertyAttribute("IdleThreshold")]
    [DefaultEvent("UserActiveEvent")]
    public class InputTime
    {
        LASTINPUTINFO lastInputInfo;
        public uint GetLastInputTime()
        {
            this.lastInputInfo.cbSize = (uint)Marshal.SizeOf(this.lastInputInfo);
            GetLastInputInfo(ref lastInputInfo);
            return lastInputInfo.dwTime;
        }
        [DllImport("User32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }

    public class Timer : ITimer
    {
        public event EventHandler<EventArgs> Tick;

        private DispatcherTimer dispatcherTimer;

        public Timer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = 100.Milliseconds();
            dispatcherTimer.Tick += TimerTick;
            dispatcherTimer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (Tick != null)
                Tick(this, e);
        }
    }

    public interface IInputSource
    {
        event EventHandler<EventArgs> KeyBoardKeyPressed;
        event EventHandler<EventArgs> MouseMoved;
    }

    public interface ITimer
    {
        event EventHandler<EventArgs> Tick;
    }

    public interface ISystemInput
    {
        Point GetCursorPosition();

        uint GetLastInputTime();
    }



    public class MouseNagPresenter
    {
        private INag nagger;
        private bool currentlyUsingMouse;

        public MouseNagPresenter(IInputSource inputSource, INag nagger)
        {
            this.nagger = nagger;
            inputSource.MouseMoved += MouseMoved;
            inputSource.KeyBoardKeyPressed += KeyBoardKeyPressed;
        }

        void KeyBoardKeyPressed(object sender, EventArgs e)
        {
            currentlyUsingMouse = false;
        }

        void MouseMoved(object sender, EventArgs e)
        {
            if (!currentlyUsingMouse)
                nagger.Nag();
            currentlyUsingMouse = true;
        }
    }


    public class TimerBasedInputSource : IInputSource
    {
        private ISystemInput systemInput;
        private Point? mousePosition;
        private uint? lastInputTime;

        public TimerBasedInputSource(ISystemInput systemInput, ITimer timer)
        {
            this.systemInput = systemInput;
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            var newMousePosition = systemInput.GetCursorPosition();
            uint newLastInputTime = systemInput.GetLastInputTime();
            if (MouseHasBeenMoved(newMousePosition) && MouseMoved != null)
                MouseMoved(this, new EventArgs());
            else if(newLastInputTime > lastInputTime && KeyBoardKeyPressed != null)
                KeyBoardKeyPressed(this, new EventArgs());

            lastInputTime = newLastInputTime;
            mousePosition = newMousePosition;
        }

        bool MouseHasBeenMoved(Point newMousePosition)
        {
            return mousePosition.HasValue && newMousePosition != mousePosition;
        }

        public event EventHandler<EventArgs> KeyBoardKeyPressed;
        public event EventHandler<EventArgs> MouseMoved;
    }
}
