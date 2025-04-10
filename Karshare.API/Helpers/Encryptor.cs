using System.Security.Cryptography;

namespace Karshare.API.Helpers
{
    public class Encryptor
    {
        //private readonly string _secretKey;
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100;

        public Encryptor(/*string secretKey*/)
        {
            //_secretKey = secretKey;
        }

        public string EncryptPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                       password,
                       SaltSize,
                       Iterations,
                       HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Iterations}.{salt}.{key}";
            }
        }

        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                                          "Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != Iterations;

            using (var algorithm = new Rfc2898DeriveBytes(
                       password,
                       salt,
                       iterations,
                       HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);

                var verified = keyToCheck.SequenceEqual(key);

                return (verified, needsUpgrade);
            }
        }
    }
}

