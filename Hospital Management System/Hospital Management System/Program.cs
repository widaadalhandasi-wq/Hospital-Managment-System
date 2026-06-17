using Hospital_Management_System.Models;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management_System
{
    public class Program
    {
        //1) Patient Registration
        public static void PatientRegistration(HospitalContext context)
        {
            //Ask patient's about personal information
            Console.WriteLine("Enter Your Patient Name:");
            string patientName = Console.ReadLine();
            Console.WriteLine("Enter Your Age:");
            int patientAge = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Your Gender:");
            string patientGender = Console.ReadLine();
            Console.WriteLine("Enter Your Phone Number:");
            string patientPhone = Console.ReadLine();
            Console.WriteLine("Enter Your Email:");
            string patientEmail = Console.ReadLine();
            Console.WriteLine("Enter Your Blood Type:");
            string patientBloodType = Console.ReadLine();

            //Generate ID
            int PationId = (context.Patients.Count) + 1;

            //Add user:
            context.Patients.Add(new Patient
            {
                patientId = PationId,
                patientName = patientName,
                patientAge = patientAge,
                patientGender = patientGender,
                patientPhone = patientPhone,
                patientBloodType = patientBloodType,
                patientEmail = patientEmail
            });
            Console.WriteLine("Pations Added Successfuly" + PationId);
        }         

        //2)  Add a New Doctor
        public static void AddaNewDoctor(HospitalContext context)
        {
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
            int doctorId = (context.Doctors.Count) + 1;

            //Add Doctor:
            context.Doctors.Add(new Doctor
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
        public static void ViewAllPatients(HospitalContext context)
        {
            Console.WriteLine("=== View All Patients ===");

            if (context.Patients.Count() == 0)
            {
                Console.WriteLine("There Is No Patient Yet");
                return;
            }

            foreach (var Patient in context.Patients)
            {            
                    Console.WriteLine("Patient ID:" + Patient.patientId);
                    Console.WriteLine("Patient Name:" + Patient.patientName);
                    Console.WriteLine("Patient Age:" + Patient.patientAge);
                    Console.WriteLine("Patient Phone Number:" + Patient.patientPhone);
                    Console.WriteLine("Patient Email:" + Patient.patientEmail);
                    Console.WriteLine("Patient Blood Type:" + Patient.patientBloodType);
            }
        }


        //4)  View All Doctors by Specialization
        public static void ViewAllDoctorsbySpecialization(HospitalContext context)
        {
            
            foreach (var Doctor in context.Doctors)
            {
                Console.WriteLine("Doctors ID:" + Doctor.doctorId);
                Console.WriteLine("Doctors Name:" + Doctor.doctorName);
                Console.WriteLine("Doctors Specialization:" + Doctor.doctorSpecialization);
             
            }
            Console.WriteLine("Enter Doctor Specialization:");
            string specialization = Console.ReadLine();

            bool found = false;

            foreach (var Doctor in context.Doctors)
            {
                if (Doctor.doctorSpecialization == specialization)
                {
                    Console.WriteLine("Found Doctor Name: " + Doctor.doctorName);
                    found = true;
                }
            }

            if (found == false)
            {
                Console.WriteLine("No Doctors found with this specialization.");
            }


        }
        
        //5)  Add an Available Time Slot for a Doctor
        public static void AddanAvailableTimeSlotforaDoctor(HospitalContext context)
        {
            foreach (var doctor in context.Doctors)
            {
                Console.WriteLine($"ID: {doctor.doctorId} | Name: {doctor.doctorName}");
            }

            Console.WriteLine("Enter Doctor ID");
            int doctorId=Convert.ToInt32(Console.ReadLine());
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

            Console.WriteLine("The slot has been added successfully.");

        }

        //6)  Book an Appointment
        public static void BookanAppointment(HospitalContext context)
        {
            //patient
            Console.WriteLine("Enter Patient ID");
            int patientID = Convert.ToInt32(Console.ReadLine());

            var patient = context.Patients.FirstOrDefault(item =>item.patientId== patientID);
            if (patient == null)
            {
                Console.WriteLine(" Patient Not Found ");
                return;
            }

            //Doctor
            Console.WriteLine("Enter Doctor ID");
            int selectedDoctor = Convert.ToInt32(Console.ReadLine());
            var doctor = context.Doctors.FirstOrDefault(item => item.doctorId == selectedDoctor);
            if (doctor == null)
            {
                Console.WriteLine(" Doctor Not Found ");
                return;
            }

            bool hasAvailableSlots=false;

            foreach (var slot in context.AvailableSlots)
            {
                if (slot.doctorId == selectedDoctor && slot.isBooked == false)

                {
                    Console.WriteLine("Slot ID:" + slot.slotId);
                    Console.WriteLine("Slot Date:" + slot.slotDate);
                    Console.WriteLine("Slot Time:" + slot.slotTime);
                    hasAvailableSlots = true;
                }

            }

            if (hasAvailableSlots == false)
            {
                Console.WriteLine(" This Doctor has No Available Slots ");
                return;
            }
            
            Console.WriteLine("Enter Slot ID");
            int selectedSlotID = Convert.ToInt32(Console.ReadLine());

            AvailableSlot chosenSlot = null;

            foreach (var slot in context.AvailableSlots)
            {
                if (slot.slotId == selectedSlotID && slot.doctorId == selectedDoctor && slot.isBooked == false)
                {
                    slot.isBooked = true;
                    chosenSlot = slot;

                }

            }
            if (chosenSlot!=null)
            {
                int newAppointmentId = context.Appointments.Count + 1;

                context.Appointments.Add(new Appointment
                {
                    appointmentId = newAppointmentId,
                    patientId = patientID,
                    doctorId = selectedDoctor,
                    appointmentDate = chosenSlot.slotDate,
                    appointmentTime = chosenSlot.slotTime, 
                    status = "Pending" 
                });
                Console.WriteLine("Appointment booked successfully");
            }
            else
            {
                Console.WriteLine("Invalid Slot ID or the slot is already taken.");
            }


        }

        //7)  Cancel an Appointment
        public static void CancelanAppointment(HospitalContext context)
        {
            Console.WriteLine("Enter Appointment ID to cancel:");
            int appointmentID = Convert.ToInt32(Console.ReadLine());

            // search for the first appointment that has same id:
            var appointment = context.Appointments.FirstOrDefault(item => item.appointmentId == appointmentID);

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

            // Update the appointment status
            appointment.status = "Cancelled";

            
            bool slotFound = false;    //INITIAL VALUE FOR NOT FOUND THE APPOINTMENT  
            foreach (var slot in context.AvailableSlots)
            {
                if (slot.doctorId == appointment.doctorId && slot.slotDate == appointment.appointmentDate && slot.slotTime == appointment.appointmentTime)
                {
                    slot.isBooked = false;   //CANCEL THE BOOKED
                    slotFound = true;     //FIND THE APPOINTMENT  
                }
            }

            if (slotFound)
            {
                Console.WriteLine("Appointment successfully cancelled and the time slot is now available.");
            }
            else
            {
              
                Console.WriteLine("Appointment cancelled, but matching time slot was not found.");
            }
        }

        //8)  Create a Medical Record After a Visit
        public static void CreateaMedicalRecordAfteraVisit(HospitalContext context)
        {
            Console.WriteLine("Enter Appointment ID to Record:");
            int appointmentId = Convert.ToInt32(Console.ReadLine());

            // search for the first appointment that has same id:
            var appointment = context.Appointments.FirstOrDefault(item => item.appointmentId == appointmentId);


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


            Console.WriteLine("Enter Doctor Diagnosis:");
            string diagnosis =Console.ReadLine();
            Console.WriteLine("Enter Prescribed Medication:");
            string medication = Console.ReadLine();
            Console.WriteLine("Enter Consultation Fee:");
            decimal fee = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Enter visit Date");
            string date=Console.ReadLine();

            context.MedicalRecords.Add(new MedicalRecord
            {   
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

            Console.WriteLine("Medical record created successfully and appointment status updated to Completed.");
        }

        //9)  Generate a Patient Medical History Report
        public static void GenerateaPatientMedicalHistoryReport(HospitalContext context)
        {
            Console.WriteLine("Enter Patient ID");
            int patientID = Convert.ToInt32(Console.ReadLine());

            var selectedpatient = context.Patients.FirstOrDefault(item => item.patientId == patientID);

            if (selectedpatient == null)
            {
                Console.WriteLine("There Is No Patient Found");
                return;
            }

            decimal totalAmountCharged = 0;
            bool hasRecords = false;


            foreach (var record in context.MedicalRecords)
            {
                if (record.patientId == patientID)
                {
                    hasRecords = true;

                    string doctorName = "Unknown Doctor";
                    var doctor = context.Doctors.FirstOrDefault(item => item.doctorId == record.doctorId);
                    if (doctor != null)
                    {
                        doctorName = doctor.doctorName;
                    }

                    Console.WriteLine("Date:" + record.visitDate);
                    Console.WriteLine("Name of Doctor: " + doctorName);
                    Console.WriteLine("Diagnosis:" + record.diagnosis);
                    Console.WriteLine("Medication Prescribed:" + record.prescription);
                    Console.WriteLine("Visit Fee: " + record.visitFee);

                    totalAmountCharged += record.visitFee;
                }   
            }
            if (hasRecords == true)
            {
                Console.WriteLine("Total Amount Charged Across All Visits: " + totalAmountCharged);
            }
            else
            {
                Console.WriteLine("This patient has no records yet.");
            }

        }


        //10) Doctor Workload and Revenue Summary
        public static void DoctorWorkloadandRevenueSummary(HospitalContext context)
        {
            var summaryList = new List<DoctorSummary>();

            bool hasAppointments = false;  //INITIAL VALUE FOR THE APPOINTMENT  

            foreach (var doctor in context.Doctors)
            {
                //intial value for completed appointment,canceled appointment and total revenue.
                int completedCount = 0;
                int cancelledCount = 0;
                decimal totalRevenue = 0;

                foreach (var appointment in context.Appointments)
                {
                    if (appointment.doctorId == doctor.doctorId)
                    {
                        hasAppointments = true;
                        if (appointment.status == "Completed")
                        {
                            completedCount++;

                            var record = context.MedicalRecords.FirstOrDefault(item => item.appointmentId == appointment.appointmentId);
                            if (record != null)
                            {
                                totalRevenue += record.visitFee;
                            }
                        }
                        else if (appointment.status == "Cancelled")
                        {
                            cancelledCount++;
                        }

                    }
                }
                summaryList.Add(new DoctorSummary
                {
                    DoctorName = doctor.doctorName,
                    Completed = completedCount,
                    Cancelled = cancelledCount,
                    Revenue = totalRevenue
                });
            }
            if (hasAppointments == false) 
            {
                Console.WriteLine("There Is No Appointments Have Been Recorded Yet In The System ");
            
            }

            // sorted so that the doctor who generated the highest revenue appears first. 
            var sortedList = summaryList.OrderByDescending(doctor => doctor.Revenue);
           
            foreach (var item in sortedList)
            {
                Console.WriteLine($"doctor: {item.DoctorName} | Completed: {item.Completed} | Revenue: {item.Revenue} | Cancelled: {item.Cancelled}");
            }


        }

     public static void Main(string[] args)
        {
            HospitalContext mainContext = new HospitalContext();
            mainContext.Patients = new List<Patient>();
            mainContext.Doctors = new List<Doctor>();
            mainContext.Appointments = new List<Appointment>();
            mainContext.MedicalRecords = new List<MedicalRecord>();
            mainContext.AvailableSlots = new List<AvailableSlot>();
         



            bool exit = false;
            while (exit == false)
            {
                Console.WriteLine("******************************************");
                Console.WriteLine(" Welcome To The Hospital Management System ");
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
                        PatientRegistration(mainContext);
                        break;
                    case 2:
                        AddaNewDoctor(mainContext);
                        break;
                    case 3:
                        ViewAllPatients(mainContext);
                        break;
                    case 4:
                        ViewAllDoctorsbySpecialization(mainContext);
                        break;
                    case 5:
                        AddanAvailableTimeSlotforaDoctor(mainContext);
                        break;
                    case 6:
                        BookanAppointment(mainContext);
                        break;
                    case 7:
                        CancelanAppointment(mainContext);
                        break;
                    case 8:
                        CreateaMedicalRecordAfteraVisit(mainContext);
                        break;
                    case 9:
                        GenerateaPatientMedicalHistoryReport(mainContext);
                        break;
                    case 10:
                        DoctorWorkloadandRevenueSummary(mainContext);
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("INVALID OPTION PLEASE TRY AGAIN");
                        break;
                }
                Console.WriteLine("Press Any Key To Continue.....");
                Console.ReadKey();
                Console.Clear();

            }
        }
    }
}

