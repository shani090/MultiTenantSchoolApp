using Microsoft.AspNetCore.Mvc;
using SmartEduHub.DTO.CollageDTO;
using SmartEduHub.Models;

namespace SmartEduHub.Interface
{
    public interface IColleges
    {
        Task<List<College>> GetAllAsync();
        Task<College?> GetByIdAsync(int id);
        Task<string> CreateAsync(CollegeCreateDTO dto);
        Task<bool> UpdateAsync(int id,CollegeUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
