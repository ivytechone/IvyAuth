using System.Security.Cryptography.X509Certificates;

namespace IvyAuth.DataModels
{
    public class JwtKeyPrivate
    {
        public string? Kid { get; set; }
        public X509Certificate2? Certificate { get; set; }
    }
}
