using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using YourProjectName.Models;
using YourProjectName.Repositories;

namespace YourProjectName
{
    public class Program
    {
        private static readonly StudentRepository _studentRepo = new StudentRepository();
        private static readonly TeacherRepository _teacherRepo = new TeacherRepository();
        private static readonly ExamRepository _examRepo = new ExamRepository();
        private static readonly ResultRepository _resultRepo = new ResultRepository();
        private static readonly string dataFilePath = "exam_system_data.json";

        public static void Main(string[] args)
        {
            LoadData(); 
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Student Exam Management System ---");
                Console.WriteLine("1.  Add Student");
                Console.WriteLine("2.  Add Teacher");
                Console.WriteLine("3.  View/Search/Sort Students");
                Console.WriteLine("4.  Add Exam");
                Console.WriteLine("5.  Record Exam Result for a Student");
                Console.WriteLine("6.  View a Student's Grades");
                Console.WriteLine("7.  View Top 3 Students in a Subject");
                Console.WriteLine("8.  Save Data to File");
                Console.WriteLine("9.  Load Data from File");
                Console.WriteLine("10. Export Student Data to CSV");
                Console.WriteLine("11. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1": AddStudent(); break;
                    case "2": AddTeacher(); break;
                    case "3": ManageStudentsView(); break;
                    case "4": AddExam(); break;
                    case "5": AddResult(); break;
                    case "6": ViewStudentResults(); break;
                    case "7": ViewTop3StudentsBySubject(); break;
                    case "8": SaveData(); break;
                    case "9": LoadData(); break;
                    case "10": ExportStudentsToCsv(); break;
                    case "11": exit = true; break;
                    default: Console.WriteLine("Invalid option. Please try again."); break;
                }
            }
        }

      

        private static void AddStudent()
        {
            try
            {
                Console.Write("Enter student name: ");
                string name = Console.ReadLine() ?? "";
                Console.Write("Enter email: ");
                string email = Console.ReadLine() ?? "";
                Console.Write("Enter level: ");
                string level = Console.ReadLine() ?? "";
                Console.Write("Enter GPA: ");
                double gpa = Convert.ToDouble(Console.ReadLine());

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Error: Name and email are required fields.");
                    return;
                }

                _studentRepo.AddStudent(new Student { Name = name, Email = email, Level = level, GPA = gpa });
                Console.WriteLine("=> Student added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Input Error: Please enter a valid number for GPA.");
            }
        }

        private static void AddTeacher()
        {
            Console.Write("Enter teacher name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Enter email: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Enter subject taught: ");
            string subject = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(subject))
            {
                Console.WriteLine("Error: Teacher name and subject are required fields.");
                return;
            }

            _teacherRepo.AddTeacher(new Teacher { Name = name, Email = email, Subject = subject });
            Console.WriteLine("=> Teacher added successfully.");
        }

        private static void ManageStudentsView()
        {
            Console.WriteLine("\n--- Student Management ---");
            Console.WriteLine("1. View All Students");
            Console.WriteLine("2. Search for a Student by Name");
            Console.WriteLine("3. Sort Students by Name");
            Console.WriteLine("4. Sort Students by GPA");
            Console.Write("Select: ");
            string choice = Console.ReadLine() ?? "";
            List<Student> students = new List<Student>();

            switch (choice)
            {
                case "1":
                    students = _studentRepo.GetAllStudents();
                    break;
                case "2":
                    Console.Write("Enter student name to search for: ");
                    string searchTerm = Console.ReadLine() ?? "";
                    students = _studentRepo.GetAllStudents()
                        .Where(s => s.Name != null && s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                case "3":
                    students = _studentRepo.GetAllStudents().OrderBy(s => s.Name).ToList();
                    break;
                case "4":
                    students = _studentRepo.GetAllStudents().OrderByDescending(s => s.GPA).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    return;
            }

            if (!students.Any())
            {
                Console.WriteLine("No students found matching the criteria.");
                return;
            }

            Console.WriteLine("\n--- Student List ---");
            foreach (var s in students)
            {
                Console.WriteLine($"ID: {s.Id}, Name: {s.Name}, Email: {s.Email}, Level: {s.Level}, GPA: {s.GPA}");
            }
        }

        private static void AddExam()
        {
            try
            {
                Console.Write("Enter the subject for the exam: ");
                string subject = Console.ReadLine() ?? "";
                Console.Write("Enter the exam date (YYYY-MM-DD): ");
                DateTime date = Convert.ToDateTime(Console.ReadLine());

                if (string.IsNullOrWhiteSpace(subject))
                {
                    Console.WriteLine("Error: Subject is a required field.");
                    return;
                }

                _examRepo.AddExam(new Exam { Subject = subject, Date = date });
                Console.WriteLine("=> Exam added successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Input Error: Invalid date format. Please use YYYY-MM-DD.");
            }
        }

        private static void AddResult()
        {
            try
            {
                Console.Write("Enter Student ID: ");
                int studentId = Convert.ToInt32(Console.ReadLine());
                var student = _studentRepo.GetStudentById(studentId);
                if (student == null)
                {
                    Console.WriteLine("Error: No student found with this ID.");
                    return;
                }

                Console.Write("Enter Exam ID: ");
                int examId = Convert.ToInt32(Console.ReadLine());
                var exam = _examRepo.GetExamById(examId);
                if (exam == null)
                {
                    Console.WriteLine("Error: No exam found with this ID.");
                    return;
                }

                Console.Write($"Enter score for '{student.Name}' in '{exam.Subject}': ");
                double score = Convert.ToDouble(Console.ReadLine());

                _resultRepo.AddResult(new Result { StudentId = studentId, ExamId = examId, Score = score });
                Console.WriteLine("=> Result recorded successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Input Error: Please enter valid numbers for ID and Score.");
            }
        }

        private static void ViewStudentResults()
        {
            try
            {
                Console.Write("Enter Student ID to view grades: ");
                int studentId = Convert.ToInt32(Console.ReadLine());
                var student = _studentRepo.GetStudentById(studentId);
                if (student == null)
                {
                    Console.WriteLine("Error: No student found with this ID.");
                    return;
                }

                var studentResults = _resultRepo.GetAllResults().Where(r => r.StudentId == studentId).ToList();

                if (!studentResults.Any())
                {
                    Console.WriteLine($"Student '{student.Name}' has no recorded results yet.");
                    return;
                }

                Console.WriteLine($"\n--- Grades for: {student.Name} ---");
                foreach (var result in studentResults)
                {
                    var exam = _examRepo.GetExamById(result.ExamId);
                    string subjectName = exam?.Subject ?? "Unknown Subject";
                    Console.WriteLine($"Subject: {subjectName}, Score: {result.Score}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Input Error: Please enter a valid ID (number).");
            }
        }

        private static void ViewTop3StudentsBySubject()
        {
            Console.Write("Enter subject name to view top students: ");
            string subject = Console.ReadLine() ?? "";

            var topStudents = _resultRepo.GetAllResults()
                .Join(_examRepo.GetAllExams(),
                    result => result.ExamId,
                    exam => exam.Id,
                    (result, exam) => new { result, exam })
                .Where(x => x.exam.Subject != null && x.exam.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.result.Score)
                .Take(3)
                .Join(_studentRepo.GetAllStudents(),
                    resExam => resExam.result.StudentId,
                    student => student.Id,
                    (resExam, student) => new { StudentName = student.Name, Score = resExam.result.Score })
                .ToList();

            if (!topStudents.Any())
            {
                Console.WriteLine($"No results found for subject '{subject}'.");
                return;
            }

            Console.WriteLine($"\n--- Top 3 Students in: {subject} ---");
            int rank = 1;
            foreach (var item in topStudents)
            {
                Console.WriteLine($"{rank}. Student: {item.StudentName}, Score: {item.Score}");
                rank++;
            }
        }

        

        private static void SaveData()
        {
            var dataStore = new DataStore
            {
                Students = _studentRepo.GetAllStudents(),
                Teachers = _teacherRepo.GetAllTeachers(),
                Exams = _examRepo.GetAllExams(),
                Results = _resultRepo.GetAllResults()
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(dataStore, options);
            File.WriteAllText(dataFilePath, jsonString);
            Console.WriteLine($"=> Data successfully saved to: {dataFilePath}");
        }

        private static void LoadData()
        {
            if (!File.Exists(dataFilePath))
            {
                Console.WriteLine("Data file not found. A new one will be created upon saving.");
                return;
            }

            string jsonString = File.ReadAllText(dataFilePath);
            var dataStore = JsonSerializer.Deserialize<DataStore>(jsonString);

            if (dataStore != null)
            {
                _studentRepo.LoadData(dataStore.Students);
                _teacherRepo.LoadData(dataStore.Teachers);
                _examRepo.LoadData(dataStore.Exams);
                _resultRepo.LoadData(dataStore.Results);
                Console.WriteLine("=> Data loaded successfully.");
            }
        }

        private static void ExportStudentsToCsv()
        {
            string exportFilePath = "students_report.csv";
            var students = _studentRepo.GetAllStudents();

            if (!students.Any())
            {
                Console.WriteLine("There are no students to export.");
                return;
            }

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Name,Email,Level,GPA");

            foreach (var student in students)
            {
                csvBuilder.AppendLine($"{student.Id},{student.Name},{student.Email},{student.Level},{student.GPA}");
            }

            File.WriteAllText(exportFilePath, csvBuilder.ToString(), Encoding.UTF8);
            Console.WriteLine($"=> Student data successfully exported to: {exportFilePath}");
        }
    }
}