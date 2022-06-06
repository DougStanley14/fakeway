using Bogus;
using Microsoft.EntityFrameworkCore;
using microsvc_userprofile.Data;
using microsvc_userprofile.Model;

public class AuthRDBLoader
{
    private AuthRContext db;
    private string deckPlateCsv;

    public AuthRDBLoader(AuthRContext db, string deckplateCsvPath)
    {
        this.db = db;
        this.deckPlateCsv = deckplateCsvPath;
    }

    public async Task Load(bool noIds = false) // Some of the EF Seeding doesn't want id's
    {
        var csvFilePath = @"DummyBunoSample.csv";
        var prsr = new DeckPlateDumpParser(csvFilePath);
        prsr.ParseDeckplateCSV();

        db.Users.AddRange(NddsUsers(noIds));
        db.Programs.AddRange(SampleProgramsFromASE(noIds));
        prsr.Platforms.ForEach(p => p.Id = 0);
        db.Platforms.AddRange(prsr.Platforms);
        await db.SaveChangesAsync();

        // Load from Deckplate CSV Data
        foreach (var org in prsr.OrgSavers)
        {
            db.ParentOrgs.Add(org);
            await db.SaveChangesAsync();
            Console.WriteLine($"Added ParentOrg {org.LongName}");
        }

        var lastOrgId = db.Organizations.Max(o => o.Id);
        db.Organizations.AddRange(DummyProducerOrgs(lastOrgId, noIds));
        db.Tags.AddRange(GenDummyTags(noIds));

        await db.SaveChangesAsync();

        await AddUsersToOrgs();

        await GenDummyOrgTags();

        await GenDummyOrgPrograms();

        await GenDummyOrgPlatforms();

        await db.SaveChangesAsync();

        var orgtags = await db.Organizations.Select(o => new
        {
            o.Code,
            Tags = o.Tags.Count()
        }).ToListAsync();
    }

    private async Task GenDummyOrgPrograms()
    {
        var progs = await db.Programs.ToArrayAsync();
        var prodOrgs = await db.Organizations.Where(o => o.OrgType == OrgType.Producer).ToListAsync();

        var rand = new Bogus.Randomizer();

        foreach (var org in prodOrgs)
        {
            List<NddsProgram> programset = rand.ArrayElements<NddsProgram>(progs, rand.Int(0, progs.Count())).ToList();

            foreach (var t in programset)
            {
                org.Programs.Add(t);
            }

            await db.SaveChangesAsync();
        }

        await db.SaveChangesAsync();
    }
    private async Task GenDummyOrgPlatforms()
    {
        var plats = await db.Platforms.ToArrayAsync();
        var prodOrgs = await db.Organizations.Where(o => o.OrgType == OrgType.Producer).ToListAsync();

        var rand = new Bogus.Randomizer();

        foreach (var org in prodOrgs)
        {
            List<Platform> platset = rand.ArrayElements<Platform>(plats, rand.Int(0, plats.Count())).ToList();

            foreach (var t in platset)
            {
                org.Platforms.Add(t);
            }

            await db.SaveChangesAsync();
        }

        await db.SaveChangesAsync();
    }

    private async Task GenDummyOrgTags()
    {
        var tags = await db.Tags.ToArrayAsync();
        var orgs = await db.Organizations.ToListAsync();

        var rand = new Bogus.Randomizer();

        foreach (var org in orgs)
        {
            List<Tag> tagset = rand.ArrayElements<Tag>(tags, rand.Int(0, tags.Count())).ToList();

            foreach (var t in tagset)
            {
                org.Tags.Add(t);
            }

            await db.SaveChangesAsync();
        }

        await db.SaveChangesAsync();
    }

    private List<Organization> DummyProducerOrgs(int lastOrgId, bool noIds)
    {
        int i = lastOrgId + 1;

        var orgs = new List<Organization>
            {
                new Organization { Id = noIds ? 0 : i++, OrgType = OrgType.Producer, ParentOrgId=1,    Code="CNS/ATM" , Name="C N S A T M" },
                new Organization { Id = noIds ? 0 : i++, OrgType = OrgType.Producer, ParentOrgId=null, Code="NProd1", Name="Producer Org 1" },
                new Organization { Id = noIds ? 0 : i++, OrgType = OrgType.Producer, ParentOrgId=null, Code="NProd2", Name="Producer Org 2" },
                new Organization { Id = noIds ? 0 : i++, OrgType = OrgType.Producer, ParentOrgId=null, Code="NProd2", Name="Producer Org 2" },
            };

        return orgs;
    }
   
    private List<Tag> GenDummyTags(bool noIds)
    {
        var i = 1;
        var rand = new Bogus.Randomizer();
        var tagbag = new List<string> { "Target", "Network", "RMF", "RDTE", "Status", "Labor", "Ship", "System", "V1", "V2", "V3", "V1.1", "Extra", "Crispy" };

        var tags = tagbag.Select(t => new Tag { Name = t }).ToList();

        return tags;
    }

    private List<ParentOrg> ExtraParentOrgs(int lastParOrgId, bool noIds)
    {
        int i = lastParOrgId + 1;

        var orgs = new List<ParentOrg>
            {
                new ParentOrg { Id = noIds ? 0 : i++, Name="CNS/ATM" , LongName="CNS Parent" },
                new ParentOrg { Id = noIds ? 0 : i++, Name="ProdOrg1", LongName="Producer Org 1" },
            };

        return orgs;
    }

    private List<User> NddsUsers(bool noIds)
    {
        int i = 1;
        return new List<User>
            {
                new User { Id = noIds ? 0 : i++, EDIPI = 9111111111L, UserName="test1"},
                new User { Id = noIds ? 0 : i++, EDIPI = 9211111111L, UserName="test2"},
                new User { Id = noIds ? 0 : i++, EDIPI = 9333333333L, UserName="test3"},
            };
    }

    private List<NddsProgram> SampleProgramsFromASE(bool noIds)
    {
        int i = 1;
        var progs = new List<NddsProgram>
        {
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "APR-39A(V)2",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "APR-39C(V)2",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "AAR-47E(V)2",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "ALE-47 (Non-PWR PC)",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "ALE-47 (w/PWR PC) ",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "AN/AAQ-24B(V)27",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "ALR-56M",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "ALQ-157A(V)1",},
            new NddsProgram{ Id = noIds ? 0 : i++, Name = "AAQ-24(B)V25",},
        };

        return progs;
    }

    private async Task AddUsersToOrgs()
    {
        var user1 = await db.Users.FindAsync(1);
        var user2 = await db.Users.FindAsync(2);
        var user3 = await db.Users.FindAsync(3);

        // Shitty QD refactor of old seeder
        var org1 = await db.Organizations.Where(s => s.Code == "A41").SingleOrDefaultAsync();
        var org2 = await db.Organizations.Where(s => s.Code == "Q64").SingleOrDefaultAsync();
        var org3 = await db.Organizations.Where(s => s.Code == "GE7").SingleOrDefaultAsync();
        var org4 = await db.Organizations.Where(s => s.Code == "SD2").SingleOrDefaultAsync();
        var org5 = await db.Organizations.Where(s => s.Code == "SE4").SingleOrDefaultAsync();
        var org6 = await db.Organizations.Where(s => s.Code == "GEY").SingleOrDefaultAsync();
        var org7 = await db.Organizations.Where(s => s.Code == "CNS/ATM").SingleOrDefaultAsync();
        var org8 = await db.Organizations.Where(s => s.Code == "NProd1").SingleOrDefaultAsync();

        var gpname = db.Organizations.GroupBy(x => x.Code).Select(g => g.Key).ToList();

        // User 1
        user1.Orgs.Add(org1);
        user1.Orgs.Add(org2);
        user1.Orgs.Add(org7);
        user1.Orgs.Add(org8);


        // User 2
        user2.Orgs.Add(org4);
        user2.Orgs.Add(org5);


        // User 3
        user3.Orgs.Add(org8);

        await db.SaveChangesAsync();
    }
}