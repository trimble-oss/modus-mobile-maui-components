namespace Trimble.Modus.Components;

public partial class BooleanColumn : DataGridColumn
{
    public static readonly BindableProperty SwitchBindableProperty = BindableProperty.Create(nameof(SwitchProperty), typeof(bool), typeof(BooleanColumn), false);

    public bool SwitchProperty
    {
        get => (bool)GetValue(SwitchBindableProperty);
        set => SetValue(SwitchBindableProperty, value);
    }
    public BooleanColumn()
	{
		InitializeComponent();
	}
}
