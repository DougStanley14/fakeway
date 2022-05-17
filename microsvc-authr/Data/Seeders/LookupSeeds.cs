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
                new NddsUser { Id = i++, EDIPI = 9111111111L, UserName="test1", IsConsumer=false, IsProducer=true},
                new NddsUser { Id = i++, EDIPI = 9222222222L, UserName="test2", IsConsumer=true,  IsProducer=false},
                new NddsUser { Id = i++, EDIPI = 9333333333L, UserName="test3", IsConsumer=true, IsProducer=true},
            };
        }

        internal static List<SecurityGroup> SecurityGroups()
        {
            int i = 1;
            return new List<SecurityGroup>
           {
                new SecurityGroup {Id = i++, Buno = 162157, Tms="C-2A", TypeModel="C-2", Org="VRC-40 DET 1", OrgCode="BK1", WingMaw="COMACCLW DET", WingMawCode="AJ0", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 162159, Tms="C-2A", TypeModel="C-2", Org="VRC-40 DET 1", OrgCode="BK1", WingMaw="COMACCLW DET", WingMawCode="AJ0", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 164362, Tms="CH-53E", TypeModel="H-53", Org="HMH-462 WTI", OrgCode="GK6", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 162493, Tms="CH-53E", TypeModel="H-53", Org="HMH-462 WTI", OrgCode="GK6", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 162524, Tms="CH-53E", TypeModel="H-53", Org="HMH-366 RESET", OrgCode="FHS", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 162524, Tms="CH-53E", TypeModel="H-53", Org="HMH-366 RESET", OrgCode="FHS", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 161391, Tms="CH-53E", TypeModel="H-53", Org="HMH-361 RESET", OrgCode="GHX", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 161391, Tms="CH-53E", TypeModel="H-53", Org="HMH-361 RESET", OrgCode="GHX", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 169444, Tms="CMV-22B", TypeModel="V-22", Org="VRM-30 DET 2", OrgCode="QD2", WingMaw="COMVRMWING", WingMawCode="PZM", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 169444, Tms="CMV-22B", TypeModel="V-22", Org="VRM-30 DET 2", OrgCode="QD2", WingMaw="COMVRMWING", WingMawCode="PZM", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 167110, Tms="KC-130J", TypeModel="C-130", Org="VMGR-352 SPMAGTFCCK", OrgCode="GDK", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 167110, Tms="KC-130J", TypeModel="C-130", Org="VMGR-352 SPMAGTFCCK", OrgCode="GDK", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168091, Tms="MH-60R", TypeModel="H-60", Org="HSM-72 DET 3", OrgCode="BL3", WingMaw="COMHSMWINGLANT", WingMawCode="AZM", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166582, Tms="MH-60R", TypeModel="H-60", Org="HSM-72 DET 3", OrgCode="BL3", WingMaw="COMHSMWINGLANT", WingMawCode="AZM", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168138, Tms="MH-60R", TypeModel="H-60", Org="HSM-71 DET C", OrgCode="PQ7", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166526, Tms="MH-60R", TypeModel="H-60", Org="HSM-71 DET C", OrgCode="PQ7", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166579, Tms="MH-60R", TypeModel="H-60", Org="HSM-79 DET 1", OrgCode="B41", WingMaw="CHSCWL", WingMawCode="AZP", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166999, Tms="MH-60R", TypeModel="H-60", Org="HSM-79 DET 1", OrgCode="B41", WingMaw="CHSCWL", WingMawCode="AZP", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168155, Tms="MH-60R", TypeModel="H-60", Org="HSM-72 DET 1", OrgCode="BL1", WingMaw="COMHSMWINGLANT", WingMawCode="AZM", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168155, Tms="MH-60R", TypeModel="H-60", Org="HSM-72 DET 1", OrgCode="BL1", WingMaw="COMHSMWINGLANT", WingMawCode="AZM", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168127, Tms="MH-60R", TypeModel="H-60", Org="HSM-71 DET A", OrgCode="PQ5", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168148, Tms="MH-60R", TypeModel="H-60", Org="HSM-71 DET A", OrgCode="PQ5", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168139, Tms="MH-60R", TypeModel="H-60", Org="HSM-78 DET 2", OrgCode="Q52", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168154, Tms="MH-60R", TypeModel="H-60", Org="HSM-78 DET 2", OrgCode="Q52", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168137, Tms="MH-60R", TypeModel="H-60", Org="HSM-71 DET B", OrgCode="PQ6", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168143, Tms="MH-60R", TypeModel="H-60", Org="HSM-71 DET B", OrgCode="PQ6", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168119, Tms="MH-60R", TypeModel="H-60", Org="HSM-78 DET 3", OrgCode="Q53", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168142, Tms="MH-60R", TypeModel="H-60", Org="HSM-78 DET 3", OrgCode="Q53", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 167059, Tms="MH-60R", TypeModel="H-60", Org="HSM-78 DET 1", OrgCode="Q51", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 167059, Tms="MH-60R", TypeModel="H-60", Org="HSM-78 DET 1", OrgCode="Q51", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168147, Tms="MH-60R", TypeModel="H-60", Org="HSM-72 DET 2", OrgCode="BL2", WingMaw="COMHSMWINGLANT", WingMawCode="AZM", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168151, Tms="MH-60R", TypeModel="H-60", Org="HSM-72 DET 2", OrgCode="BL2", WingMaw="COMHSMWINGLANT", WingMawCode="AZM", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166569, Tms="MH-60R", TypeModel="H-60", Org="HSM-37 DET 4", OrgCode="QY4", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166569, Tms="MH-60R", TypeModel="H-60", Org="HSM-37 DET 4", OrgCode="QY4", WingMaw="COMHSMWINGPAC", WingMawCode="PY9", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 167870, Tms="MH-60S", TypeModel="H-60", Org="HSC-22 DET 5", OrgCode="B65", WingMaw="CHSCWL", WingMawCode="AZP", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166349, Tms="MH-60S", TypeModel="H-60", Org="HSC-22 DET 5", OrgCode="B65", WingMaw="CHSCWL", WingMawCode="AZP", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166344, Tms="MH-60S", TypeModel="H-60", Org="HSC-21 DET 1", OrgCode="QZ1", WingMaw="CHSCWP", WingMawCode="PY8", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166344, Tms="MH-60S", TypeModel="H-60", Org="HSC-21 DET 1", OrgCode="QZ1", WingMaw="CHSCWP", WingMawCode="PY8", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166721, Tms="MV-22B", TypeModel="V-22", Org="VMM-161 SPMAGTFHOA", OrgCode="GHQ", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166495, Tms="MV-22B", TypeModel="V-22", Org="VMM-161 SPMAGTFHOA", OrgCode="GHQ", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168651, Tms="MV-22B", TypeModel="V-22", Org="VMM-261 WTI", OrgCode="FCK", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168230, Tms="MV-22B", TypeModel="V-22", Org="VMM-261 WTI", OrgCode="FCK", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("9/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168229, Tms="MV-22B", TypeModel="V-22", Org="VMM-266 SPMAGTFAFM", OrgCode="FCA", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168607, Tms="MV-22B", TypeModel="V-22", Org="VMM-266 SPMAGTFAFM", OrgCode="FCA", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168602, Tms="MV-22B", TypeModel="V-22", Org="VMM-163 SPMAGTFHOA", OrgCode="GM4", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168622, Tms="MV-22B", TypeModel="V-22", Org="VMM-163 SPMAGTFHOA", OrgCode="GM4", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166386, Tms="MV-22B", TypeModel="V-22", Org="VMM-266 CCRAM", OrgCode="FD8", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("3/31/2022 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166386, Tms="MV-22B", TypeModel="V-22", Org="VMM-266 CCRAM", OrgCode="FD8", WingMaw="SECOND MAW", WingMawCode="FZ9", QtrEndDate=DateTime.Parse("6/30/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166689, Tms="MV-22B", TypeModel="V-22", Org="VMM-161 HOA", OrgCode="GHQ", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 166495, Tms="MV-22B", TypeModel="V-22", Org="VMM-161 HOA", OrgCode="GHQ", WingMaw="THIRD MAW", WingMawCode="GZE", QtrEndDate=DateTime.Parse("12/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 168431, Tms="P-8A", TypeModel="P-8", Org="VP-46 CTF-67", OrgCode="PWB", WingMaw="PATWING 10", WingMawCode="PXA", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")},
                new SecurityGroup {Id = i++, Buno = 169339, Tms="P-8A", TypeModel="P-8", Org="VP-46 CTF-67", OrgCode="PWB", WingMaw="PATWING 10", WingMawCode="PXA", QtrEndDate=DateTime.Parse("3/31/2021 12:00:00 AM")}
           };
        }

        internal static List<UserSecurityGroup> UserSecurityGroups()
        {
            var usgs = new List<UserSecurityGroup>();

            // User 1
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "BK1")
                .Select(s => new UserSecurityGroup { NddsUserId = 1, SecurityGroupId = s.Id })
                );
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "GDK")
                .Select(s => new UserSecurityGroup { NddsUserId = 1, SecurityGroupId = s.Id })
                );
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "B65")
                .Select(s => new UserSecurityGroup { NddsUserId = 1, SecurityGroupId = s.Id })
                );

            // User 2
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "BL3")
                .Select(s => new UserSecurityGroup { NddsUserId = 2, SecurityGroupId = s.Id })
                );
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "B65")
                .Select(s => new UserSecurityGroup { NddsUserId = 2, SecurityGroupId = s.Id })
                );
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "GM4")
                .Select(s => new UserSecurityGroup { NddsUserId = 2, SecurityGroupId = s.Id })
                );

            // User 2
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "BL3")
                .Select(s => new UserSecurityGroup { NddsUserId = 3, SecurityGroupId = s.Id })
                );
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "B65")
                .Select(s => new UserSecurityGroup { NddsUserId = 3, SecurityGroupId = s.Id })
                );
            usgs.AddRange(
                LookupSeeds.SecurityGroups().Where(s => s.OrgCode == "GM4")
                .Select(s => new UserSecurityGroup { NddsUserId = 3, SecurityGroupId = s.Id })
                );

            return usgs;
        }
    }
}
