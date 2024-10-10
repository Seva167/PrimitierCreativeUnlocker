using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

// Secret key extracted from the game
string key = "tWB9ig9BZ8ZVhV0y";
string iv = "xuX8ONmUVcZkh5Ab";

byte[] kb = Encoding.UTF8.GetBytes(key);
byte[] ivb = Encoding.UTF8.GetBytes(iv);

Aes aes = Aes.Create();
aes.Key = kb;
aes.IV = ivb;

using var encryptor = aes.CreateEncryptor(kb, ivb);

var pd = new ProgressData();
for (byte i = 1; i < 255; i++)
    pd.substanceDiscovered.Add(1);

using (var hnd = File.OpenHandle(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Primitier", "Saves", ".progress"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
{
    using var fs = new FileStream(hnd, FileAccess.Write);
    using var cs = new CryptoStream(fs, encryptor, CryptoStreamMode.Write);
    using var sw = new StreamWriter(cs);
    sw.Write(JsonSerializer.Serialize(pd));

    File.SetAttributes(hnd, FileAttributes.Hidden);
}

Console.WriteLine("Progress unlocked!!\nPress any key to exit");
Console.ReadKey();

public class ProgressData
{
    public List<byte> substanceDiscovered { get; set; } = [];
}
