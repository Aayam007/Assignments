using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    public class PerformanceReview : BaseModel
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Review { get; set; }
    }
}
