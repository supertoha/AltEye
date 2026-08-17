using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using OriginalCircuit.Altium.Models.Pcb;
using OriginalCircuit.Eda.Primitives;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
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
            control.VirtualCanvas.Invalidate();
        }

        private Vector2 ToRenderVector2(CoordPoint point)
        {
            return new Vector2((float)MilsToPixels(point.X.ToMils()), (float)MilsToPixels(point.Y.ToMils()));
        }

        private Point ToRenderPoint(CoordPoint point)
        {
            return new Point(MilsToPixels(point.X.ToMils()), MilsToPixels(point.Y.ToMils()));
        }

        private double MilsToPixels(double mils)
        {
            return mils * 0.1;
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
                    if (this.Document == null) return;

                    foreach (var polygon in this.Document.Polygons)
                    {
                        var geometry = CanvasGeometry.CreatePolygon(virtualCanvas, polygon.Vertices.Select(ToRenderVector2).ToArray());
                        session.FillGeometry(geometry, Colors.Red);
                    }

                    foreach (var fill in this.Document.Fills)
                        session.FillRectangle(new Rect(ToRenderPoint(fill.Corner1), ToRenderPoint(fill.Corner2)), Colors.Orange);

                    foreach (var track in this.Document.Tracks)
                        session.DrawLine(ToRenderVector2(track.Start), ToRenderVector2(track.End), Colors.Blue, (float)track.Width.ToMils()/5F);

                    foreach (var arc in this.Document.Arcs)
                        session.DrawCircle(ToRenderVector2(arc.Center), (float)MilsToPixels(arc.Radius.ToMils()), Colors.Pink);

                    foreach (var text in this.Document.Texts)
                        session.DrawText(text.Text, 
                            new Rect(MilsToPixels(text.Bounds.Min.X.ToMils()), MilsToPixels(text.Bounds.Min.Y.ToMils()), MilsToPixels(text.Bounds.Max.X.ToMils()), MilsToPixels(text.Bounds.Max.Y.ToMils())),
                            Colors.LightCoral, null);

                    foreach (var pad in this.Document.Pads)                    
                        session.DrawCircle(ToRenderVector2(pad.Location), 10, Colors.White);
                    
                }
            }
        }
    }
}
