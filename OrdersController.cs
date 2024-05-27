using System.Security.Claims;
using CLDV_POE_PART_2.Models;
using CLDV_POE_PART_2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDV_POE_PART_2.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(int productId)
        {
            // Get the current client's ID
            var user = await userManager.GetUserAsync(User);
            string clientId = user.Id;

            // Create a new order
            var order = new Order
            {
                ClientId = clientId,
                ProductId = productId,
                OrderDate = DateTime.Now
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> Index()
        {
            var orders = new List<Order>();

            if (User.IsInRole("admin"))
            {
                // Retrieve all orders for admin users
                orders = await context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Client)
                    .ToListAsync();
            }
            else
            {
                // Retrieve orders specific to the current client
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                orders = await context.Orders
                    .Where(o => o.ClientId == userId)
                    .Include(o => o.Product)
                    .ToListAsync();
            }

            return View(orders);
        }
    }
}