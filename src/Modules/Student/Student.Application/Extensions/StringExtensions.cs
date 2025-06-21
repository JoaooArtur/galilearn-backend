
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Student.Application.Extensions
{
    public static partial class StringExtensions
    {
        [GeneratedRegex(@"[^0-9a-zA-Z\._@+]")]
        private static partial Regex RemoveSpecialCharactersRegex();

        public static string RemoveSpecialCharacters(this string input)
            => RemoveSpecialCharactersRegex().Replace(input, string.Empty);

        [GeneratedRegex(@"[^0-9a-zA-Z_@]")]
        private static partial Regex RemoveNonAlphaNumericCharactersRegex();

        public static string RemoveNonAlphaNumericCharacters(this string input)
            => RemoveNonAlphaNumericCharactersRegex().Replace(input, string.Empty);

        public static string FormatCpf(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return string.Empty;

            return cpf.Length == 11 ? $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}.{cpf[9..]}" : cpf;
        }

        public static string HashMD5(this string text)
        {
            var inputBytes = Encoding.UTF8.GetBytes(text);
            var hashBytes = MD5.HashData(inputBytes);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
