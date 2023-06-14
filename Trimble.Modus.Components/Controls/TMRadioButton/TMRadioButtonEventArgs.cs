namespace Trimble.Modus.Components
{
    public class TMRadioButtonEventArgs : EventArgs
    {
        public Object Value { get; }
        public int RadioButtonIndex { get; }

        public TMRadioButtonEventArgs(Object value, int index)
        {
            Value = value;
            RadioButtonIndex = index;
        }
    }
}
