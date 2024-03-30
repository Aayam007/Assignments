using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class Department :BaseModel
    {
        [Required]
        public string DepartmentName { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
