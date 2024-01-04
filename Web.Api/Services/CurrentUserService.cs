using Application.Abstractions;

namespace Web.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public string? UserId => httpContext.HttpContext?.User?.Claims.FirstOrDefault(x=>x.Type == "uid")?.Value;
    }
}
