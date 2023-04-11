using System.Security.Cryptography;
using System.Text;

namespace EasyBills.Security.Helpers;

/// <summary>
/// Encryption methods.
/// </summary>
public class EncryptionHelper
{
    /// <summary>
    /// Encrypt an string.
    /// </summary>
    /// <param name="str">The string to encrypt.</param>
    /// <returns>The value encrypted.</returns>
    public static string Encrypt(string str)
    {
        SHA256 sha256 = SHA256.Create();
        ASCIIEncoding encoding = new();
        StringBuilder sb = new StringBuilder();
        byte[] stream = sha256.ComputeHash(encoding.GetBytes(str));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        return sb.ToString();
    }
}
