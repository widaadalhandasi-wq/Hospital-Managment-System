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

        
        public Patient(int id, string name, int age, string gender, string phone,string email, string bloodType)
        {
            patientId = id;
            patientName= name;
            patientAge= age;
            patientGender= gender;
            patientPhone= phone;
            patientEmail= email;
            patientBloodType= bloodType;
        }

        public void PrintPatients()
        {
            Console.WriteLine("\n=== Patients List ===");
            Console.WriteLine($"ID:{patientId}" +
                              $" | Name: {patientName}  " +
                              $"| Age: {patientAge} " +
                              $"| Gender:  {patientGender}" +
                              $" | Phone: {patientPhone} " +
                              $" | Email: {patientEmail}   " +
                              $" | Blood Type: {patientBloodType}");
            Console.WriteLine("---------------------------------------------------------------------------------");
        }
    
    }
}
