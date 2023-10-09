namespace IvyAuth.Config
{
	public class StaticIdentiyStoreIdentity
	{
		public string? Email { get; set; }
		public string? Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName {get;set;}
		public string? TimeZone { get; set; }
		public string? Password { get; set; }
	}

	public class StaticIdentityStoreConfig
	{
		public IEnumerable<StaticIdentiyStoreIdentity>? identities { get; set; }
	}
}
