using Assignment.DbContexts;
using Assignment.Model;
using Assignment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Assignment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
           .ConfigureServices((context, services) =>
           {
               services.AddSingleton<ProgramLogic>();
               services.AddDbContext<ApplicationDbContext>();
               services.AddTransient<IRoleCrudService, RoleCrudService>();
               services.AddTransient<IDepartmentCrudService, DepartmentCrudService>();
               services.AddTransient<IPerformanceReviewCrudService, PerformanceReviewCrudService>();
               services.AddScoped<IEmployeeService, EmployeeService>();
           })
           .Build();
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var programLogic = scope.ServiceProvider.GetRequiredService<ProgramLogic>();
                await programLogic.Run(scope,dbContext);
            }
        }
    }
}
