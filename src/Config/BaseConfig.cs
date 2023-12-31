﻿using MelonLoader;
using System.Collections.Generic;

namespace Bloodlines
{
    public class BaseConfig
    {
        readonly string SavePath;
        readonly MelonPreferences_Category Category;
        readonly Dictionary<string, object> entries = new();
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
