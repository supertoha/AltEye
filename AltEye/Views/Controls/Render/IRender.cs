using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltEye.Views.Controls.Render
{
    internal interface IRender
    {
        void Render(CanvasDrawingSession session, ICanvasResourceCreator canvasResourceCreator);
    }
}
