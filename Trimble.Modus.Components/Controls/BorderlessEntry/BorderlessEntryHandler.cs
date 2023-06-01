#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

using Microsoft.Maui.Handlers;

namespace Trimble.Modus.Components
{
    internal partial class BorderlessEntryHandler : EntryHandler
    {
        public BorderlessEntryHandler()
        {
            Mapper.AppendToMapping("TMBorderlessEntryCustomization", MapTMEntry);
        }

        public void MapTMEntry(IEntryHandler entryHandler, IEntry entry)
        {
            if (entry is BorderlessEntry && entryHandler is BorderlessEntryHandler)
            {

#if IOS || MACCATALYST
                entryHandler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif ANDROID
                entryHandler.PlatformView.Background = null;
                entryHandler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif WINDOWS
            
                entryHandler.PlatformView.BorderBrush = null;
                entryHandler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        }
    }
}
