using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components
{
    public class TMSpinner : SKCanvasView
    {
        #region Private Properties
        private SpinnerType _spinnerType;
        private Color _spinnerColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
        private float _startAngle = 0f;
        private int minWidth = 42, minHeight = 42, animateTimerSeconds = 10;
        private float _sweepAngle = 180f;
        private float _rotationAngle = 0f;
        private Timer _animationTimer;
        #endregion
        #region Binding Properties
        public static readonly BindableProperty SpinnerTypeProperty =
            BindableProperty.Create(nameof(SpinnerType), typeof(SpinnerType), typeof(TMSpinner), defaultValue: SpinnerType.InDeterminate, propertyChanged: OnSpinnerTypeChanged);

        public static readonly BindableProperty SpinnerColorProperty =
            BindableProperty.Create(nameof(SpinnerColor), typeof(SpinnerColor), typeof(TMSpinner), defaultValue: SpinnerColor.Primary, propertyChanged: OnSpinnerColorChanged);
        #endregion
        #region Public Properties
        public SpinnerType SpinnerType
        {
            get => (SpinnerType)GetValue(SpinnerTypeProperty);
            set => SetValue(SpinnerTypeProperty, value);
        }
        public SpinnerColor SpinnerColor
        {
            get => (SpinnerColor)GetValue(SpinnerColorProperty);
            set => SetValue(SpinnerColorProperty, value);
        }
        #endregion
        public TMSpinner()
        {
            WidthRequest = minWidth;
            HeightRequest = minHeight;
            IgnorePixelScaling = false;
            StartAnimation();
        }
        #region Protected Methods
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            DrawCircle(e);
        }
        #endregion
        #region Private Methods
        private static void OnSpinnerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMSpinner tmSpinner)
            {
                tmSpinner._spinnerColor = ((SpinnerColor)newValue == SpinnerColor.Secondary) ? ResourcesDictionary.ColorsDictionary(ColorsConstants.White)
                    : ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
            }
        }
        private static void OnSpinnerTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMSpinner tmSpinner)
            {
                tmSpinner._spinnerType = (SpinnerType)newValue;
            }
        }
        private void StartAnimation()
        {
            _animationTimer = new Timer(UpdateAnimation, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(animateTimerSeconds));
        }

        private void UpdateAnimation(object state)
        {
            if (_spinnerType == SpinnerType.InDeterminate)
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
            MainThread.BeginInvokeOnMainThread(() =>
            {
                InvalidateSurface();
            });
        }
        private void DrawCircle(SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            float height = Math.Max(info.Height, minHeight);
            float width = Math.Max(info.Width, minWidth);
            if (height < minHeight || width < minWidth)
            {
                Console.WriteLine("Height or Width < 42");
            }
            float diameter = Math.Min(width, height) - 15;
            float centerX = width / 2;
            float centerY = height / 2;
            float radius = diameter / 2;

            SKRect rect = new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius);

            var arcPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = _spinnerColor.ToSKColor(),
                StrokeWidth = 15
            };

            if (_spinnerType == SpinnerType.InDeterminate)
            {
                _sweepAngle = 270;
                canvas.Save();
                canvas.RotateDegrees(_rotationAngle, centerX, centerY);
                canvas.DrawArc(rect, _startAngle, _sweepAngle, false, arcPaint);
                canvas.Restore();

            }
            else if (_spinnerType == SpinnerType.Determinate)
            {
                float startAngle = 270f;
                float progress = 1f;
                float endAngle = startAngle + (_sweepAngle * progress);
                canvas.DrawArc(rect, startAngle, endAngle - startAngle, false, arcPaint);
            }
            canvas.Flush();
        }
        #endregion
    }
}