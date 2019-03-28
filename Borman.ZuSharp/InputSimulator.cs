using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Borman.ZuSharp
{
    static class InputSimulator
    {
        public static void PressKey(Keys k)
        {
            PressKeyInternal(k, true);
            PressKeyInternal(k, false);
        }

        public static void SendChar(Char c)
        {
            SendCharInternal(c, true);
            SendCharInternal(c, false);
        }

        
        private static void PressKeyInternal(Keys k, bool down)
        {
            User32.INPUT[] inputs = new User32.INPUT[] {
                new User32.INPUT{
                    type = User32.SendInputEventType.InputKeyboard
                }
            };

            inputs[0].mkhi.ki.wVk = (byte)k;
            inputs[0].mkhi.ki.wScan = 0;
            inputs[0].mkhi.ki.time = 0;

            uint flags = down ? 0 : User32.KEYEVENTF_KEYUP;
            if ((33 <= (byte)k && (byte)k <= 46) || (91 <= (byte)k) && (byte)k <= 93)
            {
                flags |= User32.KEYEVENTF_EXTENDEDKEY;
            }

            inputs[0].mkhi.ki.dwFlags = flags;

            inputs[0].mkhi.ki.dwExtraInfo = User32.GetMessageExtraInfo();

            uint result = User32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(User32.INPUT)));

            if (result != inputs.Length)
            {
                throw new Exception("PressKeyInternal failed with error " + Marshal.GetLastWin32Error());
            }
        }

        private static void SendCharInternal(char c, bool down)
        {
            User32.INPUT[] inputs = new User32.INPUT[] {
                new User32.INPUT{
                    type = User32.SendInputEventType.InputKeyboard
                }
            };

            inputs[0].mkhi.ki.wVk = 0;
            inputs[0].mkhi.ki.wScan = c;
            inputs[0].mkhi.ki.time = 0;
            inputs[0].mkhi.ki.dwFlags = (down ? 0 : User32.KEYEVENTF_KEYUP) | User32.KEYEVENTF_UNICODE;
            inputs[0].mkhi.ki.dwExtraInfo = User32.GetMessageExtraInfo();


            uint result = User32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(User32.INPUT)));

            if (result != inputs.Length)
            {
                throw new Exception("PressKeyInternal failed with error " + Marshal.GetLastWin32Error());
            }
        }
    }

}
