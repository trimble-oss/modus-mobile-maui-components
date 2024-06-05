namespace Trimble.Modus.Components.Helpers;

public static class ResourcesDictionary
{
    public static Color GetColor(string styleKey)
    {
        return Application.Current.Resources[styleKey] as Color;
    }
}
