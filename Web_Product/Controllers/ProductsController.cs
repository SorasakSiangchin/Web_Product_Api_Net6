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
    // ApiController ��Ǩ�ͺ��觷�����¡��������ͧ��
    // ControllerBase �����褹�е�ǡѺ MVC
    // Http��ҧ� ��� ��ⵤ� (��͵�ŧ)

    [Authorize] // ��ͧ��� token ;
    public class ProductsController : ControllerBase
    {
        public readonly DatabaseContext db;
        public readonly IProductService ps;
        public ProductsController(DatabaseContext databaseContext , IProductService productService)
        {
            db = databaseContext;
            ps = productService;
        }
        // ("[action]") ��Ҩ������ method ;
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
        } // Ok �� Class �ͧ��

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
        // [FromBody] ��� Json 
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductRequest productRequest)
        {
                 
             (string erorrMesage , string imageName) = await ps.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            // return product;
            // �����������Ƿ�� 3 �ʴ����

            //------- ����� MVC ���ռ�
            //ModelState.Remove("Image");
            //if (!ModelState.IsValid) return BadRequest();
            //---------------------

            // �ŧ productRequest �� Product ���е͹ Save to dataBase ��ͧ�� Type �� Product
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

            // BadRequest ��Ѻ�
            if (id != productRequest.Id) return BadRequest();
            var result = await ps.FindById(id);
            // ��ҹ��������������� Update ��ͧ���ѹ��� *************
            //var result = db.Products.AsNoTracking().FirstOrDefault(e => e.Id.Equals(id));

            if (result == null) return NotFound();

            //result.Name = product.Name;
            //result.Price = product.Price;
            //result.Stock = product.Stock;

            #region �Ѵ����ٻ�Ҿ
            (string erorrMesage, string imageName) = await ps.UploadImage(productRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            if (!string.IsNullOrEmpty(imageName))
            {
                await ps.DeleteImage(result.Image);
                result.Image = imageName;
            }
            #endregion

            // ��Ҥ�Ңͧ productRequest �ҷѺ result
            var product = productRequest.Adapt(result);
            await ps.Update(product);
            return Ok(product);

        }

        [HttpDelete]
        // [FromQuery] int id ��������
        public async Task<ActionResult<Product>> DeleteProduct([FromQuery] int id)
        {
            var result = await ps.FindById(id);
            if (result == null) return NotFound();
            await ps.Delete(result);
            await ps.DeleteImage(result.Image);
            //if (id != 9) return NotFound();
            // NoContent() �������������� 
            return NoContent();
        }

    }
}