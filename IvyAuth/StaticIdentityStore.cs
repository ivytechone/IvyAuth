using IvyAuth.Config;
using IvyAuth.Interfaces;

namespace IvyAuth
{
	/// <summary>
	/// An identity store that relies on static data in the config.
	/// </summary>
	public class StaticIdentityStore : IIdentityStore
	{
		private Dictionary<string, StaticIdentiyStoreIdentity> _identities;

		public StaticIdentityStore(StaticIdentityStoreConfig config)
		{
			_identities = new Dictionary<string, StaticIdentiyStoreIdentity>();

			if (config is not null && config.identities is not null)
			{
				foreach (var identity in config.identities)
				{
					if (identity.UserName is not null)
					{
						_identities.Add(identity.UserName, identity);
					}
				}
			}

			if (_identities.Count == 0)
			{
				throw new Exception("No static identities loaded");
			}
			
			Console.WriteLine($"{_identities.Count} static identities loaded.");
			foreach(var i in _identities.Values)
			{
				Console.WriteLine($"Identity {i.Id}:{i.UserName}:{i.Password}");
			}			
		}

		public IIdentity? Authenticate(DataModels.UserNamePassword creds)
		{
			if (creds == null || String.IsNullOrWhiteSpace(creds.UserName) || String.IsNullOrEmpty(creds.Password))
			{
				return null;
			}

			if (!_identities.TryGetValue(creds.UserName, out StaticIdentiyStoreIdentity? identity))
			{
				return null;
			}

			if (identity.Password != creds.Password)
			{
				return null;
			}

			return new Identity()
			{
				Name = identity.Name,
				Id = identity.Id,
				TimeZone = identity.TimeZone
			};
		}
	}
}
