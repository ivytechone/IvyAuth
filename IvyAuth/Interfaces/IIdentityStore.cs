using IvyAuth.DataModels;

namespace IvyAuth.Interfaces
{
	public interface IIdentityStore
	{
		public IIdentity? Authenticate(UserNamePassword creds);
	}
}
