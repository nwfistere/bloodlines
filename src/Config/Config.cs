namespace Bloodlines
{
    public class Config : BaseConfig
    {
        public Config(string SavePath, string CategoryName) : base(SavePath, CategoryName)
        {
            Add("enable", true);
        }
    }
}
