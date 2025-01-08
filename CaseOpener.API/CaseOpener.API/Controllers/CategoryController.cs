using CaseOpener.Core.Constants;
using CaseOpener.Core.Contracts;
using CaseOpener.Core.Models.Case;
using Microsoft.AspNetCore.Mvc;

namespace CaseOpener.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly ICategoryService categoryService;

        public CategoryController(
            IAdminService _adminService,
            ICategoryService _categoryService)
        {
            adminService = _adminService;
            categoryService = _categoryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await categoryService.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("get-category")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await categoryService.GetByIdAsync(id);

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddCategory(string adminId, [FromBody] CategoryFormModel model)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                string operation = await categoryService.AddAsync(model);

                return Ok(new { Message = operation });
            }

            return BadRequest(new { Message = ReturnMessages.Unauthorized });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(string adminId, [FromBody] CategoryFormModel model)
        {
            if (await adminService.CheckUserIsAdmin(adminId))
            {
                try
                {
                    string operation = await categoryService.EditAsync(model);

                    return Ok(new { Message = operation });
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
