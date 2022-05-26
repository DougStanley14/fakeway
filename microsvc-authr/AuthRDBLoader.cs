using Bogus;
using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data;
using microsvc_authr.Model;

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
        var prsr = new BunoDumpParser(csvFilePath);
        prsr.ParseBuno();

        db.Users.AddRange(NddsUsers(noIds));

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
        db.UserOrgs.AddRange(DummyUsersInGroups());
        db.Tags.AddRange(GenDummyTags(noIds));

        await db.SaveChangesAsync();

        await GenDummyOrgTags2();

        await db.SaveChangesAsync();

        var orgtags = await db.Organizations.Select(o => new
        {
            o.Name,
            Tags = o.Tags.Count()
        }).ToListAsync();
    }

    private async Task GenDummyOrgTags2()
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
                new Organization { Id = noIds ? 0 : i++, OrgType = OrgType.Producer, ParentOrgId=1,    Name="CNS/ATM" , LongName="C N S A T M" },
                new Organization { Id = noIds ? 0 : i++, OrgType = OrgType.Producer, ParentOrgId=null, Name="ProdOrg1", LongName="Producer Org 1" },
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

    //private List<OrgTag> GenDummyOrgTags(bool noIds)
    //{
    //    var oids = db.Organizations.Select(o => o.Id).ToList();
    //    var tids = db.Tags.Select(o => o.Id).ToList();

    //    var fakeOrgTag = new Faker<OrgTag>()
    //        .RuleFor(t => t.TagId, f => f.PickRandom(tids))
    //        .RuleFor(t => t.OrganizationId, f => f.PickRandom(oids));

    //    var firstList = fakeOrgTag.Generate(50).ToList();

    //    var dedupes = firstList.GroupBy(o => new {o.TagId, o.OrganizationId}).Select(g => g.FirstOrDefault()).ToList();

    //    return dedupes;
    //}



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

    private List<UserOrg> DummyUsersInGroups()
    {
        var usgs = new List<UserOrg>();

        var gpname = db.Organizations.GroupBy(x => x.Name).Select(g => g.Key).ToList();

        // User 1
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "A41")
            .Select(s => new UserOrg { UserId = 1, OrgId = s.Id })
            );
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "Q65")
            .Select(s => new UserOrg { UserId = 1, OrgId = s.Id })
            );
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "GE7")
            .Select(s => new UserOrg { UserId = 1, OrgId = s.Id })
            );

        // User 2
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "SD2")
            .Select(s => new UserOrg { UserId = 2, OrgId = s.Id })
            );
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "SE4")
            .Select(s => new UserOrg { UserId = 2, OrgId = s.Id })
            );

        // User 3
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "GEY")
            .Select(s => new UserOrg { UserId = 3, OrgId = s.Id })
            );

        return usgs;
    }
}