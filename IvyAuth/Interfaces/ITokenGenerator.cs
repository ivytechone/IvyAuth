namespace IvyAuth.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(IIdentity identity, IApplication application, string scope);
        string GenerateCode(IIdentity identity, IApplication app, string scope, string code_challenge);
        string? ExchangeCode(string code, string code_verifier);
    }
}