using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CaseOpener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly ITransactionService transactionService;

        public AdminController(
            IAdminService _adminService,
            ITransactionService _transactionService)
        {
            adminService = _adminService;
            transactionService = _transactionService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(string adminId)
        {
            try
            {
                var users = await adminService.GetUsersAsync(adminId);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user-transactions")]
        public async Task<IActionResult> GetUserTransactions(string adminId, string userId)
        {
            try
            {
                var transactions = await adminService.GetUserTransactionsAsync(adminId, userId);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(string adminId)
        {
            try
            {
                var transactions = await adminService.GetRolesAsync(adminId);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(string adminId, string roleName)
        {
            try
            {
                string operation = await adminService.AddRoleAsync(adminId, roleName);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole(string adminId, [FromBody] RoleModel model)
        {
            try
            {
                string operation = await adminService.EditRoleAsync(adminId, model);

                return Ok(new { Message = operation });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-transaction")]
        public async Task<IActionResult> DeleteTransaction(string adminId, int transactionId)
        {
            if(await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await transactionService.DeleteTransactionAsync(transactionId);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpPut("update-transaction")]
        public async Task<IActionResult> UpdateTransaction(string adminId, int transactionId, string status)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await transactionService.UpdateTransactionStatusAsync(transactionId, status);

                    return Ok(new { Message = operation });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpGet("transaction")]
        public async Task<IActionResult> GetTransaction(string adminId, int transactionId)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    var transaction = await transactionService.GetTransactionByIdAsync(transactionId);

                    return Ok(transaction);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }
    }
}
