using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using MouseNag.Helpers;

namespace MouseNag.InputMonitoring
{
    

    public class TimerBasedInputSource : IInputSource, IDisposable
    {
        public event EventHandler<EventArgs> KeyBoardKeyPressed;
        public event EventHandler<EventArgs> MouseMoved;

        private delegate IntPtr HookDelegate(Int32 Code, IntPtr wParam, IntPtr lParam);

        HookDelegate keyBoardDelegate;
        HookDelegate mouseDelegate;
        private IntPtr mouseHandle;
        private IntPtr keyBoardHandle;

        private const Int32 WH_MOUSE_LL = 14;
        private const Int32 WH_KEYBOARD_LL = 13;

        private DispatcherTimer timer;
        private bool mouseMoved;
        private bool keyBoardPressed;

        private bool disposed;

        public TimerBasedInputSource()
        {
            timer = new DispatcherTimer();
            timer.Interval = 100.Milliseconds();
            timer.Tick += TimerTick;
            timer.Start();
            keyBoardDelegate = KeyboardHookDelegate;
            mouseDelegate = MouseHookDelegate;
            keyBoardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, keyBoardDelegate, IntPtr.Zero, 0);
            mouseHandle = SetWindowsHookEx(WH_MOUSE_LL, mouseDelegate, IntPtr.Zero, 0);
        }

        [DllImport("User32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, Int32 nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        private static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);


        [DllImport("User32.dll")]
        private static extern IntPtr SetWindowsHookEx(Int32 idHook, HookDelegate lpfn,
                                                  IntPtr hmod, Int32 dwThreadId);

        private void TimerTick(object sender, EventArgs e)
        {
            if(mouseMoved)
            {
                if (MouseMoved != null)
                {
                    MouseMoved(this, new EventArgs());
                }

                mouseMoved = false;
            }

            if(keyBoardPressed)
            {
                if(KeyBoardKeyPressed != null)
                    KeyBoardKeyPressed(this, new EventArgs());
                keyBoardPressed = false;
            }
        }

        private IntPtr KeyboardHookDelegate(Int32 Code, IntPtr wParam, IntPtr lParam)
        {
            if (Code < 0)
                return CallNextHookEx(keyBoardHandle, Code, wParam, lParam);

            keyBoardPressed = true;
            
            return CallNextHookEx(keyBoardHandle, Code, wParam, lParam);
        }

        private IntPtr MouseHookDelegate(Int32 Code, IntPtr wParam, IntPtr lParam)
        {
            if(Code < 0)
                return CallNextHookEx(mouseHandle, Code, wParam, lParam);
            
            mouseMoved = true;

            return CallNextHookEx(mouseHandle, Code, wParam, lParam);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (keyBoardHandle != IntPtr.Zero)
                    UnhookWindowsHookEx(keyBoardHandle);

                if (mouseHandle != IntPtr.Zero)
                    UnhookWindowsHookEx(mouseHandle);

                disposed = true;
            }
        }

        ~TimerBasedInputSource()
        {
            Dispose(false);
        }
    }
}
