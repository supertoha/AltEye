using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.UI;
using OriginalCircuit.Eda.Models.Pcb;
using System;

namespace AltEye.Views.Controls.Render
{
    internal class PcbTrackRender : RenderBase<IPcbTrack>
    {        
        public PcbTrackRender(IPcbTrack track) : base(track) { }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.DrawLine(RenderHelper.ToRenderVector2(this.Source.Start), RenderHelper.ToRenderVector2(this.Source.End), Colors.Blue, RenderHelper.MilsToPixels(this.Source.Width.ToMils()),
            new CanvasStrokeStyle
            {
                StartCap = CanvasCapStyle.Round,
                EndCap = CanvasCapStyle.Round
            });
        }
    }
}
