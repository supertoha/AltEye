using AltEye.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using OriginalCircuit.Altium.Models.Pcb;
using OriginalCircuit.Eda.Models.Pcb;
using OriginalCircuit.Eda.Primitives;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal class PcbTextRender : RenderBase<IPcbText>
    {
        public PcbTextRender(IPcbText source) : base(source) { }

        public override void CreateResources(ICanvasResourceCreator canvasResourceCreator)
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

            this._textColor = this.MutateColor(Defaults.PcbTextColor);
        }

        private Color _textColor;
        private CanvasTextLayout _textLayout;

        public override bool IsVisible()
        {
            return (this.Source.Layer == (int)PcbLayers.TopOverlay ||
                this.Source.Layer == (int)PcbLayers.BottomOverlay ) &&
                this.Source is PcbText pcbText && !pcbText.IsComment && !pcbText.IsHidden;
        }

        public override bool HitTest(CoordPoint point, Color pixelColor)
        {
            if (this._textColor != pixelColor) return false;

            // convert CoordPoint to local pixels
            var localX = RenderHelper.MilsToPixels(point.X.ToMils() - this.Source.Location.X.ToMils());
            var localY = RenderHelper.MilsToPixels(point.Y.ToMils() - this.Source.Location.Y.ToMils());
            return this._textLayout.LayoutBounds.Contains(new Windows.Foundation.Point(localX, localY));
        }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            session.DrawTextLayout(
                this._textLayout,
                RenderHelper.MilsToPixels(this.Source.Location.X.ToMils()),
                RenderHelper.MilsToPixels(this.Source.Location.Y.ToMils()),
                this._textColor);

            if (this.IsHover)
            {               
                session.DrawRectangle(new Windows.Foundation.Rect(RenderHelper.MilsToPixels(this.Source.Location.X.ToMils()) + this._textLayout.LayoutBounds.X,
                    RenderHelper.MilsToPixels(this.Source.Location.Y.ToMils()) + this._textLayout.LayoutBounds.Y,
                    this._textLayout.LayoutBounds.Width,
                    this._textLayout.LayoutBounds.Height), Defaults.HoverBorderColor, 0.1F);
            }
        }
    }
}
