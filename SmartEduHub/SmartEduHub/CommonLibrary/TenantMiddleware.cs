using SmartEduHub.Interface;

namespace SmartEduHub.CommonLibrary
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ITenantContextAccessor tenantContextAccessor)
        {
            if (context.Request.Headers.TryGetValue("X-College-Id", out var collegeId))
            {
                tenantContextAccessor.SetTenantId(collegeId!);
            }

            await _next(context);
        }
    }

}
