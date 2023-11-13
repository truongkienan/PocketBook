using System.Security.Cryptography;
using System.Text;

namespace Utils;

public static class Helper
{
    public static byte[] Hash(string plantext)
    {
        HashAlgorithm algorithm = HashAlgorithm.Create("SHA-512");
        return algorithm.ComputeHash(Encoding.ASCII.GetBytes(plantext));
    }
    public static string test;
    public static string RandomString(int len)
    {
        string pattern = "qwertyuiopasdfghjklzxcvbnm1234567890";
        char[] arr = new char[len];
        Random rand = new Random();
        for (int i = 0; i < len; i++)
        {
            arr[i] = pattern[rand.Next(pattern.Length)];
        }
        test = string.Join(string.Empty, arr);
        return string.Join(string.Empty, arr);
    }
}
