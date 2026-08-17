using AltEye.Views.Controls.Render;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using OriginalCircuit.Altium.Models.Pcb;
using System;
using System.Linq;
using System.Numerics;
using Windows.Foundation;

namespace AltEye.Views.Controls
{
    public sealed partial class PcbViewControl : UserControl
    {
        public PcbViewControl()
        {
            InitializeComponent();
            this.VirtualCanvas.RegionsInvalidated += VirtualCanvas_RegionsInvalidated;
            this.PointerWheelChanged += PcbViewControl_PointerWheelChanged;
            MouseNavigationBehavior();
        }

        private void MouseNavigationBehavior()
        {
            this.PointerPressed += (o, e) =>
            {
                var startMovingPoint = e.GetCurrentPoint(this).Position;
                Point? previousMovingPoint = null;
                PointerEventHandler moveHandler = (_, me) =>
                {
                    var currentMousePosition = me.GetCurrentPoint(this).Position;

                    var prevPoint = previousMovingPoint.HasValue ? previousMovingPoint.Value : startMovingPoint;

                    var horizontalShift = prevPoint.X - currentMousePosition.X;
                    var verticalShift = prevPoint.Y - currentMousePosition.Y;

                    var currentZoomFactor = Math.Pow(2D, this.Zoom);

                    var newHorizontalPosition = this.HorizontalPosition - horizontalShift / currentZoomFactor;
                    var newVerticalPosition = this.VerticalPosition - verticalShift / currentZoomFactor;

                    this.HorizontalPosition = newHorizontalPosition;
                    this.VerticalPosition = newVerticalPosition;

                    previousMovingPoint = currentMousePosition;
                };
                this.CapturePointer(e.Pointer);
                this.PointerMoved += moveHandler;
                this.PointerReleased += (_, me) => 
                {
                    this.PointerMoved -= moveHandler;
                    this.ReleasePointerCapture(e.Pointer);
                    previousMovingPoint = null;
                };
            };
        }

        private void PcbViewControl_PointerWheelChanged(object sender, PointerRoutedEventArgs pointerArgs)
        {
            var zoomStep = 0.25D;

            var currentHorizontalZoom = this.Zoom;
            var cursorPoint = pointerArgs.GetCurrentPoint(this);
            var cursorPosition = cursorPoint.Position;

            var zoomFactor = Math.Pow(2D, this.Zoom);
            var newZoom = currentHorizontalZoom + Math.Sign(cursorPoint.Properties.MouseWheelDelta) * zoomStep;
            var newZoomFactor = Math.Pow(2D, newZoom);

            var currentPointerXPositionM = cursorPosition.X / zoomFactor;
            var currentPointerYPositionM = cursorPosition.Y / zoomFactor;
            var newPositionXPx = currentPointerXPositionM * newZoomFactor;
            var newPositionYPx = currentPointerYPositionM * newZoomFactor;
            var horizontalZoomShiftPx = newPositionXPx - cursorPosition.X;
            var verticalZoomShiftPx = newPositionYPx - cursorPosition.Y;

            this.HorizontalPosition -= horizontalZoomShiftPx / newZoomFactor;
            this.VerticalPosition -= verticalZoomShiftPx / newZoomFactor;
            this.Zoom = newZoom;
        }

        private IRender[] _items = [];

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        private readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(PcbViewControl),
            new PropertyMetadata(0D, new PropertyChangedCallback(ZoomChanged)));

        private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PcbViewControl control) return;
            control.VirtualCanvas.Invalidate();
        }

        public double HorizontalPosition
        {
            get => (double)GetValue(HorizontalPositionProperty);
            set => SetValue(HorizontalPositionProperty, value);
        }

        private readonly DependencyProperty HorizontalPositionProperty = DependencyProperty.Register(nameof(HorizontalPosition), typeof(double), typeof(PcbViewControl),
            new PropertyMetadata(0D, new PropertyChangedCallback(HorizontalPositionPropertyChanged)));

        private static void HorizontalPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PcbViewControl control) return;
            control.VirtualCanvas.Invalidate();
        }

        public double VerticalPosition
        {
            get => (double)GetValue(VerticalPositionProperty);
            set => SetValue(VerticalPositionProperty, value);
        }

        private readonly DependencyProperty VerticalPositionProperty = DependencyProperty.Register(nameof(VerticalPosition), typeof(double), typeof(PcbViewControl),
            new PropertyMetadata(0D, new PropertyChangedCallback(VerticalPositionPropertyChanged)));

        private static void VerticalPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PcbViewControl control) return;
            control.VirtualCanvas.Invalidate();
        }

        public PcbDocument Document
        {
            get => (PcbDocument)GetValue(DocumentProperty);
            set => SetValue(DocumentProperty, value);
        }

        private readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof(Document), typeof(PcbDocument), typeof(PcbViewControl),
            new PropertyMetadata(null, new PropertyChangedCallback(DocumentChanged)));

        private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PcbViewControl control) return;

            if (e.NewValue is PcbDocument newDocument)
            {
                control._items = 
                    [
                        ..newDocument.Polygons.Select(x => new PcbPolygonRender(x)),
                        ..newDocument.Fills.Select(x => new PcbFillRender(x)),
                        ..newDocument.Tracks.Select(x => new PcbTrackRender(x)),
                        ..newDocument.Arcs.Select(x => new PcbArcRender(x)),
                        ..newDocument.Pads.Select(x => new PcbPadRender(x)),
                        ..newDocument.Texts.Select(x => new PcbTextRender(x)),                    
                    ];
                    
            }

            control.VirtualCanvas.Invalidate();
        }

        private void VirtualCanvas_RegionsInvalidated(CanvasVirtualControl sender, CanvasRegionsInvalidatedEventArgs args)
        {
            if (sender is not CanvasVirtualControl virtualCanvas) return;

            var zoomRank = (float)Math.Pow(2D, this.Zoom);

            foreach (var region in args.InvalidatedRegions)
            {
                using (var session = sender.CreateDrawingSession(region))
                {
                    session.Transform = new Matrix3x2(zoomRank, 0,
                        0, zoomRank,
                        (float)this.HorizontalPosition * zoomRank, (float)this.VerticalPosition * zoomRank);

                    foreach (var itemRender in this._items)
                        itemRender.Render(session, virtualCanvas);
                }
            }
        }
    }
}
