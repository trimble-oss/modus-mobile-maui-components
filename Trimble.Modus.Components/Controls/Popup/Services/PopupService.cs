﻿using Trimble.Modus.Components.Popup.Interfaces;

namespace Trimble.Modus.Components.Popup.Services;

public static class PopupService
{
    static IPopupNavigation? _customNavigation;
    static readonly Lazy<IPopupNavigation> implementation = new(() => CreatePopupNavigation(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Gets if the plugin is supported on the current platform.
    /// </summary>
    public static bool IsSupported => implementation.Value != null;

    /// <summary>
    /// Current plugin implementation to use
    /// </summary>
    public static IPopupNavigation Instance
    {
        get
        {
            IPopupNavigation lazyEvalPopupNavigation = _customNavigation ?? implementation.Value;

            if (lazyEvalPopupNavigation == null)
            {
                throw NotImplementedInReferenceAssembly();
            }

            return lazyEvalPopupNavigation;
        }
    }

    public static void SetInstance(IPopupNavigation instance)
    {
        _customNavigation = instance;
    }

    public static void RestoreDefaultInstance()
    {
        _customNavigation = null;
    }

    static IPopupNavigation CreatePopupNavigation()
    {
        return new PopupNavigation();
    }

    internal static Exception NotImplementedInReferenceAssembly() =>
        new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
}


