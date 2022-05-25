<Query Kind="Statements">
  <Connection>
    <ID>b573e052-9198-40a3-81c2-bdcece59abe0</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>(localdb)\mssqllocaldb</Server>
    <Database>NDDSMeta</Database>
    <DisplayName>NDDS_ef</DisplayName>
    <DriverData>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

var db = this;

var usgs = new List<UserOrgs>();

var gpname = db.Organizations.GroupBy(x => x.Name).Select(g => g.Key).ToList();

// User 1
usgs.AddRange(
	db.Organizations.Where(s => s.Name == "A41")
	.Select(s => new UserOrgs { UserId = 1, OrgId = s.Id })
	);
usgs.AddRange(
	db.Organizations.Where(s => s.Name == "Q65")
	.Select(s => new UserOrgs { UserId = 1, OrgId = s.Id })
	);
usgs.AddRange(
	db.Organizations.Where(s => s.Name == "GE7")
	.Select(s => new UserOrgs { UserId = 1, OrgId = s.Id })
	);

// User 2
usgs.AddRange(
	db.Organizations.Where(s => s.Name == "SD2")
	.Select(s => new UserOrgs { UserId = 2, OrgId = s.Id })
	);
usgs.AddRange(
	db.Organizations.Where(s => s.Name == "SE4")
	.Select(s => new UserOrgs { UserId = 2, OrgId = s.Id })
	);

// User 3
usgs.AddRange(
	db.Organizations.Where(s => s.Name == "GEY")
	.Select(s => new UserOrgs { UserId = 3, OrgId = s.Id })
	);

db.UserOrgs.AddRange(usgs);

db.SaveChanges();