using OriginalCircuit.Eda.Primitives;
using System;
using System.Numerics;
using Windows.Foundation;

namespace AltEye.Views.Controls.Render
{
    internal static class RenderHelper
    {
        public static Vector2 ToRenderVector2(CoordPoint point)
        {
            return new Vector2((float)MilsToPixels(point.X.ToMils()), (float)MilsToPixels(point.Y.ToMils()));
        }

        public static Point ToRenderPoint(CoordPoint point)
        {
            return new Point(MilsToPixels(point.X.ToMils()), MilsToPixels(point.Y.ToMils()));
        }

        public static float MilsToPixels(double mils)
        {
            return (float)mils * 0.1F;
        }

        public static float PixelsToMils(double pixels)
        {
            return (float)pixels * 10F;
        }
    }
}
