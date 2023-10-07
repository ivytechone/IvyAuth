using IvyAuth.DataModels;

namespace IvyAuth.Interfaces
{
	public interface IIdentityStore
	{
		public IIdentity? Authenticate(string userId, string password);
	}
}
