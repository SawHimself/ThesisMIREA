using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.SecuritySettings;
using System.Diagnostics;
using System.Security.Claims;
using WebApplication3.Data;
using WebApplication3.Models;
//using static System.Net.Mime.MediaTypeNames;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        private static bool test1 = false;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            bool useHtmlEscaping = _configuration.GetValue<bool>("SecurySettings:UseHtmlEscaping");
            ViewBag.UseHtmlEscaping = useHtmlEscaping;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var secrets = await _context.secrets
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return View(secrets);
        }

        [HttpPost]
        public async Task<IActionResult> AddSecret(string Data, string Description)
        {
            if (string.IsNullOrWhiteSpace(Data) || string.IsNullOrWhiteSpace(Description))
            {
                return RedirectToAction("Index");
            }

            SecretData newData = new SecretData()
            {
                Id = Guid.NewGuid().ToString(),
                Data = Data,
                Description = Description,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            newData.UserId = userId;

            _context.secrets.Add(newData);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSecret(int DataId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            //_context.secrets.Delete
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            ViewData["Test1"] = test1;
            return View();
        }

/*        [HttpPost]
        public IActionResult ToggleTest1([FromBody] ToggleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Неверные данные запроса.");
            }
            bool isChecked = request.Checked;

            if(SecurityProvider.UpdateRule("UseHtmlEscaping", isChecked))
            {
                return Ok(new { message = "Данные успешно обработаны", status = isChecked });
            }
            else
            {
                return StatusCode(500);
            }
        }*/

        public class ToggleRequest
        {
            public bool Checked { get; set; }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
