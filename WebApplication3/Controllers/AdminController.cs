using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.SecuritySettings;
using System.Text.Json;

namespace WebApplication3.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {

        [HttpPost("ToggleTest1")]
        public IActionResult ToggleTest1([FromBody] ToggleRequest request)
        {
            if (request == null)
            {
                return BadRequest("Неверные данные запроса.");
            }
            bool isChecked = request.Checked;

            if (SecurityProvider.UpdateRule("UseHtmlEscaping", isChecked))
            {
                return Ok(new { message = "Данные успешно обработаны", status = isChecked });
            }
            else
            {
                return StatusCode(500);
            }
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return StatusCode(200, "Нашёл!");
        }

        public class ToggleRequest
        {
            public bool Checked { get; set; }
        }
    }
}
