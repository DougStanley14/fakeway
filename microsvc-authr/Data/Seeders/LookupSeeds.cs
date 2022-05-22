using microsvc_authr.Model;

namespace microsvc_authr.Data.Seeders
{
    public partial class LookupSeeds
    {
        public static List<NddsUser> NddsUsers()
        {
            int i = 1;
            return new List<NddsUser>
            {
                new NddsUser { Id = i++, EDIPI = 9111111111L, UserName="test1", Consumer=false, Producer=true},
                new NddsUser { Id = i++, EDIPI = 9222222222L, UserName="test2", Consumer=true,  Producer=false},
                new NddsUser { Id = i++, EDIPI = 9333333333L, UserName="test3", Consumer=true,  Producer=true},
            };
        }

        // Deprecated when Hiearchy Implemented
        //internal static List<UserSecurityGroup> UserSecurityGroups()
        //{
        //    var usgs = new List<UserSecurityGroup>();

        //    // User 1
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "BK1")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 1, SecurityGroupId = s.Id })
        //        );
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "GDK")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 1, SecurityGroupId = s.Id })
        //        );
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "B65")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 1, SecurityGroupId = s.Id })
        //        );

        //    // User 2
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "BL3")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 2, SecurityGroupId = s.Id })
        //        );
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "B65")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 2, SecurityGroupId = s.Id })
        //        );
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "GM4")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 2, SecurityGroupId = s.Id })
        //        );

        //    // User 2
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "BL3")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 3, SecurityGroupId = s.Id })
        //        );
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "B65")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 3, SecurityGroupId = s.Id })
        //        );
        //    usgs.AddRange(
        //        LookupSeeds.FlatSecurityGroups().Where(s => s.OrgCode == "GM4")
        //        .Select(s => new UserSecurityGroup { NddsUserId = 3, SecurityGroupId = s.Id })
        //        );

        //    return usgs;
        //}
    }
}
