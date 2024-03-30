using Assignment.DbContexts;
using Assignment.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Assignment.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(e=>e.Department).Include(e=>e.EmployeeRoles).ToListAsync();
        }

        public async Task AssignEmployeeToDepartmentAsync(int employeeId, int departmentId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            employee.DepartmentId = departmentId;
            await _context.SaveChangesAsync();
        }

        public async Task AddEmployeeRoleAsync(int employeeId, int roleId)
        {
            var employeeRole = new EmployeeRole { EmployeeId = employeeId, RoleId = roleId };
            _context.EmployeeRoles.Add(employeeRole);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Role>> GetEmployeeRolesAsync(int employeeId)
        {
            var roles = await _context.EmployeeRoles
                .Where(er => er.EmployeeId == employeeId)
                .Select(er => er.Role)
                .ToListAsync();

            return roles;
        }
    }

}
