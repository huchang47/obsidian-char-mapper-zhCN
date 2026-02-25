using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TekkenInputMethod.Core
{
    public class HotkeyManager : IDisposable
    {
        private const int WM_HOTKEY = 0x0312;
        private const int MOD_ALT = 0x0001;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int MOD_WIN = 0x0008;

        private IntPtr _windowHandle;
        private int _currentId = 0;

        public event EventHandler<HotkeyEventArgs>? HotkeyPressed;

        public HotkeyManager(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public bool RegisterHotkey(string hotkeyString, int id)
        {
            try
            {
                var (modifiers, key) = ParseHotkey(hotkeyString);
                return RegisterHotKey(_windowHandle, id, modifiers, (uint)key);
            }
            catch (Exception ex)
            {
                Logger.Error($"注册热键失败: {hotkeyString}", ex);
                return false;
            }
        }

        public bool UnregisterHotkey(int id)
        {
            try
            {
                return UnregisterHotKey(_windowHandle, id);
            }
            catch (Exception ex)
            {
                Logger.Error($"注销热键失败: {id}", ex);
                return false;
            }
        }

        public void UnregisterAll()
        {
            for (int i = 0; i <= _currentId; i++)
            {
                UnregisterHotKey(_windowHandle, i);
            }
            _currentId = 0;
        }

        public void ProcessHotkeyMessage(Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();
                HotkeyPressed?.Invoke(this, new HotkeyEventArgs(id));
            }
        }

        private (uint modifiers, Keys key) ParseHotkey(string hotkeyString)
        {
            uint modifiers = 0;
            Keys key = Keys.None;

            var parts = hotkeyString.Split('+', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                switch (trimmed.ToUpper())
                {
                    case "CTRL":
                    case "CONTROL":
                        modifiers |= MOD_CONTROL;
                        break;
                    case "ALT":
                        modifiers |= MOD_ALT;
                        break;
                    case "SHIFT":
                        modifiers |= MOD_SHIFT;
                        break;
                    case "WIN":
                    case "WINDOWS":
                        modifiers |= MOD_WIN;
                        break;
                    default:
                        if (Enum.TryParse<Keys>(trimmed, true, out var parsedKey))
                        {
                            key = parsedKey;
                        }
                        break;
                }
            }

            return (modifiers, key);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public void Dispose()
        {
            UnregisterAll();
        }
    }

    public class HotkeyEventArgs : EventArgs
    {
        public int Id { get; }

        public HotkeyEventArgs(int id)
        {
            Id = id;
        }
    }
}
