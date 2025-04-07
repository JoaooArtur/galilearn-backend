
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Student.Application.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string input)
            => Regex.Replace(input, @"[^0-9a-zA-Z\._@+]", string.Empty);

        public static string RemoveNonAlphaNumericCharacters(this string input)
            => Regex.Replace(input, @"[^0-9a-zA-Z_@]", string.Empty);

        public static string FormatCpf(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return string.Empty;

            return cpf.Length == 11 ? $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}.{cpf[9..]}" : cpf;
        }
        public static string HashMD5(this string text)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(text);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
