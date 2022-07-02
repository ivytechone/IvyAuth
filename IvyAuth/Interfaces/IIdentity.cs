namespace IvyAuth.Interfaces
{
	public interface IIdentity
	{
		string Id { get; }
		string Name { get; }
		string TimeZone { get; }
	}
}
