using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.UI;
using OriginalCircuit.Altium.Models.Pcb;
using System;
using System.Linq;

namespace AltEye.Views.Controls.Render
{
    internal class PcbPolygonRender : RenderBase<PcbPolygon>
    {
        public PcbPolygonRender(PcbPolygon source) : base(source) { }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            if (this.Source.IsHidden) return;

            var geometry = CanvasGeometry.CreatePolygon(canvasResourceCreator, this.Source.Vertices.Select(ToRenderVector2).ToArray());
            session.FillGeometry(geometry, Colors.Red);
        }
    }
}
