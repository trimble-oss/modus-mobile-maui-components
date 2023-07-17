using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trimble.Modus.Components;

namespace DemoApp.Views;

public class PersonDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate TextCellTemplate { get; set; }
    public DataTemplate ViewCellTemplate { get; set; }

    public DataTemplate SelectedTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return SelectedTemplate;
    }
}
