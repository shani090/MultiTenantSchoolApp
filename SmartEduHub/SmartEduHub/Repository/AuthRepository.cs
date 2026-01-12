using SmartEduHub.Data;
using SmartEduHub.DTO;
using SmartEduHub.Interface;
using SmartEduHub.Models;

namespace SmartEduHub.Repository
{
    public class AuthRepository: IUser
    {
        private readonly SchoolDbContext _context;
        public AuthRepository(SchoolDbContext context)
        {
            _context = context;
        }
        

        public Task<User?> LoginAsync(LoginDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(RegisterDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
