using IvyAuth.Config;
using IvyAuth.Interfaces;
using IvyAuth.DataModels;
using System.Security.Cryptography.X509Certificates;

namespace IvyAuth
{
	/// <summary>
	/// A Certificate Manager that loads from PEM data in config
	/// </summary>
	public class StaticCertManager : ICertificateManager
	{
		private readonly StaticCertManagerConfig _config;

		public StaticCertManager(StaticCertManagerConfig? config)
		{
			if (config == null ||
			  string.IsNullOrWhiteSpace(config.certPem) ||
			  string.IsNullOrWhiteSpace(config.certKey) ||
			  string.IsNullOrWhiteSpace(config.aidCertPem) ||
			  string.IsNullOrWhiteSpace(config.aidCertPem))
			{
				throw new Exception("StaticCertManagerConfig Missing");
			}

			_config = config;
		}

		public JwtKeyPrivate GetPrimaryCertificateWithPrivateKey() => new JwtKeyPrivate() {
			Kid = "1",
			Certificate = X509Certificate2.CreateFromPem(_config.certPem, _config.certKey)
		};

		public X509Certificate2 GetAidCertificateWithPrivateKey() => X509Certificate2.CreateFromPem(_config.aidCertPem, _config.aidCertKey);

		public IEnumerable<JwtKey> GetPublicKeys()
		{
			yield return new JwtKey()
			{
				Kid = "1",
				Key = _config.certPem
			};
		}
	}
}
