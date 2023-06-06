namespace Trimble.Modus.Components.Helpers
{
    public class AppDataHelper
    {
        /// <summary>
        /// Convert string to respective Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
