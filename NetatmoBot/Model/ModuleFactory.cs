using NetatmoBot.Model.Modules;

namespace NetatmoBot.Model
{
    public static class ModuleFactory
    {
        public static Module Create(string moduleType)
        {
            switch (moduleType)
            {
                case "NAMain":
                    return new MainModule();
                case "NAModule1":
                    return new OutdoorModule();
                case "NAModule3":
                    return new RainModule();
                case "NAModule4":
                    return new IndoorModule();
                default:
                    return new GenericModule();
            }
        }
    }
}