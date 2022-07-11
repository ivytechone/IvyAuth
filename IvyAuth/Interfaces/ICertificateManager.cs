using System.Security.Cryptography.X509Certificates;

namespace IvyAuth.Interfaces
{
	public interface ICertificateManager
	{
		X509Certificate2 GetCertificateWithPrivateKey();
		X509Certificate2 GetAidCertificateWithPrivateKey();
	}
}
