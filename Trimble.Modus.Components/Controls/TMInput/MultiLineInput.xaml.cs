namespace Trimble.Modus.Components;

using System;
using Trimble.Modus.Components.Controls.BaseInput;

public partial class MultiLineInput : BaseInput
{
    #region Bindable Properties
    public static readonly BindableProperty AutoSizeProperty =
            BindableProperty.Create(nameof(AutoSize), typeof(bool), typeof(MultiLineInput), false, propertyChanged: OnAutoSizePropertyChanged);
    public new static readonly BindableProperty HeightRequestProperty =
           BindableProperty.Create(nameof(HeightRequest), typeof(int), typeof(MultiLineInput), propertyChanged: OnHeightRequestChanged);

    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets AutoSizeProperty
    /// </summary>
    public bool AutoSize
    {
        get => (bool)GetValue(AutoSizeProperty);
        set => SetValue(AutoSizeProperty, value);
    }
    public new int HeightRequest
    {
        get => (int)GetValue(HeightRequestProperty);
        set => SetValue(HeightRequestProperty, value);
    }
    #endregion
    public MultiLineInput()
    {
        InitializeComponent();
    }
    #region Methods
    protected override void RetrieveAndProcessChildElement()
    {
        base.RetrieveAndProcessChildElement();

        // Additional logic specific to TMInput
        InputBorder = (Border)GetTemplateChild("inputBorder");
        HelperIcon = (Image)GetTemplateChild("inputHelperIcon");
        HelperLabel = (Label)GetTemplateChild("inputHelperLabel");
        HelperLayout = (HorizontalStackLayout)GetTemplateChild("inputHelperLayout");
        ControlLabel = (ControlLabel)GetTemplateChild("controlLabel");
    }

    internal override InputView GetCoreContent()
    {
        return this.FindByName<BorderlessEditor>("inputBorderlessEditor"); ;
    }
    private static void OnAutoSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MultiLineInput multiLineInput)
        {
            multiLineInput.inputBorderlessEditor.AutoSize = multiLineInput.AutoSize ? (EditorAutoSizeOption)1 : (EditorAutoSizeOption)0;
        }
    }
    private static void OnHeightRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MultiLineInput multiLineInput && (int)newValue > 44)
        {
            multiLineInput.inputBorderlessEditor.HeightRequest = (int)newValue;
        }
    }

    private void InputBorderlessEditor_Focused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEditor)
        {
            SetBorderColor(this);
        }
    }

    private void InputBorderlessEditor_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEditor)
        {
            SetBorderColor(this);
        }
    }
    #endregion
}
