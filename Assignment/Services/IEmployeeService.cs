using Assignment.Model;

namespace Assignment.Services
{
    public interface IEmployeeService
    {
        Task AddEmployeeRoleAsync(int employeeId, int roleId);
        Task AssignEmployeeToDepartmentAsync(int employeeId, int departmentId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<List<Role>> GetEmployeeRolesAsync(int employeeId);
    }
}