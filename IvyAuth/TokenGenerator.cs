using IvyAuth.Interfaces;
using JWT.Algorithms;
using JWT.Builder;

namespace IvyAuth 
{
    // todo need a way to delete expired codes
    public class TokenGenerator : ITokenGenerator
    {
        public readonly ICertificateManager _certificateManager;
        private readonly Dictionary<string, AuthCode> _codes;
        public TokenGenerator(ICertificateManager certificateManager)
        {
            _certificateManager = certificateManager;
            _codes = new Dictionary<string, AuthCode>();
        }

        public string? ExchangeCode(string code, string code_verifier)
        {
            if (_codes.TryGetValue(code, out AuthCode? authCode) && authCode != null)
            {
                return authCode.GetToken(code_verifier);
            }
            else
            {
                return null;
            }
        }

        public string GenerateCode(IIvyIdentity identity, IApplication app, string scope, string code_challenge)
        {
            var token = GenerateToken(identity, app, scope);
            var authCode = new AuthCode(token, code_challenge);
            _codes.Add(authCode.Code, authCode);
            return authCode.Code;
        }

        public string GenerateToken(IIvyIdentity identity, IApplication app, string scope)
        {            
            var cert = _certificateManager.GetPrimaryCertificateWithPrivateKey();

			var token = JwtBuilder.Create()
				.WithAlgorithm(new RS256Algorithm(cert.Certificate))
				.AddHeader("kid", cert.Kid)
				.AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(60).ToUnixTimeSeconds())
				.AddClaim("iss", "ivytech.one")
				.AddClaim("aud", app.Id)
				.AddClaim("sub", identity.Id)
				.AddClaim("name", $"{identity.FirstName} {identity.LastName}")
                .AddClaim("scope", scope)
				.AddClaim("zoneinfo", identity.TimeZone)
				.Encode();
            
            return token;
        }
    }
}
