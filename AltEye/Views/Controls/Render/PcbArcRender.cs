using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using OriginalCircuit.Eda.Models.Pcb;
using System;

namespace AltEye.Views.Controls.Render
{
    internal class PcbArcRender : RenderBase<IPcbArc>
    {
        public PcbArcRender(IPcbArc source) : base(source) { }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.DrawCircle(RenderHelper.ToRenderVector2(this.Source.Center), RenderHelper.MilsToPixels(this.Source.Radius.ToMils()), Colors.Pink);
        }
    }
}
