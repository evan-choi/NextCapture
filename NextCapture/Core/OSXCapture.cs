using NextCapture.Input;
using NextCapture.Input.Hotkey;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCapture.Core
{
    public enum CaptureMode
    {
        Unknown = 0,
        FullScreen = 1,
        Drag = 2,
        Window = 4
    }

    public enum SaveMode
    {
        Unknown = 0,
        Desktop = 1,
        Clipboard = 2
    }


    internal class OSXCapture
    {
        public event EventHandler CaptureModeChanged;

        public List<IDataBus<Bitmap>> BitmapBuses { get; } 
            = new List<IDataBus<Bitmap>>();

        public SaveMode SaveMode { get; private set; } = SaveMode.Unknown;

        private CaptureMode _captureMode = CaptureMode.Unknown;
        public CaptureMode CaptureMode
        {
            get
            {
                return _captureMode;
            }
            private set
            {
                _captureMode = value;
                CaptureModeChanged?.Invoke(this, null);
            }
        }

        private BitmapDataBus deskBus;
        private BitmapDataBus clipBus;

        public OSXCapture()
        {
            InitBitmapBuses();
            InitHotkey();
        }

        private void InitHotkey()
        {
            HotkeyManager.Register("Capture_Cancel", new Hotkey()
            {
                SubKeys = new[] { VKeys.Escape },
                Action = new HotKeyEvent(Capture_Cancel)
            });

            HotkeyManager.Register("Capture_Full", new Hotkey()
            {
                ModifierKey = VKeys.Shift,
                SubKeys = new[] { VKeys.D3 },
                Action = new HotKeyEvent(Capture_Full)
            });

            HotkeyManager.Register("Capture_Area", new Hotkey()
            {
                ModifierKey = VKeys.Shift,
                SubKeys = new[] { VKeys.D4 },
                Action = new HotKeyEvent(Capture_Area)
            });
        }

        private void Capture_Cancel(Hotkey sender, HotkeyEventArgs e)
        {
            CaptureMode = CaptureMode.Unknown;
            SaveMode = SaveMode.Unknown;
        }

        private void InitBitmapBuses()
        {
            BitmapBuses.Add(
                deskBus = new DesktopBus()
                {
                    Enabled = true
                });

            BitmapBuses.Add(
                clipBus = new ClipboardBus()
                {
                    Enabled = false
                });
        }

        private void Capture_Area(Hotkey sender, HotkeyEventArgs e)
        {
            StartCapture(CaptureMode.Drag);
        }

        private void Capture_Full(Hotkey sender, HotkeyEventArgs e)
        {
            StartCapture(CaptureMode.FullScreen);
        }

        private bool IsPressedAlt()
        {
            foreach (VKeys key in new[] { VKeys.LMenu, VKeys.LWin })
            {
                if (Keyboard.IsPressed(key))
                    return true;
            }

            return false;
        }

        private void SendBitmap(Bitmap bitmap)
        {
            foreach (var bus in BitmapBuses.Reverse<IDataBus<Bitmap>>())
            {
                if (bus.SendData(bitmap))
                {
                    continue;
                }
            }
        }

        public void StartCapture(CaptureMode mode)
        {
            if (IsPressedAlt())
            {
                clipBus.Enabled = (Keyboard.IsPressed(VKeys.LControlKey));

                CaptureMode = mode;

                switch (CaptureMode)
                {
                    case CaptureMode.Unknown:
                        break;

                    case CaptureMode.FullScreen:
                        break;

                    case CaptureMode.Drag:
                        break;

                    case CaptureMode.Window:
                        break;
                }
            }
        }

        public void EndCapture(Bitmap bitmap)
        {
            
        }
    }
}