using Microsoft.Graphics.Canvas;
using OriginalCircuit.Eda.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace AltEye.Views.Controls.Render
{
    internal class RenderBase<T> : IRender where T : class
    {
        public RenderBase(T source)
        {
            this.Source = source;
        }

        public T Source { get; init; }

        protected Vector2 ToRenderVector2(CoordPoint point)
        {
            return new Vector2((float)MilsToPixels(point.X.ToMils()), (float)MilsToPixels(point.Y.ToMils()));
        }

        protected Point ToRenderPoint(CoordPoint point)
        {
            return new Point(MilsToPixels(point.X.ToMils()), MilsToPixels(point.Y.ToMils()));
        }

        protected double MilsToPixels(double mils)
        {
            return mils * 0.1;
        }

        public virtual void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
        }
    }
}
