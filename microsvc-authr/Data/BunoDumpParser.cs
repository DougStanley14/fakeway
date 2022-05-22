using CsvHelper;
using CsvHelper.Configuration;
using microsvc_authr.Model;
using System.Globalization;

namespace microsvc_authr.Data
{
    public class BunoDumpParser
    {
        public string BunoCsvPath { get; set; }
        public List<WingMaw> WMSeeders { get; set; }
        public List<WingMaw> WMSavers { get; set; }

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

                var wmid = 1;
                var sgid = 1;
                var tmid = 1;
                var bnid = 1;

                WMSeeders = recs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
                                   .Select(wg => new WingMaw
                                   {
                                       Id = wmid++,
                                       Name = wg.Key.WingMaw,
                                       WingMawCode = wg.Key.WingMawCode,
                                       SecGroups = wg.GroupBy(s => new { s.OrgCode, s.Org })
                                      .Select(g => new SecurityOrgGroup
                                      {
                                          Id = sgid++,
                                          Name = g.Key.OrgCode,
                                          GroupOrg = g.Key.Org,
                                          WingMawId = wmid - 1, // offset autoincrement
                                          TMSs = g.GroupBy(c => c.Tms)
                                                 .Select(tmg => new TMS
                                                 {
                                                     Id = tmid++,
                                                     Name = tmg.Key,
                                                     SecurityOrgGroupId = sgid - 1, // offset autoincrement
                                                     Bunos = tmg.Select(t => new Buno
                                                     {
                                                         Id = bnid++,
                                                         BunoCode = t.Buno,
                                                         Location = t.Location,
                                                         QtrEndDate = t.QtrEndDate,
                                                         TMSId = tmid - 1 // offset autoincrement
                                                     }).ToList()
                                                 }).ToList()
                                      }).ToList()
                                   }).ToList();

                // Savers Don't Need Keys
                WMSavers = recs.GroupBy(s => new { s.WingMaw, s.WingMawCode })
                                   .Select(wg => new WingMaw
                                   {
                                       Name = wg.Key.WingMaw,
                                       WingMawCode = wg.Key.WingMawCode,
                                       SecGroups = wg.GroupBy(s => new { s.OrgCode, s.Org })
                                      .Select(g => new SecurityOrgGroup
                                      {
                                          Name = g.Key.OrgCode,
                                          GroupOrg = g.Key.Org,
                                          TMSs = g.GroupBy(c => c.Tms)
                                                 .Select(tmg => new TMS
                                                 {
                                                     Name = tmg.Key,
                                                     Bunos = tmg.Select(t => new Buno
                                                     {
                                                         BunoCode = t.Buno,
                                                         Location = t.Location,
                                                         QtrEndDate = t.QtrEndDate,
                                                     }).ToList()
                                                 }).ToList()
                                      }).ToList()
                                   }).ToList();
            }
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
}
