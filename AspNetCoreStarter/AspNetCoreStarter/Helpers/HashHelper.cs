using System.Security.Cryptography;
using System.Text;

namespace AspNetCoreStarter.Helpers
{
    public static class HashHelper
    {
        public static string Hash(string input)
        {
            StringBuilder hash = new StringBuilder();

            using (SHA256Managed crypt = new SHA256Managed())
            {
                byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(input));

                foreach (byte theByte in crypto)
                    hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
