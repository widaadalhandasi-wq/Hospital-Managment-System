using Hospital_Management_System.Models;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management_System
{
    public class Program
    {
        public static HospitalContext context = new HospitalContext
        {
            Patients = new List<Patient>() 
            {
            new Patient(1, "Widaad", 27, "Female", "12345678", "widaad@gmail.com", "O+"),
            new Patient(2, "Ebtisaam", 27, "Female", "12342278", "Ebtisaam@gmail.com", "O+"),
            new Patient(3, "Fathia", 30, "Female", "99345678", "Fathia@gmail.com", "O+"),
            new Patient(4, "Maya", 25, "Female", "77345678", "Maya@gmail.com", "O+"),
            new Patient(5, "Malak", 27, "Female", "22345678", "malak@gmail.com", "O+")
            },
            Doctors = new List<Doctor>(),
            Appointments = new List<Appointment>(),
            MedicalRecords = new List<MedicalRecord>(),
            AvailableSlots = new List<AvailableSlot>()
        };

      

    //1) Patient Registration
    public static void PatientRegistration(List<Patient> Patients)
        {
            Console.WriteLine("\n=== Register New Patient ===");

            //Ask patient's about personal information
            Console.WriteLine("Enter Your Patient Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Your Age:");
            int age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Your Gender:");
            string gender = Console.ReadLine();
            Console.WriteLine("Enter Your Phone Number:");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter Your Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter Your Blood Type:");
            string bloodType = Console.ReadLine();

            //Generate ID
            int PationId = (Patients.Count) + 1;

            //Add user:
            //context.Patients.Add(new Patient
            //{
            //    patientId = PationId,
            //    patientName = patientName,
            //    patientAge = patientAge,
            //    patientGender = patientGender,
            //    patientPhone = patientPhone,
            //    patientBloodType = patientBloodType,
            //    patientEmail = patientEmail
            //});


            //or 
            //Add user:
            Patients.Add(new Patient(PationId, name, age, phone, email, gender, bloodType));

            Console.WriteLine($"Pations Added Successfuly.Assigned ID: {PationId}");
        }         

        //2)  Add a New Doctor
        public static void AddaNewDoctor(List<Doctor> Doctors)
        {
            Console.WriteLine("\n=== Add New Doctor ===");

            //Ask Doctors's about personal information
            Console.WriteLine("Enter Your Doctor Name:");
            string DoctorName = Console.ReadLine();
            Console.WriteLine("Enter Your Specialization:");
            string DoctorSpecialization = Console.ReadLine();
            Console.WriteLine("Enter Your Phone Number:");
            string DoctorPhone = Console.ReadLine();
            Console.WriteLine("Enter Your Email:");
            string DoctorEmail = Console.ReadLine();
            Console.WriteLine("Enter Your consultation Fee:");
            decimal ConsultationFee = Convert.ToDecimal( Console.ReadLine());

            //Generate Doctor ID
            int doctorId = (Doctors.Count) + 1;

            //Add Doctor:
            Doctors.Add(new Doctor
            {
                doctorId= doctorId,
                doctorName = DoctorName,
                doctorSpecialization = DoctorSpecialization,
                doctorPhone = DoctorPhone,
                doctorEmail = DoctorEmail,
                consultationFee = ConsultationFee,
                
            });
            Console.WriteLine("Doctors Added Successfuly" + doctorId);
        }

        //3)  View All Patients
        public static void ViewAllPatients(List<Patient> Patients)
        {
            Console.WriteLine("=== View All Patients ===");

            if (Patients.Count() == 0)
            {
                Console.WriteLine("There Is No Patient Yet");
                return;
            }

            // LINQ: ForEach to print each patient
            foreach (Patient p in Patients)
            {
                p.PrintPatients();
            }

        }

        //4)  View All Doctors by Specialization
        public static void ViewAllDoctorsbySpecialization(List<Doctor> Doctors)
        {
            Console.WriteLine("\n=== Search Doctors by Specialization ===");

            Console.WriteLine("Enter Doctor Specialization:");
            string specialization = Console.ReadLine();


            //linq

            List<Doctor> doctorLists = Doctors.Where(d => d.doctorSpecialization.ToLower() == specialization).ToList();

            if (doctorLists.Count == 0)
            {
                Console.WriteLine("No Doctors found with this specialization.");
                return;
            }


            doctorLists.ForEach(Doctor => Console.WriteLine($"Doctor ID: {Doctor.doctorId}  , Doctor Name: {Doctor.doctorName} , Doctor Specialization: {Doctor.doctorSpecialization}"));



            //or 

            //    foreach (Doctor doctor in doctorLists)
            //    {
            //        Console.WriteLine($"ID: {doctor.doctorId}  |  Name: {doctor.doctorName}" +
            //                          $"  |  Phone: {doctor.doctorPhone}  |  Fee: {doctor.consultationFee:C}");
            //    //}

        }


        //5)  Add an Available Time Slot for a Doctor
        public static void AddanAvailableTimeSlotforaDoctor(HospitalContext context)
        {
            Console.WriteLine("\n=== Add Available Slot for Doctor ===");

            //foreach (Doctor d in context.Doctors)
            //{
            //    Console.WriteLine($"  ID: {d.doctorId}  |  {d.doctorName}  ({d.doctorSpecialization})");
            //}

            //or

            //linq
            List<Doctor> doctorLists = context.Doctors.ToList();

            if (doctorLists.Count == 0)
            {
                Console.WriteLine("No Doctors found");
                return;
            }

            doctorLists.ForEach(doctor => Console.WriteLine($"  ID: {doctor.doctorId}  |  {doctor.doctorName}  ({doctor.doctorSpecialization})"));

            Console.WriteLine("Enter Doctor ID");
            int doctorId=Convert.ToInt32(Console.ReadLine());

            bool result = context.Doctors.Any(d => d.doctorId == doctorId);

            if (result == false)
            {
                Console.WriteLine("no doctor found with id");
                return;
            }

            Console.WriteLine("Enter Slot Date");
            string slotDate = Console.ReadLine();
            Console.WriteLine("Enter Slot Time");
            string slotTime = Console.ReadLine();

            int slotId = (context.AvailableSlots.Count) + 1;

            context.AvailableSlots.Add(new AvailableSlot
            {
                slotId= slotId,
                doctorId = doctorId,
                slotDate = slotDate,
                slotTime = slotTime,
                isBooked = false        //availabel to booked
            });

            Console.WriteLine($"The slot has been added successfully.{slotId}");
        }

        //6)  Book an Appointment
        public static void BookanAppointment(HospitalContext context) //create / add 
        {
            Console.WriteLine("\n=== Book an Appointment ===");

            //patient
            Console.WriteLine("Enter Patient ID");
            int patientID = Convert.ToInt32(Console.ReadLine());

            var patient = context.Patients.FirstOrDefault(item => item.patientId == patientID);
            if (patient == null)
            {
                Console.WriteLine(" Patient Not Found ");
                return;
            }
            //call function to bring doctor with Specialization

            ViewAllDoctorsbySpecialization(context.Doctors);

            Console.WriteLine("Enter Doctor ID");
            int selectedDoctorId = Convert.ToInt32(Console.ReadLine());

            Doctor doctor = context.Doctors.FirstOrDefault(item => item.doctorId == selectedDoctorId);
            if (doctor == null)
            {
                Console.WriteLine(" Doctor Not Found ");
                return;
            }

            //linq
       
            List<AvailableSlot> AvailableSlots=context.AvailableSlots.Where(slot => slot.doctorId == selectedDoctorId && slot.isBooked == false).ToList();
            if (AvailableSlots.Count == 0)
            {
                Console.WriteLine(" This Doctor has No Available Slots");
            }


            Console.WriteLine($"\nAvailable slots for Dr. {doctor.doctorName}:");
            AvailableSlots.ForEach(slot => Console.WriteLine($"  Slot ID: {slot.slotId}  |  Date: {slot.slotDate}  |  Time: {slot.slotTime}"));


            Console.WriteLine("Enter Slot ID");
            int selectedSlotID = Convert.ToInt32(Console.ReadLine());

            // linq

            AvailableSlot chosenSlot = AvailableSlots.FirstOrDefault(slot => slot.slotId == selectedSlotID);

            if (chosenSlot == null)
            {
                Console.WriteLine("Slot not found or already booked.");
                return;
            }

            int newAppointmentId = context.Appointments.Count + 1;

            context.Appointments.Add(new Appointment
            {
                appointmentId = newAppointmentId,
                patientId = patientID,
                doctorId = selectedDoctorId,
                appointmentDate = chosenSlot.slotDate,
                appointmentTime = chosenSlot.slotTime,
                status = "Scheduled"
            });

            chosenSlot.isBooked = true;

            Console.WriteLine($"Appointment booked successfully! Appointment ID: {newAppointmentId}" +
                              $" | Date: {chosenSlot.slotDate} | Time: {chosenSlot.slotTime}");
        }

        //7)  Cancel an Appointment
        public static void CancelanAppointment(HospitalContext context)
        {
            Console.WriteLine("Enter Appointment ID to cancel:");
            int appointmentID = Convert.ToInt32(Console.ReadLine());

            // search for the first appointment that has same id:
            Appointment appointment = context.Appointments.FirstOrDefault(item => item.appointmentId == appointmentID);

            if (appointment == null)
            {
                Console.WriteLine("Error: Appointment ID not found.");
                return;
            }

            if (appointment.status == "Cancelled")
            {
                Console.WriteLine("Error: This appointment is already cancelled.");
                return;
            }

            if (appointment.status == "Completed")
            {
                Console.WriteLine("This appointment is already cancelled.");
                return;
            }
         

            // LINQ 

            AvailableSlot matchingSlot = context.AvailableSlots.FirstOrDefault(slot =>
                slot.doctorId == appointment.doctorId &&
                slot.slotDate == appointment.appointmentDate &&
                slot.slotTime == appointment.appointmentTime);

            if (matchingSlot != null)
            {
                matchingSlot.isBooked = false;

                // Update the appointment status
                appointment.status = "Cancelled";

                Console.WriteLine($"Appointment{appointmentID} successfully cancelled and the time slot is now available.");
            }
        
        }

        //8)  Create a Medical Record After a Visit
        public static void CreateaMedicalRecordAfteraVisit(HospitalContext context)
        {
            Console.WriteLine("Enter Appointment ID to Record:");
            int appointmentId = Convert.ToInt32(Console.ReadLine());

            // search for the first appointment that has same id:
            Appointment appointment = context.Appointments.FirstOrDefault(item => item.appointmentId == appointmentId);


            if (appointment == null)
            {
                Console.WriteLine("Error: Appointment ID not found.");
                return;
            }

            // Check appointment
            if (appointment.status == "Completed")
            {
                Console.WriteLine("Error: This appointment has already been processed.");
                return;
            }
            if (appointment.status == "Cancelled")
            {
                Console.WriteLine("Cannot create a medical record for a cancelled appointment.");
                return;
            }

            decimal fee = context.Doctors
                         .Where(d => d.doctorId == appointment.doctorId)
                         .Select(d => d.consultationFee)
                         .FirstOrDefault();

            Console.WriteLine("Enter Doctor Diagnosis:");
            string diagnosis =Console.ReadLine();
            Console.WriteLine("Enter Prescribed Medication:");
            string medication = Console.ReadLine();
            Console.WriteLine("Enter visit Date");
            string date=Console.ReadLine();

            int recordId = context.MedicalRecords.Count + 1;

            context.MedicalRecords.Add(new MedicalRecord
            {
                recordId=recordId,
                appointmentId= appointment.appointmentId,
                patientId = appointment.patientId,
                doctorId = appointment.doctorId,
                diagnosis = diagnosis,
                prescription= medication,
                visitFee= fee,
                visitDate= date
            });

            // Update the appointment status
            appointment.status = "Completed";

            Console.WriteLine($"Medical record created successfully and appointment status updated to Completed. RecordId {recordId} | Fee charged: {fee}" );
        }

        //9)  Generate a Patient Medical History Report
        public static void GenerateaPatientMedicalHistoryReport(HospitalContext context)
        {
            Console.WriteLine("Enter Patient ID");
            int patientID = Convert.ToInt32(Console.ReadLine());

            Patient selectedpatient = context.Patients.FirstOrDefault(item => item.patientId == patientID);

            if (selectedpatient == null)
            {
                Console.WriteLine("There Is No Patient Found");
                return;
            }

            //linq

            List<MedicalRecord> patientRecords=context.MedicalRecords.Where(item => item.patientId == patientID).ToList();

            if (patientRecords.Count ==0)
            {
                Console.WriteLine("This patient has no midecal records yet.");
                return;
            }

            Console.WriteLine($"\n--- Medical History for {selectedpatient.patientName} (ID: {patientID}) ---");

            patientRecords.ForEach(record =>

             {
                 string doctorName = context.Doctors
                                    .Where(d => d.doctorId == record.doctorId)
                                    .Select(d => d.doctorName)
                                    .FirstOrDefault() ?? "Unknown Doctor";

                 Console.WriteLine($"Date: {record.recordId}");
                 Console.WriteLine($"Date: {record.visitDate}");
                 Console.WriteLine($"Doctor: {doctorName}");
                 Console.WriteLine($"Diagnosis: {record.diagnosis}");
                 Console.WriteLine($"Medication Prescribed: {record.prescription}");
                 Console.WriteLine($"Visit Fee: {record.visitFee:C}");
                 Console.WriteLine("--------------------------------------------------");
             });

        
            decimal totalAmountCharged = patientRecords.Sum(record => record.visitFee);

            Console.WriteLine($"Total Amount Charged Across All Visits: {totalAmountCharged}");
          
        }

        //10) Doctor Workload and Revenue Summary
        public static void DoctorWorkloadandRevenueSummary(HospitalContext context)
        {

            Console.WriteLine("\n=== Doctor Workload & Revenue Summary ===");

            if (context.Appointments.Count == 0)
            {
                Console.WriteLine("No appointments have been recorded yet.");
                return;
            }

            var summaryReport = context.Doctors
              .Select(doctor => new
              {
                  doctor.doctorId,
                  doctor.doctorName,
                  doctor.doctorSpecialization,
             
                  completed = context.Appointments.Count(app => app.doctorId == doctor.doctorId && app.status == "Completed"),
             
                  cancelled = context.Appointments.Count(app => app.doctorId == doctor.doctorId && app.status == "Cancelled"),
                 
                  totalRevenue = context.MedicalRecords
                      .Where(record => record.doctorId == doctor.doctorId)
                      .Sum(record => record.visitFee)
              })
              .OrderByDescending(item => item.totalRevenue)
              .ToList();

            Console.WriteLine("\nRank | Doctor Name | Specialization | Completed | Cancelled | Total Revenue");
            Console.WriteLine("-------------------------------------------------------------------------");
           
            // عدّاد خارجي للترتيب
            int rank = 1;
            foreach (var x in summaryReport)
            {
          
                Console.WriteLine($"#{rank} | {x.doctorName} | {x.doctorSpecialization} | {x.completed} | {x.cancelled} | {x.totalRevenue:C}");

                rank++; // زيادة الترتيب 1 في كل لفة
            }

        }
       
        public static void Main(string[] args)
        {
            //HospitalContext mainContext = new HospitalContext();
            //mainContext.Doctors = new List<Doctor>();
            //mainContext.Appointments = new List<Appointment>();
            //mainContext.MedicalRecords = new List<MedicalRecord>();
            //mainContext.AvailableSlots = new List<AvailableSlot>();

            ////SEED DATA
            //mainContext.Patients = new List<Patient>()
            //{
            //    new Patient(1,"Widaad",27,"Female","12345678","widaad@gmail.com","O+"),
            //    new Patient(2,"Ebtisaam",27,"Female","12342278","Ebtisaam@gmail.com","O+"),
            //    new Patient(3,"Fathia",30,"Female","99345678","Fathia@gmail.com","O+"),
            //    new Patient(4,"Maya",25,"Female","77345678","Maya@gmail.com","O+"),
            //    new Patient(5,"Malak",27,"Female","22345678","widaad@gmail.com","O+"),
            //};



            bool exit = false;
            while (exit == false)
            {
                Console.WriteLine("******************************************");
                Console.WriteLine(" Hospital Management System ");
                Console.WriteLine("******************************************");
                Console.WriteLine("*** Please Select Your Options ***");
                Console.WriteLine("1) Patient Registration ");
                Console.WriteLine("2) Add a New Doctor ");
                Console.WriteLine("3) View All Patients ");
                Console.WriteLine("4) View All Doctors by Specialization ");
                Console.WriteLine("5) Add an Available Time Slot for a Doctor ");
                Console.WriteLine("6) Book an Appointment ");
                Console.WriteLine("7) Cancel an Appointment");
                Console.WriteLine("8) Create a Medical Record After a Visit ");
                Console.WriteLine("9) Generate a Patient Medical History Report ");
                Console.WriteLine("10) Doctor Workload and Revenue Summary ");
                Console.WriteLine("0) Exit");

                int option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        PatientRegistration(context.Patients);
                        break;
                    case 2:
                        AddaNewDoctor(context.Doctors);
                        break;
                    case 3:
                        ViewAllPatients(context.Patients);
                        break;
                    case 4:
                        ViewAllDoctorsbySpecialization(context.Doctors);
                        break;
                    case 5:
                        AddanAvailableTimeSlotforaDoctor(context);
                        break;
                    case 6:
                        BookanAppointment(context);
                        break;
                    case 7:
                        CancelanAppointment(context);
                        break;
                    case 8:
                        CreateaMedicalRecordAfteraVisit(context);
                        break;
                    case 9:
                        GenerateaPatientMedicalHistoryReport(context);
                        break;
                    case 10:
                        DoctorWorkloadandRevenueSummary(context);
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("INVALID OPTION PLEASE TRY AGAIN");
                        break;
                }
                if (exit)
                {
                    Console.WriteLine("Press Any Key To Continue.....");
                    Console.ReadKey();
                    Console.Clear();
                }

            }

            Console.WriteLine("GoodBye...");

        }
    }
}

