using Lab3.Models;
using Lab3.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class OrderManagerController : Controller
    {
        private readonly IOderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        public OrderManagerController(IOderRepository orderRepository, IOrderDetailsRepository orderDetailsRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderDetailsRepository.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.UtcNow;
                await _orderRepository.CreateAsync(order);
                return RedirectToAction("Index", "Home");
            }
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderRepository.DeleteAsync(id);
            if (!result)
                return NotFound();

            return RedirectToAction("Index", "Home"); 
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }
    }
}
