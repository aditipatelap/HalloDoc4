
using DataAccess.Models;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogic.Interface
{
    public interface IJwtService
    {
        public string GenerateToken(Aspnetuser user);
        public bool ValidateToken(string token, out JwtSecurityToken jWTSecurityToken);


    }
}
