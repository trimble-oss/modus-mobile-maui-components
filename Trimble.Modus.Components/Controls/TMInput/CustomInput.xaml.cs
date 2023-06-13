using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components.Controls;

public partial class CustomInput : ContentView
{

    #region Private Properties

    private const double disabledOpacity = 0.4;

    private ValidationResponse _validationResponse;

    #endregion

    #region Bindable Properties

    /// <summary>
    /// Identifies the TextProperty bindable property. This property is used to display the property text
    /// </summary>
    public static readonly BindableProperty TextProperty =
       BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomInput), propertyChanged: OnTextChanged);

    /// <summary>
    /// Identifies the IsPassword property, this property is used to hide the text entered into dots.
    /// </summary>
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(CustomInput), false);

    /// <summary>
    /// Gets or sets the type of keyboard is used when the entry is focused.
    /// </summary>
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(CustomInput), Keyboard.Default);

    /// <summary>
    /// Gets or sets the placeholder text of the text input
    /// </summary>
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomInput), string.Empty);

    /// <summary>
    /// Used to set the return command of the entry
    /// </summary>
    public static readonly BindableProperty ReturnCommandProperty =
        BindableProperty.Create(nameof(ReturnCommand), typeof(ICommand), typeof(CustomInput), null);

    /// <summary>
    /// Used to set the properties for the return command
    /// </summary>
    public static readonly BindableProperty ReturnCommandParameterProperty =
        BindableProperty.Create(nameof(ReturnCommandParameter), typeof(object), typeof(CustomInput), null, BindingMode.OneWay, null);

    /// <summary>
    /// Gets or sets the type of the return button in the keyboard
    /// </summary>
    public static readonly BindableProperty ReturnTypeProperty =
        BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(CustomInput), ReturnType.Default);

    /// <summary>
    /// Gets or sets the maximum allowed length of the text entry
    /// </summary>
    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(CustomInput), int.MaxValue);

    /// <summary>
    /// Gets or sets the text for the title label in the control
    /// </summary>
    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets the text for helper text label in the control
    /// </summary>
    public static readonly BindableProperty HelperTextProperty =
        BindableProperty.Create(nameof(HelperText), typeof(string), typeof(CustomInput), null, propertyChanged: OnHelperTextChanged);

    /// <summary>
    /// Gets or sets the image source for the left icon in the entry
    /// </summary>
    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets the image source for the right icon in the entry
    /// </summary>
    public static readonly BindableProperty RightIconSourceProperty =
        BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets the command for left icon
    /// </summary>
    /// 
    public static readonly BindableProperty LeftIconCommandProperty =
        BindableProperty.Create(nameof(LeftIconCommand), typeof(ICommand), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets the command property for left icon
    /// </summary>
    public static readonly BindableProperty LeftIconCommandParameterProperty =
        BindableProperty.Create(nameof(LeftIconCommandParameter), typeof(object), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets the command for right icon
    /// </summary>
    public static readonly BindableProperty RightIconCommandProperty =
        BindableProperty.Create(nameof(RightIconCommand), typeof(ICommand), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets the command property for right icon
    /// </summary>
    public static readonly BindableProperty RightIconCommandParameterProperty =
        BindableProperty.Create(nameof(RightIconCommandParameter), typeof(object), typeof(CustomInput), null);

    /// <summary>
    /// Gets or sets value that indicates whether the input control is enabled or not.
    /// </summary>
    public static new readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(CustomInput), true, propertyChanged: OnEnabledPropertyChanged);

    /// <summary>
    /// Gets or sets value that indicates whether the input control is readonly or not.
    /// </summary>
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(CustomInput), false, propertyChanged: OnReadOnlyPropertyChanged);

    #endregion

    #region Public properties
    /// <summary>
    /// Delegate for the input validation function
    /// </summary>
    /// <param name="sender">Reference to the sender input control</param>
    /// <returns>Tuple of bool and string, bool represent the result of the text validation and string represent the text to be displayed</returns>
    public delegate Tuple<bool, string> InputValidationHandler(object sender);
    ///// <summary>
    ///// Public event handler to be invoked when text is changed
    ///// </summary>
    public event EventHandler<TextChangedEventArgs> TextChanged;

    /// <summary>
    /// Public event handler to hold the input validation function
    /// </summary>
    public event InputValidationHandler InputValidation;

    /// <summary>
    /// Gets or sets the helper text
    /// </summary>
    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the helper text
    /// </summary>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    /// <summary>
    /// Gets or sets the entry text
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the text color
    /// </summary>
    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets the readonly property of the control
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// Gets or sets the keyboard type property for text input
    /// </summary>
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }
    /// <summary>
    /// Gets or sets the Placeholder value 
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// Gets or sets the Placeholder value 
    /// </summary>
    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    /// <summary>
    /// Gets or sets the Return command
    /// </summary>
    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the return command parameter
    /// </summary>
    public object ReturnCommandParameter
    {
        get => GetValue(ReturnCommandParameterProperty);
        set => SetValue(ReturnCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the title text
    /// </summary>
    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum length
    /// </summary>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
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

    #region Constructor

    public CustomInput()
    {
        InitializeComponent();
        SetDefault(this);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Default method to set the deafult values to input
    /// </summary>
    /// <param name="customInput"></param>

    private static void SetDefault(CustomInput customInput)
    {
        customInput._validationResponse = ValidationResponse.Info;
        SetBorderColor(customInput);
    }

    /// <summary>
    /// Triggered when the IsReadOnly property is changed
    /// </summary>
    /// <param name="bindable">Object</param>
    /// <param name="oldValue">Old value</param>
    /// <param name="newValue">New value</param>
    private static void OnReadOnlyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomInput tmInput)
        {
            tmInput.UpdateBorderColors(tmInput);
        }
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomInput customInput)
        {
            Tuple<bool, string> result = null;

            if (!string.IsNullOrEmpty((string)newValue))
            {
                result = customInput.InputValidation?.Invoke(customInput);
                if (result != null && !string.IsNullOrEmpty(result.Item2))
                {
                    customInput.inputHelperLabel.Text = result.Item2;
                    var response = result.Item1 ? ValidationResponse.Success : ValidationResponse.Error;
                    // To avoid multiple times updating needs to check old value not equal to current value
                    if (customInput._validationResponse != response)
                    {
                        customInput._validationResponse = response;
                        SetBorderColor(customInput);
                    }

                }
            }
            else
            {
                // To avoid multiple times updating needs to check old value not equal to current value
                if (customInput._validationResponse != ValidationResponse.Info)
                {
                    customInput._validationResponse = ValidationResponse.Info;
                    SetBorderColor(customInput);
                }
            }
            customInput.TextChanged?.Invoke(customInput, new TextChangedEventArgs((string)oldValue, (string)newValue));
        }
    }

    private static void SetBorderColor(CustomInput customInput)
    {
        if (!customInput.inputBorderlessEntry.IsFocused)
        {
            customInput.inputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Black);
            customInput.inputBorder.StrokeThickness = 1;

            if (!string.IsNullOrEmpty(customInput.HelperText))
            {
                customInput.inputHelperIcon.Source = ImageSource.FromFile(ImageConstants.HelperImage);
                customInput.inputHelperLabel.Text = customInput.HelperText;
            }
        }
        else
        {
            switch (customInput._validationResponse)
            {
                case ValidationResponse.Success:
                    customInput.inputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.Green);
                    customInput.inputHelperIcon.Source = ImageSource.FromFile(ImageConstants.TickImage);
                    customInput.inputBorder.StrokeThickness = 1;
                    break;
                case ValidationResponse.Error:
                    customInput.inputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.DangerRed);
                    customInput.inputHelperIcon.Source = ImageSource.FromFile(ImageConstants.ErrorImage);
                    customInput.inputBorder.StrokeThickness = 1;
                    break;
                default:
                    customInput.inputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleBlue);
                    customInput.inputHelperIcon.Source = ImageSource.FromFile(ImageConstants.HelperImage);
                    customInput.inputHelperLabel.Text = customInput.HelperText;
                    customInput.inputBorder.StrokeThickness = 2;
                    break;
            }
        }
    }

    private static void OnHelperTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomInput customInput)
        {
            SetBorderColor(customInput);
        }
    }

    private void InputBorderlessEntry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEntry)
        {
            SetBorderColor(this);
        }
    }
    private void UpdateBorderColors(CustomInput customInput)
    {

        if (customInput.IsReadOnly)
        {
            SetReadOnlyStyles(customInput);
        }
        else
        {
            if (customInput.IsEnabled)
            {
                customInput.inputBorder.Opacity = customInput.inputLabel.Opacity = customInput.inputHelperLayout.Opacity = 1;
                customInput.inputBorder.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.White);
                SetBorderColor(customInput);
            }
            else
            {
                customInput.inputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray);
                customInput.inputBorder.StrokeThickness = 1;
                customInput.inputBorder.Opacity = customInput.inputLabel.Opacity = customInput.inputHelperLayout.Opacity = disabledOpacity;
            }
        }
    }
    private static void SetReadOnlyStyles(CustomInput customInput)
    {
        customInput.inputBorder.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleReadOnlyGray);
        customInput.inputBorder.Stroke = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleReadOnlyGray);
        customInput.inputBorder.StrokeThickness = 0;
        customInput.inputBorder.Opacity = customInput.inputLabel.Opacity = customInput.inputHelperLayout.Opacity = 1;
    }
    private void InputBorderlessEntry_Unfocused(object sender, FocusEventArgs e)
    {
        if (sender is BorderlessEntry)
        {
            SetBorderColor(this);
        }
    }

    private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CustomInput customInput)
        {
            customInput.UpdateBorderColors(customInput);
        }
    }

    #endregion

}
