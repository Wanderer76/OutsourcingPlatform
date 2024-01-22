using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutsourcePlatformApp.Dto;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Service;

namespace OutsourcePlatformApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalAreaController : ControllerBase
    {
        private readonly PersonalAreaService personalAreaService;

        public PersonalAreaController(PersonalAreaService personalAreaService)
        {
            this.personalAreaService = personalAreaService;
        }

        [Authorize(Roles = "executor_role")]
        [HttpGet("executor/area")]
        public async Task<IActionResult> GetExecutorPersonalArea([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                var result = await personalAreaService.GetPersonalArea(token);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "customer_role")]
        [HttpGet("customer/area")]
        public async Task<IActionResult> GetCustomerPersonalArea([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                var result = await personalAreaService.GetPersonalArea(token);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("executor/update")]
        public async Task<IActionResult> UpdateExecutorCommonInfo([FromHeader(Name = "Authorization")] string token,
            [FromBody] ExecutorObject executorObject)
        {
            try
            {
                var updatedExecutor = await personalAreaService.UpdateExecutor(executorObject, token);
                return await Task.Run(() => Ok(updatedExecutor));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "customer_role")]
        [HttpPost("customer/update")]
        public async Task<IActionResult> UpdateCustomerCommonInfo([FromHeader(Name = "Authorization")] string token,
            [FromBody] CustomerUpdateDto customer)
        {
            try
            {
                var updatedCustomer = await personalAreaService.EditCustomer(customer, token);
                return await Task.Run(() => Ok(updatedCustomer));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("categories/update")]
        public async Task<IActionResult> UpdateCategories([FromHeader(Name = "Authorization")] string token,
            [FromBody] List<CategoryDto> categories)
        {
            try
            {
                var updatedCategories = await personalAreaService.EditCategories(categories, token);
                return await Task.Run(() => Ok(updatedCategories));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("skills/update")]
        public async Task<IActionResult> UpdateSkills([FromHeader(Name = "Authorization")] string token,
            [FromBody] List<SkillDto> skills)
        {
            try
            {
                var updatedSkills = await personalAreaService.EditSkills(skills, token);
                return await Task.Run(() => Ok(updatedSkills));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "customer_role,executor_role")]
        [HttpPost("contacts/update")]
        public async Task<IActionResult> UpdateUserContacts([FromHeader(Name = "Authorization")] string token,
            [FromBody] ContactsUpdateDto contacts)
        {
            try
            {
                var updatedContacts = await personalAreaService.UpdateContacts(contacts, token);
                return Ok(updatedContacts);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "executor_role")]
        [HttpPost("educations/update")]
        public async Task<IActionResult> UpdateEducations([FromHeader(Name = "Authorization")] string token,
            [FromBody] List<EducationUpdateDto> educations)
        {
            try
            {
                var updatedEducations = await personalAreaService.UpdateEducations(educations, token);
                return await Task.Run(() => Ok(updatedEducations));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("contacts/list")]
        public IActionResult GetContactsList()
        {
            var result = new List<string>()
            {
                "Вконтакте","Телеграм","Github","Gitlab","Pinterest"
            };
            return Ok(result);
        }
    }
}