using Karshare.API.Models;

namespace Karshare.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByUserName(string username);
        Task<User> Create(User user);
        Task<User> Update(string mail, User user);
        Task Delete(string mail);
    }
}
