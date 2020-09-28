namespace Resolver
{
    public class AccessTokenResolver : IAccessTokenResolver
    {
        // usually implemented as _httpContextAccessor.HttpContext.Headers["Authorization"].Replace("Bearer ", "");
        public string AccessToken => "some token";
    }
}
