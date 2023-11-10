using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Product.DTOS.Account;
using Web_Product.interfaces;
using Web_Product.Models;

namespace Web_Product.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccounService accountService;

        public AccountController(IAccounService accountService)
        {
            this.accountService = accountService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest registerRequest)
        {
            var account = registerRequest.Adapt<Account>();
            await accountService.Register(account);
            return Ok(new {msg = "OK"});
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            var account = await accountService.Login(loginRequest.Username, loginRequest.Password);
            if (account == null)
            {
                return NotFound();
            }
            var token = accountService.GenerateToken(account);
            //return Ok(new { msg = "OK", token = token });
            var resultToken = accountService.GetInfo(token);
            return Ok(new { token=token, resultToken=resultToken });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Info()
        {
            //อ่านค่า Token (คล้ายๆ การอ่าน session)
            // HttpContext เป็น class ทำการ get token ขึ้นมา
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) return Unauthorized();

            var account = accountService.GetInfo(accessToken);
            return Ok(new
            {
                username = account.Username,
                role = account.Role.Name
            });
        }
    }
}
