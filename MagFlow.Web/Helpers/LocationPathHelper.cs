namespace MagFlow.Web.Helpers
{
    public static class LocationPathHelper
    {
        public static string GetModule(string moduleName)
        {
            switch(moduleName)
            {
                case "item": return "ware";
                case "product": return "ware";
                default: return moduleName;
            }
        }
    }
}
