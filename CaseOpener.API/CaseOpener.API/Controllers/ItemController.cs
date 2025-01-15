using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Item;
using CaseOpener.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaseOpener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IItemService itemService;

        public ItemController(
            IAdminService _adminService,
            IItemService _itemService)
        {
            adminService = _adminService;
            itemService = _itemService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetItems(string adminId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                var items = await itemService.GetItemsAsync();

                return Ok(items);
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem(string adminId, [FromBody] ItemFormModel model)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await itemService.AddItemAsync(model);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpPut("update-item")]
        public async Task<IActionResult> UpdateItem(string adminId, [FromBody] ItemFormModel model)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await itemService.EditItemAsync(model);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpDelete("delete-item")]
        public async Task<IActionResult> DeleteItem(string adminId, int itemId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await itemService.DeleteItemAsync(itemId);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpGet("get-item")]
        public async Task<IActionResult> GetItem(int itemId)
        {
            try
            {
                var item = await itemService.GetItemByIdAsync(itemId);

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-item-inventory")]
        public async Task<IActionResult> AddItemToInventory([FromBody] InventoryItemModel model)
        {
            string operation = await itemService.AddItemToInventoryAsync(model);

            return Ok(new { Message = operation });
        }

        [HttpDelete("remove-item")]
        public async Task<IActionResult> RemoveItemFromInventory(int id, string userId) 
        {
            try
            {
                string operation = await itemService.RemoveItemFromInventoryAsync(id, userId);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-inventory-item")]
        public async Task<IActionResult> GetInventoryItem(int id, string userId) 
        {
            try
            {
                var inventoryItem = await itemService.GetInventoryItemByIdAsync(id, userId);

                return Ok(inventoryItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-items")]
        public async Task<IActionResult> GetUserItems(string userId)
        {
            var items = await itemService.GetUserInventoryItemsAsync(userId);

            return Ok(items);
        }
    }
}
