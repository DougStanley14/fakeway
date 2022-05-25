using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data;
using microsvc_authr.Model;

try
{
    //var csvFilePath = @"C:\projects\work\NDDS\Deckplate BUNOs.csv";
    var dbname = "NDDSMeta";
    var csvFilePath = @"DummyBunoSample.csv";
    var prsr = new BunoDumpParser(csvFilePath);

    prsr.ParseBuno();

    var NDDSConnStr = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={dbname};Integrated Security=True";

    var NDDSOptsBldr = new DbContextOptionsBuilder<AuthRContext>();
    NDDSOptsBldr.UseSqlServer(NDDSConnStr, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
    NDDSOptsBldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    CreateNDDSMeta(NDDSConnStr, dbname);

    using (var db = new AuthRContext(NDDSOptsBldr.Options))
    {
        db.Database.ExecuteSqlRaw($"ALTER DATABASE {dbname} SET RECOVERY FULL");

        prsr.Platforms.ForEach(p => p.Id = 0);
        db.Platforms.AddRange(prsr.Platforms);
        db.SaveChanges();

        foreach (var org in prsr.OrgSavers)
        {
            db.ParentOrgs.Add(org);

            var orgplats = org.Orgs.SelectMany(o => o.OrgPlatforms)
                                       .ToList();

            db.SaveChanges();
            Console.WriteLine($"Added ParentOrg {org.LongName}");
        }
    }

    Console.WriteLine("Done");
    Console.ReadLine();
}
catch (Exception ex)
{
    throw ex;
}

void CreateNDDSMeta(string CORTAConnStr, string dbname)
{
    try
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthRContext>();
        optionsBuilder.UseSqlServer(CORTAConnStr);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        using (var db = new AuthRContext(optionsBuilder.Options))
        {
            Console.WriteLine($"Dropping {db.Database.GetDbConnection().Database} on {db.Database.GetDbConnection().DataSource}");
            db.Database.EnsureDeleted();
            Console.WriteLine($"Creating {db.Database.GetDbConnection().Database} on {db.Database.GetDbConnection().DataSource}");
            db.Database.EnsureCreated();
            db.Database.ExecuteSqlRaw($"ALTER DATABASE {dbname} SET RECOVERY BULK_LOGGED");
        }
    }
    catch (Exception ex)
    {
        throw ex;
    }
}