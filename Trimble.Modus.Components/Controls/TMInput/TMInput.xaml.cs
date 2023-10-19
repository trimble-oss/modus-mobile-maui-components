using System.Runtime.CompilerServices;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls.BaseInput;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TMInput : BaseInput
{
    #region Bindable Properties
    /// <summary>
    /// Identifies the IsPassword property, this property is used to hide the text entered into dots.
    /// </summary>
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(TMInput), false);
    /// <summary>
    /// Gets or sets the image source for the left icon in the entry
    /// </summary>
    ///
    public View LeftView
    {
        get => (View)GetValue(LeftViewProperty);
        set => SetValue(LeftViewProperty, value);
    }

    public static readonly BindableProperty LeftViewProperty =
        BindableProperty.Create(
            nameof(LeftView),
            typeof(View),
            typeof(TMInput),
            null);
    public View RightView
    {
        get => (View)GetValue(RightViewProperty);
        set => SetValue(RightViewProperty, value);
    }

    public static readonly BindableProperty RightViewProperty =
        BindableProperty.Create(
            nameof(RightView),
            typeof(View),
            typeof(TMInput),
            null);

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TMInput), null);

    /// <summary>
    /// Gets or sets the image source for the right icon in the entry
    /// </summary>
    public static readonly BindableProperty RightIconSourceProperty =
        BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TMInput), null);

    /// <summary>
    /// Gets or sets the command for left icon
    /// </summary>
    /// 
    public static readonly BindableProperty LeftIconCommandProperty =
        BindableProperty.Create(nameof(LeftIconCommand), typeof(ICommand), typeof(TMInput), null);

    /// <summary>
    /// Gets or sets the command property for left icon
    /// </summary>
    public static readonly BindableProperty LeftIconCommandParameterProperty =
        BindableProperty.Create(nameof(LeftIconCommandParameter), typeof(object), typeof(TMInput), null);

    /// <summary>
    /// Gets or sets the command for right icon
    /// </summary>
    public static readonly BindableProperty RightIconCommandProperty =
        BindableProperty.Create(nameof(RightIconCommand), typeof(ICommand), typeof(TMInput), null);

    /// <summary>
    /// Gets or sets the command property for right icon
    /// </summary>
    public static readonly BindableProperty RightIconCommandParameterProperty =
        BindableProperty.Create(nameof(RightIconCommandParameter), typeof(object), typeof(TMInput), null);

    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets the helper text
    /// </summary>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
    /// <summary>
    /// Gets or sets the left icon in entry's source
    /// </summary>
    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the right icon in entry's source
    /// </summary>
    public ImageSource RightIconSource
    {
        get => (ImageSource)GetValue(RightIconSourceProperty);
        set => SetValue(RightIconSourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the command for left icon
    /// </summary>
    public ICommand LeftIconCommand
    {
        get => (ICommand)GetValue(LeftIconCommandProperty);
        set => SetValue(LeftIconCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter for left icon
    /// </summary>
    public object LeftIconCommandParameter
    {
        get => GetValue(LeftIconCommandParameterProperty);
        set => SetValue(LeftIconCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the right icon command
    /// </summary>
    public ICommand RightIconCommand
    {
        get => (ICommand)GetValue(RightIconCommandProperty);
        set => SetValue(RightIconCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the right icon command parameter
    /// </summary>
    public object RightIconCommandParameter
    {
        get => GetValue(LeftIconCommandParameterProperty);
        set => SetValue(LeftIconCommandParameterProperty, value);
    }
    #endregion
    public TMInput()
    {
        InitializeComponent();
    }

    protected override void RetrieveAndProcessChildElement()
    {
        base.RetrieveAndProcessChildElement();

        // Additional logic specific to TMInput
        InputBorder = (Border)GetTemplateChild("inputBorder");
        HelperIcon = (Image)GetTemplateChild("inputHelperIcon");
        HelperLabel = (Label)GetTemplateChild("inputHelperLabel");
        HelperLayout = (HorizontalStackLayout)GetTemplateChild("inputHelperLayout");
        InputLabel = (Label)GetTemplateChild("inputLabel");
    }

    internal override View GetCoreContent()
    {
        return this.FindByName<BorderlessEntry>("inputBorderlessEntry"); ;
    }

    private void InputBorderlessEntry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEntry)
        {
            SetBorderColor(this);
        }
    }

    private void InputBorderlessEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEntry)
        {
            SetBorderColor(this);
        }
    }

    #region Internal methods
    /// <summary>
    /// Method to Toggle the IsEnabled state of the right icon
    /// Used in <see cref="TMNumberInput"/>
    /// </summary>
    internal void ToggleRightIconState(bool enabledState)
    {
        //inputRightIcon.IsEnabled = enabledState;
        //inputRightIcon.Opacity = enabledState ? 1 : disabledOpacity;
    }

    /// <summary>
    /// Method to Toggle the IsEnabled state of the left icon
    /// Used in <see cref="TMNumberInput"/>
    /// </summary>
    internal void ToggleLeftIconState(bool enabledState)
    {
        //inputLeftIcon.IsEnabled = enabledState;
        //inputLeftIcon.Opacity = enabledState ? 1 : disabledOpacity;
    }

    /// <summary>
    /// Used to center align texts in Number Input
    /// Used in <see cref="TMNumberInput"/>
    /// </summary>
    internal void SetCenterTextAlignment()
    {
        inputBorderlessEntry.HorizontalTextAlignment = TextAlignment.Center;
    }

    #endregion
}
