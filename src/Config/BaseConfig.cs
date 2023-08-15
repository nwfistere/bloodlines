using MelonLoader;

namespace Bloodlines.Config
{
    public class BaseConfig
    {
        private readonly string SavePath;
        private readonly MelonPreferences_Category Category;
        private readonly Dictionary<string, object> entries = new();
        public BaseConfig(string SavePath, string CategoryName)
        {
            this.SavePath = SavePath;
            Category = MelonPreferences.CreateCategory(CategoryName);
            Category.SetFilePath(SavePath);
        }
        protected void Add<T>(string key, T defaultValue)
        {
            entries[key] = Category.CreateEntry(key, defaultValue);
        }

        public T GetValue<T>(string key)
        {
            MelonPreferences_Entry<T> entry = entries[key] as MelonPreferences_Entry<T>;
            return entry.Value;
        }

        public void SetValue<T>(string key, T value)
        {
            (entries[key] as MelonPreferences_Entry<T>).Value = value;
        }
    }
}
