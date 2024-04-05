using Lab3.Models;
using Lab3.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductManagerController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductManagerController(IProductRepository productRepository,
       ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }


        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Lưu hình ảnh vào thư mục wwwroot/images
                var fileName = file.FileName;
                var filePath = Path.Combine("wwwroot", "images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                // Trả về đường dẫn hình ảnh
                return "/images/" + fileName;
            }
            return null;
        }


        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
    }
}
