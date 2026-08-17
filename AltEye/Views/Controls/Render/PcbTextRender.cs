using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using OriginalCircuit.Eda.Models.Pcb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AltEye.Views.Controls.Render
{
    internal class PcbTextRender : RenderBase<IPcbText>
    {
        public PcbTextRender(IPcbText source) : base(source) { }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            var format = new CanvasTextFormat
            {
                FontFamily = this.Source.FontName,
                FontSize = (float)MilsToPixels(this.Source.Height.ToMils()),
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                VerticalAlignment = CanvasVerticalAlignment.Top
            };

            using var layout = new CanvasTextLayout(
                canvasResourceCreator,
                this.Source.Text,
                format,
                float.MaxValue,
                float.MaxValue);

            session.DrawTextLayout(
                layout,
                (float)MilsToPixels(this.Source.Location.X.ToMils()),
                (float)MilsToPixels(this.Source.Location.Y.ToMils()),
                Colors.LightCoral);
        }
    }
}
