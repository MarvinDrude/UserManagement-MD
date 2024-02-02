using System.Security.Cryptography;
using System.Text;

namespace UserManagement.Utilities;

public static class RandomUtils {

    public static string GetRandomReadableString(string alphabet, int length) {

        var builder = new StringBuilder();

        for(int e = 0; e < length; e++) {

            builder.Append(
                alphabet[RandomNumberGenerator.GetInt32(0, alphabet.Length)]
            );

        }

        return builder.ToString();

    }

}
