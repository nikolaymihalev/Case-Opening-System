using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Case;
using CaseOpener.Core.Models.Item;
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

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchedCases(string name)
        {
            var cases = await caseService.GetAllCasesAsync(name);

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

        [HttpPost("add-case-item")]
        public async Task<IActionResult> AddCase(string adminId, int caseId, int itemId, double probability)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                string operation = await caseService.AddItemToCaseAsync(caseId, itemId, probability);

                return Ok(new { Message = operation });
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

                var inventoryItem = new InventoryItemModel()
                {
                    UserId = userId,
                    ItemId = openedItem.Id
                };

                string operation = await itemService.AddItemToInventoryAsync(inventoryItem);

                if(operation == string.Format(ReturnMessages.SuccessfullyAdded, "item to inventory"))
                {
                    return Ok(openedItem);
                }

                throw new ArgumentException(ReturnMessages.OperationFailed);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("item-probabilities")]
        public async Task<IActionResult> GetCaseItemProbabilities(int caseId)
        {
            var items = await caseService.GetCaseItemsProbabilities(caseId);

            return Ok(items);
        }

        [HttpGet("user-has-case")]
        public async Task<IActionResult> UserHasCase(int caseId, string userId) 
        {
            int result = await caseService.DoesUserHaveCaseAsync(userId, caseId);

            return Ok(result);
        }

        [HttpPost("buy-case")]
        public async Task<IActionResult> BuyCase(int caseId, string userId, int? quantity)
        {
            try
            {
                int quantityC = 1;

                if (quantity != null)
                {
                    quantityC = (int)quantity;
                }

                string operation = await caseService.BuyCaseAsync(caseId, userId, quantityC);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("bought-cases")]
        public async Task<IActionResult> GetBoughtCases(string userId)
        {
            try
            {
                var cases = await caseService.GetUsersCasesAsync(userId);

                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-opened-cases")]
        public async Task<IActionResult> GetUserOpenedCases(string userId)
        {
            try
            {
                var cases = await caseService.GetUserOpenedCasesAsync(userId);

                return Ok(cases);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
