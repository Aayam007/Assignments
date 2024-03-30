using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
