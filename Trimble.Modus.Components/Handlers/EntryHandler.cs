#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

using Microsoft.Maui.Handlers;

namespace Trimble.Modus.Components.Handlers;

internal partial class EntryHandler : Microsoft.Maui.Handlers.EntryHandler
{
    public EntryHandler()
    {
        Mapper.AppendToMapping("TMBorderlessEntryCustomization", MapTMEntry);
    }

    public void MapTMEntry(IEntryHandler entryHandler, IEntry entry)
    {
        if (entry is BorderlessEntry && entryHandler is EntryHandler)
        {

#if IOS || MACCATALYST
            entryHandler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif ANDROID
            entryHandler.PlatformView.Background = null;
            entryHandler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
        
            entryHandler.PlatformView.BorderBrush = null;
            entryHandler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
            entryHandler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
#endif
        }
    }
}
