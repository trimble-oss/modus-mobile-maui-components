// ﻿-------------------------------------------------------------------------
// File name : TopNavbarMainPageViewModel.cs
//
// Purpose : ContentPage class for TopNavbarMainPageViewModel
//
// Author : Balasubramanian R
//
// Created on : 21/01/2024
//
// Copyright (c) 2024 Trimble Inc. All rights reserved.
// -------------------------------------------------------------------------
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoApp.ViewModels
{
    public partial class TopNavbarMainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        bool _allowSearch;

        [ObservableProperty]
        bool _showRightContent;

        [ObservableProperty]
        bool _showLeftContent;

        [ObservableProperty]
        ContentView _rightContentView;

        [ObservableProperty]
        ContentView _leftContentView;

        partial void OnShowRightContentChanged(bool oldValue, bool newValue)
        {
            if(newValue)
            {
                var newContentView = new ContentView();
                var newImage = new Image() { Source = "account_icon.png" };
                newContentView.Content = newImage;
                RightContentView = newContentView;
            }
            else
            {
                RightContentView = null;
            }
        }
        partial void OnShowLeftContentChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                var newContentView = new ContentView();
                var newImage = new Image() { Source = "contact_icon.png" };
                newContentView.Content = newImage;
                LeftContentView = newContentView;
            }
            else
            {
                LeftContentView = null;
            }
        }
    }
}

