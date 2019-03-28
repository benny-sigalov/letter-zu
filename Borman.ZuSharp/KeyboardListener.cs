using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Generic;
using Borman.ZuSharp;

namespace Ownskit.Utils
{
    /// <summary>
    /// Listens keyboard globally.
    /// 
    /// <remarks>Uses WH_KEYBOARD_LL.</remarks>
    /// </summary>
    public class KeyboardListener : IDisposable
    {
        /// <summary>
        /// Creates global keyboard listener.
        /// </summary>
        public KeyboardListener()
        {
            // Dispatcher thread handling the KeyDown/KeyUp events.
            this.dispatcher = Dispatcher.CurrentDispatcher;
            
            // We have to store the LowLevelKeyboardProc, so that it is not garbage collected runtime
            hookedLowLevelKeyboardProc = (User32.LowLevelKeyboardProc)LowLevelKeyboardProc;

            // Set the hook
            hookId = SetHook(hookedLowLevelKeyboardProc);

            // Assign the hronous callback event
            hookedKeyboardCallback = new KeyboardCallback(KeyboardListener_KeyboardCallback);
        }

        private Dispatcher dispatcher;

        /// <summary>
        /// Destroys global keyboard listener.
        /// </summary>
        ~KeyboardListener()
        {
            Dispose();
        }

        /// <summary>
        /// Fired when any of the keys is pressed down.
        /// </summary>
        public event RawKeyEventHandler KeyDown;

        /// <summary>
        /// Fired when any of the keys is released.
        /// </summary>
        public event RawKeyEventHandler KeyUp;

        #region Inner workings

        /// <summary>
        /// Hook ID
        /// </summary>
        private IntPtr hookId = IntPtr.Zero;

        /// <summary>
        /// hronous callback hook.
        /// </summary>
        /// <param name="character">Character</param>
        /// <param name="keyEvent">Keyboard event</param>
        /// <param name="vkCode">VKCode</param>
        private delegate bool KeyboardCallback(User32.KeyEvent keyEvent, int vkCode);

        /// <summary>
        /// Actual callback hook.
        /// 
        /// <remarks>Calls hronously the Callback.</remarks>
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return User32.CallNextHookEx(hookId, nCode, wParam, lParam);
            }



            if (wParam.ToUInt32() == (int)User32.KeyEvent.WM_KEYDOWN ||
                wParam.ToUInt32() == (int)User32.KeyEvent.WM_KEYUP ||
                wParam.ToUInt32() == (int)User32.KeyEvent.WM_SYSKEYDOWN ||
                wParam.ToUInt32() == (int)User32.KeyEvent.WM_SYSKEYUP)
            {


                bool handled = hookedKeyboardCallback.Invoke((User32.KeyEvent)wParam.ToUInt32(), Marshal.ReadInt32(lParam));
                if (handled)
                {
                    return (IntPtr)1;
                }
            }

            return User32.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Event to be invoked hronously (BeginInvoke) each time key is pressed.
        /// </summary>
        private KeyboardCallback hookedKeyboardCallback;

        /// <summary>
        /// Contains the hooked callback in runtime.
        /// </summary>
        private User32.LowLevelKeyboardProc hookedLowLevelKeyboardProc;

        /// <summary>
        /// HookCallback procedure that calls accordingly the KeyDown or KeyUp events.
        /// </summary>
        /// <param name="keyEvent">Keyboard event</param>
        /// <param name="vkCode">VKCode</param>
        /// <param name="character">Character as string.</param>
        /// <returns>Was the event handled</returns>
        bool KeyboardListener_KeyboardCallback(User32.KeyEvent keyEvent, int vkCode)
        {
            RawKeyEventArgs args = null;

            switch (keyEvent)
            {
                // KeyDown events
                case User32.KeyEvent.WM_KEYDOWN:
                    if (KeyDown != null)
                    {
                        args = new RawKeyEventArgs(vkCode, false);
                        dispatcher.Invoke(new RawKeyEventHandler(KeyDown), this, args);
                    }
                    break;
                case User32.KeyEvent.WM_SYSKEYDOWN:
                    if (KeyDown != null)
                    {
                        args = new RawKeyEventArgs(vkCode, true);
                        dispatcher.Invoke(new RawKeyEventHandler(KeyDown), this, args);
                    }
                    break;

                // KeyUp events
                case User32.KeyEvent.WM_KEYUP:
                    if (KeyUp != null)
                    {
                        args = new RawKeyEventArgs(vkCode, false);
                        dispatcher.Invoke(new RawKeyEventHandler(KeyUp), this, args);
                    }
                    break;
                case User32.KeyEvent.WM_SYSKEYUP:
                    if (KeyUp != null)
                    {
                        args = new RawKeyEventArgs(vkCode, true);
                        dispatcher.Invoke(new RawKeyEventHandler(KeyUp), this, args);
                    }
                    break;

                default:
                    break;
            }

            return args != null && args.IsHandled;
        }

        private IntPtr SetHook(User32.LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return User32.SetWindowsHookEx(User32.WH_KEYBOARD_LL, proc, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the hook.
        /// <remarks>This call is required as it calls the UnhookWindowsHookEx.</remarks>
        /// </summary>
        public void Dispose()
        {
            User32.UnhookWindowsHookEx(hookId);
        }

        #endregion
    }

    /// <summary>
    /// Raw KeyEvent arguments.
    /// </summary>
    public class RawKeyEventArgs : EventArgs
    {
        /// <summary>
        /// VKCode of the key.
        /// </summary>
        public int VKCode;

        /// <summary>
        /// WPF Key of the key.
        /// </summary>
        public Key Key;

        /// <summary>
        /// Is the hitted key system key.
        /// </summary>
        public bool IsSysKey;

        /// <summary>
        /// Is the event handled, already
        /// </summary>
        public bool IsHandled;

        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <returns>Returns string representation of this key, if not possible empty string is returned.</returns>
        public override string ToString()
        {
            return VKCode.ToString();
        }

        /// <summary>
        /// Create raw keyevent arguments.
        /// </summary>
        /// <param name="VKCode"></param>
        /// <param name="isSysKey"></param>
        /// <param name="Character">Character</param>
        public RawKeyEventArgs(int VKCode, bool isSysKey)
        {
            this.VKCode = VKCode;
            this.IsSysKey = isSysKey;
            this.Key = System.Windows.Input.KeyInterop.KeyFromVirtualKey(VKCode);
        }

    }

    /// <summary>
    /// Raw keyevent handler.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="args">raw keyevent arguments</param>
    public delegate void RawKeyEventHandler(object sender, RawKeyEventArgs args);





}