using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;

namespace Bookify.web.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private string _policyName;
        public HangfireAuthorizationFilter(string policyName)
        {
            _policyName = policyName;
        }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext=context.GetHttpContext();
            var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var isAuthorized = authService.AuthorizeAsync(httpContext.User,_policyName)
                .ConfigureAwait(true).GetAwaiter().GetResult().Succeeded;
            return isAuthorized;
        }
    }
}
