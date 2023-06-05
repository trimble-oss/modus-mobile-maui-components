namespace Trimble.Modus.Components.Helpers
{
    public class AppDataHelper
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
