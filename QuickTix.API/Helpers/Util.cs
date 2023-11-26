using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HouseMate.API.Helpers
{
    public static class Util
    {
        const int keySize = 64;
        const int iterations = 350000;
        public static bool IsValidEmail(string email)
        {
            if (email != null)
            {
                return Regex.IsMatch(email, @"\A[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}\z")
                    && Regex.IsMatch(email, @"^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*");
            }

            return false;
        }

        public static string GenerateRandomDigits(int length)
        {
            Random random = new();
            const string chars = "0123456789";
            string result = new(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        public static string GenerateRandomString(int length, bool specialChar = false)
        {
            Random random = new();

            string characters = specialChar ? "@$ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz@_!$" : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            string result = new(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result.ToUpper();
        }

        public static string HashPassword(string password, string salt)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static bool IsOTP(string userOTP)
        {
            int OTPlength = userOTP.Length;

            if(OTPlength != 6)
            {
                return false;
            }
            return long.TryParse(userOTP, out _);
        }

        public static string HashOTP(string userOTP)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(userOTP),
                Convert.FromBase64String("AgQGCAoMDhASFA=="),
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        public static bool VerifyOTP(string userOTP, string hashedOTPstored)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(userOTP, Convert.FromBase64String("AgQGCAoMDhASFA=="), iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedOTPstored));
        }
    }
}
