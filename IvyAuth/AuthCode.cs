using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
namespace IvyAuth
{

public class AuthCode
{
    public AuthCode(string token, string code_challenge)
    {
        Code = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
        Code_Challenge = code_challenge;
        Token = token;
        Expiration = DateTime.UtcNow.AddMinutes(1);
    }

    public string GetToken(string code_verifier)
    {
        // todo validate code_verifier
        // todo validate expiration
        return Token;
    }

    public string Code {get; private set;}
    private string Code_Challenge {get; set;}
    private string Token {get; set;}
    private DateTime Expiration {get; set;}
}
}