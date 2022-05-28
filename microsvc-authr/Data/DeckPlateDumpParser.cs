using CsvHelper;
using CsvHelper.Configuration;
using microsvc_authr.Model;
using System.Globalization;

namespace microsvc_authr.Data
{
    public class DeckPlateDumpParser
    {
        public string BunoCsvPath { get; set; }
        public List<ParentOrg> OrgSeeders { get; set; }
        public List<ParentOrg> OrgSavers { get; set; }

        public List<Platform> Platforms { get; set; }

        public DeckPlateDumpParser(string csvPath)
        {
            BunoCsvPath = csvPath;
        }

        public void ParseDeckplateCSV()
        {
            var csvFilePath = BunoCsvPath;

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<SecurityGroupMap>();
                var records = csv.GetRecords<SecurityGroupFlatDump>();

                // filter out all but latest Bunos
                var cleanRecs = records.GroupBy( r => r.Buno)
                                       .Select( g => g.OrderByDescending(x => x.QtrEndDate)
                                                      .FirstOrDefault())
                                       .ToList();
                                                      
                                                      
                Platforms = GenPlatformsAndBunos(cleanRecs);


                var parid = 1;
                var orgid = 1;
                var pltid = 1;
                var bunid = 1;

                // May not be Necessary
                OrgSeeders = cleanRecs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
                                   .Select(wg => new ParentOrg
                                   {
                                       Id = parid++,
                                       Name =  wg.Key.WingMawCode,
                                       ParentOrgType = ParentOrgType.WingMaw,
                                       LongName = wg.Key.WingMaw,
                                       Orgs = wg.GroupBy(s => new { s.OrgCode, s.Org })
                                      .Select(g => new Organization
                                      {
                                          Id = orgid++,
                                          Code = g.Key.OrgCode,
                                          OrgType = OrgType.Consumer,
                                          Name = g.Key.Org,
                                          ParentOrgId = parid - 1, // offset autoincrement
                                          Platforms = g.GroupBy(c => c.Tms)
                                                          .Select(tmg => Platforms
                                                                          .SingleOrDefault( p => p.Name == tmg.Key)
                                                          ).ToList()

                                      }).ToList()
                                   }).ToList();

                // Keys are Nuanced becaus of Many to Many Org to Platform Relationship
                OrgSavers = cleanRecs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
                                   .Select(wg => new ParentOrg
                                   {
                                       Name = wg.Key.WingMawCode,
                                       ParentOrgType = ParentOrgType.WingMaw,
                                       LongName = wg.Key.WingMaw,
                                       Orgs = wg.GroupBy(s => new { s.OrgCode, s.Org })
                                      .Select(g => new Organization
                                      {
                                          Code = g.Key.OrgCode,
                                          OrgType = OrgType.Consumer,
                                          Name = g.Key.Org,
                                          Platforms = g.GroupBy(c => c.Tms)
                                                          .Select(tmg => Platforms
                                                                          .SingleOrDefault(p => p.Name == tmg.Key)
                                                          ).ToList()
                                      }).ToList()
                                   }).ToList();
            }
        }

        private List<Platform> GenPlatformsAndBunos(List<SecurityGroupFlatDump> recs)
        {
            var id = 1;
            var plats = recs.GroupBy(r => r.Tms)
                            .Select(g => new Platform { 
                                Id = id++, 
                                Name = g.Key,
                                TypeModel = g.FirstOrDefault().TypeModel,
                                Bunos = g.Select(t => new Buno
                                {
                                    BunoCode = t.Buno,
                                    Location = t.Location,
                                    QtrEndDate = t.QtrEndDate,
                                    PlatformId = id - 1 // offset autoincrement
                                }).ToList()
                            }).ToList();
            return plats;
        }
    }

    public class SecurityGroupMap : ClassMap<SecurityGroupFlatDump>
    {
        public SecurityGroupMap()
        {
            Map(m => m.Buno).Name("BUNO");
            Map(m => m.Tms).Name("TMS");
            Map(m => m.TypeModel).Name("Type Model");
            Map(m => m.Org).Name("Org");
            Map(m => m.OrgCode).Name("Org Code");
            Map(m => m.Location).Name("Location");
            Map(m => m.WingMaw).Name("Wing/Maw");
            Map(m => m.WingMawCode).Name("Wing/MAW Code");
            Map(m => m.QtrEndDate).Name("Quarter End Date");
        }
    }

    //public void ParseBunoOld()
    //{
    //    var csvFilePath = BunoCsvPath;

    //    using (var reader = new StreamReader(csvFilePath))
    //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    //    {

    //        csv.Context.RegisterClassMap<SecurityGroupMap>();
    //        var records = csv.GetRecords<SecurityGroupFlatDump>();

    //        var recs = records.ToList();

    //        Platforms = GenPlatforms(recs);

    //        var parid = 1;
    //        var orgid = 1;
    //        var pltid = 1;
    //        var bunid = 1;

    //        WMSeeders = recs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
    //                           .Select(wg => new NddsParentOrg
    //                           {
    //                               Id = parid++,
    //                               Name = wg.Key.WingMawCode,
    //                               ParentOrgType = NddsParentOrgType.WingMaw,
    //                               LongName = wg.Key.WingMaw,
    //                               NddsOrgs = wg.GroupBy(s => new { s.OrgCode, s.Org })
    //                              .Select(g => new NddsOrg
    //                              {
    //                                  Id = orgid++,
    //                                  Name = g.Key.OrgCode,
    //                                  LongName = g.Key.Org,
    //                                  NddsParentOrgId = parid - 1, // offset autoincrement
    //                                  TMSs = g.GroupBy(c => c.Tms)
    //                                         .Select(tmg => new Platform
    //                                         {
    //                                             Id = pltid++,
    //                                             Name = tmg.Key,
    //                                             NddsOrgId = orgid - 1, // offset autoincrement
    //                                             Bunos = tmg.Select(t => new Buno
    //                                             {
    //                                                 Id = bunid++,
    //                                                 BunoCode = t.Buno,
    //                                                 Location = t.Location,
    //                                                 QtrEndDate = t.QtrEndDate,
    //                                                 TMSId = pltid - 1 // offset autoincrement
    //                                             }).ToList()
    //                                         }).ToList()
    //                              }).ToList()
    //                           }).ToList();

    //        // Savers Don't Need Keys
    //        WMSavers = recs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
    //                           .Select(wg => new NddsParentOrg
    //                           {
    //                               Name = wg.Key.WingMawCode,
    //                               ParentOrgType = NddsParentOrgType.WingMaw,
    //                               LongName = wg.Key.WingMaw,
    //                               NddsOrgs = wg.GroupBy(s => new { s.OrgCode, s.Org })
    //                              .Select(g => new NddsOrg
    //                              {
    //                                  Name = g.Key.OrgCode,
    //                                  LongName = g.Key.Org,
    //                                  TMSs = g.GroupBy(c => c.Tms)
    //                                         .Select(tmg => new Platform
    //                                         {
    //                                             Name = tmg.Key,
    //                                             Bunos = tmg.Select(t => new Buno
    //                                             {
    //                                                 BunoCode = t.Buno,
    //                                                 Location = t.Location,
    //                                                 QtrEndDate = t.QtrEndDate,
    //                                             }).ToList()
    //                                         }).ToList()
    //                              }).ToList()
    //                           }).ToList();
    //    }
    //}

}
