using NextCapture.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextCapture.Input.Hotkey
{
    public delegate void HotKeyEvent(Hotkey sender, HotkeyEventArgs e);

    public static class HotkeyManager
    {
        private static Dictionary<string, Hotkey> hkDict;
        private static SynchronizationContext syncContext;

        static HotkeyManager()
        {
            Init();
        }

        public static void Init()
        {
            if (syncContext == null)
            {
                syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
                hkDict = new Dictionary<string, Hotkey>();

                Keyboard.Init();
                Keyboard.Hook.KeyDown += Hook_KeyDown;
            }
        }

        public static bool Register(string hkName, Hotkey hk)
        {
            if (!hkDict.ContainsKey(hkName))
            {
                hk.Name = hkName;
                hkDict[hk.Name] = hk;

                return true;
            }

            return false;
        }

        public static bool UnRegister(string hkName)
        {
            if (hkDict.ContainsKey(hkName))
            {
                hkDict.Remove(hkName);

                return true;
            }

            return false;
        }

        private static void Hook_KeyDown(object sender, KeyboardHookEventArgs e)
        {
            IntPtr activeWnd = UnsafeNativeMethods.GetForegroundWindow();
            IntPtr focus = UnsafeNativeMethods.GetFocus();

            foreach (var hk in GetAvailableHotkey())
            {
                // 핫키 보조키와 눌린 보조키가 없거나, 있는경우 마지막 키코드가 보조키여서는 안됨.
                // 핫키 대상이 글로벌이거나, 아닌경우 대상과 일치하는지 확인.
                if (((!hk.HasModifierKey && Keyboard.ModifierKeys == VKeys.None) ||
                    (hk.HasModifierKey && !Keyboard.IsModifierKey(e.KeyCode))) &&
                    (hk.IsGlobal || (!hk.IsGlobal && (hk.Target == activeWnd || hk.Target == focus))))
                {
                    var arg = new HotkeyEventArgs(activeWnd);
                    if (hk.Target == focus && focus != IntPtr.Zero)
                        arg.Target = focus;

                    syncContext.Send((o) => hk.Action?.Invoke(hk, arg), null);

                    if (arg.Handled && !hk.HasModifierKey && hk.SubKeys.Contains(e.KeyCode))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = arg.Handled;
                    }
                }
            }
        }

        private static IEnumerable<Hotkey> GetAvailableHotkey()
        {
            foreach (var hk in hkDict.Values)
            {
                if (hk.Enabled)
                {
                    bool flag = true;

                    if (hk.ModifierKey.HasFlag(VKeys.Shift) && !Keyboard.IsShift) flag = false;
                    if (hk.ModifierKey.HasFlag(VKeys.Control) && !Keyboard.IsControl) flag = false;
                    if (hk.ModifierKey.HasFlag(VKeys.Alt) && !Keyboard.IsAlt) flag = false;

                    if (flag)
                    {
                        foreach (var vk in hk.SubKeys)
                        {
                            if (!Keyboard.IsPressed(vk))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }

                    if (flag)
                        yield return hk;
                }
            }
        }
    }
}
