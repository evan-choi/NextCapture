using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace NextCapture.Interop
{
    internal static class SafeNativeMethods
    {
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

        [DllImport(ExternDll.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern int GetCurrentThreadId();

        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern IntPtr LoadLibrary(string libFilename);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class LOGFONT
        {
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

            public override string ToString()
            {
                return
                    $"lfHeight={lfHeight}, " +
                    $"lfWidth={lfWidth}, " +
                    $"lfEscapement={lfEscapement}, " +
                    $"lfOrientation={lfOrientation}, " +
                    $"lfWeight={lfWeight}, " +
                    $"lfItalic={lfItalic}, " +
                    $"lfUnderline={lfUnderline}, " +
                    $"lfStrikeOut={lfStrikeOut}, " +
                    $"lfCharSet={lfCharSet}, " +
                    $"lfOutPrecision={lfOutPrecision}, " +
                    $"lfClipPrecision={lfClipPrecision}, " +
                    $"lfQuality={lfQuality}, " +
                    $"lfPitchAndFamily={lfPitchAndFamily}, " +
                    $"lfFaceName={lfFaceName}";
            }
        }
    }
}
