namespace microsvc_userprofile.DTO
{
    public class UserClamisMeta
    {
        public long EDIPI { get; set; }
        public string UserName { get; set; }
        public List<UserOrgsClaimsMeta> OrgClaimsMeta { get; set; }
    }
}
