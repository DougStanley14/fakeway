using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace microsvc_gate.Services
{
    public interface IUserProfileClient
    {
        Task<string> GetAuthRToken();
    }

    public class UserProfileClient : IUserProfileClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _tokenSecret;
        private readonly ILogger<UserProfileClient> _logger;
        private readonly ClaimsPrincipal _user;

        public UserProfileClient(HttpClient httpClient,
                                 IHttpContextAccessor httpCxtAcc,
                                 ILogger<UserProfileClient> logger,
                                 IConfiguration config)
        {
            _tokenSecret = config["Jwt:TwoGuidSecret"];
            _logger = logger;
            _user = httpCxtAcc.HttpContext.User;

            httpClient.BaseAddress = new Uri(config["UserProfilServiceBaseUrl"]);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {GenSimpleAuthTokenForUserProfile()}");
            _httpClient = httpClient;

            _logger.LogInformation($"UserProfile Service Url - '{httpClient.BaseAddress}'"); 
        }

        private string GenSimpleAuthTokenForUserProfile()
        {
            string sskSecret = _tokenSecret;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var expirationDate = DateTime.UtcNow.AddDays(2);

            var edipi = _user.Claims
                             .Where(c => c.Type == "edipi")              // From AuthN
                             .Select(x => x.Value).FirstOrDefault();

            var username = _user.Claims
                             .Where(c => c.Type == "preferred_username") // From AuthN
                             .Select(x => x.Value).FirstOrDefault();

            _logger.LogInformation($"Getting AuthR for Edipi - '{edipi}'");

            var claims = new List<Claim> {
                new Claim("edipi", edipi),
                new Claim("username", username ),
            };

            var jwt = new JwtSecurityToken(issuer: "NDDS",
                                           audience: "NDDS",
                                           claims: claims,
                                           expires: expirationDate,
                                           signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<string> GetAuthRToken()
        {
            // config["UserProfilServiceBaseUrl"]/AuthToken
            var result = await _httpClient.GetStringAsync("AuthToken");

            return result;

        }
    }
}
