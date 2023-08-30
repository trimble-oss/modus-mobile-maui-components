namespace Trimble.Modus.Components.Handlers;

internal partial class LabelHandler : Microsoft.Maui.Handlers.LabelHandler
{
    public LabelHandler()
    {
        Label.ControlsLabelMapper.AppendToMapping(
          nameof(Label.LineBreakMode), UpdateMaxLines);

        Label.ControlsLabelMapper.AppendToMapping(
          nameof(Label.MaxLines), UpdateMaxLines);
    }
    static void UpdateMaxLines(Microsoft.Maui.Handlers.LabelHandler handler, ILabel label)
    {
        var textView = handler.PlatformView;
        if (label is Label controlsLabel)
        {

#if ANDROID
            
            if (textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End)
            {
            textView.SetMaxLines(controlsLabel.MaxLines);
            }
#elif IOS
            if (textView.LineBreakMode == UIKit.UILineBreakMode.TailTruncation)
            {
                textView.Lines = controlsLabel.MaxLines;
            }
#endif
        }
    }
}

