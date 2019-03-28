using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ownskit.Utils;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;
using System.Runtime.InteropServices;

namespace Borman.ZuSharp
{
    public class ZuLogic : IDisposable
    {        
        public ILogicContainer Container { get; set; }

        private KeyboardListener KListener { get; set; }
        private RawKeyEventHandler DownHandler { get; set; }
        private RawKeyEventHandler UpHandler { get; set; }

        public ZuLogicSettings Settings { get; set; }
        private Layout ActiveLayout { get; set; }
        private ContextState Context { get; set; }

        public DateTime LastKeyDownOrRestart { get; set; }

        public ZuLogic()
        {
            LastKeyDownOrRestart = DateTime.Now;
        }

        private void DoStart()
        {
            LoadConfiguration();
            LoadLayout();
            DownHandler = new RawKeyEventHandler(KListener_KeyDown);
            UpHandler = new RawKeyEventHandler(KListener_KeyUp);

            Context = new ContextState
            {
                LastCombinationKeys = new List<KeyPressInfo>(),
                NextUpKeyToIgnore = null
            };

            KListener = new KeyboardListener();
            KListener.KeyDown += DownHandler;
            KListener.KeyUp += UpHandler;

            if (Settings.UseScrollLock && !IsActive)
            {
                Activate();
            }
        }

        public bool IsActive
        {
            get
            {
                if (!Started)
                {
                    return false;
                }

                if (!Settings.UseScrollLock)
                {
                    return true;
                }

                return Control.IsKeyLocked(Keys.Scroll);
            }
        }

        private IntPtr GetCurrentFocusWindow()
        {
            IntPtr foregroundWindow = User32.GetForegroundWindow();
            uint foregroundThreadId = User32.GetWindowThreadId(foregroundWindow);
            uint currentThreadId = Kernel32.GetCurrentThreadId();

            if (User32.AttachThreadInput(currentThreadId, foregroundThreadId, true))
            {
                IntPtr hwndFocused = User32.GetFocus();
                if (hwndFocused != IntPtr.Zero)
                {
                    return hwndFocused;
                }

                User32.AttachThreadInput(currentThreadId, foregroundThreadId, false);
            }

            return foregroundWindow;
        }

        public bool IsTargetLayout
        {
            get
            {
                uint currentWindowThreadId = User32.GetWindowThreadId(GetCurrentFocusWindow());
                IntPtr layout = User32.GetKeyboardLayout(currentWindowThreadId);
                
                uint lang = User32.LoWord(layout);
                bool matchingLayout = lang == Settings.BindToWindowsLayout;
                return matchingLayout;
            }
        }

        private bool Started
        {
            get
            {
                return KListener != null;
            }
        }

        private void DoStop()
        {
            if (Started)
            {
                KListener.KeyDown -= DownHandler;
                KListener.KeyUp -= UpHandler;
                KListener.Dispose();
                KListener = null;
            }
        }

        private void LoadLayout()
        {
            try
            {
                string folder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Layouts");
                string file = Path.Combine(folder, Settings.LayoutFile);

                if (Settings.GermanKeyboard)
                {
                    string extention = Path.GetExtension(file);
                    string germanLayoutFile = file.Substring(0, file.Length - extention.Length) + ".QWERTZ" + extention;

                    if (File.Exists(germanLayoutFile))
                    {
                        file = germanLayoutFile;
                    }
                }
                ActiveLayout = Layout.LoadLayout(file);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem loading layout: " + ex.Message, ex);
            }
        }

        private void LoadConfiguration()
        {
            ConfigurationManager.RefreshSection("appSettings");
            Settings = new ZuLogicSettings
            {
                 LayoutFile = ConfigurationManager.AppSettings["LayoutFile"],
                 BindToWindowsLayout = uint.Parse(ConfigurationManager.AppSettings["BindToWindowsLayout"]),
                 UseScrollLock = bool.Parse(ConfigurationManager.AppSettings["UseScrollLock"]),
                 UseEscape = bool.Parse(ConfigurationManager.AppSettings["UseEscape"]),
                 GermanKeyboard = bool.Parse(ConfigurationManager.AppSettings["GermanKeyboard"]),
            };
        }

        private void HandleKeyDown(RawKeyEventArgs args)
        {
            LastKeyDownOrRestart = DateTime.Now;
            // Container.Log(false, "{0} down", args.Key);            

            if (Context.IsSendingKey)
            {
                return;
            }

            if (!IsActive)
            {
                return;
            }
            
            Context.NextUpKeyToIgnore = null;

            if (Settings.UseEscape && args.Key == Key.Escape)
            {
                List<char> chars;
                List<KeyPressInfo> keysUsedForLastLetter;
                bool effectiveEscape = EscapeCombination(Context.LastCombinationKeys, out chars, out keysUsedForLastLetter);
                
                if (effectiveEscape)
                {
                    args.IsHandled = true;
                    SendBackSpace();
                    SendChars(chars);
                }

                Context.LastCombinationKeys = keysUsedForLastLetter;

                LogState();

                return;
            }

            var newKey = new KeyPressInfo
            {
                Key = args.Key,
                CapsLockPressed = Control.IsKeyLocked(Keys.CapsLock),
                ShiftPressed = User32.IsAsyncKeyPressed(Keys.LShiftKey, Keys.RShiftKey),
                AltPressed = User32.IsAsyncKeyPressed(Keys.LMenu, Keys.RMenu),
                ControlPressed = User32.IsAsyncKeyPressed(Keys.LControlKey, Keys.RControlKey),
                WndPressed = User32.IsAsyncKeyPressed(Keys.LWin, Keys.RWin),
                FocusedWindowHandler = User32.GetForegroundWindow(),
                Escaped = false
            };

            Context.LastCombinationKeys.Add(newKey);

            //if (newKey.Key != System.Windows.Input.Key.LeftCtrl && newKey.Key != System.Windows.Input.Key.LWin && newKey.Key != System.Windows.Input.Key.LeftAlt && newKey.Key != System.Windows.Input.Key.LeftShift)
            //{
            //    int i = 6;
            //}

            if (Context.LastCombinationKeys.Count > ActiveLayout.MaxCompinationLength)
            {
                Context.LastCombinationKeys.RemoveAt(0);
            }

            KeyCombination combination = FindNewKeyCombination();

            if (combination != null)
            {
                if (!IsTargetLayout)
                {
                    return;
                }

                args.IsHandled = true;
                Context.NextUpKeyToIgnore = args.Key;
                if (combination.Keys.Count > 1)
                {
                    SendBackSpace();
                }

                SendChar(GetCombinationChar(combination, Context.LastCombinationKeys[0], false));
                
                Container.Log(false, "Sending {0}", combination.CharacterLowerCase);
            }

            LogState();
        }

        private void LogState()
        {
            //var keys =
            //    (
            //                from keyInfo in Context.LastCombinationKeys
            //                select keyInfo.Key
            //    ).ToList();

            //Container.Log(false, "Context keys: {0}", KeyCombination.GetKeyCombinationHash(keys));
        }



        private bool EscapeCombination(
            List<KeyPressInfo> combinationPressInfos,
            out List<char> chars,
            out List<KeyPressInfo> keysUsedForLastLetter)
        {
            chars = new List<char>();
            keysUsedForLastLetter = new List<KeyPressInfo>();
            List<KeyPressInfo> pressInfoCopy = new List<KeyPressInfo>(combinationPressInfos);

            // no combinatino to escape
            if (pressInfoCopy.Count == 0)
            {
                return false;
            }

            // already escaped char
            if (pressInfoCopy[0].Escaped)
            {
                return false;
            }
            
            // escape single char
            if (pressInfoCopy.Count == 1)
            {
                var combination = FindCombination(pressInfoCopy[0]);

                // cannot escape
                if (!combination.EscapedCharacterLowerCase.HasValue || !combination.EscapedCharacterUpperCase.HasValue)
                {
                    return false;
                }
                else // can escape
                {
                    keysUsedForLastLetter = pressInfoCopy;
                    keysUsedForLastLetter[0].Escaped = true;

                    chars.Add(GetCombinationChar(combination, pressInfoCopy[0], true));
                    return true;
                }
            }

            // escape sequence
            pressInfoCopy.ForEach(k => k.Escaped = false);
            bool isfirstKey = true;

            while (pressInfoCopy.Count > 0)
            {
                var combination = FindCombination(pressInfoCopy);

                if (combination == null || isfirstKey)
                {
                    isfirstKey = false;

                    var oldestCharCombination = FindCombination(pressInfoCopy[0]);

                    if (oldestCharCombination != null)
                    {
                        chars.Add(GetCombinationChar(oldestCharCombination, pressInfoCopy[0], false));
                    }

                    pressInfoCopy.RemoveAt(0);
                    continue;
                }

                chars.Add(GetCombinationChar(combination, pressInfoCopy[0], false));
                keysUsedForLastLetter = pressInfoCopy;
                break;
            }

            return true;
        }
 
        private KeyCombination FindNewKeyCombination()
        {
            IntPtr currentFocusedWindow = Context.LastCombinationKeys.Count > 0 ?
                Context.LastCombinationKeys[Context.LastCombinationKeys.Count - 1].FocusedWindowHandler : IntPtr.Zero;

            while (Context.LastCombinationKeys.Count > 0)
            {
                var oldestKey = Context.LastCombinationKeys[0];

                if (oldestKey.ControlPressed || oldestKey.AltPressed || oldestKey.WndPressed)
                {
                    Context.LastCombinationKeys.RemoveAt(0);
                    continue;
                }

                if (oldestKey.FocusedWindowHandler != currentFocusedWindow){
                    Context.LastCombinationKeys.RemoveAt(0);
                    continue;
                }

                KeyCombination combination = FindCombination(Context.LastCombinationKeys);
                if (combination!=null)
                {
                    return combination;
                }                

                Context.LastCombinationKeys.RemoveAt(0);
            }

            return null;
        }

        private KeyCombination FindCombination(KeyPressInfo combinationPressInfo)
        {
            return FindCombination(new List<KeyPressInfo> { combinationPressInfo });
        }

        private KeyCombination FindCombination(List<KeyPressInfo> combinationPressInfos)
        {
            var keys = (from k in combinationPressInfos select k.Key).ToList();

            string combinationHash = KeyCombination.GetKeyCombinationHash(keys);

            if (ActiveLayout.CombinationsDict.ContainsKey(combinationHash))
            {
                return ActiveLayout.CombinationsDict[combinationHash];
            }

            return null;
        }

        private char GetCombinationChar(KeyCombination combination, KeyPressInfo firstCombinationKey , bool escaped)
        {
            if (escaped)
            {
                bool useUpper;
                if (combination.IsEscapedCharacterCapsLockSensitive)
                {
                    useUpper = firstCombinationKey.ShiftPressed && !firstCombinationKey.CapsLockPressed ||
                                !firstCombinationKey.ShiftPressed && firstCombinationKey.CapsLockPressed;
                }
                else
                {
                    useUpper = firstCombinationKey.ShiftPressed;
                }

                return useUpper ? combination.EscapedCharacterUpperCase.Value : combination.EscapedCharacterLowerCase.Value;
            }
            else
            {
                bool useUpper;
                if (combination.IsCharacterCapsLockSensitive)
                {
                    useUpper = firstCombinationKey.ShiftPressed && !firstCombinationKey.CapsLockPressed ||
                                !firstCombinationKey.ShiftPressed && firstCombinationKey.CapsLockPressed;
                }
                else
                {
                    useUpper = firstCombinationKey.ShiftPressed;
                }

                return useUpper ? combination.CharacterUpperCase : combination.CharacterLowerCase;

            }
        }

        private void SendBackSpace()
        {
            Context.IsSendingKey = true;
            InputSimulator.PressKey(Keys.Back);
            Context.IsSendingKey = false;
        }

        private void SendChar(char c)
        {
            SendChars(new List<char> { c });
        }

        private void SendChars(List<char> chars)
        {
            Context.IsSendingKey = true;
            chars.ForEach( c=> InputSimulator.SendChar(c));
            Context.IsSendingKey = false;
        }

        private void HandleKeyUp(RawKeyEventArgs args)
        {
            if (Context.IsSendingKey)
            {
                return;
            }

            if (!IsActive)
            {
                return;
            }

            if (args.Key == Context.NextUpKeyToIgnore)
            {
                args.IsHandled = true;
                Context.NextUpKeyToIgnore = null;
            }
        }


        public class ZuLogicSettings
        {
            public string LayoutFile { get; set; }
            public uint BindToWindowsLayout { get; set; }
            public bool UseScrollLock { get; set; }
            public bool UseEscape { get; set; }
            public bool GermanKeyboard { get; set; }            
        }

        private class KeyPressInfo{
            public Key Key { get; set; }
            public bool ShiftPressed { get; set; }
            public bool CapsLockPressed { get; set; }
            public bool AltPressed { get; set; }
            public bool ControlPressed { get; set; }
            public bool WndPressed { get; set; }
            public IntPtr FocusedWindowHandler { get; set; }
            public bool Escaped { get; set; }
        }

        private class ContextState
        {
            public List<KeyPressInfo> LastCombinationKeys { get; set; }
            public Key? NextUpKeyToIgnore { get; set; }
            public bool IsSendingKey { get; set; }
        }

        #region Wrappers

        public void Start()
        {
            try
            {
                if (Container == null)
                {
                    throw new Exception("Cannot start without Container");
                }

                DoStart();
            }
            catch (Exception ex)
            {
                try
                {
                    Stop();
                    if (Container != null)
                    {
                        Container.Log(true, "Cannot start logic: " + ex.Message);
                    }

                }
                catch { }
            }
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            try
            {
                DoStop();
            }
            catch (Exception ex)
            {
                try
                {
                    if (Container != null)
                    {
                        Container.Log(true, "Cannot stop logic: " + ex.Message);
                    }
                }
                catch { }
            }
        }

        public void ToggleActivate()
        {
            if (IsActive)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }

        }

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            if (Settings.UseScrollLock)
            {
                InputSimulator.PressKey(Keys.Scroll);
            }
            else
            {
                Start();
            }
        }

        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            if (Settings.UseScrollLock)
            {
                InputSimulator.PressKey(Keys.Scroll);
            }
            else
            {
                Stop();
            }
        }

        void KListener_KeyUp(object sender, RawKeyEventArgs args)
        {
            try
            {
                HandleKeyUp(args);
            }
            catch (Exception ex)
            {
                try
                {
                    if (Container != null)
                    {
                        Container.Log(true, "Generic error during KeyUp handling: " + ex.Message);
                    }
                }
                catch { }
            }
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            try
            {
                HandleKeyDown(args);
            }
            catch (Exception ex)
            {
                try
                {
                    if (Container != null)
                    {
                        Container.Log(true, "Generic error during KeyDown handling: " + ex.Message);
                    }
                }
                catch { }
            }
        }


        #endregion

        #region IDisposable

        public void Dispose()
        {
            Stop();
        }

        #endregion



        
    }


}


