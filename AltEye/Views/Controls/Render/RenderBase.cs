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

        protected bool _isSelected;

        public T Source { get; init; }

        protected Color MutateColor(Color baseColor)
        {
            return Color.FromArgb(Convert.ToByte((baseColor.A)),
                Convert.ToByte((baseColor.R + Random.Shared.Next(-2, 3)) % 255),
                Convert.ToByte((baseColor.G + Random.Shared.Next(-2, 3)) % 255),
                Convert.ToByte((baseColor.B + Random.Shared.Next(-2, 3)) % 255));
        }

        public void Select(bool isSelected)
        {
            this._isSelected = isSelected;
        }

        public virtual void CreateResources(ICanvasResourceCreator canvasResourceCreator, ColorIndex<IRender> colorIndex)
        {

        }

        public virtual void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
        }

        public virtual bool HitTest(CoordPoint point)
        {
            return false;
        }
    }
}
