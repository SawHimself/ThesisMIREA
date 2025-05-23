using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.SecuritySettings;
using System.Diagnostics;
using System.Security.Claims;
using Services.ProcessingTime;
using WebApplication3.Data;
using WebApplication3.Models;
using Services.ProcessingTime;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        private static bool test1 = false;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IConfiguration configuration, IRequestTimingService timingService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _timingService = timingService;
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
        public async Task<IActionResult> AddSecret(string? data, string? description)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (SecurityProvider.GetRule("MagicQuotes"))
            {
                data = data?.Replace("'", "\\'").Replace("\"", "\\\"");

                description = description?.Replace("'", "\\'").Replace("\"", "\\\"");
            }
            
            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Index");
            }

            SecretData newData = new SecretData
            {
                Id = Guid.NewGuid().ToString(),
                Data = data ?? "",
                Description = description,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UserId = userId
            };

            _context.secrets.Add(newData);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSecret(string dataId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentData = await _context.secrets.FirstOrDefaultAsync(s => s.Id == dataId & s.UserId == userId);
            if(currentData != null)
            {
                _context.secrets.Remove(currentData);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }

        private readonly IRequestTimingService _timingService;
        public IActionResult Privacy()
        {            
            ViewData["Test1"] = test1;
            var timings = _timingService.GetLatest();
            return View(timings);
        }

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
