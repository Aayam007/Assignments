using Assignment.DbContexts;
using Assignment.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Assignment.Services
{
    public class DepartmentCrudService : IDepartmentCrudService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentCrudService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department> CreateDepartmentAsync(string departmentName)
        {
            var department = new Department { DepartmentName = departmentName };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }

}
