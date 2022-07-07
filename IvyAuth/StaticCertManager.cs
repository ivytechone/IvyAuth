using IvyAuth.Config;
using IvyAuth.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace IvyAuth
{
	/// <summary>
	/// A Certificate Manager that loads from PEM data in config
	/// </summary>
	public class StaticCertManager : ICertificateManager
	{
		private readonly StaticCertManagerConfig _config;

		public StaticCertManager(StaticCertManagerConfig config)
		{
			if (config == null || String.IsNullOrWhiteSpace(config.certPem) || String.IsNullOrWhiteSpace(config.certKey))
			{
				throw new Exception("StaticCertManagerConfig Missing");
			}

			_config = config;
		}

		public X509Certificate2 GetCertificateWithPrivateKey() => X509Certificate2.CreateFromPem(_config.certPem, _config.certKey);
		public X509Certificate2 GetAidCertificateWithPrivateKey() => X509Certificate2.CreateFromPem(_config.aidCertKey, _config.aidCertKey);
	}
}
