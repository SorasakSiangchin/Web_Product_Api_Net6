using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Product.DTOS.Product;
using Web_Product.interfaces;
using Web_Product.Models;

namespace Web_Product.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // ApiController ตรวจสอบสิ่งที่เรียกเขา้มาเบื้องต้น
    // ControllerBase ตัวแม่คนละตัวกับ MVC
    // Httpต่างๆ คือ โปรโตคอ (ข้อตกลง)

    [Authorize] // ต้องการ token ;
    public class ProductsController : ControllerBase
    {
        public readonly DatabaseContext db;
        public readonly IProductService ps;
        public ProductsController(DatabaseContext databaseContext , IProductService productService)
        {
            db = databaseContext;
            ps = productService;
        }
        // ("[action]") ถ้าจะใช้ชื่อ method ;
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Enumerable.Range(1, 5).Select(index => $"Hello index {index}").ToList();
        }

        [HttpGet("[action]")]
        //r => new
        //{
        //    id = r.Id,
        //    name = r.Name,
        //    price = r.Price,
        //    stock = r.Stock,
        //    cal = r.Price* r.Stock,
        //    categoryName = r.Category.Name
        //}
        public async Task<IActionResult> GetProduct()
        {  
            return Ok((await ps.FindAll()).Select(ProductResponse.FromProduct));
        }

        // [action]/ 
        [HttpGet("{id}")]
        //[Authorize(Roles = "Cutomer,User")]
        public async Task<IActionResult> GetProductByID(int id)
        {
            // => Ok(db.Products.Include(e => e.Category).Select(ProductResponse.FromProduct).FirstOrDefault(x => x.ProductId.Equals(id)));
           
            var data = await ps.FindById(id);
            return Ok(ProductResponse.FromProduct(data));
        } // Ok เป็น Class ของเขา

        //[HttpGet("{id}/search")] // product/99/search?name=555555 ;
        //public IActionResult GetProductByName(int id ,[FromQuery] string name)
        [HttpGet("search")] // product/99/search?name=555555 ;
        public async Task<IActionResult> GetProductByName([FromQuery] string name)
        {
            // Contains
            //var data = db.Products.Where(p => p.Name.Contains(name)).ToList();
            var result = await ps.Search(name);
            return Ok((result).Select(ProductResponse.FromProduct));
        }

        [HttpPost]
        // [FromBody] คือ Json 
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest)
        {
                 
             (string erorrMesage , string imageName) = await ps.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            // return product;
            // พารามิเตอร์ตัวที่ 3 แสดงค่า

            //------- ถ้าเป็น MVC จะมีผล
            //ModelState.Remove("Image");
            //if (!ModelState.IsValid) return BadRequest();
            //---------------------

            // แปลง productRequest เป็น Product เพราะตอน Save to dataBase ต้องใช้ Type เป็น Product
            var product = productRequest.Adapt<Product>();
            product.Image = imageName;
            await ps.Create(product);
            // redirect
            //return CreatedAtAction(nameof(GetProductByID),new {Id = 999},product);
            return CreatedAtAction(nameof(AddProduct), product);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromForm] ProductRequest productRequest)
        {

            // BadRequest กลับไป
            if (id != productRequest.Id) return BadRequest();
            var result = await ps.FindById(id);
            // ใช้งานเสร็จแล้วไปได้แล้ว Update ต้องใช้อันนี้ *************
            //var result = db.Products.AsNoTracking().FirstOrDefault(e => e.Id.Equals(id));

            if (result == null) return NotFound();

            //result.Name = product.Name;
            //result.Price = product.Price;
            //result.Stock = product.Stock;

            #region จัดการรูปภาพ
            (string erorrMesage, string imageName) = await ps.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            if (!string.IsNullOrEmpty(imageName))
            {
                await ps.DeleteImage(result.Image);
                result.Image = imageName;
            }
            #endregion

            // เอาค่าของ productRequest มาทับ result
            var product = productRequest.Adapt(result);
            await ps.Update(product);
            return Ok(product);

        }

        [HttpDelete]
        // [FromQuery] int id ใส่เต็มยศ
        public async Task<ActionResult<Product>> DeleteProduct([FromQuery] int id)
        {
            var result = await ps.FindById(id);
            if (result == null) return NotFound();
            await ps.Delete(result);
            await ps.DeleteImage(result.Image);
            //if (id != 9) return NotFound();
            // NoContent() ไม่ได้ส่งอะไรเลย 
            return NoContent();
        }

    }
}