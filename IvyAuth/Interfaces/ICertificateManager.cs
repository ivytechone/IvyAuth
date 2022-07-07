using System.Security.Cryptography.X509Certificates;

namespace IvyAuth.Interfaces
{
	public interface ICertificateManager
	{
		IEnumerable<X509Certificate2> GetPublicKeyCertificates();
		X509Certificate2 GetCertificateWithPrivateKey();
	}
}
