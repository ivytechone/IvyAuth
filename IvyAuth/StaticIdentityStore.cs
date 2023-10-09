using IvyAuth.Config;
using IvyAuth.Interfaces;

namespace IvyAuth
{
	/// <summary>
	/// An identity store that relies on static data in the config.
	/// Does not hash password, for use in testing only.
	/// </summary>
	public class StaticIdentityStore : IIdentityStore
	{
		private readonly Dictionary<string, IIvyIdentity> _identities;

		public StaticIdentityStore(StaticIdentityStoreConfig? config)
		{
			_identities = new Dictionary<string, IIvyIdentity>();

			if (config is not null && config.identities is not null)
			{
				foreach (var configIdentity in config.identities)
				{
					if (configIdentity is not null &&
					    !string.IsNullOrWhiteSpace(configIdentity.Email) &&
					    !string.IsNullOrWhiteSpace(configIdentity.Id) &&
						!string.IsNullOrWhiteSpace(configIdentity.FirstName) &&
						!string.IsNullOrWhiteSpace(configIdentity.LastName) &&
						!string.IsNullOrWhiteSpace(configIdentity.TimeZone) &&
						!string.IsNullOrWhiteSpace(configIdentity.Password))
					{
						var identity = new IvyIdentity(configIdentity.Email, configIdentity.Id, configIdentity.FirstName, configIdentity.LastName, configIdentity.TimeZone, string.Empty, configIdentity.Password);

						Console.WriteLine($"Adding {identity.Email}");
						_identities.Add(identity.Email, identity);
					}
				}
			}

			if (_identities.Count == 0)
			{
				throw new Exception("No static identities loaded");
			}
			
			Console.WriteLine($"{_identities.Count} static identities loaded.");
		}

		public IIvyIdentity? Authenticate(string email, string password)
		{
			Console.WriteLine($"au :{email}:{password}:{_identities.Count}");
			if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
			{
				return null;
			}

			if (!_identities.TryGetValue(email, out IIvyIdentity? identity))
			{
				return null;
			}

			Console.WriteLine($"pw:{identity.PasswordHash}");

			if (identity.PasswordHash != password)
			{
				return null;
			}

			return identity;
		}
	}
}
