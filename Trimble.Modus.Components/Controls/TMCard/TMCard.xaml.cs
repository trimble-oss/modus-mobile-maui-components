using System.Windows.Input;

namespace Trimble.Modus.Components;

public partial class TMCard : ContentView
{
    #region Fields 
    private EventHandler _clicked;
    #endregion

    #region Public properties
    public new Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }
    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    public event EventHandler Clicked
    {
        add { _clicked += value; }
        remove { _clicked -= value; }
    }
    public ICommand Command
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

    public new static readonly BindableProperty PaddingProperty =
       BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(TMCard), defaultValue: new Thickness(16));

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TMCard), defaultValue: false);

    public static readonly BindableProperty ClickedEventProperty =
          BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(TMCard));

    public static readonly BindableProperty CommandProperty =
           BindableProperty.Create(nameof(Command), typeof(Command), typeof(TMCard), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TMCard), null);

    #endregion
    public TMCard()
    {
        InitializeComponent();
        this.border.Shadow.Offset = DeviceInfo.Platform == DevicePlatform.iOS ? new Point(0, 2) : new Point(-1, 1);
    }

    #region Overriden Methods
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (this.border != null && Content is View view)
        {
            this.border.Content = view;
            Content = this.border;
        }
    }
    #endregion

    private void OnTapped(object sender, TappedEventArgs e)
    {
        Command?.Execute(CommandParameter);
        _clicked?.Invoke(this, e);
        this.border.SetDynamicResource(BackgroundColorProperty, "CardHoverBackgroundColor");
        this.border.Content.Opacity = 0.3;
        this.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            this.border.SetDynamicResource(BackgroundColorProperty, "CardBackgroundColor");
            this.border.Content.Opacity = 1;
            return false;
        });
    }
}
