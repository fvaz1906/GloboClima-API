using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace GloboClima.Infra.CrossCutting.Security
{
    public class SHA2
    {
        private readonly static string salt = "GloboClima!@#2024-ce7a0bb8-e1d0-479d-9cb7-d1485bb2c7b8";

        public static string GenerateHash(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
