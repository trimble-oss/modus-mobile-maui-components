using System.Collections.Specialized;
using System.Windows.Input;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Controls.Layouts;
using Trimble.Modus.Components.Enums;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public class TMRadioButtonGroup : StackLayout, IDisposable
{
    #region Private fields
    private readonly Label _label = new Label() { IsVisible = false, FontSize = 14, Margin = new Thickness(0, 0, 0, 4), TextColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.TrimbleGray) };
    private readonly WrapLayout _buttonContainer = new WrapLayout();
    private readonly List<TMRadioButton> _radioButtons = new List<TMRadioButton>();
    #endregion

    #region Bindable Properties
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<string>), typeof(TMRadioButtonGroup), null, propertyChanged: OnRadioItemsSourceChanged);

    public static new readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(TMRadioButtonGroup), StackOrientation.Vertical);

    public static readonly BindableProperty GroupTitleProperty = BindableProperty.Create(nameof(GroupTitle), typeof(string), typeof(TMRadioButtonGroup), null, propertyChanged: OnGroupTitleChanged);

    public static new readonly BindableProperty IsEnabledProperty = BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(TMRadioButtonGroup), true, propertyChanged: OnIsEnabledChanged);

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(CheckboxSize), typeof(TMRadioButtonGroup), CheckboxSize.Default, propertyChanged: OnSizeChanged);

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TMRadioButtonGroup), -1, BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

    public static readonly BindableProperty SelectedRadioButtonChangedCommandProperty = BindableProperty.Create(nameof(SelectedRadioButtonChangedCommand), typeof(ICommand), typeof(TMRadioButtonGroup));

    #endregion

    #region Public Properties & events

    /// <summary>
    /// Observable collection of <see cref="TMRadioButton"/> in this Group
    /// </summary>
    public IEnumerable<string> ItemsSource
    {
        get { return (IEnumerable<string>)GetValue(ItemsSourceProperty); }
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
    public new StackOrientation Orientation
    {
        get { return (StackOrientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
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
    public string GroupTitle
    {
        get => (string)GetValue(GroupTitleProperty);
        set => SetValue(GroupTitleProperty, value);
    }

    #endregion

    #region Constructor
    public TMRadioButtonGroup()
    {
        this.Children.Add(_label);
        this.Children.Add(_buttonContainer);
        _buttonContainer.SetBinding(WrapLayout.OrientationProperty, new Binding(nameof(Orientation), source: this));
    }

    /// <summary>
    /// Called when ItemsSource is changed
    /// </summary>
    private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        // The collection has items added to it.
        if (e.Action == NotifyCollectionChangedAction.Add)
            HandleAdd(e);

        // The collection has items removed from it.
        else if (e.Action == NotifyCollectionChangedAction.Remove)
            HandleRemove(e);

        // The collection has been reset.
        else if (e.Action == NotifyCollectionChangedAction.Reset)
            HandleReset(e);

        // The collection has items replaced in it.
        else if (e.Action == NotifyCollectionChangedAction.Replace)
            HandleReplace(e);

        // The collection has items moved within it.
        else if (e.Action == NotifyCollectionChangedAction.Move)
            HandleMove(e);
    }

    /// <summary>
    /// Handle the add event when items are added to the collection
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleAdd(NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.NewItems)
        {
            if (item is string radioButtonString && !string.IsNullOrEmpty(radioButtonString))
            {
                // Create a new radio button from the string
                var radioButton = new TMRadioButton() { Text = radioButtonString, CreatedFromItemSource = true };

                // Add the radio button to the UI
                this.Children.Add(radioButton);
            }
            else
            {
                Console.WriteLine(Constants.RadioButtonEmptyTextError);
            }
        }
    }

    /// <summary>
    /// Handle the remove event when items are removed from the collection
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleRemove(NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.OldItems)
        {
            if (item is string radioButtonString)
            {
                // Remove the radio button from the list of radio buttons.
                var index = _radioButtons.FindIndex(x => x.Text == radioButtonString);
                _radioButtons.RemoveAt(index);

                // Remove the radio button from the UI.
                _buttonContainer.Children.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// Handle the reset event when the collection is reset
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleReset(NotifyCollectionChangedEventArgs e)
    {
        foreach (var radioButton in _radioButtons.ToList())
        {
            if (radioButton.CreatedFromItemSource)
            {
                _radioButtons.Remove(radioButton);
                _buttonContainer.Children.Remove(radioButton);
            }
        }
    }

    /// <summary>
    /// Handle the replace event when items are replaced in the collection
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleReplace(NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.NewItems)
        {
            if (item is string radioButtonString)
            {
                var index = _radioButtons.FindIndex(x => x.Text == radioButtonString);
                _radioButtons[index].Text = radioButtonString;
            }
        }
    }

    /// <summary>
    /// Handle the move event when items are moved within the collection
    /// </summary>
    /// <param name="e">The event arguments</param>
    private void HandleMove(NotifyCollectionChangedEventArgs e)
    {
        var oldItem = ItemsSource.ElementAt(e.OldStartingIndex);
        var newItem = ItemsSource.ElementAt(e.NewStartingIndex);
        var newIndex = _radioButtons.FindIndex(x => x.Text == newItem);
        var oldIndex = _radioButtons.FindIndex(x => x.Text == oldItem);

        SwapRadioButtons(oldIndex, newIndex);
        SwapChildren(oldIndex, newIndex);
    }

    private void SwapRadioButtons(int oldIndex, int newIndex)
    {
        var buttonTemp = _radioButtons[oldIndex];
        _radioButtons[oldIndex] = _radioButtons[newIndex];
        _radioButtons[newIndex] = buttonTemp;
    }

    private void SwapChildren(int oldIndex, int newIndex)
    {
        // get the two views to swap
        IView oldView = _buttonContainer.Children[oldIndex];
        IView newView = _buttonContainer.Children[newIndex];

        IView[] childrenArray = new IView[_buttonContainer.Children.Count];
        _buttonContainer.Children.CopyTo(childrenArray, 0);

        // Remove all elements from the container
        _buttonContainer.Children.Clear();

        // Swap the two elements
        childrenArray[oldIndex] = newView;
        childrenArray[newIndex] = oldView;

        // Add back all elements to the container
        foreach (var child in childrenArray)
        {
            _buttonContainer.Children.Add(child);
        }
    }
    #endregion

    #region private methods

    /// <summary>
    /// Update the selection state of the radio buttons in the group
    /// </summary>
    private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((TMRadioButtonGroup)bindable).UpdateSelectedStates();
        ((TMRadioButtonGroup)bindable).SetDefaultSelectedRadioButton();
    }

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
        if (oldValue is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= group.OnItemsSourceCollectionChanged;
        }
        if (newValue is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += group.OnItemsSourceCollectionChanged;
        }
        group.PopulateItems();
    }

    /// <summary>
    /// Populate radiobuttons in items source in the group
    /// </summary>
    private void PopulateItems()
    {
        if (ItemsSource == null) return;
        _radioButtons.RemoveAll(x => x.CreatedFromItemSource);
        foreach (var child in _buttonContainer.Children.Where(x => x is TMRadioButton radioButton && radioButton.CreatedFromItemSource).ToList())
        {
            _buttonContainer.Children.Remove(child);
        }
        
        foreach (var item in ItemsSource)
        {
            if (string.IsNullOrEmpty(item.ToString()))
            {
                Console.WriteLine(Constants.RadioButtonEmptyTextError);
                continue;
            }
            var radioButton = new TMRadioButton()
            {
                Text = item,
                CreatedFromItemSource = true
            };
            Children.Add(radioButton);
        }
    }
    /// <summary>
    /// Sets the <see cref="TMRadioButton.IsSelected"/> property of the radio buttons in the group
    /// </summary>
    private void UpdateSelectedStates()
    {
        int index = 0;
        SelectChildrenAtIndex(_buttonContainer.Children, index);
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
        base.OnChildAdded(child);
        if (child is TMRadioButton radioButton)
        {
            this.Children.Remove(radioButton);
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
        if (_radioButtons.Count == SelectedIndex + 1)
        {
            radioButton.IsSelected = true;
        }
        radioButton.IsEnabled = IsEnabled;
        radioButton.Size = Size;
        radioButton.SelectionChanged += RadioButtonSelectionChanged;
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
        int index = _radioButtons.IndexOf(selectedRadioButton);
        SelectedIndex = index;
        object value = selectedRadioButton.Value;
        var eventArgs = new TMRadioButtonEventArgs(value, index);
        SelectedRadioButtonChangedCommand?.Execute(eventArgs);
        SelectedRadioButtonChanged?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Loops through all the children of the radio button group
    /// and sets the IsSelected property of the radio buttons in the group
    /// </summary>
    /// <param name="children">List of children's in <see cref="TMRadioButtonGroup"/> </param>
    /// <param name="index">Index of the selected <see cref="TMRadioButton"/></param>
    private void SelectChildrenAtIndex(IList<IView> children, int index)
    {
        foreach (var child in children)
        {
            if (child is TMRadioButton radioButton)
            {
                radioButton.IsSelected = index == SelectedIndex;
                index++;
            }
        }
    }

    #endregion
    public void Dispose()
    {
        foreach (var radioButton in _radioButtons)
        {
            radioButton.SelectionChanged -= RadioButtonSelectionChanged;
        }
    }
}
