using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

public partial class TextCell : ViewCell
{
    #region Private Fields
    private ListView previousParent = null;
    #endregion
    #region Bindable Properties
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(TextCell), default(string));

    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(TextCell));

    public static readonly BindableProperty RightIconSourceProperty =
       BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(TextCell));

    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(TextCell), default(string));


    #endregion
    #region Public Fields
    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }

    public ImageSource RightIconSource
    {
        get => (ImageSource)GetValue(RightIconSourceProperty);
        set => SetValue(RightIconSourceProperty, value);
    }
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public Color setterColor;
    #endregion
    #region Constructor
    public TextCell()
    {
        InitializeComponent();
    }
    #endregion
    #region Protected Methods
    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (this.Parent != null)
        {
            previousParent = this.Parent as ListView;
            ((ListView)this.Parent).ItemTapped += TextCell_ItemSelected;
        }
        else
        {
            previousParent.ItemTapped -= TextCell_ItemSelected;
            previousParent = null;
        }
    }
    #endregion
    #region Private Methods
    private void TextCell_ItemSelected(object sender, ItemTappedEventArgs e)
    {
        if (sender is TMListView tMListView)
        {
            grid.BackgroundColor = Colors.White;
            foreach (var item in tMListView.selectableItems)
            {
                if (item == BindingContext)
                {
                    grid.BackgroundColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
                }
            }
        }
    }
    #endregion
}
