using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAddCharacter.Config
{
    public class BaseConfig
    {
        private readonly string SavePath;
        private MelonPreferences_Category Category;
        private Dictionary<string, object> entries = new Dictionary<string, object>();
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
