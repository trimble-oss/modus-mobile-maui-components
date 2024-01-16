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

        public TopNavbarMainPageViewModel()
        {
        }
    }
}

