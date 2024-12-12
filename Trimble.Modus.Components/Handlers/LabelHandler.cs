using Microsoft.Maui.Handlers;

namespace Trimble.Modus.Components.Handlers;

internal partial class LabelHandler : Microsoft.Maui.Handlers.LabelHandler
{
    public LabelHandler()
    {
#if ANDROID
        Mapper.AppendToMapping(nameof(Label.LineBreakMode), UpdateMaxLines);
        Mapper.AppendToMapping(nameof(Label.MaxLines), UpdateMaxLines);
#endif
    }

    private static void UpdateMaxLines(ILabelHandler handler, ILabel label)
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

