using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data;

try
{
    //var csvFilePath = @"C:\projects\work\NDDS\Deckplate BUNOs.csv";
    var csvFilePath = @"Deckplate BUNOs.csv";
    var prsr = new BunoDumpParser(csvFilePath);

    prsr.ParseBuno();

    var NDDSConnStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NDDSMeta;Integrated Security=True";

    var NDDSOptsBldr = new DbContextOptionsBuilder<NddsAuthRContext>();
    NDDSOptsBldr.UseSqlServer(NDDSConnStr, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
    NDDSOptsBldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    CreateNDDSMeta(NDDSConnStr);

    using (var db = new NddsAuthRContext(NDDSOptsBldr.Options))
    {
        db.Database.ExecuteSqlRaw("ALTER DATABASE NDDSMeta SET RECOVERY FULL");

        foreach (var wm in prsr.WMSavers)
        {
            db.WingMaws.Add(wm);
            db.SaveChanges();
            Console.WriteLine($"Added WingMaw {wm.WingMawCode}");
        }
    }

    Console.WriteLine("Done");
    Console.ReadLine();
}
catch (Exception ex)
{
    throw ex;
}

void CreateNDDSMeta(string CORTAConnStr)
{
    try
    {
        var optionsBuilder = new DbContextOptionsBuilder<NddsAuthRContext>();
        optionsBuilder.UseSqlServer(CORTAConnStr);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        using (var db = new NddsAuthRContext(optionsBuilder.Options))
        {
            Console.WriteLine($"Dropping {db.Database.GetDbConnection().Database} on {db.Database.GetDbConnection().DataSource}");
            db.Database.EnsureDeleted();
            Console.WriteLine($"Creating {db.Database.GetDbConnection().Database} on {db.Database.GetDbConnection().DataSource}");
            db.Database.EnsureCreated();
            db.Database.ExecuteSqlRaw("ALTER DATABASE NDDSMeta SET RECOVERY BULK_LOGGED");
        }
    }
    catch (Exception ex)
    {
        throw ex;
    }
}