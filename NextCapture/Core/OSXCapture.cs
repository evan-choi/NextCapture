using NextCapture.Input;
using NextCapture.Input.Hotkey;
using NextCapture.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using MouseStruct = NextCapture.Interop.NativeMethods.MOUSEHOOKSTRUCT;
using NextCapture.Interop;
using NextCapture.Input.Mouse;
using System.Runtime.InteropServices;
using System.Media;
using System.Diagnostics;

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

    internal class OSXCapture : IHookFilter<MouseStruct>
    {
        public event EventHandler<CaptureModeChangedEventArgs> CaptureModeChanged;
        public event EventHandler<FocusedWindowEventArgs> FocusedWindowChanged;
        public event EventHandler<Point> BeginDragCapture;
        public event EventHandler<Point> EndDragCapture;

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
                var oldValue = _captureMode;
                _captureMode = value;

                CaptureModeChanged?.Invoke(this, new CaptureModeChangedEventArgs(oldValue, value));
            }
        }

        private bool isDragging = false;
        public bool IsDragging
        {
            get
            {
                return isDragging;
            }
        }

        private IntPtr focusedWindow = IntPtr.Zero;
        public IntPtr FocusedWindowHandle
        {
            get
            {
                return focusedWindow;
            }
        }

        private BitmapDataBus deskBus;
        private BitmapDataBus clipBus;

        private Point dragStartPosition;
        private bool isVirtualDown = false;

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

            HotkeyManager.Register("Capture_Window", new Hotkey()
            {
                ModifierKey = VKeys.Space,
                SubKeys = new[] { VKeys.Space },
                Action = new HotKeyEvent(Capture_Window)
            });
        }

        private void Capture_Window(Hotkey sender, HotkeyEventArgs e)
        {
            if (CaptureMode == CaptureMode.Drag)
            {
                CaptureMode = CaptureMode.Window;
                e.Handled = true;
            }
        }

        private void Capture_Cancel(Hotkey sender, HotkeyEventArgs e)
        {
            if (CaptureMode != CaptureMode.Unknown)
            {
                if (CaptureMode == CaptureMode.Window)
                {
                    SetFocusedWindow(IntPtr.Zero);
                }

                CaptureMode = CaptureMode.Unknown;
                SaveMode = SaveMode.Unknown;
                e.Handled = true;
            }
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
                        var screen = Screen.FromPoint(Control.MousePosition);

                        EndCapture(ScreenCapture.Capture(screen.Bounds));

                        break;

                    case CaptureMode.Drag:
                    case CaptureMode.Window:
                        //isVirtualDown = true;
                        //Mouse.LeftDown(int.MaxValue, int.MaxValue);
                        break;
                }
            }
        }

        public void EndCapture(Bitmap bitmap)
        {
            CaptureMode = CaptureMode.Unknown;

            if (bitmap == null) return;
            
            foreach (var bus in BitmapBuses.Reverse<IDataBus<Bitmap>>())
            {
                if (bus.SendData(bitmap))
                    return;
            }
        }

        private void SetFocusedWindow(IntPtr newWindow)
        {
            IntPtr oldWindow = focusedWindow;
            focusedWindow = newWindow;

            if (newWindow != oldWindow)
            {
                FocusedWindowChanged?.Invoke(this, new FocusedWindowEventArgs(oldWindow, newWindow));
            }
        }

        public bool HookProc(IntPtr wParam, IntPtr lParam, MouseStruct data)
        {
            switch (wParam.ToInt32())
            {
                case NativeMethods.WM_MOUSEMOVE:
                    if (CaptureMode == CaptureMode.Window)
                        SetFocusedWindow(WindowHelper.ChildFromPoint(Control.MousePosition));

                    break;

                case NativeMethods.WM_LBUTTONDOWN:
                    if (CaptureMode != CaptureMode.Unknown)
                    {
                        // prevent mouse focusing
                        if (isVirtualDown)
                        {
                            isVirtualDown = false;
                            break;
                        }

                        switch (CaptureMode)
                        {
                            case CaptureMode.Drag:
                                isDragging = true;

                                dragStartPosition = new Point(data.x, data.y);
                                BeginDragCapture?.Invoke(this, dragStartPosition);
                                break;

                            case CaptureMode.Window:
                                // TODO: Window Capture
                                break;
                        }

                        return true;
                    }
                    break;

                case NativeMethods.WM_LBUTTONUP:
                    if (CaptureMode == CaptureMode.Drag && isDragging)
                    {
                        isDragging = false;

                        var sw = new Stopwatch();

                        var dragEndPosition = new Point(data.x, data.y);
                        EndDragCapture?.Invoke(this, dragEndPosition);

                        EndCapture(ScreenCapture.Capture(
                            RectangleEx.GetRectangle(
                                dragStartPosition, 
                                dragEndPosition)));

                        return true;
                    }
                    else if (CaptureMode == CaptureMode.Window)
                    {
                        if (focusedWindow != IntPtr.Zero)
                        {
                            IntPtr captureHandle = focusedWindow;

                            SetFocusedWindow(IntPtr.Zero);
                            EndCapture(null/* ScreenCapture.Capture(captureHandle) */);
                        }
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}