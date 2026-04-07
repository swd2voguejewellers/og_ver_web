using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestSPA.Interfaces;
using TestSPA.Models;
using TestSPA.ViewModel;

namespace TestSPA.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOGRepository _OGRepository;

        public HomeController(IOGRepository oGRepository)
        {
            _OGRepository = oGRepository;
    
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateOG([FromBody] OGVerificationVM model)
        {
            if (model == null || model.Details == null || !model.Details.Any())
                return BadRequest("No verification details provided.");

            var result = await _OGRepository.SaveOrUpdateAsync(model);
            if (result.success)
                return Ok(new { success = true, message = result.ogVerifyNo.ToString() });
            else
                return StatusCode(500, new { success = false, message = "Save failed." });
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestRates()
        {
            var rates = await _OGRepository.GetLatestRatesAsync();
            if (rates == null)
                return NotFound();
            return Ok(rates);
        }

        [HttpGet]
        public async Task<IActionResult> GetOGDetails(int ogVerifyNo)
        {
            if (ogVerifyNo <= 0)
                return BadRequest("Invalid verification number.");

            var details = await _OGRepository.GetOGVerificationsByNumberAsync(ogVerifyNo);
            if (details == null || !details.Any())
                return NotFound();

            return Ok(details);
        }

        [HttpGet]
        public async Task<IActionResult> CalculateActVal(decimal? weight, decimal? carat)
        {
            var result = await _OGRepository.CalculateActValueAsync(weight, carat);
            return Ok(result); // returns null or decimal
        }

        [HttpGet]
        public async Task<IActionResult> GetStaff()
        {
            var staff = await _OGRepository.GetActiveStaffAsync();
            return Ok(staff);
        }
    }
}
