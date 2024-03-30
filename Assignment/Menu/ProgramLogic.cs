using Assignment.DbContexts;
using Assignment.Model;
using Assignment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Assignment
{
    public class ProgramLogic
    {
        private readonly ILogger<ProgramLogic> _logger;
        private readonly IRoleCrudService _roleService;
        private readonly IDepartmentCrudService _departmentService;
        private readonly IPerformanceReviewCrudService _performanceReviewService;
        private readonly IEmployeeService _employeeService;

        public ProgramLogic(ILogger<ProgramLogic> logger, IRoleCrudService roleService, IDepartmentCrudService departmentService, IPerformanceReviewCrudService performanceReviewService, IEmployeeService employeeService)
        {
            _logger = logger;
            _roleService = roleService;
            _departmentService = departmentService;
            _performanceReviewService = performanceReviewService;
            _employeeService = employeeService;
        }

        public async Task Run(IServiceScope scope, ApplicationDbContext dbContext)
        {
            try
            {
                _logger.LogInformation("Starting application...");
                await dbContext.Database.MigrateAsync();
                var roleService = scope.ServiceProvider.GetRequiredService<IRoleCrudService>();
                var departmentService = scope.ServiceProvider.GetRequiredService<IDepartmentCrudService>();
                var performanceReviewService = scope.ServiceProvider.GetRequiredService<IPerformanceReviewCrudService>();
                var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                await SeedRolesAsync(roleService);
                await SeedDepartmentsAsync(departmentService);
                await SeedEmployeeAsync(employeeService);
                _logger.LogInformation("Application initialized successfully.");

                Console.WriteLine("Welcome to the application!");

                string correctUsername = "admin";
                string correctPassword = "admin123";

                Console.Write("Enter your username: ");
                string username = Console.ReadLine();

                Console.Write("Enter your password: ");
                string password = ReadPassword();

                if (username == correctUsername && password == correctPassword)
                {
                    Console.WriteLine("Login successful!");
                    while (true)
                    {
                        Console.WriteLine("Welcome to Management System");
                        Console.WriteLine("1. Employee Management");
                        Console.WriteLine("2.Employee Role Management");
                        Console.WriteLine("3. Department Management");
                        Console.WriteLine("4. Performance Review Management");
                        Console.WriteLine("5. Generate Performance Report");
                        Console.WriteLine("6. Exit");
                        Console.Write("Please enter your choice: ");

                        string input = Console.ReadLine();
                        int choice;
                        if (int.TryParse(input, out choice))
                        {
                            switch (choice)
                            {
                                case 1:
                                    await ManageEmployees(scope.ServiceProvider.GetService<IEmployeeService>());
                                    break;
                                case 2:
                                    await ManageEmployeeRoles(scope.ServiceProvider.GetService<IEmployeeService>(), scope.ServiceProvider.GetService<IRoleCrudService>());
                                    break;
                                case 3:
                                    await ManageEmployeeDepartment(scope.ServiceProvider.GetService<IEmployeeService>(), scope.ServiceProvider.GetService<IDepartmentCrudService>());
                                    break;
                                case 4:
                                    await ManageEmployeePerformanceReview(scope.ServiceProvider.GetService<IPerformanceReviewCrudService>(), scope.ServiceProvider.GetService<IEmployeeService>());
                                    break;
                                case 5:
                                    GeneratePerformanceReport(scope.ServiceProvider.GetService<IPerformanceReviewCrudService>(), scope.ServiceProvider.GetService<IEmployeeService>());
                                    break;
                                case 6:
                                    Console.WriteLine("Exiting...");
                                    return;
                                default:
                                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during application startup: {Message}", ex.Message);

                Console.WriteLine("An error occurred during application startup: " + ex.Message);
            }
        }

        #region Generating Report Part
        static async Task GeneratePerformanceReport(IPerformanceReviewCrudService performanceReviewService, IEmployeeService employeeService)
        {
            try
            {
                Console.WriteLine("Generating performance report...");

                var employees = await employeeService.GetAllEmployeesAsync();

                foreach (var employee in employees)
                {
                    var reviews = await performanceReviewService.GetPerformanceReviewsByEmployeeIdAsync(employee.Id);

                    Console.WriteLine($"Employee: {employee.Name}");

                    if (reviews.Count == 0)
                    {
                        Console.WriteLine("No performance reviews found for this employee.");
                    }
                    else
                    {
                        Console.WriteLine($"Total Reviews: {reviews.Count}");
                        Console.WriteLine("Review Details:");

                        foreach (var review in reviews)
                        {
                            Console.WriteLine($"Review Id: {review.Id}, Review: {review.Review}");
                        }
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating performance report: {ex.Message}");
            }
        }

        #endregion
        #region Manage Function
        static async Task ManageEmployees(IEmployeeService employeeService)
        {
            while (true)
            {
                Console.WriteLine("Employee Management");
                Console.WriteLine("1. Display All Employees");
                Console.WriteLine("2. Back to Main Menu");
                Console.Write("Please enter your choice: ");

                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await DisplayAllEmployees(employeeService);
                            break;
                        case 2:
                            Console.WriteLine("Going back to main menu...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static async Task ManageEmployeeRoles(IEmployeeService employeeService, IRoleCrudService roleService)
        {
            while (true)
            {
                Console.WriteLine("Employee Role Management");
                Console.WriteLine("1. Assign Role to Employee");
                Console.WriteLine("2. Back to Main Menu");
                Console.Write("Please enter your choice: ");

                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await AssignRoleToEmployee(employeeService, roleService);
                            break;
                        case 2:
                            Console.WriteLine("Going back to main menu...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 2.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        static async Task ManageEmployeeDepartment(IEmployeeService employeeService, IDepartmentCrudService departmentService)
        {
            Console.WriteLine("Employee Department Management");
            Console.WriteLine("1. Assign Department to Employee");
            Console.WriteLine("2. Back to Main Menu");
            Console.Write("Please enter your choice: ");

            string input = Console.ReadLine();
            int choice;
            if (int.TryParse(input, out choice))
            {
                switch (choice)
                {
                    case 1:
                        await AssignDepartmentToEmployee(employeeService, departmentService);
                        break;
                    case 2:
                        Console.WriteLine("Going back to main menu...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 2.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static async Task ManageEmployeePerformanceReview(IPerformanceReviewCrudService performanceReviewService, IEmployeeService employeeService)
        {
            while (true)
            {


                Console.WriteLine("Employee Performance Review Management");
                Console.WriteLine("1. Add Performance Review for Employee");
                Console.WriteLine("2. View Performance Review for Employee");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Please enter your choice: ");

                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await AddPerformanceReviewForEmployee(performanceReviewService, employeeService);
                            break;
                        case 2:
                            await ViewPerformanceReviewForEmployee(performanceReviewService, employeeService);
                            break;
                        case 3:
                            Console.WriteLine("Going back to main menu...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        #endregion
        #region Supprotive Function
        static async Task AddPerformanceReviewForEmployee(IPerformanceReviewCrudService performanceReviewService, IEmployeeService employeeService)
        {
            Console.WriteLine("Add Performance Review for Employee");

            await DisplayAllEmployees(employeeService);

            Console.Write("Enter Employee Id: ");
            int employeeId;
            if (!int.TryParse(Console.ReadLine(), out employeeId))
            {
                Console.WriteLine("Invalid Employee Id.");
                return;
            }

            var employee = await employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            Console.Write("Enter Performance Review: ");
            string review = Console.ReadLine();

            await performanceReviewService.CreatePerformanceReviewAsync(employeeId, review);

            Console.WriteLine("Performance review added successfully.");
        }

        static async Task ViewPerformanceReviewForEmployee(IPerformanceReviewCrudService performanceReviewService, IEmployeeService employeeService)
        {
            Console.WriteLine("View Performance Review for Employee");

            await DisplayAllEmployees(employeeService);

            Console.Write("Enter Employee Id: ");
            int employeeId;
            if (!int.TryParse(Console.ReadLine(), out employeeId))
            {
                Console.WriteLine("Invalid Employee Id.");
                return;
            }

            var employee = await employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            var reviews = await performanceReviewService.GetPerformanceReviewsByEmployeeIdAsync(employeeId);
            if (reviews.Count == 0)
            {
                Console.WriteLine("No performance reviews found for this employee.");
                return;
            }

            Console.WriteLine("Performance Reviews:");
            foreach (var review in reviews)
            {
                Console.WriteLine($"Review Id: {review.Id}, Review: {review.Review}");
            }
        }

        static async Task AssignDepartmentToEmployee(IEmployeeService employeeService, IDepartmentCrudService departmentService)
        {
            while (true)
            {
                Console.WriteLine("Assign Department to Employee");

                await DisplayAllEmployees(employeeService);

                Console.Write("Enter Employee Id: ");
                int employeeId;
                if (!int.TryParse(Console.ReadLine(), out employeeId))
                {
                    Console.WriteLine("Invalid Employee Id.");
                    return;
                }

                var employee = await employeeService.GetEmployeeByIdAsync(employeeId);
                if (employee == null)
                {
                    Console.WriteLine("Employee not found.");
                    return;
                }

                var departments = await departmentService.GetAllDepartmentsAsync();
                Console.WriteLine("Available Departments:");
                foreach (var item in departments)
                {
                    Console.WriteLine($"Department Id: {item.Id}, Name: {item.DepartmentName}");
                }

                Console.Write("Enter Department Id to assign: ");
                int departmentId;
                if (!int.TryParse(Console.ReadLine(), out departmentId))
                {
                    Console.WriteLine("Invalid Department Id.");
                    return;
                }

                var department = await departmentService.GetDepartmentByIdAsync(departmentId);
                if (department == null)
                {
                    Console.WriteLine("Department not found.");
                    return;
                }

                await employeeService.AssignEmployeeToDepartmentAsync(employeeId, departmentId);

                Console.WriteLine("Department assigned successfully.");
            }
        }

        static async Task AssignRoleToEmployee(IEmployeeService employeeService, IRoleCrudService roleService)
        {
            Console.WriteLine("Assign Role to Employee");

            await DisplayAllEmployees(employeeService);

            Console.Write("Enter Employee Id: ");
            int employeeId;
            if (!int.TryParse(Console.ReadLine(), out employeeId))
            {
                Console.WriteLine("Invalid Employee Id.");
                return;
            }

            var employee = await employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }

            var roles = await roleService.GetAllRolesAsync();
            Console.WriteLine("Available Roles:");
            foreach (var item in roles)
            {
                Console.WriteLine($"Role Id: {item.Id}, Name: {item.Name}");
            }

            Console.Write("Enter Role Id to assign: ");
            int roleId;
            if (!int.TryParse(Console.ReadLine(), out roleId))
            {
                Console.WriteLine("Invalid Role Id.");
                return;
            }

            var role = await roleService.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                Console.WriteLine("Role not found.");
                return;
            }

            await employeeService.AddEmployeeRoleAsync(employeeId, roleId);

            Console.WriteLine("Role assigned successfully.");
        }
        static async Task DisplayAllEmployees(IEmployeeService employeeService)
        {
            Console.WriteLine("Displaying All Employees");

            List<Employee> employees = await employeeService.GetAllEmployeesAsync();

            foreach (var employee in employees)
            {
                Console.WriteLine($"Employee Id: {employee.Id}, Name: {employee.Name}, Department: {employee.Department.DepartmentName}, Role : {employee.EmployeeRoles.OrderBy(er => er.UpdatedAt)?.FirstOrDefault()?.Role.Name}");
            }
        }
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        #endregion
        #region Seeding of Role, Department and Employees
        static async Task SeedRolesAsync(IRoleCrudService roleService)
        {
            var roles = await roleService.GetAllRolesAsync();
            if (roles.Count == 0)
            {
                await roleService.CreateRoleAsync("CEO");
                await roleService.CreateRoleAsync("Manager");
                await roleService.CreateRoleAsync("Developer");
                Console.WriteLine("Roles seeded successfully.");
            }
        }

        static async Task SeedDepartmentsAsync(IDepartmentCrudService departmentCrudService)
        {
            var department = await departmentCrudService.GetAllDepartmentsAsync();
            if (department.Count == 0)
            {
                await departmentCrudService.CreateDepartmentAsync("UpperManagement Department");
                await departmentCrudService.CreateDepartmentAsync("Production Department");
                await departmentCrudService.CreateDepartmentAsync("Manager Department");
                Console.WriteLine("Department seeded successfully.");
            }
        }

        static async Task SeedEmployeeAsync(IEmployeeService employeeService)
        {
            var employees = await employeeService.GetAllEmployeesAsync();
            if (employees.Count == 0)
            {
                await employeeService.CreateEmployeeAsync(new Employee()
                {
                    Name = "Employee",
                    DepartmentId = 1,
                });
                await employeeService.CreateEmployeeAsync(new Employee()
                {
                    Name = "Employee2",
                    DepartmentId = 2,
                });
                await employeeService.CreateEmployeeAsync(new Employee()
                {
                    Name = "Employee3",
                    DepartmentId = 3,
                });
                Console.WriteLine("Employees seeded successfully.");
            }
        }

        #endregion
    }
}
