using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseOpener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService _adminService)
        {
            adminService = _adminService;
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
    }
}
