﻿using CsvHelper;
using CsvHelper.Configuration;
using microsvc_authr.Model;
using System.Globalization;

namespace microsvc_authr.Data
{
    public class BunoDumpParser
    {
        public string BunoCsvPath { get; set; }
        public List<ParentOrg> WMSeeders { get; set; }
        public List<ParentOrg> WMSavers { get; set; }

        public List<Platform> Platforms { get; set; }

        public BunoDumpParser(string csvPath)
        {
            BunoCsvPath = csvPath;
        }

        public void ParseBuno()
        {
            var csvFilePath = BunoCsvPath;

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                csv.Context.RegisterClassMap<SecurityGroupMap>();
                var records = csv.GetRecords<SecurityGroupFlatDump>();

                var recs = records.ToList();

                Platforms = GenPlatformsAndBunos(recs);


                var parid = 1;
                var orgid = 1;
                var pltid = 1;
                var bunid = 1;

                WMSeeders = recs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
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
                                          Name = g.Key.OrgCode,
                                          OrgType = OrgType.Consumer,
                                          LongName = g.Key.Org,
                                          ParentOrgId = parid - 1, // offset autoincrement
                                          OrgPlatforms = g.GroupBy(c => c.Tms)
                                                          .Select(tmg => new OrgPlatform { 
                                                            PlatformId = Platforms
                                                                          .SingleOrDefault( p => p.Name == tmg.Key)
                                                                          .Id
                                                          }).ToList()

                                      }).ToList()
                                   }).ToList();

                // Keys are Nuanced becaus of Many to Many Org to Platform Relationship
                WMSavers = recs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
                                   .Select(wg => new ParentOrg
                                   {
                                       Name = wg.Key.WingMawCode,
                                       ParentOrgType = ParentOrgType.WingMaw,
                                       LongName = wg.Key.WingMaw,
                                       Orgs = wg.GroupBy(s => new { s.OrgCode, s.Org })
                                      .Select(g => new Organization
                                      {
                                          Name = g.Key.OrgCode,
                                          OrgType = OrgType.Consumer,
                                          LongName = g.Key.Org,
                                          OrgPlatforms = g.GroupBy(c => c.Tms)
                                                          .Select(tmg => new OrgPlatform
                                                          {
                                                              PlatformId = Platforms
                                                                          .SingleOrDefault(p => p.Name == tmg.Key)
                                                                          .Id
                                                          }).ToList()
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