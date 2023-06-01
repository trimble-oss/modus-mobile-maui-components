using System.Windows.Input;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public class TMRadioButtonGroup : StackLayout, IDisposable
{
    private readonly List<TMRadioButton> _radioButtons = new();

    #region Bindable Properties

    public static new readonly BindableProperty IsEnabledProperty = BindableProperty.Create(
        nameof(IsEnabled),
        typeof(bool),
        typeof(TMRadioButtonGroup),
        true,
        propertyChanged: OnIsEnabledChanged);
    
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
        nameof(Size),
        typeof(CheckboxSize),
        typeof(TMRadioButtonGroup),
        CheckboxSize.Default,        
        propertyChanged: OnSizeChanged);

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(TMRadioButtonGroup),
        -1,
        BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((TMRadioButtonGroup)bindable).UpdateSelectedStates();
            ((TMRadioButtonGroup)bindable).SetDefaultSelectedRadioButton();
        }
    );

    public static readonly BindableProperty SelectedRadioButtonChangedCommandProperty =
        BindableProperty.Create(
            nameof(SelectedRadioButtonChangedCommand),
            typeof(ICommand),
            typeof(TMRadioButtonGroup)
        );

    #endregion

    #region Public Properties & events

    /// <summary>
    /// IsEnabled property for the group
    /// </summary>
    public new bool IsEnabled {
        get { return (bool)GetValue (IsEnabledProperty); }
        set { SetValue (IsEnabledProperty, value); } 
    }

        /// <summary>
        /// Gets or sets a value indicating the size of <see cref="TMRadioButton"/> in this group
        /// </summary>
        public CheckboxSize Size
        {
            get => (CheckboxSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

    /// <summary>
    /// Triggered when <see cref="SelectedIndex"/> changes.
    /// </summary>
    public event EventHandler<TMRadioButtonEventArgs> SelectedRadioButtonChanged;

    /// <summary>
    /// SelectedIndex of <see cref="TMRadioButton"/> in this Group
    /// </summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// Triggered when <see cref="SelectedIndex"/> changes.
    /// Has the <see cref="TMRadioButtonEventArgs"/> as a Command Parameter
    /// </summary>
    public ICommand SelectedRadioButtonChangedCommand
    {
        get { return (ICommand)GetValue(SelectedRadioButtonChangedCommandProperty); }
        set { SetValue(SelectedRadioButtonChangedCommandProperty, value); }
    }

    #endregion


    #region private methods

    /// <summary>
    /// Disable radio buttons in the group when the <see cref="IsEnabled"/> property changes
    /// </summary>
    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMRadioButtonGroup radioButtonGroup = (TMRadioButtonGroup)bindable;
        if(radioButtonGroup._radioButtons.Count > 1){
            foreach (TMRadioButton radioButton in radioButtonGroup._radioButtons)
            {
                radioButton.IsEnabled = (bool)newValue;
            }
        }
    }

    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMRadioButtonGroup group && group._radioButtons.Count > 1)
        {
            foreach (TMRadioButton button in group._radioButtons)
            {
                button.Size = (CheckboxSize)newValue;
            }
        }
    }

    /// <summary>
    /// Sets the <see cref="TMRadioButton.IsSelected"/> property of the radio buttons in the group
    /// </summary>
    private void UpdateSelectedStates()
    {
        int index = 0;
        LoopChildren(Children, index);
    }

    private void SetDefaultSelectedRadioButton()
    {
        if (SelectedIndex < 0 || SelectedIndex >= _radioButtons.Count)
        {
            return;
        }

        _radioButtons[SelectedIndex].IsSelected = true;
    }

    /// <summary>
    /// Loops through the children of the <see cref="TMRadioButtonGroup"/> and sets the <see cref="TMRadioButton.IsSelected"/> property of the radio buttons in the group
    /// </summary>
    /// <param name="children">The children of the <see cref="TMRadioButtonGroup"/></param>
    protected override void OnChildAdded(Element child)
    {

        if (child is TMRadioButton radioButton)
        {
            base.OnChildAdded(child);
            AddRadioButtons(radioButton);
        }
        else if (child is Layout layout)
        {
            throw new ArgumentException($"Only {nameof(TMRadioButton)} can be added to {nameof(TMRadioButtonGroup)}");
        }
    }

    /// <summary>
    /// Adds a <see cref="TMRadioButton"/> to the list of radio buttons.
    /// </summary>
    /// <param name="radioButton">The <see cref="TMRadioButton"/> to add.</param>
    private void AddRadioButtons(TMRadioButton radioButton)
    {
        _radioButtons.Add(radioButton);
        if(_radioButtons.Count == SelectedIndex + 1){
            radioButton.IsSelected = true;
        }
        radioButton.IsEnabled = IsEnabled;
        radioButton.Size = Size;
        radioButton.SelectedChanged += RadioButtonSelectionChanged;
    }


    /// <summary>
    /// This method is called when a radio button is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadioButtonSelectionChanged(object sender, EventArgs e)
    {
        if (sender is not TMRadioButton selectedRadioButton || !selectedRadioButton.IsSelected)
        {
            return;
        }

        foreach (var radioButton in _radioButtons)
        {
            if (radioButton != selectedRadioButton)
            {
                radioButton.IsSelected = false;
            }
        }

        int index = FindIndex(selectedRadioButton);
        SelectedIndex = index;
        object value = selectedRadioButton.Value;
        var eventArgs = new TMRadioButtonEventArgs(value, index);
        SelectedRadioButtonChangedCommand?.Execute(eventArgs);
        SelectedRadioButtonChanged?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Finds the index of the selected radio button in the group
    /// </summary>
    /// <param name="radioButton">Selected Radio button</param>
    /// <returns>integer index</returns>
    private int FindIndex(TMRadioButton radioButton)
    {
        int index = 0;

        return FindIndexRecursive(Children, radioButton, index);
    }

    /// <summary>
    /// Finds the index of the selected radio button in the group
    /// </summary>
    /// <param name="children">List of children's in <see cref="TMRadioButtonGroup"/></param>
    private int FindIndexRecursive(IList<IView> children, TMRadioButton radioButton, int index)
    {
        foreach (var child in children)
        {
            if (child is TMRadioButton)
            {
                if (child == radioButton)
                {
                    return index;
                }
                index++;
            }
            else if (child is Layout childLayout)
            {
                int childIndex = FindIndexRecursive(childLayout.Children, radioButton, index);
                if (childIndex != -1)
                {
                    return childIndex;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Loops through all the children of the radio button group
    /// and sets the IsSelected property of the radio buttons in the group
    /// </summary>
    /// <param name="children">List of children's in <see cref="TMRadioButtonGroup"/> </param>
    /// <param name="index">Index of the selected <see cref="TMRadioButton"/></param>
    private void LoopChildren(IList<IView> children, int index)
    {
        foreach (var child in children)
        {
            if (child is TMRadioButton radioButton)
            {
                radioButton.IsSelected = index == SelectedIndex;
                index++;
            }
            else if (child is Layout childLayout)
            {
                LoopChildren(childLayout.Children, index);
            }
        }
    }

    #endregion
    public void Dispose()
    {
        foreach (var radioButton in _radioButtons)
        {
            radioButton.SelectedChanged -= RadioButtonSelectionChanged;
        }
    }
}
