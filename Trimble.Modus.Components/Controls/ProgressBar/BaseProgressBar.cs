using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

internal class BaseProgressBar : SKCanvasView
{
    private readonly double _defaultHeightRequest = 16;
    private readonly double _smallHeightRequest = 8;

    // actual canvas instance to draw on
    private SKCanvas _canvas;

    // rectangle which will be used to draw the Progress Bar
    private SKRect _drawRect;

    // holds information about the dimensions, etc.
    private SKImageInfo _info;

    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
       nameof(Progress), typeof(float), typeof(BaseProgressBar), 0.0f, propertyChanged: OnBindablePropertyChanged);

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
   nameof(Size), typeof(ProgressBarSize), typeof(BaseProgressBar), ProgressBarSize.Default, propertyChanged: OnSizeChangedProperty);

    public float Progress
    {
        get => (float)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public ProgressBarSize Size
    {
        get => (ProgressBarSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public BaseProgressBar()
    {
        this.HeightRequest = _defaultHeightRequest;
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        _canvas = e.Surface.Canvas;
        _canvas.Clear(); // clears the canvas for every frame
        _info = e.Info;
        _drawRect = new SKRect(0, 0, _info.Width, _info.Height);

        DrawBase();
        DrawProgress();
    }

    private void DrawProgress()
    {
        using var progressPath = new SKPath();

        var progressRect = new SKRect(0, 0, _info.Width * Progress, _info.Height);

        progressPath.AddRect(progressRect);

        _canvas.DrawPath(progressPath, new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            Color = Helpers.ResourcesDictionary.ColorsDictionary(Constant.ColorsConstants.TrimbleBlue).ToSKColor()
        });
    }
    private void DrawBase()
    {
        using var basePath = new SKPath();

        basePath.AddRect(_drawRect);

        _canvas.DrawPath(basePath, new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            StrokeWidth = 5,
            IsStroke = true,
            Color = Helpers.ResourcesDictionary.ColorsDictionary(Constant.ColorsConstants.TrimbleNeutralGray).ToSKColor(),
            IsAntialias = true
        });
    }

    private static void OnBindablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((BaseProgressBar)bindable).InvalidateSurface();
    }

    private static void OnSizeChangedProperty(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BaseProgressBar progressBar)
        {
            progressBar.HeightRequest = progressBar.Size == ProgressBarSize.Default ? progressBar._defaultHeightRequest : progressBar._smallHeightRequest;
            progressBar.InvalidateSurface();
        }
    }


}
