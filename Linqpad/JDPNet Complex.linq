<Query Kind="Program">
  <Connection>
    <ID>ceb98730-a1f9-45ee-8392-dffe38c868e7</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="EF7Driver" PublicKeyToken="469b5aa5a4331a8c">EF7Driver.StaticDriver</Driver>
    <CustomAssemblyPath>E:\Repos\Sydney\fakeway\microsvc-authr\bin\Debug\net6.0\microsvc-authr.dll</CustomAssemblyPath>
    <AppConfigPath>E:\Repos\Sydney\fakeway\microsvc-authr\bin\Debug\net6.0\appsettings.json</AppConfigPath>
    <CustomTypeName>microsvc_authr.Data.AuthRContext</CustomTypeName>
    <DisplayName>NDDSctx</DisplayName>
    <DriverData>
      <UseDbContextOptions>false</UseDbContextOptions>
    </DriverData>
  </Connection>
  <Reference Relative="..\..\..\..\Sydney\fakeway\microsvc-userprofile\bin\Debug\net6.0\microsvc-userprofile.dll">E:\Repos\Sydney\fakeway\microsvc-userprofile\bin\Debug\net6.0\microsvc-userprofile.dll</Reference>
  <NuGetReference>JsonDiffPatch.Net</NuGetReference>
  <Namespace>JsonDiffPatchDotNet</Namespace>
  <Namespace>microsvc_authr.Services</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>JsonDiffPatchDotNet.Formatters.JsonPatch</Namespace>
</Query>

async Task Main()
{
	var db = this;
	var users = await GetUsersMeta(db);	
	var jsopts = new JsonSerializerOptions { WriteIndented = true };
	
	var user1 = users.First();
	var u1j = JsonSerializer.Serialize(user1, jsopts);
	
	var user2 = JsonSerializer.Deserialize<UserClamisMeta>(u1j);
	user2.OrgClaimsMeta.RemoveAt(0);
	user2.OrgClaimsMeta.First().Bunos.RemoveRange(1, 6);
	var u2j = JsonSerializer.Serialize(user2, jsopts);
	
	// JsonDiffPatch.Net - https://github.com/wbish/jsondiffpatch.net
	var patchdoc = new JsonDiffPatch().Diff(u1j, u2j);
	
	patchdoc.Dump();
	

}


public async Task<List<UserClamisMeta>> GetUsersMeta(UserQuery db)
{
	var users = await db.Users
				  .Select(u => new UserClamisMeta
				  {
					  EDIPI = u.EDIPI,
					  UserName = u.UserName,
					  OrgClaimsMeta = u.Orgs.Select(o => new UserOrgsClaimsMeta
					  {
						  OrgName = o.Code,
						  OrgType = o.OrgType,
						  Platforms = o.Platforms.Select(p => p.Name).ToList(),
						  Programs = o.Programs.Select(p => p.Name).ToList(),
						  Bunos = o.Platforms.SelectMany(p => p.Bunos.Select(b => b.BunoCode)).ToList()
					  }).ToList()
				  }).ToListAsync();
				  
	return users;
}
