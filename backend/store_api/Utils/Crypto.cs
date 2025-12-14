using System.Security.Cryptography;
using System.Text;

namespace mongo_api.Utils;

public class Crypto
{
    public static string ToHexString(string input)
    {
        var inBytes = Encoding.UTF8.GetBytes(input);
        var inHash = SHA3_256.Create().ComputeHash(inBytes);
        return Convert.ToHexString(inHash);
    }

    public static bool IsHexStringValid(string input, string hex)
    {
        var inHex = ToHexString(input);
        return hex.Equals(inHex);
    }
}