﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components.Constant;
using Trimble.Modus.Components.Helpers;

namespace Trimble.Modus.Components;

    public class SelectableItem : BindableObject
{
    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(object), typeof(SelectableItem), null);

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SelectableItem), false);

    public SelectableItem(object data)
    {
        Data = data;
    }

    public Color SetterColor { get; set; }

    public SelectableItem(object data, bool isSelected)
    {
        Data = data;
        IsSelected = isSelected;
        SetterColor = ResourcesDictionary.ColorsDictionary(ColorsConstants.BluePale);
    }

    public object Data
    {
        get { return GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }
}

public class SelectableItem<T> : SelectableItem
{
    public SelectableItem(T data)
        : base(data)
    {
    }

    public SelectableItem(T data, bool isSelected)
        : base(data, isSelected)
    {
    }

    public new T Data
    {
        get { return (T)base.Data; }
        set { base.Data = value; }
    }
}

