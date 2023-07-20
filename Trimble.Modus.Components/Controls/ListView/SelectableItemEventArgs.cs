using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trimble.Modus.Components;

public class SelectableItemEventArgs : EventArgs
{
    public object SelectableItem { get; }
    public int SelectableItemIndex { get; }

    public SelectableItemEventArgs(object selectableItem, int selectableitemindex)
    {
        SelectableItem = selectableItem;
        SelectableItemIndex = selectableitemindex;
    }
}
