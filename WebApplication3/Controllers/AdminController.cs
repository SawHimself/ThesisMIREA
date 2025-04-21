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

        [HttpPost("ChangeHtmlEscaping")]
        public IActionResult ChangeHtmlEscaping([FromBody] ToggleRequest? request)
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
        
        [HttpPost("ChangeSqlEscaping")]
        public IActionResult ChangeSqlEscaping([FromBody] ToggleRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Неверные данные запроса.");
            }
            bool isChecked = request.Checked;

            if (SecurityProvider.UpdateRule("UseSQLEscaping", isChecked))
            {
                return Ok(new { message = "Данные успешно обработаны", status = isChecked });
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPost("ChangeXFrameOptions")]
        public IActionResult ChangeXFrameOptions([FromBody] ToggleRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Неверные данные запроса.");
            }
            bool isChecked = request.Checked;

            if (SecurityProvider.UpdateRule("UseXFrameOptions", isChecked))
            {
                return Ok(new { message = "Данные успешно обработаны", status = isChecked });
            }
            else
            {
                return StatusCode(500);
            }
        }

        /*        
        [HttpGet("test")]
        public IActionResult Test()
        {
            return StatusCode(200, "Нашёл!");
        }
        */

        public class ToggleRequest
        {
            public bool Checked { get; set; }
        }
    }
}
