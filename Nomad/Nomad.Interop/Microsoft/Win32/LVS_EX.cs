﻿namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum LVS_EX : uint
    {
        LVS_EX_AUTOAUTOARRANGE = 0x1000000,
        LVS_EX_AUTOCHECKSELECT = 0x8000000,
        LVS_EX_AUTOSIZECOLUMNS = 0x10000000,
        LVS_EX_BORDERSELECT = 0x8000,
        LVS_EX_CHECKBOXES = 4,
        LVS_EX_COLUMNOVERFLOW = 0x80000000,
        LVS_EX_COLUMNSNAPPOINTS = 0x40000000,
        LVS_EX_DOUBLEBUFFER = 0x10000,
        LVS_EX_FLATSB = 0x100,
        LVS_EX_FULLROWSELECT = 0x20,
        LVS_EX_GRIDLINES = 1,
        LVS_EX_HEADERDRAGDROP = 0x10,
        LVS_EX_HEADERINALLVIEWS = 0x2000000,
        LVS_EX_HIDELABELS = 0x20000,
        LVS_EX_INFOTIP = 0x400,
        LVS_EX_JUSTIFYCOLUMNS = 0x200000,
        LVS_EX_LABELTIP = 0x4000,
        LVS_EX_MULTIWORKAREAS = 0x2000,
        LVS_EX_ONECLICKACTIVATE = 0x40,
        LVS_EX_REGIONAL = 0x200,
        LVS_EX_SIMPLESELECT = 0x100000,
        LVS_EX_SINGLEROW = 0x40000,
        LVS_EX_SNAPTOGRID = 0x80000,
        LVS_EX_SUBITEMIMAGES = 2,
        LVS_EX_TRACKSELECT = 8,
        LVS_EX_TRANSPARENTBKGND = 0x400000,
        LVS_EX_TRANSPARENTSHADOWTEXT = 0x800000,
        LVS_EX_TWOCLICKACTIVATE = 0x80,
        LVS_EX_UNDERLINECOLD = 0x1000,
        LVS_EX_UNDERLINEHOT = 0x800
    }
}

