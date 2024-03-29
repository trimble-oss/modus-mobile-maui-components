﻿using Microsoft.Maui.Handlers;

namespace Trimble.Modus.Components.Platforms.iOS;

internal class PopupPageHandler : PageHandler
{
    public PopupPageHandler()
    {
        this.SetMauiContext(MauiUIApplicationDelegate.Current.Application.Windows[0].Handler.MauiContext); //Still a hack?
    }

    protected override Microsoft.Maui.Platform.ContentView CreatePlatformView()
    {
        return base.CreatePlatformView();
    }

    protected override void DisconnectHandler(Microsoft.Maui.Platform.ContentView nativeView)
    {
        base.DisconnectHandler(nativeView);
    }
}
