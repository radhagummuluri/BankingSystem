using Hellang.Middleware.ProblemDetails;

namespace Microsoft.AspNetCore.Builder {
   public static class ErrorHandlingApplicationBuilderExtensions {
      public static void UseExceptionHandling(this IApplicationBuilder app) {
         //use Hellang.Middleware.ProblemDetails package for api requests
         app.UseWhen(ctx => ctx.IsApiRequest(), builder => builder.UseProblemDetails());
      }
   }
}
