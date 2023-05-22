﻿namespace Trimble.Modus.Components.Popup.Enums;

[Flags]
internal enum PaddingSide
{
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    All = Left | Top | Right | Bottom
}