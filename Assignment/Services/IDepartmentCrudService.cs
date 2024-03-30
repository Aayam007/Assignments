using Assignment.Model;

namespace Assignment.Services
{
    public interface IDepartmentCrudService
    {
        Task<Department> CreateDepartmentAsync(string departmentName);
        Task DeleteDepartmentAsync(int id);
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task UpdateDepartmentAsync(Department department);
    }
}