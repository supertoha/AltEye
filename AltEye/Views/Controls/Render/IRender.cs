using Microsoft.Graphics.Canvas;
using OriginalCircuit.Eda.Primitives;
using System.Numerics;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal interface IRender
    {
        void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator);

        void CreateResources(ICanvasResourceCreator canvasResourceCreator, ColorIndex<IRender> colorIndex);
        bool HitTest(CoordPoint point);
        //bool HitTest(Vector2 point);

        void Select(bool isSelected);
    }
}
