namespace IvyAuth.Interfaces
{
	public interface IIvyIdentity
	{
		string Email { get; }
		string Id { get; }
		string FirstName {get; }
		string LastName { get; }
		string TimeZone { get; }
		string Salt { get; }
    	string PasswordHash { get; }
	}
}
