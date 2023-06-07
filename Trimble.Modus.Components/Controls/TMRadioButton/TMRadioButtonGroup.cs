using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trimble.Modus.Components.Collection;
using Trimble.Modus.Components.Controls.Layouts;
using Trimble.Modus.Components.Enums;

namespace Trimble.Modus.Components;

public class TMRadioButtonGroup : StackLayout, IDisposable
{
    #region Private fields
    private readonly Label _label = new Label() { IsVisible = false, FontSize = 14, Margin = new Thickness(0, 0, 0, 4), TextColor = (Color)BaseComponent.colorsDictionary()["TrimbleGray"]};
    private readonly WrapLayout _buttonContainer = new WrapLayout();    
    private readonly List<TMRadioButton> _radioButtons = new List<TMRadioButton>();
    #endregion

    #region Bindable Properties
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), 
        typeof(RadioButtonCollection<string>), 
        typeof(TMRadioButtonGroup), 
        new RadioButtonCollection<string>(),
        propertyChanged: OnRadioItemsSourceChanged);

    public static new readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(TMRadioButtonGroup),
        StackOrientation.Vertical);

    public static readonly BindableProperty GroupTitleProperty = BindableProperty.Create(
        nameof(GroupTitle),
        typeof(string),
        typeof(TMRadioButtonGroup),
        null,
        propertyChanged: OnGroupTitleChanged);

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
    /// Observable collection of <see cref="TMRadioButton"/> in this Group
    /// </summary>
    public RadioButtonCollection<string> ItemsSource
    {
        get { return (RadioButtonCollection<string>)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// IsEnabled property for the group
    /// </summary>
    public new bool IsEnabled
    {
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    /// <summary>
    /// Orientation of the container
    /// </summary>
    public new StackOrientation Orientation { 
        get { return (StackOrientation)GetValue (OrientationProperty); }
        set { SetValue (OrientationProperty, value); } 
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

    /// <summary>
    /// Gets or sets the title of the group
    /// </summary>
    public string GroupTitle { 
        get => (string)GetValue(GroupTitleProperty); 
        set => SetValue(GroupTitleProperty, value);
    }

    #endregion

    #region Constructor
    public TMRadioButtonGroup()
    {
        this.Children.Add(_label);
        this.Children.Add(_buttonContainer);
        ItemsSource.OnAdded += OnItemsAdded;
        ItemsSource.OnRemoved += OnItemsRemoved;
        ItemsSource.OnCleared += OnItemsCleared;
        _buttonContainer.SetBinding(WrapLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));
    }
    #endregion

    #region private methods

    /// <summary>
    /// Disable radio buttons in the group when the <see cref="IsEnabled"/> property changes
    /// </summary>
    private static void OnIsEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMRadioButtonGroup radioButtonGroup = (TMRadioButtonGroup)bindable;
        if (radioButtonGroup._radioButtons.Count > 1)
        {
            foreach (TMRadioButton radioButton in radioButtonGroup._radioButtons)
            {
                radioButton.IsEnabled = (bool)newValue;
            }
        }
        radioButtonGroup._label.Opacity = (bool)newValue ? 1 : 0.5;
    }

    /// <summary>
    /// Update the title of the group when the <see cref="GroupTitle"/> property changes
    /// </summary>
    private static void OnGroupTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMRadioButtonGroup radioGroup = (TMRadioButtonGroup)bindable;
        string text = (string)newValue;
        radioGroup._label.Text = text;
        radioGroup._label.IsVisible = !String.IsNullOrEmpty(text);
    }

    /// <summary>
    /// Update the size of the radio buttons when the <see cref="Size"/> property changes
    /// </summary>
    private static void OnSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TMRadioButtonGroup group = (TMRadioButtonGroup)bindable;
        if (group._radioButtons.Count > 1)
        {
            foreach (TMRadioButton button in group._radioButtons)
            {
                button.Size = (CheckboxSize)newValue;
            }
        }
        group._label.FontSize = (CheckboxSize)newValue == CheckboxSize.Default ? 14 : 16;
    }

    /// <summary>
    /// Updates the ItemsSource
    /// </summary>
    private static void OnRadioItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {        
        TMRadioButtonGroup group = (TMRadioButtonGroup)bindable;
        if (group == null)
        {
            return;
        }

        group.PopulateItems();
    }

    /// <summary>
    /// Populate radiobuttons in items source in the group
    /// </summary>
    private void PopulateItems()
    {
        if (ItemsSource == null) return;
        //RadioItemsSource.Clear();
        foreach (var item in ItemsSource)
        {
            var radioButton = new TMRadioButton()
            {
                Text = item
            };
            Children.Add(radioButton);
        }
        ItemsSource.OnAdded += OnItemsAdded;
        ItemsSource.OnRemoved += OnItemsRemoved;
        ItemsSource.OnCleared += OnItemsCleared;
    }
    /// <summary>
    /// Sets the <see cref="TMRadioButton.IsSelected"/> property of the radio buttons in the group
    /// </summary>
    private void UpdateSelectedStates()
    {
        int index = 0;
        LoopChildren(_buttonContainer.Children, index);
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
            this.Children.Remove(radioButton);
            base.OnChildAdded(child);
            AddRadioButtons(radioButton);
        }
        else if (this.Children.Count > 2 && child is not TMRadioButton)
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
        _buttonContainer.Children.Add(radioButton);
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

        return FindIndexRecursive(_buttonContainer.Children, radioButton, index);
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

    /// <summary>
    /// Add a new radio button to the group for each item in the list
    /// </summary>
    private void OnItemsAdded(object sender, ItemsChangedEventArgs<string> e)
    {
        var index = e.Index;
        var item = this.ItemsSource[index];
        this.Children.Add(new TMRadioButton() { Text = item });
    }

    /// <summary>
    /// Removes the radio button from the group
    /// </summary>
    private void OnItemsRemoved(object sender, ItemsChangedEventArgs<string> e)
    {
        _radioButtons.RemoveAt(e.Index);
        _buttonContainer.Children.RemoveAt(e.Index);
    }

    /// <summary>
    /// Clears the radio button group
    /// </summary>
    private void OnItemsCleared(object sender, EventArgs e)
    {
        _radioButtons.Clear();
        _buttonContainer.Children.Clear();
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
