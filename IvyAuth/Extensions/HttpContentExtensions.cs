using IvyAuth;

namespace IvyAuth.Extensions
{
    public static class HttpContextExtensions
    {
        public static String? GetApplicationId(this HttpContext context)
        {
            var appIdHeader = context.Request.Headers.Where(x => x.Key == "x-application-id");
            var appId = appIdHeader.FirstOrDefault().Value;

            if (!String.IsNullOrWhiteSpace(appId))
            {
                return appId;
            }
            return null;            
        }
                
        public static String? GetTimeZone(this HttpContext context)
        {
            var timeZoneHeader = context.Request.Headers.Where(x => x.Key == "x-timezone");
            return timeZoneHeader.FirstOrDefault().Value;     
        }
    }
}