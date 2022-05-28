using microsvc_authr.Model;

namespace microsvc_authr.Data.Seeders
{
    public partial class LookupSeeds
    {
        public static List<User> NddsUsers()
        {
            int i = 1;
            return new List<User>
            {
                new User { Id = i++, EDIPI = 9111111111L, UserName="test1"},
                new User { Id = i++, EDIPI = 9211111111L, UserName="test2"},
                new User { Id = i++, EDIPI = 9333333333L, UserName="test3"},
            };
        }

        public static List<Organization> DummyProducerOrgs(int MaxOrgId)
        {
            int i = MaxOrgId + 1;

            var orgs = new List<Organization>
            {
                new Organization { /*Id = i++, ParentOrgId = 1, */ OrgType = OrgType.Producer, Code="CNS/ATM"},
                new Organization { /*Id = i++, ParentOrgId = 1, */ OrgType = OrgType.Producer, Code="ProdOrg1"},
            };

            return orgs;
        }
    }
}
