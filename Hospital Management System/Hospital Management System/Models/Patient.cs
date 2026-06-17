using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Models
{
    public class Patient
    {
        public int patientId {  get; set; }
        public string patientName {  get; set; }
        public int patientAge {  get; set; }
        public string patientGender {  get; set; }
        public string patientPhone {  get; set; }
        public string patientEmail {  get; set; }
        public string patientBloodType {  get; set; }

    }
}
