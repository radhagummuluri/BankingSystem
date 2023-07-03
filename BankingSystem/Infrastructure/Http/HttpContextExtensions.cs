using VendorInbox.Web.Infrastructure.Http;

namespace Microsoft.AspNetCore.Http {
   public static class HttpContextExtensions {
      public static bool IsApiRequest(this HttpContext httpContext) =>
         httpContext.Request.Path.StartsWithSegments(HttpConstants.API_ENDPOINT, StringComparison.OrdinalIgnoreCase);
   }
}
