using DataLayer.Enums;
using System.Security.Claims;

namespace CourseTrack.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string? GetEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string? GetFullName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            var claim = principal.FindFirst("FullName");
            return claim?.Value;
        }

        public static string? GetRole(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            var claim = principal.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }
    }
}
