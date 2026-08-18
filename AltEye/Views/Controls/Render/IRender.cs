using Microsoft.Graphics.Canvas;
using OriginalCircuit.Eda.Primitives;
using System.Numerics;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal interface IRender
    {
        void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator);

        void CreateResources(ICanvasResourceCreator canvasResourceCreator);
        bool HitTest(CoordPoint point, Color pixelColor);
        void Select(bool isSelected);

        bool IsVisible();
    }
}
