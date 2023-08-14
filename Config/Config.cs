using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAddCharacter.Config
{
    public class Config : BaseConfig
    {
        public Config(string SavePath, string CategoryName) : base(SavePath, CategoryName)
        {
            Add("enable", true);
        }
    }
}
