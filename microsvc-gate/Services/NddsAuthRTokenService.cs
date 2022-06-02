using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace microsvc_gate.Services
{
    public class NddsAuthRTokenService
    {
        public AuthToken GenerateToken(AuthUser user)
        {
            string sskSecret = "{7445AF03-2612-4C82-A918-47215729FF9B}{41AAEA73-5C8C-4ADF-9A83-E6731FA00BF5}";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var expirationDate = DateTime.UtcNow.AddDays(2);

            //var claims = await GetUserClaims(userName, edipi);

            var claims = new List<Claim> {
                new Claim("AuthR", "Good"),
            };

            var jwt = new JwtSecurityToken(issuer: "NDDS",
                                           audience: "NDDS",
                                           claims: claims,
                                           expires: expirationDate,
                                           signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var authToken = new AuthToken();
            authToken.Token = encodedJwt;
            authToken.ExpirationDate = expirationDate;

            return authToken;
        }
    }

    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class AuthUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public long EDIPI { get; set; }
    }
}
