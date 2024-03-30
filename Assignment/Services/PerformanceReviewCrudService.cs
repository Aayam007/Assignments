using Assignment.DbContexts;
using Assignment.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Assignment.Services
{
    public class PerformanceReviewCrudService : IPerformanceReviewCrudService
    {
        private readonly ApplicationDbContext _context;

        public PerformanceReviewCrudService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PerformanceReview> CreatePerformanceReviewAsync(int employeeId, string review)
        {
            var performanceReview = new PerformanceReview { EmployeeId = employeeId, Review = review };
            _context.PerformanceReviews.Add(performanceReview);
            await _context.SaveChangesAsync();
            return performanceReview;
        }

        public async Task<List<PerformanceReview>> GetAllPerformanceReviewsAsync()
        {
            return await _context.PerformanceReviews.ToListAsync();
        }

        public async Task<List<PerformanceReview>> GetPerformanceReviewsByEmployeeIdAsync(int employeeId)
        {
            return await _context.PerformanceReviews.Where(pr => pr.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<PerformanceReview> GetPerformanceReviewByIdAsync(int id)
        {
            return await _context.PerformanceReviews.FindAsync(id);
        }

        public async Task UpdatePerformanceReviewAsync(PerformanceReview performanceReview)
        {
            _context.Entry(performanceReview).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePerformanceReviewAsync(int id)
        {
            var performanceReview = await _context.PerformanceReviews.FindAsync(id);
            _context.PerformanceReviews.Remove(performanceReview);
            await _context.SaveChangesAsync();
        }
    }
}
