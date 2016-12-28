using System;
using System.Windows.Forms;

using NextCapture.Interop;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NextCapture
{
    class AeroWindow : Form
    {
        bool aeroEnabled;
        public bool IsAeroEnabled { get { return aeroEnabled; } }

        private Dictionary<string, Rectangle> hitVisibles = new Dictionary<string, Rectangle>();

        public AeroWindow()
        {
            var designSize = this.ClientSize;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Size = designSize;
        }

        protected void AddHitVisibleBounds(string name, Rectangle bound)
        {
            hitVisibles[name] = bound;
        }

        protected void RemoveHitVisibleBounds(string name)
        {
            if (hitVisibles.ContainsKey(name))
                hitVisibles.Remove(name);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;

                if (!aeroEnabled)
                    cp.ClassStyle |= NativeMethods.CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int dwmEnabled = 0;

                UnsafeNativeMethods.DwmIsCompositionEnabled(ref dwmEnabled);

                return (dwmEnabled == 1 ? true : false);
            }

            return false;
        }

        protected override void WndProc(ref Message m)
        {
            if (aeroEnabled)
            {
                switch (m.Msg)
                {
                    case NativeMethods.WM_SYSCOMMAND:
                        if (m.WParam.ToInt32() == 0xF120)
                        {
                            Console.WriteLine("!!");
                            m.Result = new IntPtr(1);
                        }

                        if (m.WParam.ToInt32() == 0xF020)
                        {
                            Console.WriteLine("!!2");
                            m.Result = new IntPtr(1);
                        }
                        break;

                    case NativeMethods.WM_GETMINMAXINFO:
                        WmGetMinMaxInfo(ref m);
                        break;

                    case NativeMethods.WM_NCPAINT:
                            var attrValue = 2;

                            var margins = new NativeMethods.MARGINS()
                            {
                                cyBottomHeight = 1,
                                cxLeftWidth = 1,
                                cxRightWidth = 1,
                                cyTopHeight = 1
                            };

                            UnsafeNativeMethods.DwmSetWindowAttribute(this.Handle, 2, ref attrValue, 4);
                            UnsafeNativeMethods.DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                        break;

                    case NativeMethods.WM_NCCALCSIZE:
                        if (m.WParam.ToInt32() == 1)
                        {
                            m.Result = new IntPtr(0xF0);
                            return;
                        }
                        break;
                }
            }

            base.WndProc(ref m);

            if (m.Msg == NativeMethods.WM_NCHITTEST && (int)m.Result == NativeMethods.HTCLIENT)
            {
                var pos = MousePosition - (Size)this.Location;

                foreach (var bound in hitVisibles.Values)
                    if (bound.Contains(pos))
                        return;

                m.Result = (IntPtr)NativeMethods.HTCAPTION;
            }
        }

        private void WmGetMinMaxInfo(ref Message m)
        {
            var mmi = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
            IntPtr monitor = UnsafeNativeMethods.MonitorFromWindow(m.HWnd, NativeMethods.MONITOR_DEFAULTTONEAREST);
            
            if (monitor != IntPtr.Zero)
            {
                var mInfo = new NativeMethods.MONITORINFO();
                mInfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFO));
                UnsafeNativeMethods.GetMonitorInfo(monitor, ref mInfo);

                mmi.ptMaxPosition.X = Math.Abs(mInfo.rcWork.left - mInfo.rcMonitor.left);
                mmi.ptMaxPosition.Y = Math.Abs(mInfo.rcWork.top - mInfo.rcMonitor.top);
                mmi.ptMaxSize.X = Math.Abs(mInfo.rcWork.right - mInfo.rcWork.left);
                mmi.ptMaxSize.Y = Math.Abs(mInfo.rcWork.bottom - mInfo.rcWork.top);
            }

            Marshal.StructureToPtr(mmi, m.LParam, true);
        }
    }
}
