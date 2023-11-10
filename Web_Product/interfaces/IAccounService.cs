using Web_Product.Models;

namespace Web_Product.interfaces
{
    public interface IAccounService
    {
        Task Register(Account account);
        Task<Account> Login(string username, string password);
        // ถ้า login เสร็จแล้ว ก็ส่งตั๋วไปให้   
        string GenerateToken(Account account);
        // เอาไว้แสดงข้อมูลใน Token
        Account GetInfo(string accessToken);

    }
}
