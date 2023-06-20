using System.Runtime.CompilerServices;

namespace Trimble.Modus.Components;

public partial class MultiLineInput : BaseInput
{
    private static readonly BindableProperty ContentHeightProperty =
        BindableProperty.Create(nameof(ContentHeight), typeof(double), typeof(MultiLineInput), default(double), propertyChanged: OnHeightChanged);

    public double ContentHeight
    {
        get { return (double)GetValue(ContentHeightProperty); }
        set { SetValue(ContentHeightProperty, value); }
    }
  
    private static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MultiLineInput input)
        {
            input.editor.HeightRequest = (double)newValue;
             

        }
        if(bindable is BaseInput baseInput)
        {
            baseInput.Height = (double)newValue;
        }
    }
    public MultiLineInput()
    {
        InitializeComponent();
      //  RightIconSource = ImageSource.FromResource("Trimble.Modus.Components.Images.warning.png");
        setEditorIcon = true;
        BindingContext = this;

    }

    private void editor_Focused(object sender, FocusEventArgs e)
    {
        if (sender is InputView)
        {
            SetBorderColor(this);
        }
    }

    private void editor_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is InputView)
        {
            SetBorderColor(this);
        }
    }
}
