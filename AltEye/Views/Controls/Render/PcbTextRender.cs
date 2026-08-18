using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using OriginalCircuit.Eda.Models.Pcb;
using OriginalCircuit.Eda.Primitives;
using System;
using System.Numerics;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal class PcbTextRender : RenderBase<IPcbText>
    {
        public PcbTextRender(IPcbText source) : base(source) { }

        public override void CreateResources(ICanvasResourceCreator canvasResourceCreator, ColorIndex<IRender> colorIndex)
        {
            var format = new CanvasTextFormat
            {
                FontFamily = this.Source.FontName,
                FontSize = RenderHelper.MilsToPixels(this.Source.Height.ToMils()),
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                VerticalAlignment = CanvasVerticalAlignment.Top
            };

            this._textLayout = new CanvasTextLayout(
                canvasResourceCreator,
                this.Source.Text,
                format,
                float.MaxValue,
                float.MaxValue);

            this._textColor = this.MutateColor(Colors.LightCoral);
            colorIndex.Add(this._textColor, this);
        }

        private Color _textColor;
        private CanvasTextLayout _textLayout;


        public override bool HitTest(CoordPoint point)
        {            
            

            return this.Source.Bounds.Contains(point);
        }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.DrawTextLayout(
                this._textLayout,
                RenderHelper.MilsToPixels(this.Source.Location.X.ToMils()),
                RenderHelper.MilsToPixels(this.Source.Location.Y.ToMils()),
                this._textColor);

            if (this._isSelected)
                session.DrawRectangle(new Windows.Foundation.Rect(RenderHelper.ToRenderPoint(this.Source.Bounds.Min), RenderHelper.ToRenderPoint(this.Source.Bounds.Max)),
                    Colors.Black, 0.1F);
        }
    }
}
