Project Setup instruction
1.	Change your server name in Application dbcontext like mine is “.” 
(Server=.;Database=EmployeeManagementDb;Trusted_Connection=True;")
2.	Then add migration using this command
 dotnet ef migrations add InitialCreate
3.	Update database using this command
dotnet ef database update
4.	Build application and run.



Time and Space complexity
GeneratePerformanceReport
Time Complexity: O(m * n), where 'm' represents the number of employees and 'n' represents the average number of reviews per employee.
Space Complexity: O(1), as it primarily depends on the memory usage for storing employees, their performance reviews, and any additional resources used during execution.
Other Methods (ManageEmployees, ManageEmployeeRoles, ManageEmployeeDepartment, ManageEmployeePerformanceReview, etc.):
Time Complexity: O(n) to O(n log n), where 'n' represents the number of employees, departments, roles, or performance reviews. This complexity arises due to input/output operations, looping, and invoking other methods.
Space Complexity: O(n) or higher, as it primarily depends on the memory usage for storing variables, data structures, and any additional resources utilized during execution. The space complexity can vary based on the size of data being processed.

Supporting Methods (AddPerformanceReviewForEmployee, ViewPerformanceReviewForEmployee, AssignDepartmentToEmployee, AssignRoleToEmployee, DisplayAllEmployees, etc.):
Time Complexity: Varies depending on the specific operations performed within each method. It ranges from O(1) to O(n), where 'n' represents the size of data being processed.
Space Complexity: Varies depending on the memory usage for storing variables, data structures, and any additional resources utilized during execution. It typically ranges from O(1) to O(n) or higher.
