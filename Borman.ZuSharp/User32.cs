using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Borman.ZuSharp
{
    public static class User32
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardLayoutName([Out] StringBuilder pwszKLID);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        /// <summary>The GetForegroundWindow function returns a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetGUIThreadInfo(uint idThread, ref GuiThreadInfo lpgui);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool attach);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        public static bool IsAsyncKeyPressed(System.Windows.Forms.Keys vKey)
        {
            return ((int)GetAsyncKeyState(vKey) & 0x8000) != 0;
        }

        public static bool IsAsyncKeyPressed(System.Windows.Forms.Keys vKey1, System.Windows.Forms.Keys vKey2)
        {
            return IsAsyncKeyPressed(vKey1) || IsAsyncKeyPressed(vKey2);
        }

        public static GuiThreadInfo? GetGUIThreadInfo(uint idThread)
        {
            var info = new GuiThreadInfo();
            info.cbSize = Marshal.SizeOf(info);
            if (!GetGUIThreadInfo(0, ref info))
            {
                return null;
            }

            return info;
        }

        public static uint GetWindowThreadId(IntPtr hWnd)
        {
            uint dummyProcessId;
            return User32.GetWindowThreadProcessId(hWnd, out dummyProcessId);
        }

        /// <summary>
        /// The event type contained in the union field
        /// </summary>
        public enum SendInputEventType : int
        {
            /// <summary>
            /// Contains Mouse event data
            /// </summary>
            InputMouse,
            /// <summary>
            /// Contains Keyboard event data
            /// </summary>
            InputKeyboard,
            /// <summary>
            /// Contains Hardware event data
            /// </summary>
            InputHardware
        }

        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004;
        public const uint KEYEVENTF_SCANCODE = 0x0008;

        /// <summary>
        /// Used in mouseData if XDOWN or XUP specified
        /// </summary>
        [Flags]
        public enum MouseDataFlags : uint
        {
            /// <summary>
            /// First button was pressed or released
            /// </summary>
            XBUTTON1 = 0x0001,
            /// <summary>
            /// Second button was pressed or released
            /// </summary>
            XBUTTON2 = 0x0002
        }

        /// <summary>
        /// The flags that a MouseInput.dwFlags can contain
        /// </summary>
        [Flags]
        public enum MouseEventFlags : uint
        {
            /// <summary>
            /// Movement occured
            /// </summary>
            MOUSEEVENTF_MOVE = 0x0001,
            /// <summary>
            /// button down (pair with an up to create a full click)
            /// </summary>
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            /// <summary>
            /// button up (pair with a down to create a full click)
            /// </summary>
            MOUSEEVENTF_LEFTUP = 0x0004,
            /// <summary>
            /// button down (pair with an up to create a full click)
            /// </summary>
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            /// <summary>
            /// button up (pair with a down to create a full click)
            /// </summary>
            MOUSEEVENTF_RIGHTUP = 0x0010,
            /// <summary>
            /// button down (pair with an up to create a full click)
            /// </summary>
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            /// <summary>
            /// button up (pair with a down to create a full click)
            /// </summary>
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            /// <summary>
            /// button down (pair with an up to create a full click)
            /// </summary>
            MOUSEEVENTF_XDOWN = 0x0080,
            /// <summary>
            /// button up (pair with a down to create a full click)
            /// </summary>
            MOUSEEVENTF_XUP = 0x0100,
            /// <summary>
            /// Wheel was moved, the value of mouseData is the number of movement values
            /// </summary>
            MOUSEEVENTF_WHEEL = 0x0800,
            /// <summary>
            /// Map X,Y to entire desktop, must be used with MOUSEEVENT_ABSOLUTE
            /// </summary>
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            /// <summary>
            /// The X and Y members contain normalised Absolute Co-Ords. If not set X and Y are relative
            /// data to the last position (i.e. change in position from last event)
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        /// <summary>
        /// The mouse data structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInputData
        {
            /// <summary>
            /// The x value, if ABSOLUTE is passed in the flag then this is an actual X and Y value
            /// otherwise it is a delta from the last position
            /// </summary>
            public int dx;
            /// <summary>
            /// The y value, if ABSOLUTE is passed in the flag then this is an actual X and Y value
            /// otherwise it is a delta from the last position
            /// </summary>
            public int dy;
            /// <summary>
            /// Wheel event data, X buttons
            /// </summary>
            public uint mouseData;
            /// <summary>
            /// ORable field with the various flags about buttons and nature of event
            /// </summary>
            public MouseEventFlags dwFlags;
            /// <summary>
            /// The timestamp for the event, if zero then the system will provide
            /// </summary>
            public uint time;
            /// <summary>
            /// Additional data obtained by calling app via GetMessageExtraInfo
            /// </summary>
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        /// <summary>
        /// Captures the union of the three three structures.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct MouseKeybdhardwareInputUnion
        {
            /// <summary>
            /// The Mouse Input Data
            /// </summary>
            [FieldOffset(0)]
            public MouseInputData mi;

            /// <summary>
            /// The Keyboard input data
            /// </summary>
            [FieldOffset(0)]
            public KEYBDINPUT ki;

            /// <summary>
            /// The hardware input data
            /// </summary>
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        /// <summary>
        /// The Data passed to SendInput in an array.
        /// </summary>
        /// <remarks>Contains a union field type specifies what it contains </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            /// <summary>
            /// The actual data type contained in the union Field
            /// </summary>
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam);
        public static int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// Key event
        /// </summary>
        public enum KeyEvent : int
        {
            /// <summary>
            /// Key down
            /// </summary>
            WM_KEYDOWN = 256,

            /// <summary>
            /// Key up
            /// </summary>
            WM_KEYUP = 257,

            /// <summary>
            /// System key up
            /// </summary>
            WM_SYSKEYUP = 261,

            /// <summary>
            /// System key down
            /// </summary>
            WM_SYSKEYDOWN = 260
        }

        /// <summary>
        /// Retrieves the High Word of a WParam of a WindowMessage
        /// </summary>
        /// <param name="ptr">The pointer to the WParam</param>
        /// <returns>The unsigned integer for the High Word</returns>
        public static uint HiWord(IntPtr ptr)
        {
            if (((uint)ptr & 0x80000000) == 0x80000000)
                return ((uint)ptr >> 16);
            else
                return ((uint)ptr >> 16) & 0xffff;
        }

        /// <summary>
        /// Retrieves the Low Word of a WParam of a WindowMessage
        /// </summary>
        /// <param name="ptr">The pointer to the WParam</param>
        /// <returns>The unsigned integer for the Low Word</returns>
        public static uint LoWord(IntPtr ptr)
        {
            return (uint)ptr & 0xffff;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GuiThreadInfo
        {
            public int cbSize;
            public uint flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public Rect rcCaret;
        }
    }
}
