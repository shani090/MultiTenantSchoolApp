using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartEduHub.DTO.CollageDTO;
using SmartEduHub.Interface;
using SmartEduHub.Models;

namespace SmartEduHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegesController : ControllerBase
    {
        private readonly IColleges _colleges;
        private readonly ILogger<CollegesController> _logger;
        public CollegesController(IColleges colleges, ILogger<CollegesController> logger) 
        {
        _colleges = colleges;
        _logger = logger;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _colleges.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll Colleges API");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("GetBy{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var college = await _colleges.GetByIdAsync(id);
                if (college == null)
                    return NotFound("College not found");

                return Ok(college);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetById API. ID: {id}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost("CreateCollege")]
        public async Task<IActionResult> Create(CollegeCreateDTO dto)
        {
            try
            {
                var created = await _colleges.CreateAsync(dto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create College API");
                return StatusCode(500, "Unable to create college");
            }
        }

        [HttpPut("UpdateCollege{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CollegeUpdateDTO dto)
        {
            try
            {
                var updated = await _colleges.UpdateAsync(id, dto);
                if (!updated)
                    return NotFound("College not found");

                return Ok("Updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Update API. ID: {id}");
                return StatusCode(500, "Unable to update college");
            }
        }

        [HttpDelete("SoftDelete{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _colleges.DeleteAsync(id);
                if (!deleted)
                    return NotFound("College not found");

                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Delete API. ID: {id}");
                return StatusCode(500, "Unable to delete college");
            }
        }
    }
}
