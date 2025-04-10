using System.IdentityModel.Tokens.Jwt;

namespace Karshare.API.Helpers
{
    public static class JwtDecoder
    {
        public static string RemoveBearer(string jwt)
        {
            return jwt.Split(' ')[1];
        }

        public static string GetEmail(string jwt)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(RemoveBearer(jwt)).Subject;
        }
    }
}
