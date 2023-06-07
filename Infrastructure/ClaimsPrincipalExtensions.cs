using System.Security.Claims;

namespace CarStory.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role).Value;
        }
    }
}
