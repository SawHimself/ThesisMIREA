using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.SecuritySettings;
using System.Text.Json;
using Services.ProcessingTime;

namespace WebApplication3.Controllers
{
    
    [Route("api/admin")]
    [ApiController]
    public class AdminController(IRequestTimingService timingService) : Controller
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

        [HttpPost("ChangeCorsOptions")]
        public IActionResult ChangeCorsOptions([FromBody] ToggleRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Неверные данные запроса.");
            }

            bool isChecked = request.Checked;

            if (SecurityProvider.UpdateRule("UseCORS", isChecked))
            {
                return Ok(new { message = "Данные успешно обработаны", status = isChecked });
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPost("ChangeCspOptions")]
        public IActionResult ChangeCspOptions([FromBody] ToggleRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Неверные данные запроса.");
            }

            bool isChecked = request.Checked;

            if (SecurityProvider.UpdateRule("UseCSP", isChecked))
            {
                return Ok(new { message = "Данные успешно обработаны", status = isChecked });
            }
            else
            {
                return StatusCode(500);
            }
        }

        public class ToggleRequest
        {
            public bool Checked { get; set; }
        }
    }
}