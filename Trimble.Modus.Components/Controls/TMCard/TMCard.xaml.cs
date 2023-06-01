using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace Trimble.Modus.Components.Controls.TMCard;

public partial class TMCard : ContentView
{
    #region Fields 
    private readonly TapGestureRecognizer _tapGestureRecognizer;
    private Shadow _shadow;
    private Border border;
    private const int _borderRadius = 2;
    #endregion

    #region Public properties
    public Command Command
    {
        get => (Command)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    #region Bindable Properties
    public static readonly BindableProperty CommandProperty =
           BindableProperty.Create(nameof(Command), typeof(Command), typeof(TMCard), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TMCard), null);
    #endregion

    public TMCard()
    {
        _shadow = new Shadow
        {
            Brush = Colors.Black,
            Radius = 15,
            Opacity = 100
        };
        border = new Border
        {
            Padding = 16,
            Shadow = _shadow,
            BackgroundColor = Colors.White,
            Stroke = Colors.Transparent,
            StrokeShape = new Rectangle
            {
                RadiusX = _borderRadius,
                RadiusY = _borderRadius
            },

        };
        GestureRecognizers.Add(_tapGestureRecognizer = new TapGestureRecognizer());
        _tapGestureRecognizer.NumberOfTapsRequired = 1;
        _tapGestureRecognizer.Tapped += OnTapped;
    }
    /// <summary>
    /// Triggered when tapping the card
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnTapped(object sender, EventArgs e)
    {
        Command?.Execute(CommandParameter);
        border.BackgroundColor = (Color)BaseComponent.colorsDictionary()["ToastBlue"];
        border.Stroke = (Color)BaseComponent.colorsDictionary()["TrimbleBlueClicked"];
        this.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {

            border.BackgroundColor = Colors.White;
            border.Stroke = Colors.Transparent;

            return false;
        });
    }
    #region Overriden Methods
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (border != null && Content is View view)
        {

            border.Content = view;
            Content = border;
        }
    }

    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);
        if (BindingContext == null)
        {
            BindingContext = child;
        }
    }
    #endregion

}