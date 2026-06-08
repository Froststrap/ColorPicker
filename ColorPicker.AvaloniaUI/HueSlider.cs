using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using ColorPicker.Models;

namespace ColorPicker;

[TemplatePart(Name = "PART_Handle", Type = typeof(Control))]
internal class HueSlider : TemplatedControl
{
    public static readonly StyledProperty<double> SmallChangeProperty = AvaloniaProperty.Register<HueSlider, double>(
        nameof(SmallChange), 1);

    public static readonly StyledProperty<double> ValueProperty = AvaloniaProperty.Register<HueSlider, double>(
        nameof(Value));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var handlerPart = e.NameScope.Find<Control>("PART_Handle");

        if (handlerPart != null)
        {
            handlerPart.AddHandler(PointerPressedEvent, OnMouseDown, RoutingStrategies.Tunnel);
            handlerPart.AddHandler(PointerReleasedEvent, OnMouseUp, RoutingStrategies.Tunnel);
            handlerPart.AddHandler(PointerMovedEvent, OnMouseMove, RoutingStrategies.Tunnel);
            handlerPart.AddHandler(PointerWheelChangedEvent, OnPreviewMouseWheel, RoutingStrategies.Tunnel);
        }
    }

    public double SmallChange
    {
        get => GetValue(SmallChangeProperty);
        set => SetValue(SmallChangeProperty, value);
    }

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private void OnMouseDown(object sender, PointerPressedEventArgs e)
    {
        e.Pointer.Capture(this);
        UpdateValue(e.GetPosition(this));
        e.Handled = true;
    }

    private void OnMouseMove(object sender, PointerEventArgs e)
    {
        if (Equals(e.Pointer.Captured, this))
        {
            UpdateValue(e.GetPosition(this));
            e.Handled = true;
        }
    }

    private void OnMouseUp(object sender, PointerReleasedEventArgs e)
    {
        e.Pointer.Capture(null);
    }

    private void UpdateValue(Point mousePos)
    {
        double ratio = Math.Clamp(mousePos.Y / Bounds.Height, 0, 1);
        Value = ratio * 360;
    }

    private void OnPreviewMouseWheel(object sender, PointerWheelEventArgs args)
    {
        Value = MathHelper.Mod(Value - (SmallChange * args.Delta.Y), 360);
        args.Handled = true;
    }
}