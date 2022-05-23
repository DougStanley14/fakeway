using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace microsvc_filemeta.AuthPolicies
{

    public class FileMetaAuthorizationHandler : 
        AuthorizationHandler<MeetsPlatformRequirement, NDFileMeta>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MeetsPlatformRequirement requirement,
            NDFileMeta fileMeta)
        {
            var claims = context.User.Claims.ToList();

            if (UserCanAddMetaData(claims, fileMeta))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool UserCanAddMetaData(List<Claim> claims, NDFileMeta fileMeta)
        {
            // Get All User Allowed Platforms from Claims
            var allplats = claims.Where(c => c.Type == "Platform")
                                 .Select(c => new {
                                     SecGrp = c.Value.ToString().Split(':').First(),
                                     Platform = c.Value.ToString().Split(':').Last()
                                 }).ToList();

            // Get Allow Platforms from Security Group Set
            var allowedPlats = allplats.Where(c => c.SecGrp == fileMeta.UserSecGroup)
                                       .Select( c => c.Platform)
                                       .ToList();


            var canAdd = fileMeta.Platform.All(p => allowedPlats.Contains(p));

            return canAdd;                         
        }
    }

    public class MeetsPlatformRequirement : IAuthorizationRequirement
    {

    }

}
