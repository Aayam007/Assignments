using Assignment.Model;

namespace Assignment.Services
{
    public interface IPerformanceReviewCrudService
    {
        Task<PerformanceReview> CreatePerformanceReviewAsync(int employeeId, string review);
        Task DeletePerformanceReviewAsync(int id);
        Task<List<PerformanceReview>> GetAllPerformanceReviewsAsync();
        Task<PerformanceReview> GetPerformanceReviewByIdAsync(int id);
        Task<List<PerformanceReview>> GetPerformanceReviewsByEmployeeIdAsync(int employeeId);
        Task UpdatePerformanceReviewAsync(PerformanceReview performanceReview);
    }
}