namespace IvyAuth.Interfaces
{
	public interface IIdentityStore
	{
		public IIvyIdentity? Authenticate(string email, string password);
	}
}
