//string s = "Hello Extension Methods";
//int i = s.ArmCount();
//Console.WriteLine(i);






//public static class MyExtensions
//{
//    //this ถ้าต้องการขยายเรื่องอะไร ต้องใส่
//    public static int ArmCount(this string str)
//    {
//        return str.Split(new char[] { ' ', '.', '?' },StringSplitOptions.RemoveEmptyEntries).Length;
//    }
//}


using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Enter a password: ");
        string password = Console.ReadLine();

        // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
        // สร้างอเรขึ้นมาเปล่าๆ
        byte[] salt = new byte[128 / 8];
        // RNGCryptoServiceProvider เอาไว้สุ่มตัวเลข
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            // เอาไปใส่ใน salt
            rngCsp.GetNonZeroBytes(salt);
        }
        //Convert.ToBase64String(salt) เอามาแสดงผลโดยการเอามาแปลงเป็น string ก่อน เก็บเป็นฐาน 16
        Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            // หยอด password ไป
            password: password,
            // เอา salt มาผสมกับ password
            salt: salt,
            //KeyDerivationPrf.HMACSHA256 อันกอริทึมในการเข้ารหัส(มีหลายตัว) หรือ เป็นวิธีการเขา้รหัส *** เปลี่ยนได้
            prf: KeyDerivationPrf.HMACSHA256,
            // จำนวนรอบในการเข้ารหัส
            //จากตัวอย่างจะเอารหัสมาโม้ 100000 ครั้ง *** เปลี่ยนได้
            iterationCount: 100000,
            // อาจารย์
            numBytesRequested: 256 / 8));
        Console.WriteLine($"Hashed: {hashed}");
        // Database ที่ต้องเก็บ มี hashed , salt ;
    }
}

/*
 * SAMPLE OUTPUT
 *
 * Enter a password: Xtw9NMgx
 * Salt: CGYzqeN4plZekNC88Umm1Q==
 * Hashed: Gt9Yc4AiIvmsC1QQbe2RZsCIqvoYlst2xbz0Fs8aHnw=
 */



