using AltEye.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using OriginalCircuit.Altium.Models.Pcb;
using OriginalCircuit.Eda.Models.Pcb;
using OriginalCircuit.Eda.Primitives;
using System;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal class PcbTrackRender : RenderBase<IPcbTrack>
    {        
        public PcbTrackRender(IPcbTrack track) : base(track) { }

        public override void CreateResources(ICanvasResourceCreator canvasResourceCreator)
        {
            this._trackColor = this.MutateColor(Defaults.PcbTrackColor);
        }

        private Color _trackColor;

        public override bool HitTest(CoordPoint point, Color pixelColor)
        {
            if (!this.IsHover && this._trackColor != pixelColor) return false;

            return this.Source.Bounds.Contains(point);
        }

        public override bool IsVisible()
        {            
            return this.Source.Layer == (int)PcbLayers.TopLayer ||
                this.Source.Layer == (int)PcbLayers.BottomOverlay ||
                this.Source.Layer >= (int)PcbLayers.MidLayerMin && this.Source.Layer < (int)PcbLayers.MidLayerMax  ||
                this.Source.Layer == (int)PcbLayers.BottomLayer;
        }

        public override void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator)
        {
            var trachColor = this.IsHover ? Defaults.PcbHoverTrackColor : this._trackColor;
            session.DrawLine(RenderHelper.ToRenderVector2(this.Source.Start),
                RenderHelper.ToRenderVector2(this.Source.End),
                trachColor,
                RenderHelper.MilsToPixels(this.Source.Width.ToMils()),
                new CanvasStrokeStyle
                {
                    StartCap = CanvasCapStyle.Round,
                    EndCap = CanvasCapStyle.Round
                });
        }
    }
}
