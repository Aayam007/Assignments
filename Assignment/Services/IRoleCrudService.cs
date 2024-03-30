using Assignment.Model;

namespace Assignment.Services
{
    public interface IRoleCrudService
    {
        Task<Role> CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(int id);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task UpdateRoleAsync(Role role);
    }
}