using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using OriginalCircuit.Altium.Models.Pcb;
using System;
using Windows.Foundation;

namespace AltEye.Views.Controls.Render
{
    internal class PcbFillRender : RenderBase<IPcbFill>
    {
        public PcbFillRender(IPcbFill source) : base(source) { }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.FillRectangle(new Rect(ToRenderPoint(this.Source.Corner1), ToRenderPoint(this.Source.Corner2)), Colors.Orange);
        }
    }
}
