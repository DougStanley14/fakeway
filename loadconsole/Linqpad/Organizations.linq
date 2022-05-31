<Query Kind="Expression">
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
</Query>

Organizations.Select( o => new {
	o.OrgType,
	o.Code,
	o.Name,
	Parent = o.ParentOrg.Name,
	Plats = o.Platforms.Count(),
	Progs = o.Programs.Count(),
	Tags = o.Tags.Count()
})