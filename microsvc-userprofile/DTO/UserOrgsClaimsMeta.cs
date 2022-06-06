using microsvc_userprofile.Model;

namespace microsvc_userprofile.DTO
{
    public class UserOrgsClaimsMeta
    {
        public string OrgName { get; set; }
        public OrgType OrgType { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Programs { get; set; }
        public List<int> Bunos { get; set; }
    }
}
