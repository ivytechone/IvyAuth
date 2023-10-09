using IvyAuth.Interfaces;

namespace IvyAuth
{
	public class IvyIdentity : IIvyIdentity
	{
		public IvyIdentity(string email, string id, string firstName, string lastName, string timeZone, string salt, string passwordHash)
		{
			Email = email;
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			TimeZone = timeZone;
			Salt = salt;
			PasswordHash = passwordHash;
		}

		public string Email { get; private set; }
		public string Id { get; private set; }
		public string FirstName {get; private set; }
		public string LastName { get; private set; }
		public string TimeZone { get; private set; }
		public string Salt { get; private set; }
    	public string PasswordHash { get; private set; }
	}
}
