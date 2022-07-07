namespace IvyAuth.Config
{
	public class StaticIdentiyStoreIdentity
	{
		public string? UserName { get; set; }
		public string? Name { get; set; }
		public string? Id { get; set; }
		public string? Password { get; set; }
		public string? TimeZone { get; set; }
	}

	public class StaticIdentityStoreConfig
	{
		public IEnumerable<StaticIdentiyStoreIdentity>? identities { get; set; }
	}
}
