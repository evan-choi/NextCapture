using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NextCapture.Interop
{
    internal static partial class NativeMethods
    {
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [Flags]
        internal enum SPI : int
        {
            GETBEEP = 0x0001,
            SETBEEP = 0x0002,
            GETMOUSE = 0x0003,
            SETMOUSE = 0x0004,
            GETBORDER = 0x0005,
            SETBORDER = 0x0006,
            GETKEYBOARDSPEED = 0x000A,
            SETKEYBOARDSPEED = 0x000B,
            LANGDRIVER = 0x000C,
            ICONHORIZONTALSPACING = 0x000D,
            GETSCREENSAVETIMEOUT = 0x000E,
            SETSCREENSAVETIMEOUT = 0x000F,
            GETSCREENSAVEACTIVE = 0x0010,
            SETSCREENSAVEACTIVE = 0x0011,
            GETGRIDGRANULARITY = 0x0012,
            SETGRIDGRANULARITY = 0x0013,
            SETDESKWALLPAPER = 0x0014,
            SETDESKPATTERN = 0x0015,
            GETKEYBOARDDELAY = 0x0016,
            SETKEYBOARDDELAY = 0x0017,
            ICONVERTICALSPACING = 0x0018,
            GETICONTITLEWRAP = 0x0019,
            SETICONTITLEWRAP = 0x001A,
            GETMENUDROPALIGNMENT = 0x001B,
            SETMENUDROPALIGNMENT = 0x001C,
            SETDOUBLECLKWIDTH = 0x001D,
            SETDOUBLECLKHEIGHT = 0x001E,
            GETICONTITLELOGFONT = 0x001F,
            SETDOUBLECLICKTIME = 0x0020,
            SETMOUSEBUTTONSWAP = 0x0021,
            SETICONTITLELOGFONT = 0x0022,
            GETFASTTASKSWITCH = 0x0023,
            SETFASTTASKSWITCH = 0x0024,

            SETDRAGFULLWINDOWS = 0x0025,
            GETDRAGFULLWINDOWS = 0x0026,
            GETNONCLIENTMETRICS = 0x0029,
            SETNONCLIENTMETRICS = 0x002A,
            GETMINIMIZEDMETRICS = 0x002B,
            SETMINIMIZEDMETRICS = 0x002C,
            GETICONMETRICS = 0x002D,
            SETICONMETRICS = 0x002E,
            SETWORKAREA = 0x002F,
            GETWORKAREA = 0x0030,
            SETPENWINDOWS = 0x0031,
            GETHIGHCONTRAST = 0x0042,
            SETHIGHCONTRAST = 0x0043,
            GETKEYBOARDPREF = 0x0044,
            SETKEYBOARDPREF = 0x0045,
            GETSCREENREADER = 0x0046,
            SETSCREENREADER = 0x0047,
            GETANIMATION = 0x0048,
            SETANIMATION = 0x0049,
            GETFONTSMOOTHING = 0x004A,
            SETFONTSMOOTHING = 0x004B,
            SETDRAGWIDTH = 0x004C,
            SETDRAGHEIGHT = 0x004D,
            SETHANDHELD = 0x004E,
            GETLOWPOWERTIMEOUT = 0x004F,
            GETPOWEROFFTIMEOUT = 0x0050,
            SETLOWPOWERTIMEOUT = 0x0051,
            SETPOWEROFFTIMEOUT = 0x0052,
            GETLOWPOWERACTIVE = 0x0053,
            GETPOWEROFFACTIVE = 0x0054,
            SETLOWPOWERACTIVE = 0x0055,
            SETPOWEROFFACTIVE = 0x0056,
            SETCURSORS = 0x0057,
            SETICONS = 0x0058,
            GETDEFAULTINPUTLANG = 0x0059,
            SETDEFAULTINPUTLANG = 0x005A,
            SETLANGTOGGLE = 0x005B,
            GETWINDOWSEXTENSION = 0x005C,
            SETMOUSETRAILS = 0x005D,
            GETMOUSETRAILS = 0x005E,
            SETSCREENSAVERRUNNING = 0x0061,
            SCREENSAVERRUNNING = SETSCREENSAVERRUNNING,
            GETFILTERKEYS = 0x0032,
            SETFILTERKEYS = 0x0033,
            GETTOGGLEKEYS = 0x0034,
            SETTOGGLEKEYS = 0x0035,
            GETMOUSEKEYS = 0x0036,
            SETMOUSEKEYS = 0x0037,
            GETSHOWSOUNDS = 0x0038,
            SETSHOWSOUNDS = 0x0039,
            GETSTICKYKEYS = 0x003A,
            SETSTICKYKEYS = 0x003B,
            GETACCESSTIMEOUT = 0x003C,
            SETACCESSTIMEOUT = 0x003D,

            GETSERIALKEYS = 0x003E,
            SETSERIALKEYS = 0x003F,
            GETSOUNDSENTRY = 0x0040,
            SETSOUNDSENTRY = 0x0041,
            GETSNAPTODEFBUTTON = 0x005F,
            SETSNAPTODEFBUTTON = 0x0060,
            GETMOUSEHOVERWIDTH = 0x0062,
            SETMOUSEHOVERWIDTH = 0x0063,
            GETMOUSEHOVERHEIGHT = 0x0064,
            SETMOUSEHOVERHEIGHT = 0x0065,
            GETMOUSEHOVERTIME = 0x0066,
            SETMOUSEHOVERTIME = 0x0067,
            GETWHEELSCROLLLINES = 0x0068,
            SETWHEELSCROLLLINES = 0x0069,
            GETMENUSHOWDELAY = 0x006A,
            SETMENUSHOWDELAY = 0x006B,

            GETWHEELSCROLLCHARS = 0x006C,
            SETWHEELSCROLLCHARS = 0x006D,

            GETSHOWIMEUI = 0x006E,
            SETSHOWIMEUI = 0x006F,

            GETMOUSESPEED = 0x0070,
            SETMOUSESPEED = 0x0071,
            GETSCREENSAVERRUNNING = 0x0072,
            GETDESKWALLPAPER = 0x0073,

            GETAUDIODESCRIPTION = 0x0074,
            SETAUDIODESCRIPTION = 0x0075,

            GETSCREENSAVESECURE = 0x0076,
            SETSCREENSAVESECURE = 0x0077,

            GETHUNGAPPTIMEOUT = 0x0078,
            SETHUNGAPPTIMEOUT = 0x0079,
            GETWAITTOKILLTIMEOUT = 0x007A,
            SETWAITTOKILLTIMEOUT = 0x007B,
            GETWAITTOKILLSERVICETIMEOUT = 0x007C,
            SETWAITTOKILLSERVICETIMEOUT = 0x007D,
            GETMOUSEDOCKTHRESHOLD = 0x007E,
            SETMOUSEDOCKTHRESHOLD = 0x007F,
            GETPENDOCKTHRESHOLD = 0x0080,
            SETPENDOCKTHRESHOLD = 0x0081,
            GETWINARRANGING = 0x0082,
            SETWINARRANGING = 0x0083,
            GETMOUSEDRAGOUTTHRESHOLD = 0x0084,
            SETMOUSEDRAGOUTTHRESHOLD = 0x0085,
            GETPENDRAGOUTTHRESHOLD = 0x0086,
            SETPENDRAGOUTTHRESHOLD = 0x0087,
            GETMOUSESIDEMOVETHRESHOLD = 0x0088,
            SETMOUSESIDEMOVETHRESHOLD = 0x0089,
            GETPENSIDEMOVETHRESHOLD = 0x008A,
            SETPENSIDEMOVETHRESHOLD = 0x008B,
            GETDRAGFROMMAXIMIZE = 0x008C,
            SETDRAGFROMMAXIMIZE = 0x008D,
            GETSNAPSIZING = 0x008E,
            SETSNAPSIZING = 0x008F,
            GETDOCKMOVING = 0x0090,
            SETDOCKMOVING = 0x0091,

            GETACTIVEWINDOWTRACKING = 0x1000,
            SETACTIVEWINDOWTRACKING = 0x1001,
            GETMENUANIMATION = 0x1002,
            SETMENUANIMATION = 0x1003,
            GETCOMBOBOXANIMATION = 0x1004,
            SETCOMBOBOXANIMATION = 0x1005,
            GETLISTBOXSMOOTHSCROLLING = 0x1006,
            SETLISTBOXSMOOTHSCROLLING = 0x1007,
            GETGRADIENTCAPTIONS = 0x1008,
            SETGRADIENTCAPTIONS = 0x1009,
            GETKEYBOARDCUES = 0x100A,
            SETKEYBOARDCUES = 0x100B,
            GETMENUUNDERLINES = GETKEYBOARDCUES,
            SETMENUUNDERLINES = SETKEYBOARDCUES,
            GETACTIVEWNDTRKZORDER = 0x100C,
            SETACTIVEWNDTRKZORDER = 0x100D,
            GETHOTTRACKING = 0x100E,
            SETHOTTRACKING = 0x100F,
            GETMENUFADE = 0x1012,
            SETMENUFADE = 0x1013,
            GETSELECTIONFADE = 0x1014,
            SETSELECTIONFADE = 0x1015,
            GETTOOLTIPANIMATION = 0x1016,
            SETTOOLTIPANIMATION = 0x1017,
            GETTOOLTIPFADE = 0x1018,
            SETTOOLTIPFADE = 0x1019,
            GETCURSORSHADOW = 0x101A,
            SETCURSORSHADOW = 0x101B,
            GETMOUSESONAR = 0x101C,
            SETMOUSESONAR = 0x101D,
            GETMOUSECLICKLOCK = 0x101E,
            SETMOUSECLICKLOCK = 0x101F,
            GETMOUSEVANISH = 0x1020,
            SETMOUSEVANISH = 0x1021,
            GETFLATMENU = 0x1022,
            SETFLATMENU = 0x1023,
            GETDROPSHADOW = 0x1024,
            SETDROPSHADOW = 0x1025,
            GETBLOCKSENDINPUTRESETS = 0x1026,
            SETBLOCKSENDINPUTRESETS = 0x1027,

            GETUIEFFECTS = 0x103E,
            SETUIEFFECTS = 0x103F,

            GETDISABLEOVERLAPPEDCONTENT = 0x1040,
            SETDISABLEOVERLAPPEDCONTENT = 0x1041,
            GETCLIENTAREAANIMATION = 0x1042,
            SETCLIENTAREAANIMATION = 0x1043,
            GETCLEARTYPE = 0x1048,
            SETCLEARTYPE = 0x1049,
            GETSPEECHRECOGNITION = 0x104A,
            SETSPEECHRECOGNITION = 0x104B,

            GETFOREGROUNDLOCKTIMEOUT = 0x2000,
            SETFOREGROUNDLOCKTIMEOUT = 0x2001,
            GETACTIVEWNDTRKTIMEOUT = 0x2002,
            SETACTIVEWNDTRKTIMEOUT = 0x2003,
            GETFOREGROUNDFLASHCOUNT = 0x2004,
            SETFOREGROUNDFLASHCOUNT = 0x2005,
            GETCARETWIDTH = 0x2006,
            SETCARETWIDTH = 0x2007,

            GETMOUSECLICKLOCKTIME = 0x2008,
            SETMOUSECLICKLOCKTIME = 0x2009,
            GETFONTSMOOTHINGTYPE = 0x200A,
            SETFONTSMOOTHINGTYPE = 0x200B,

            GETFONTSMOOTHINGCONTRAST = 0x200C,
            SETFONTSMOOTHINGCONTRAST = 0x200D,

            GETFOCUSBORDERWIDTH = 0x200E,
            SETFOCUSBORDERWIDTH = 0x200F,
            GETFOCUSBORDERHEIGHT = 0x2010,
            SETFOCUSBORDERHEIGHT = 0x2011,

            GETFONTSMOOTHINGORIENTATION = 0x2012,
            SETFONTSMOOTHINGORIENTATION = 0x2013,

            GETMINIMUMHITRADIUS = 0x2014,
            SETMINIMUMHITRADIUS = 0x2015,
            GETMESSAGEDURATION = 0x2016,
            SETMESSAGEDURATION = 0x2017,
        }

        [Flags]
        internal enum SPIF : int
        {
            None = 0,
            UPDATEINIFILE = 0x01,
            SENDCHANGE = 0x02,
            SENDWININICHANGE = SENDCHANGE,
        }

        [Flags]
        internal enum WS : uint
        {
            OVERLAPPED = 0x00000000,
            POPUP = 0x80000000,
            CHILD = 0x40000000,
            MINIMIZE = 0x20000000,
            VISIBLE = 0x10000000,
            DISABLED = 0x08000000,
            CLIPSIBLINGS = 0x04000000,
            CLIPCHILDREN = 0x02000000,
            MAXIMIZE = 0x01000000,
            BORDER = 0x00800000,
            DLGFRAME = 0x00400000,
            VSCROLL = 0x00200000,
            HSCROLL = 0x00100000,
            SYSMENU = 0x00080000,
            THICKFRAME = 0x00040000,
            GROUP = 0x00020000,
            TABSTOP = 0x00010000,

            MINIMIZEBOX = 0x00020000,
            MAXIMIZEBOX = 0x00010000,

            CAPTION = BORDER | DLGFRAME,
            TILED = OVERLAPPED,
            ICONIC = MINIMIZE,
            SIZEBOX = THICKFRAME,
            TILEDWINDOW = OVERLAPPEDWINDOW,

            OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
            POPUPWINDOW = POPUP | BORDER | SYSMENU,
            CHILDWINDOW = CHILD,
        }

        [Flags]
        internal enum WS_EX : uint
        {
            None = 0,
            DLGMODALFRAME = 0x00000001,
            NOPARENTNOTIFY = 0x00000004,
            TOPMOST = 0x00000008,
            ACCEPTFILES = 0x00000010,
            TRANSPARENT = 0x00000020,
            MDICHILD = 0x00000040,
            TOOLWINDOW = 0x00000080,
            WINDOWEDGE = 0x00000100,
            CLIENTEDGE = 0x00000200,
            CONTEXTHELP = 0x00000400,
            RIGHT = 0x00001000,
            LEFT = 0x00000000,
            RTLREADING = 0x00002000,
            LTRREADING = 0x00000000,
            LEFTSCROLLBAR = 0x00004000,
            RIGHTSCROLLBAR = 0x00000000,
            CONTROLPARENT = 0x00010000,
            STATICEDGE = 0x00020000,
            APPWINDOW = 0x00040000,
            LAYERED = 0x00080000,
            NOINHERITLAYOUT = 0x00100000,
            LAYOUTRTL = 0x00400000,
            COMPOSITED = 0x02000000,
            NOACTIVATE = 0x08000000,
            OVERLAPPEDWINDOW = (WINDOWEDGE | CLIENTEDGE),
            PALETTEWINDOW = (WINDOWEDGE | TOOLWINDOW | TOPMOST),
        }

        [Flags]
        internal enum GWL
        {
            WNDPROC = -4,
            HINSTANCE = -6,
            HWNDPARENT = -8,
            STYLE = -16,
            EXSTYLE = -20,
            USERDATA = -21,
            ID = -12
        }

        [Flags]
        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
            CAPTUREBLT = 0x40000000
        }

        internal class NONCLIENTMETRICS
        {
            public int cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS));
            public int iBorderWidth = 0;
            public int iScrollWidth = 0;
            public int iScrollHeight = 0;
            public int iCaptionWidth = 0;
            public int iCaptionHeight = 0;
            [MarshalAs(UnmanagedType.Struct)]
            public SafeNativeMethods.LOGFONT lfCaptionFont = default(SafeNativeMethods.LOGFONT);
            public int iSmCaptionWidth = 0;
            public int iSmCaptionHeight = 0;
            [MarshalAs(UnmanagedType.Struct)]
            public SafeNativeMethods.LOGFONT lfSmCaptionFont = default(SafeNativeMethods.LOGFONT);
            public int iMenuWidth = 0;
            public int iMenuHeight = 0;
            [MarshalAs(UnmanagedType.Struct)]
            public SafeNativeMethods.LOGFONT lfMenuFont = default(SafeNativeMethods.LOGFONT);
            [MarshalAs(UnmanagedType.Struct)]
            public SafeNativeMethods.LOGFONT lfStatusFont = default(SafeNativeMethods.LOGFONT);
            [MarshalAs(UnmanagedType.Struct)]
            public SafeNativeMethods.LOGFONT lfMessageFont = default(SafeNativeMethods.LOGFONT);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;

        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct HIGHCONTRAST_I
        {
            public int cbSize;
            public int dwFlags;
            public IntPtr lpszDefaultScheme;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r)
            {
                this.left = r.Left;
                this.top = r.Top;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            public System.Drawing.Size Size
            {
                get
                {
                    return new System.Drawing.Size(this.right - this.left, this.bottom - this.top);
                }
            }

            public override string ToString()
            {
                return $"{{X={this.left},Y={this.top},Width={this.right - this.left},Height={this.bottom - this.top}}}";
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class LOGFONT
        {
            public LOGFONT()
            {
            }

            public LOGFONT(LOGFONT lf)
            {
                this.lfHeight = lf.lfHeight;
                this.lfWidth = lf.lfWidth;
                this.lfEscapement = lf.lfEscapement;
                this.lfOrientation = lf.lfOrientation;
                this.lfWeight = lf.lfWeight;
                this.lfItalic = lf.lfItalic;
                this.lfUnderline = lf.lfUnderline;
                this.lfStrikeOut = lf.lfStrikeOut;
                this.lfCharSet = lf.lfCharSet;
                this.lfOutPrecision = lf.lfOutPrecision;
                this.lfClipPrecision = lf.lfClipPrecision;
                this.lfQuality = lf.lfQuality;
                this.lfPitchAndFamily = lf.lfPitchAndFamily;
                this.lfFaceName = lf.lfFaceName;
            }

            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MOUSEHOOKSTRUCT
        {
            public int x = 0;
            public int y = 0;
            public IntPtr hWnd = IntPtr.Zero;
            public int wHitTestCode = 0;
            public int dwExtraInfo = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public UnsafeNativeMethods.KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        };
    }
}
