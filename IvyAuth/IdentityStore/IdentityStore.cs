using System.Collections.Concurrent;
using System.Security.Cryptography;
using IvyAuth.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace IvyAuth
{
public class IdentityStore : IIdentityStore
{
    private readonly ConcurrentDictionary<string, IIvyIdentity> _credentials;

    public IdentityStore()
    {
        _credentials = new ConcurrentDictionary<string, IIvyIdentity>();
    }

    public IIvyIdentity? Authenticate(string email, string password)
    {
        if (!_credentials.TryGetValue(email, out var identity))
        {
            identity = LoadFromDisk(email);
            if (identity == null)
            {
                return null;
            }
        }

        if (identity.PasswordHash == PasswordHash(Convert.FromBase64String(identity.Salt), password))
        {
            return identity;
        }

        return null;
    }

    private IvyIdentity? LoadFromDisk(string email)
    {
        var id = IvyIdentity.LoadFromDisk(email);
        if (id != null)
        {
            _credentials.TryAdd(id.Email, id);
        }

        return id;
    }

    private string GenerateSalt()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(12));
    }

    private string PasswordHash(byte[] salt, string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100000, 32));
    }
}
}