using System;
using System.Runtime.InteropServices;
using Windows.UI;

namespace AltEye.Views.Commons
{
    internal static class ScreenHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern uint GetPixel(IntPtr hDC, int x, int y);

        public static Color GetScreenPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);

            // 0x00BBGGRR
            uint pixel = GetPixel(hdc, x, y);

            ReleaseDC(IntPtr.Zero, hdc);

            byte r = (byte)(pixel & 0x000000FF);
            byte g = (byte)((pixel & 0x0000FF00) >> 8);
            byte b = (byte)((pixel & 0x00FF0000) >> 16);

            return Color.FromArgb(255, r, g, b);
        }
    }
}
