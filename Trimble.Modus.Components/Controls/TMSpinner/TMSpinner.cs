using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;
using Trimble.Modus.Components.Enums;
using Size = Trimble.Modus.Components.Enums.Size;
using System.Diagnostics;

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
        private int _strokeWidth;
        internal bool isDisposed;
        #endregion
        #region Binding Properties
        public static readonly BindableProperty SpinnerTypeProperty =
            BindableProperty.Create(nameof(SpinnerType), typeof(SpinnerType), typeof(TMSpinner), defaultValue: SpinnerType.InDeterminate, propertyChanged: OnSpinnerTypeChanged);

        public static readonly BindableProperty SpinnerColorProperty =
            BindableProperty.Create(nameof(SpinnerColor), typeof(SpinnerColor), typeof(TMSpinner), defaultValue: SpinnerColor.Primary, propertyChanged: OnSpinnerColorChanged);

        internal static readonly BindableProperty FillColorProperty =
            BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(TMSpinner), defaultValue: Colors.Transparent, propertyChanged: OnFillColorChanged);

        internal static readonly BindableProperty SpinnerSizeProperty =
            BindableProperty.Create(nameof(SpinnerSize), typeof(Size), typeof(TMSpinner), defaultValue: Size.Default, propertyChanged: OnSpinnerSizeChanged);

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
        internal Color FillColor
        {
            get => (Color)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }
        internal Size SpinnerSize
        {
            get => (Size)GetValue(SpinnerSizeProperty);
            set => SetValue(SpinnerSizeProperty, value);
        }

        #endregion
        public TMSpinner()
        {
#if WINDOWS
            _strokeWidth = 5;
            minWidth = 34;
            minHeight = 34;
#else
            _strokeWidth = 15;
            minWidth = 32;
            minHeight = 32;
#endif
            CreateSpinner();
        }

        private void CreateSpinner()
        {

            WidthRequest = minWidth;
            HeightRequest = minHeight;
            IgnorePixelScaling = false;
            _animationTimer?.Dispose();
            StartAnimation();
        }
        #region Protected Methods
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            DrawCircle(e);
        }
        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (Parent == null)
            {
                isDisposed = true;
            }
            else
            {
                isDisposed = false;
            }
        }
        #endregion
        #region Private Methods
        private static void OnSpinnerSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMSpinner tmSpinner)
            {
                Console.WriteLine("Spinner Size Changed", (Size)newValue);
                switch ((Size)newValue)
                {
                    case Size.XSmall:
#if WINDOWS
                        tmSpinner.minWidth = 26;
                        tmSpinner.minHeight = 26;
                        tmSpinner._strokeWidth = 3;
#else
                        tmSpinner.minWidth = 22;
                        tmSpinner.minHeight = 22;
                        tmSpinner._strokeWidth = 8;
#endif
                        break;
                    case Size.Small:
#if WINDOWS
                        tmSpinner.minWidth = 30;
                        tmSpinner.minHeight = 30;
                        tmSpinner._strokeWidth = 4;
#else
                        tmSpinner.minWidth = 26;
                        tmSpinner.minHeight = 26;
                        tmSpinner._strokeWidth = 12;
#endif
                        break;
                    case Size.Large:
#if WINDOWS
                        tmSpinner.minWidth = 38;
                        tmSpinner.minHeight = 38;
                        tmSpinner._strokeWidth = 5;
#else
                        tmSpinner.minWidth = 36;
                        tmSpinner.minHeight = 36;
                        tmSpinner._strokeWidth = 15;
#endif
                        break;
                    default:
                    case Size.Default:
#if WINDOWS
                        tmSpinner.minWidth = 34;
                        tmSpinner.minHeight = 34;
                        tmSpinner._strokeWidth = 5;
#else
                        tmSpinner.minWidth = 32;
                        tmSpinner.minHeight = 32;
                        tmSpinner._strokeWidth = 15;
#endif
                        break;
                }
                tmSpinner.CreateSpinner();

            }
        }

        private static void OnSpinnerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMSpinner tmSpinner)
            {
                if(tmSpinner.SpinnerColor == SpinnerColor.Primary)
                {
                    tmSpinner.SetDynamicResource(TMSpinner.FillColorProperty, "SpinnerPrimaryFillColor");
                }
                else
                {
                    tmSpinner.SetDynamicResource(TMSpinner.FillColorProperty, "SpinnerSecondaryFillColor");
                }
            }
        }

        private static void OnFillColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMSpinner tmSpinner)
            {
                tmSpinner._spinnerColor = (Color)newValue;
            }
        }

        private static void OnSpinnerTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is TMSpinner tmSpinner)
            {
                tmSpinner._spinnerType = (SpinnerType)newValue;
            }
        }

        internal void StartAnimation()
        {
            _animationTimer = new Timer(UpdateAnimation, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(animateTimerSeconds));
        }

        private void UpdateAnimation(object state)
        {
            if (isDisposed)
            {
                _animationTimer?.Dispose();
            }
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
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Dispatcher.Dispatch(InvalidateSurface);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(InvalidateSurface);
            }
        }
        private void DrawCircle(SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            float height = Math.Max(info.Height, minHeight);
            float width = Math.Max(info.Width, minWidth);
            float diameter = Math.Min(width, height) - 15;
            float centerX = width / 2;
            float centerY = height / 2;
            float radius = diameter / 2;

            SKRect rect = new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius);

            var arcPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = _spinnerColor.ToSKColor(),
                StrokeWidth = _strokeWidth
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
