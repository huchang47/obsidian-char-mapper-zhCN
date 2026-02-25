using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TekkenInputMethod.Core
{
    public class KeyboardHook
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        
        public event EventHandler<KeyPressedEventArgs> KeyPressed;
        
        public void HookKeyboard()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }
        
        public void UnhookKeyboard()
        {
            UnhookWindowsHookEx(_hookID);
        }
        
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;
                
                // 检测 Shift 键状态
                bool isShiftPressed = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
                
                var args = new KeyPressedEventArgs(key, isShiftPressed);
                KeyPressed?.Invoke(this, args);
                
                // 如果事件处理程序标记为已处理，则阻止原始按键传递
                if (args.Handled)
                {
                    return (IntPtr)1;
                }
            }
            
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
    
    public class KeyPressedEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool Handled { get; set; }
        public bool IsShiftPressed { get; }
        
        public KeyPressedEventArgs(Keys key, bool isShiftPressed = false)
        {
            Key = key;
            IsShiftPressed = isShiftPressed;
            Handled = false;
        }
    }
}
