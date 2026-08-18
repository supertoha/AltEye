using Microsoft.Graphics.Canvas;
using OriginalCircuit.Eda.Models.Pcb;
using System;

namespace AltEye.Views.Controls.Render
{
    internal class PcbRegionRender : RenderBase<IPcbRegion>
    {
        public PcbRegionRender(IPcbRegion source) : base(source)
        {
        }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.DrawRectangle(new Windows.Foundation.Rect(
                RenderHelper.MilsToPixels(this.Source.Bounds.Min.X.ToMils()),
                RenderHelper.MilsToPixels(this.Source.Bounds.Min.Y.ToMils()),
                RenderHelper.MilsToPixels(this.Source.Bounds.Width.ToMils()),
                RenderHelper.MilsToPixels(this.Source.Bounds.Height.ToMils())),
                Defaults.HoverBorderColor, 0.1F);
        }
    }
}
