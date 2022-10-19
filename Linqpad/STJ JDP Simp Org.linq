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
  <NuGetReference>SystemTextJson.JsonDiffPatch</NuGetReference>
  <Namespace>microsvc_authr.Services</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json.JsonDiffPatch</Namespace>
  <Namespace>microsvc_authr.Model</Namespace>
</Query>

async Task Main()
{
	var db = this;
	var orgs = await db.Organizations.Take(3).ToListAsync();	
	var jsopts = new JsonSerializerOptions { WriteIndented = true };
	
	var org1 = orgs.First();
	var g1j = JsonSerializer.Serialize(org1, jsopts);
	
	var org2 = JsonSerializer.Deserialize<Organization>(g1j);
	org2.Code = "blah Blah";
	org2.Description = "How Now";
	var g2j = JsonSerializer.Serialize(org2, jsopts);
	
	// System.Text.Json.JsonDiffPatcher - https://github.com/weichch/system-text-json-jsondiffpatch
	var diff = JsonDiffPatcher.Diff(g1j, g2j);
	
	diff.Dump();
	
}

