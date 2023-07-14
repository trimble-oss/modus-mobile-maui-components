namespace Trimble.Modus.Components;

public partial class TextColumn : DataGridColumn
{
    public static readonly BindableProperty DescriptionBindableProperty = BindableProperty.Create(nameof(DescriptionProperty), typeof(string), typeof(TextColumn), string.Empty);
    public static readonly BindableProperty StringFormatProperty = BindableProperty.Create(nameof(StringFormat), typeof(string), typeof(TextColumn));

    public string DescriptionProperty
    {
        get => (string)GetValue(DescriptionBindableProperty);
        set => SetValue(DescriptionBindableProperty, value);
    }

    /// <summary>
    /// String format for the cell
    /// </summary>
    public string StringFormat
    {
        get => (string)GetValue(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
    }

    public TextColumn()
	{
		InitializeComponent();
	}
}
