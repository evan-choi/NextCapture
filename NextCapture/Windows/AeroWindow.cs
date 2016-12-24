using System;
using System.Windows.Forms;

using NextCapture.Interop;

namespace NextCapture
{
    class AeroWindow : Form
    {
        bool aeroEnabled;
        public bool IsAeroEnabled { get { return aeroEnabled; } }

        public AeroWindow()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
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
                m.Result = (IntPtr)NativeMethods.HTCAPTION;
        }
    }
}
