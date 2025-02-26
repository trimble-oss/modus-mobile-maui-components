using CommunityToolkit.Maui.Behaviors;
using Trimble.Modus.Components.Mobile.InField.Constant;

namespace Trimble.Modus.Components.Mobile.InField;

public partial class TMBattery : ContentView
{
    public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(nameof(IconTintColor), typeof(Color), typeof(TMBattery), Colors.Black, propertyChanged: OnIconTintColorChanged);

    public static readonly BindableProperty BatteryDetailProperty = BindableProperty.Create(nameof(BatteryDetail), typeof(BatteryDetails), typeof(TMBattery), null, propertyChanged: OnBatteryDetailChanged);

    public static readonly BindableProperty BatteryPositionProperty = BindableProperty.Create(nameof(BatteryPosition), typeof(BatteryPosition), typeof(TMBattery), BatteryPosition.Horizontal, propertyChanged: OnBatteryPositionChanged);

    public Color IconTintColor
    {
        get => (Color)GetValue(IconTintColorProperty);
        set => SetValue(IconTintColorProperty, value);
    }

    public BatteryPosition BatteryPosition
    {
        get => (BatteryPosition)GetValue(BatteryPositionProperty);
        set => SetValue(BatteryPositionProperty, value);
    }

    public BatteryDetails? BatteryDetail
    {
        get => (BatteryDetails)GetValue(BatteryDetailProperty);
        set => SetValue(BatteryDetailProperty, value);
    }

    public TMBattery()
    {
        InitializeComponent();

        UpdateBatteryIcon(this);
        UpdateTintColor();
    }

    private static void OnIconTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMBattery batteryControl)
        {
            batteryControl.UpdateTintColor();
        }
    }

    private static void OnBatteryPositionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMBattery batteryControl)
        {
            batteryControl.UpdateBatteryIcon(batteryControl);
        }
    }

    private static void OnBatteryDetailChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TMBattery batteryControl)
        {
            batteryControl.UpdateBatteryIcon(batteryControl);
        }
    }

    private void UpdateBatteryIcon(TMBattery batteryControl)
    {
        if (batteryControl.BatteryDetail != null)
        {
            batteryGrid.IsVisible = true;
            batteryLabel.Text = batteryControl.BatteryDetail.Level + "%";
            if (batteryControl.BatteryPosition == BatteryPosition.Horizontal)
            {
                if (batteryControl.BatteryDetail.IsCharging)
                {
                    batteryImage.Source = ImageConstants.Battery_Horizontal_Charging;
                }
                else
                {
                    if (batteryControl.BatteryDetail.Level <= 10)
                    {
                        batteryImage.Source = ImageConstants.Battery_Horizontal_0;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 25)
                    {
                        batteryImage.Source = ImageConstants.Battery_Horizontal_25;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 50)
                    {
                        batteryImage.Source = ImageConstants.Battery_Horizontal_50;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 90)
                    {
                        batteryImage.Source = ImageConstants.Battery_Horizontal_75;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 100)
                    {
                        batteryImage.Source = ImageConstants.Battery_Horizontal_100;
                    }
                }
            }
            else
            {
                if (batteryControl.BatteryDetail.IsCharging)
                {
                    batteryImage.Source = ImageConstants.Battery_Vertical_Charging;
                }
                else
                {
                    if (batteryControl.BatteryDetail.Level <= 10)
                    {
                        batteryImage.Source = ImageConstants.Battery_Vertical_0;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 25)
                    {
                        batteryImage.Source = ImageConstants.Battery_Vertical_25;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 50)
                    {
                        batteryImage.Source = ImageConstants.Battery_Vertical_50;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 90)
                    {
                        batteryImage.Source = ImageConstants.Battery_Vertical_75;
                    }
                    else if (batteryControl.BatteryDetail.Level <= 100)
                    {
                        batteryImage.Source = ImageConstants.Battery_Vertical_100;

                    }
                }
            }
           
        }
        else
        {
            batteryGrid.IsVisible = false;
        }
    }

    private void UpdateTintColor()
    {
        var behavior = new IconTintColorBehavior
        {
            TintColor = IconTintColor
        };

        batteryImage.Behaviors.Add(behavior);

        batteryLabel.TextColor = IconTintColor;
    }
}
