using CLDV_POE_PART_2.Models;
using CLDV_POE_PART_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_POE_PART_2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        
        public IActionResult Index()
        {
            var products = context.Products.ToList();
            return View(products);
        }


        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
            newFileName += Path.GetExtension(productDto.ImageFile.FileName);
            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            // save the new product in the database
            Product product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Category = productDto.Category,                             
                ImageFileName = newFileName,
                Availability = productDto.Availability,
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");

        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // create productDto from product
            var productDto = new ProductDto()
            {
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                Availability = product.Availability,
                
            };

            ViewData["ProductId"] = id;
            ViewData["ImageFileName"] = product.ImageFileName;

            return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if(product == null)
{
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                

                return View(productDto);
            }

            // update the image file if we have a new image file
            string newFileName = product.ImageFileName;
            if (productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }

                // delete the old image file
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the product in the database
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.ImageFileName = newFileName;
            product.Availability = productDto.Availability;

            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
    }
}
