using IvyAuth.Interfaces;
using Newtonsoft.Json;

namespace IvyAuth
{
    public class IvyIdentity : IIvyIdentity
    {
        public IvyIdentity(string email, string id, string firstName, string lastName, string timeZone, string salt, string passwordHash)
        {
            Email = email;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            TimeZone = timeZone;
            Salt = salt;
            PasswordHash = passwordHash;
            TwoFactorMethods = new List<TwoFactorMethod>();
        }

        public static IvyIdentity? LoadFromDisk(string email)
        {
            var serializer = new JsonSerializer();

            using (var fileReader = File.OpenText($"/home/azureuser/IvyAuth/{email}.json"))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var identity = serializer.Deserialize<IvyIdentity>(jsonReader);

                if (identity != null)
                {
                    identity.Email = email;
                    return identity;
                }
            }
            return null;
        }

        public string Email { get; private set; }
        public string Id { get; private set; }
        public string FirstName {get; private set; }
        public string LastName { get; private set; }
        public string TimeZone { get; private set; }
        public string Salt { get; private set; }
        public string PasswordHash { get; private set; }
        public IEnumerable<TwoFactorMethod> TwoFactorMethods {get; private set;}
    }
}
