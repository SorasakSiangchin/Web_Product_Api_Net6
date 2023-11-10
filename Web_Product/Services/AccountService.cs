using Web_Product.interfaces;
using Web_Product.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Web_Product.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Web_Product.Services
{
    public class AccountService : IAccounService
    {
        public readonly DatabaseContext DatabaseContext;
        private readonly jwtSetting jwtSetting;

        public AccountService(DatabaseContext databaseContext , jwtSetting jwtSetting)
        {
            this.DatabaseContext = databaseContext;
            this.jwtSetting = jwtSetting;
        }

        public async Task<Account> Login(string username, string password)
        {
            var account = await DatabaseContext.Accounts.Include(x => x.Role)
                .SingleOrDefaultAsync(p => p.Username == username);

            if (account != null && VerifyPassword(account.Password, password))
            {
                return account;
            }
            return null;

        }

        public async Task Register(Account account)
        {
            var result = await DatabaseContext.Accounts.SingleOrDefaultAsync(e => e.Username == account.Username);
            if (result != null) throw new Exception("Existing account");
            // นำ password ไปทำการ Hash 
            account.Password = CreateHashPassword(account.Password);
            await DatabaseContext.Accounts.AddAsync(account);
            await DatabaseContext.SaveChangesAsync();   
        }

        private string CreateHashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = HashPassword(password, salt);

            var hpw = $"{Convert.ToBase64String(salt)}.{hashed}";
            return hpw;
        }

        private bool VerifyPassword(string saltAndHashFromDB, string password)
        {
            // ทำการแยกส่วนเป็น 2 สว่น เป็นอเร
            var parts = saltAndHashFromDB.Split('.', 2);
            if (parts.Length != 2) return false;
            // ไปเอาเกลือมา
            // Convert.FromBase64String ให้กลับเหมือนเดิมปกติมันเป็นไบต์
            var salt = Convert.FromBase64String(parts[0]);

            var passwordHash = parts[1];

            string hashed = HashPassword(password, salt);

            return hashed == passwordHash;
        }

        private string HashPassword(string password ,Byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
              password: password,
              salt: salt,
              prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 100000,
              numBytesRequested: 256 / 8));
            return hashed;
        }

        public string GenerateToken(Account account)
        {
            #region PAYLOAD
            var claims = new[] {
                // Claim เป็นของเขา จะใส่อะไรก็ได้
                // ใส่ PAYLOAD 
                // JwtRegisteredClaimNames.Sub เป็น key ของเขา
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                //ตอนที่เรา login เราส่ง account.Role เข้ามา
                new Claim("role",account.Role.Name),
                new Claim("additonal","TestSomething"),
                new Claim("todo day","10/10/99"),
                new Claim("game","Rov")
            };
            #endregion
            return BuildToken(claims);
        }
        
        public Account GetInfo(string accessToken)
        {
            //as JwtSecurityToken แปลงค่า Token (ถอดรหัส)
            var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;

            // ค้นหา sub 
            var username = token.Claims.First(claim => claim.Type == "sub").Value;
            // ค้นหา role 
            var role = token.Claims.First(claim => claim.Type == "role").Value;

            var account = new Account
            {
                Username = username,
                Role = new Role
                {
                    Name = role
                }
            };

            return account;

        }

        // 
        private string BuildToken(Claim[] claims)
        { 
            // ดึงขอ้มูลเข้ามาและแปลงให้เป็นวันที่
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSetting.Expire));
            // ดึง key มาทำการเข้ารหัส
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            // จะเข้ารหัสด้วยวิธีอะไร
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //สร้าง Token *** 
            //JwtSecurityToken เป็นของเข้า
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                // หมดอายุเมื่อไร
                expires: expires,
                signingCredentials: creds
            );
            // WriteToken เขียนออกมา เป็น string 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
