using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Case;
using CaseOpener.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaseOpener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService caseService;
        private readonly IAdminService adminService;
        private readonly IItemService itemService;

        public CaseController(
            ICaseService _caseService,
            IAdminService _adminService,
            IItemService _itemService)
        {
            adminService = _adminService;
            caseService = _caseService;
            itemService = _itemService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCases()
        {
            var cases = await caseService.GetAllCasesAsync();

            return Ok(cases);
        }

        [HttpGet("get-case")]
        public async Task<IActionResult> GetCase(int id)
        {
            try
            {
                var caseModel = await caseService.GetCaseByIdAsync(id);

                return Ok(caseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCase(string adminId, [FromBody] CaseFormModel model) 
        {
            if(await adminService.CheckUserIsAdmin(adminId))
            {
                string operation = await caseService.AddCaseAsync(model);

                return Ok(new { Message = operation });
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCase(string adminId, [FromBody] CaseFormModel model)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await caseService.EditCaseAsync(model);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCase(string adminId, int caseId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await caseService.DeleteCaseAsync(caseId);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpPost("open")]
        public async Task<IActionResult> OpenCase(int caseId, string userId) 
        {
            try
            {
                var openedItem = await caseService.OpenCaseAsync(caseId, userId);

                var inventoryItem = new InventoryItem()
                {
                    UserId = userId,
                    ItemId = openedItem.Id
                };

                string operation = await itemService.AddItemToInventoryAsync(inventoryItem);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("claim-daily-reward")]
        public async Task<IActionResult> ClaimDailyReward(string userId)
        {
            try
            {
                var openedItem = await caseService.OpenDailyRewardAsync(userId);

                var inventoryItem = new InventoryItem()
                {
                    UserId = userId,
                    ItemId = openedItem.Id
                };

                string operation = await itemService.AddItemToInventoryAsync(inventoryItem);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-has-case")]
        public async Task<IActionResult> UserHasCase(int caseId, string userId) 
        {
            bool result = await caseService.DoesUserHaveCase(userId, caseId);

            return Ok(new { Message = result.ToString() });
        }
    }
}
