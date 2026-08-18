using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using OriginalCircuit.Eda.Models.Pcb;
using OriginalCircuit.Eda.Primitives;
using System;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal class PcbViaRender : RenderBase<IPcbVia>
    {
        public PcbViaRender(IPcbVia source) : base(source) {}

        private Color _padColor;
        private Color _holeColor;

        public override void CreateResources(ICanvasResourceCreator canvasResourceCreator)
        {
            this._padColor = this.MutateColor(Defaults.PcbViaPadColor);
            this._holeColor = this.MutateColor(Defaults.PcbViaHoleColor);
        }

        public override bool HitTest(CoordPoint point, Color pixelColor)
        {
            if (!this.IsHover && this._padColor != pixelColor && this._holeColor != pixelColor) return false;

            return this.Source.Bounds.Contains(point);
        }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.FillCircle(RenderHelper.ToRenderVector2(this.Source.Location),
                RenderHelper.MilsToPixels(this.Source.Diameter.ToMils()/2F), this._padColor);
            
            session.FillCircle(RenderHelper.ToRenderVector2(this.Source.Location), RenderHelper.MilsToPixels(this.Source.HoleSize.ToMils()) / 4F,
                this._holeColor);

            if (this.IsHover)
            {
                session.DrawRectangle(new Windows.Foundation.Rect(RenderHelper.MilsToPixels(this.Source.Bounds.Min.X.ToMils()),
                    RenderHelper.MilsToPixels(this.Source.Bounds.Min.Y.ToMils()),
                    RenderHelper.MilsToPixels(this.Source.Bounds.Width.ToMils()),
                    RenderHelper.MilsToPixels(this.Source.Bounds.Height.ToMils())), Defaults.HoverBorderColor, 0.1F);
            }
        }
    }
}
