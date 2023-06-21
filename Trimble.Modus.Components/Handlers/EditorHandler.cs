using Microsoft.Maui.Handlers;
#if IOS || MACCATALYST
using UIKit;
#endif
#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif

namespace Trimble.Modus.Components.Handlers
{
    internal partial class EditorHandler : Microsoft.Maui.Handlers.EditorHandler
    {

        public EditorHandler()
        {
            Mapper.AppendToMapping("TMBorderlessEditorCustomization", MapTMEditor);
        }

        public static void MapTMEditor(IEditorHandler editorHandler, IEditor editor)
        {
            if (editorHandler is EditorHandler customEditorHandler && editor is BorderlessEditor)
            {

#if IOS || MACCATALYST
                editorHandler.PlatformView.Layer.BorderWidth = 0;
                editorHandler.PlatformView.Layer.BorderColor = UIColor.Clear.CGColor;
#elif ANDROID
                editorHandler.PlatformView.Background = null;
                editorHandler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif WINDOWS
        
            editorHandler.PlatformView.BorderBrush = null;
            editorHandler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        }
    }
}
