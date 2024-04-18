
using DataAccess.Models;
using DataAccess.ViewModel;
using System.IdentityModel.Tokens.Jwt;

namespace BusinessLogic.Interface
{
    public interface IJwtService
    {
        public string GenerateToken(LoginModel user);
        public bool ValidateToken(string token, out JwtSecurityToken jWTSecurityToken);


    }
}
