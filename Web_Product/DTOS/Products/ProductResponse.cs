


namespace Web_Product.DTOS.Product

{

    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }


        // Models.Product product ส่งตัวจริงเข้ามาก่อน
        static public ProductResponse FromProduct(Models.Product product)
        {
            // return ตัวมันเอง
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Image = product.Image,
                Stock = product.Stock,
                Price = product.Price,

                CategoryName = product.Category.Name
            };
        }

    }
}
