using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Models
{
    public class DoctorSummary
    {
        public string DoctorName { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public decimal Revenue { get; set; }
    }
}
