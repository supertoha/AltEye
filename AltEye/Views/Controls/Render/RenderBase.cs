using Microsoft.Graphics.Canvas;
using OriginalCircuit.Eda.Primitives;
using System;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal class RenderBase<T> : IRender where T : class
    {
        public RenderBase(T source)
        {
            this.Source = source;
        }

        public static IRender HoverItem { get; set; }

        public bool IsHover => RenderBase<IRender>.HoverItem == this;

        public T Source { get; init; }

        protected Color MutateColor(Color baseColor)
        {
            return Color.FromArgb(Convert.ToByte((baseColor.A)),
                Convert.ToByte(Math.Min(255, Math.Max(0, baseColor.R + Random.Shared.Next(-2, 3)))),
                Convert.ToByte(Math.Min(255, Math.Max(0, baseColor.G + Random.Shared.Next(-2, 3)))),
                Convert.ToByte(Math.Min(255, Math.Max(0, baseColor.B + Random.Shared.Next(-2, 3)))));
        }

        public virtual bool IsVisible()
        {
            return true;
        }

        public void Select(bool isSelected)
        {
            RenderBase<IRender>.HoverItem = isSelected ? this: null;
        }

        public virtual void CreateResources(ICanvasResourceCreator canvasResourceCreator)
        {

        }

        public virtual void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
        }

        public virtual bool HitTest(CoordPoint point, Color pixelColor)
        {
            return false;
        }
    }
}
