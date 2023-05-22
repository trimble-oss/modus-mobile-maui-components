﻿
using System;
using Microsoft.Maui.Platform;
using Trimble.Modus.Components.Popup.Interfaces;
using Trimble.Modus.Components.Popup.Pages;
using Trimble.Modus.Components.Platforms.Windows;
using Trimble.Modus.Components.Popup.Services;

namespace Trimble.Modus.Components.Windows.Implementation
{
    internal class PopupPlatformWindows : IPopupPlatform
    {
        private IPopupNavigation PopupNavigationInstance => PopupService.Instance;

        //public event EventHandler OnInitialized
        //{
        //    add => Popup.OnInitialized += value;
        //    remove => Popup.OnInitialized -= value;
        //}

        //public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public PopupPlatformWindows()
        {
            //SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        public static bool SendBackPressed(Action? backPressedHandler = null)
        {
            var popupNavigationInstance = PopupService.Instance;

            if (popupNavigationInstance.PopupStack.Count > 0)
            {
                var lastPage = popupNavigationInstance.PopupStack[popupNavigationInstance.PopupStack.Count - 1];

                var isPreventClose = lastPage.SendBackButtonPressed();

                if (!isPreventClose)
                {
                    popupNavigationInstance.PopAsync();
                }

                return true;
            }

            backPressedHandler?.Invoke();

            return false;
        }

        public async Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current.MainPage;

            var popup = new global::Microsoft.UI.Xaml.Controls.Primitives.Popup();

            // Use TOPLATFORM to create your handlers
            // I'd recommend wiring up all your services through ConfigurePopups
            // builder.Services.AddScoped<IPopupPlatform, PopupPlatform>();
            // builder.Services.AddScoped<IPopupNavigation, PopupNavigation>();
            // Then you can use contructor resolution instead of singletons

            var renderer = (PopupPageRenderer)page.ToPlatform(Application.Current.MainPage.Handler.MauiContext);

            renderer.Prepare(popup);
            popup.Child = renderer;


            // https://github.com/microsoft/microsoft-ui-xaml/issues/3389
            popup.XamlRoot = 
                Application.Current.MainPage.Handler.MauiContext.Services.GetService<Microsoft.UI.Xaml.Window>().Content.XamlRoot;

            popup.IsOpen = true;
            page.ForceLayout();

            await Task.Delay(5);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new Exception("Popup page is null");

            var renderer = (PopupPageRenderer)page.ToPlatform(Application.Current.MainPage.Handler.MauiContext);
            var popup = renderer.Container;

            if (popup != null)
            {
                renderer.Destroy();

                Cleanup(page);
                page.Parent = null;
                popup.Child = null;
                popup.IsOpen = false;
            }

            await Task.Delay(5);
        }

        internal static void Cleanup(VisualElement element)
        {
            element.Handler?.DisconnectHandler();
        }
    }
}