using System;
using System.Security.Cryptography;


namespace RepositoryLayer.Hashing
{
    public class Password_Hash
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int PassSize = 36;
        private const int Iterations = 10000;
        public  string PasswordHashing(string userPass)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            byte[] hashByte = new byte[PassSize];
            Array.Copy(salt, 0, hashByte, 0, SaltSize);
            Array.Copy(hash, 0, hashByte, SaltSize, HashSize);
            string hashedPassword = Convert.ToBase64String(hashByte);
            return hashedPassword;
        }

        public  bool VerifyPassword(string userPass, string storedHashPass)
        {
            byte[] hashByte = Convert.FromBase64String(storedHashPass);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashByte, 0, salt, 0, SaltSize);
            var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            bool result= ComparePassword(hash,hashByte);
            return result;
        }

        private bool ComparePassword(byte[] hash, byte[] hashByte)
        {
            for(int i = 0;i<HashSize;i++)
            {
                if (hashByte[i+SaltSize]!= hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
