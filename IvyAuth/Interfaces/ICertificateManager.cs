using System.Security.Cryptography.X509Certificates;
using IvyAuth.DataModels;

namespace IvyAuth.Interfaces
{
	public interface ICertificateManager
	{
		JwtKeyPrivate GetPrimaryCertificateWithPrivateKey();
		IEnumerable<JwtKey> GetPublicKeys();
		X509Certificate2 GetAidCertificateWithPrivateKey();
	}
}
