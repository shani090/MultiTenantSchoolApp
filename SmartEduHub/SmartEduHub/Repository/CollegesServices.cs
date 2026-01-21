using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartEduHub.Data;
using SmartEduHub.DTO.CollageDTO;
using SmartEduHub.Interface;
using SmartEduHub.Models;

namespace SmartEduHub.Repository
{
    public class CollegesServices: IColleges
    {
        private readonly SchoolDbContext _context;
        private readonly ILogger<CollegesServices> _logger;
        private readonly IMapper _mapper;
        public CollegesServices(SchoolDbContext context, ILogger<CollegesServices> logger, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<List<College>> GetAllAsync()
        {
            try
            {
                return await _context.Colleges
                    .Where(x => x.IsActive == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching colleges list");
                throw ex;
            }
        }


        public async Task<College?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Colleges.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching college with ID {id}");
                throw ex;
            }
        }

        public async Task<string> CreateAsync(CollegeCreateDTO dto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var college = _mapper.Map<College>(dto);
                college.CreatedDate = DateTime.Now;
                _context.Colleges.Add(college);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation($"College created successfully. Code: {college.Code}");
                return "Created Sucess";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error while creating college");
                throw ex;
            }
        }

        public async Task<bool> UpdateAsync(int id,CollegeUpdateDTO dto)
        {
            try
            {
                var existing = await _context.Colleges.FindAsync(id);
                if (existing == null) return false;

                existing.Name = dto.Name ?? existing.Name;
                existing.Code = dto.Code ?? existing.Code;
                existing.Address = dto.Address ?? existing.Address;
                existing.ContactEmail = dto.ContactEmail ?? existing.ContactEmail;
                existing.ContactPhone = dto.ContactPhone ?? existing.ContactPhone;
                existing.LogoUrl = dto.LogoUrl ?? existing.LogoUrl;
                existing.IsActive = dto.IsActive;
                //existing.UpdatedBy = DateTime.Now;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"College updated successfully. ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating college ID {id}");
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var college = await _context.Colleges.FindAsync(id);
                if (college == null) return false;

                college.IsActive = false;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"College soft deleted. ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting college ID {id}");
                throw ex;
            }
        }
    }
}
