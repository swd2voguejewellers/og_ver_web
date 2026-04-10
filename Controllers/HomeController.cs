using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSPA.Interfaces;
using TestSPA.Models;
using TestSPA.ViewModel;

namespace TestSPA.Controllers
{
    [Authorize]
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

            if (model.OGVerifyNo.HasValue && await _OGRepository.IsApprovedAsync(model.OGVerifyNo.Value))
                return Conflict(new { success = false, message = "Approved records cannot be edited." });

            var result = await _OGRepository.SaveOrUpdateAsync(model);
            if (result.success)
                return Ok(new { success = true, message = result.ogVerifyNo.ToString() });
            else
                return StatusCode(500, new { success = false, message = "Save failed." });
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOG([FromBody] OGVerificationVM model)
        {
            if (!string.Equals(User.FindFirstValue("user_type"), "MGR", StringComparison.OrdinalIgnoreCase))
                return Forbid();

            if (model == null || model.Details == null || !model.Details.Any())
                return BadRequest("No verification details provided.");

            if (model.OGVerifyNo.HasValue && await _OGRepository.IsApprovedAsync(model.OGVerifyNo.Value))
                return Conflict(new { success = false, message = "Approved records cannot be edited." });

            if (model.OGVerifyNo.HasValue && !await _OGRepository.CanApproveBranchAsync(model.OGVerifyNo.Value))
                return StatusCode(403, new { success = false, message = "You cannot approve records from another branch." });

            var result = await _OGRepository.ApproveAsync(model);
            if (result.success)
                return Ok(new { success = true, message = result.ogVerifyNo.ToString() });

            return StatusCode(500, new { success = false, message = "Approval failed." });
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

            var isApproved = await _OGRepository.IsApprovedAsync(ogVerifyNo);
            return Ok(new { details, isApproved });
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
