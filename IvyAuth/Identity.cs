using IvyAuth.Interfaces;

namespace IvyAuth
{
	public class Identity : IIdentity
	{
		public string Id { get; set; }
		public string Name {get; set; }
		public string TimeZone { get; set; }
	}
}
