using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using OriginalCircuit.Eda.Models.Pcb;
using System;

namespace AltEye.Views.Controls.Render
{
    internal class PcbPadRender : RenderBase<IPcbPad>
    {
        public PcbPadRender(IPcbPad source) : base(source) { }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.FillCircle(ToRenderVector2(this.Source.Location), (float)MilsToPixels(this.Source.Size.X.ToMils()) / 2F, Colors.White);

            if (this.Source.HoleType == OriginalCircuit.Eda.Enums.PadHoleType.Round)
                session.FillCircle(ToRenderVector2(this.Source.Location), (float)MilsToPixels(this.Source.HoleSize.ToMils()) / 2F, Colors.Black);
        }
    }
}
