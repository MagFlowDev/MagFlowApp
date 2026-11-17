namespace MagFlow.Web.Helpers
{
    public static class LocationPathHelper
    {
        public static string GetModule(string moduleName)
        {
            switch(moduleName)
            {
                case "item": return "wares";
                case "product": return "wares";
                case "stocktake": return "warehouse";
                default: return moduleName;
            }
        }
    }
}
