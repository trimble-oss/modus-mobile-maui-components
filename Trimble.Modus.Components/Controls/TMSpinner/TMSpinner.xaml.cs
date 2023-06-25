using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components
{
    public class TMSpinner : SKCanvasView
    {

        private SpinnerType _spinnerType;
        private float _startAngle = 0f;
        private float _sweepAngle = 180f;
        private float _rotationAngle = 0f;
        private Timer _animationTimer;

        public TMSpinner()
        {
            WidthRequest = 100;
            HeightRequest = 100;
            IgnorePixelScaling = false;
            _spinnerType = SpinnerType.Determinate;
            StartAnimation();
        }

        private void StartAnimation()
        {
            _animationTimer = new Timer(UpdateAnimation, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(20));
        }

        private void UpdateAnimation(object state)
        {
            if (_spinnerType == SpinnerType.Indeterminate)
            {
                _rotationAngle += 5f;
            }
            else if (_spinnerType == SpinnerType.Determinate)
            {
                _startAngle += 5f;
                _sweepAngle += 5f;
                if (_sweepAngle >= 360f)
                {
                    _sweepAngle = 0f;
                    _startAngle = 0f;
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                InvalidateSurface();
            });
        }


        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            DrawCircle(e);
        }

        private void DrawCircle(SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            float diameter = Math.Min(info.Width, info.Height) - 200;
            float centerX = info.Width / 2;
            float centerY = info.Height / 2;
            float radius = diameter / 2;

            SKRect rect = new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius);

            var arcPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue).ToSKColor(),
                StrokeWidth = 15
            };

            if (_spinnerType == SpinnerType.Indeterminate)
            {
                canvas.Save();
                canvas.RotateDegrees(_rotationAngle, centerX, centerY);
                canvas.DrawArc(rect, _startAngle, _sweepAngle, false, arcPaint);
                canvas.Restore();

            }
            else if (_spinnerType == SpinnerType.Determinate)
            {
                float startAngle = 0f;
                float progress = 1f; 
                float endAngle = startAngle + (_sweepAngle * progress);
                canvas.DrawArc(rect, 270, endAngle - startAngle, false, arcPaint);
            }
        }
    }
}
