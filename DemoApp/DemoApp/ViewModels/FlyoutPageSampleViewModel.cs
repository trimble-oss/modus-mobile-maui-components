// ﻿-------------------------------------------------------------------------
// File name : TopNavbarSamplePageViewmodel.cs
//
// Purpose : ContentPage class for FlyoutPageSampleViewModel
//
// Author : Balasubramanian R
//
// Created on : 21/01/2024
//
// Copyright (c) 2024 Trimble Inc. All rights reserved.
// -------------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Trimble.Modus.Components;

namespace DemoApp.ViewModels
{
    public partial class FlyoutPageSampleViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<TMFlyoutMenuItem> flyoutMenuItems;
        public FlyoutPageSampleViewModel()
        {
            FlyoutMenuItems = new ObservableCollection<TMFlyoutMenuItem>() { new TMFlyoutMenuItem() { IconSource = "account_icon.png", TargetType = typeof(MainPage), Title = "Home" } };
        }
        
    }
}

