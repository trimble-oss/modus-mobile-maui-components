using System.Collections;

namespace Trimble.Modus.Components.Controls;

public partial class TMListView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TMListView), null);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TMListView), null, propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty ItemSelectedCommandProperty =
            BindableProperty.Create(nameof(ItemSelectedCommand), typeof(Command), typeof(TMListView));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public Command ItemSelectedCommand
        {
            get { return (Command)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public event EventHandler<object> ItemSelected;

        public TMListView()
        {
            BindingContext = this;

            var listView = new ListView
            {
                ItemsSource = ItemsSource,
                ItemTemplate = new DataTemplate(() =>
                {
                    var leftIcon = new Image();
                    var rightIcon = new Image();
                    var titleLabel = new Label();

                    // Customize the appearance of the leftIcon, rightIcon, and titleLabel as needed.

                    var layout = new Grid();
                    layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Left icon column
                    layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Title column
                    layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Right icon column

                    layout.Add(leftIcon, 0, 0);
                    layout.Add(titleLabel, 1, 0);
                    layout.Add(rightIcon, 2, 0);

                    var cell = new ViewCell { View = layout };
                    cell.Tapped += OnCellTapped;

                    return cell;
                })
            };

            Content = listView;
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customListView = (TMListView)bindable;
            customListView.ItemSelected?.Invoke(customListView, newValue);
            customListView.ItemSelectedCommand?.Execute(newValue);
        }

        private void OnCellTapped(object sender, EventArgs e)
        {
            var cell = (ViewCell)sender;
            var selectedItem = cell.BindingContext;
            SelectedItem = selectedItem;
        }
    }
