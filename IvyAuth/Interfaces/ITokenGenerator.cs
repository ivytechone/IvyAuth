namespace IvyAuth.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(IIvyIdentity identity, IApplication application, string scope);
        string GenerateCode(IIvyIdentity identity, IApplication app, string scope, string code_challenge);
        string? ExchangeCode(string code, string code_verifier);
    }
}