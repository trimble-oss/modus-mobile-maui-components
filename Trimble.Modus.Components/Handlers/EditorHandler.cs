using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
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

#if ANDROID
                editorHandler.PlatformView.SetPadding(0, 0, 0, 0);
                editorHandler.PlatformView.Background = null;
                editorHandler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif WINDOWS
                editorHandler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
                editorHandler.PlatformView.BorderBrush = null;
                editorHandler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        }
    }
}
