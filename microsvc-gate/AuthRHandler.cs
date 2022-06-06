using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using microsvc_gate.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

public class AuthRHandler: DelegatingHandler
{
    //private readonly IHttpContextAccessor _httpCtx;
    private readonly string _tokenSecret;
    private readonly ClaimsPrincipal _user;
    private readonly IMemoryCache _tokenCache;
    private readonly ILogger _logger;
    private readonly IUserProfileClient _userProfClient;

    public AuthRHandler(IUserProfileClient userProfileClient,
                        IHttpContextAccessor httpCxtAcc,
                        ILogger<AuthRHandler> logger,
                        IMemoryCache memoryCache,
                        IConfiguration config)
    {
        //_httpCtx = httpCxt;
        _tokenSecret = config["Jwt:TwoGuidSecret"];
        _logger = logger;
        _userProfClient = userProfileClient;
        _user = httpCxtAcc.HttpContext.User;
        _tokenCache = memoryCache;

    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var edipi = _user.Claims
                 .Where(c => c.Type == "edipi")              // From AuthN
                 .Select(x => x.Value).FirstOrDefault();

        string accessToken = null;

        IEnumerable<string> headerValues;
        if (request.Headers.TryGetValues("Authorization", out headerValues))
        {
            if (!_tokenCache.TryGetValue(edipi, out accessToken))
            {
                _logger.LogInformation($"Token for {edipi} not in Cache getting from Svc");
                accessToken = await _userProfClient.GetAuthRToken();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(240)); // 4 Minutes

                // Set object in cache
                _tokenCache.Set(edipi, accessToken, cacheOptions);
            }
            else
            {
                _logger.LogInformation($"Got Token for {edipi} From Cache");
            }

            


            request.Headers.Remove("Authorization");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private string GenerateToken(ClaimsPrincipal user)
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

        return encodedJwt;
    }

    private string AuthNTokenForUserMeta(ClaimsPrincipal user)
    {
        // We Only Need a Token with the Edipi 
        //string sskSecret = "{7445AF03-2612-4C82-A918-47215729FF9B}{41AAEA73-5C8C-4ADF-9A83-E6731FA00BF5}";
        string sskSecret = _tokenSecret;
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskSecret));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expirationDate = DateTime.UtcNow.AddDays(2);


        var claims = new List<Claim> {
                new Claim("AuthR", "Good"),
            };

        var jwt = new JwtSecurityToken(issuer: "NDDS",
                                       audience: "NDDS",
                                       claims: claims,
                                       expires: expirationDate,
                                       signingCredentials: signingCredentials);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
