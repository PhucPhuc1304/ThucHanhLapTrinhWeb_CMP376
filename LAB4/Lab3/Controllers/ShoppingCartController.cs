using Lab3.Extensions;
using Lab3.Models;
using Lab3.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Lab3.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {


        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await GetProductFromDatabase(productId);


            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                Image = product.ImageUrl
            };
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);


            HttpContext.Session.SetObjectAsJson("Cart", cart);


            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            decimal totalPrice = cart.Items.Sum(item => item.Price * item.Quantity);

            ViewBag.TotalPrice = totalPrice;
            return View(cart);
        }
        // Các actions khác...
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }


        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");


            if (cart is not null)
            {
                cart.RemoveItem(productId);


                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }


            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            return View(new Order());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return RedirectToAction("Index");
            }

            // Gán thông tin người dùng vào đơn hàng
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.ApplicationUser = user; 
            // Tạo các chi tiết đơn hàng từ giỏ hàng
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            // Thêm đơn hàng vào cơ sở dữ liệu và lưu thay đổi
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Xóa giỏ hàng từ Session sau khi đã thanh toán thành công
            HttpContext.Session.Remove("Cart");

            // Chuyển hướng người dùng đến trang hoàn thành đơn hàng
            return View("OrderCompleted", order.Id);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            if (cart != null)
            {
                var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);

                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;

                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                }
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index", "Home");
        }

    }
}