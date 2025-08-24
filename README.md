🧍🏻# Student Exam Management System

📄##Description

This is a console-based C# application designed to manage student, teacher, exam, and result data. It provides functionalities for adding, viewing, searching, and sorting students, adding teachers and exams, recording exam results, and generating reports.

🚀##Features

- *Student Management*: Add, view, search, and sort students by name or GPA.
- *Teacher Management*: Add and view teacher details.
- *Exam Management*: Add new exams with subjects and dates.
- *Result Management*: Record and view student exam results.
- *Reporting*: View top students in a subject, export student data to CSV.
- *Data Persistence*: Save and load all application data to/from a JSON file.

📑##Project Structure
. 
├── DataStore.cs          # Defines the structure for saving/loading all application data.
├── Exam.cs               # Represents an exam entity.
├── Person.cs             # Abstract base class for Person (Student, Teacher).
├── Program.cs            # Main application entry point and console UI logic.
├── Result.cs             # Represents an exam result for a student.
├── Student.cs            # Represents a student entity.
├── Teacher.cs            # Represents a teacher entity.
├── ExamRepository.cs     # Handles data operations for Exam objects.
├── ResultRepository.cs   # Handles data operations for Result objects.
├── StudentRepository.cs  # Handles data operations for Student objects.
└── TeacherRepository.cs  # Handles data operations for Teacher objects.


##Getting Started

###Prerequisites

- .NET SDK (version 6.0 or higher)

###Installation

1. Clone the repository:
   bash
   git clone <repository_url>
   cd YourProjectName
   
2. Build the project:
   bash
   dotnet build
   

###Running the Application

To run the application from the command line:

bash
dotnet run


##Usage

Upon running the application, a menu will be displayed with various options:


--- Student Exam Management System ---
1.  Add Student
2.  Add Teacher
3.  View/Search/Sort Students
4.  Add Exam
5.  Record Exam Result for a Student
6.  View a Student's Grades
7.  View Top 3 Students in a Subject
8.  Save Data to File
9.  Load Data from File
10. Export Student Data to CSV
11. Exit


Follow the prompts to interact with the system. Data is saved to exam_system_data.json and student reports to students_report.csv in the application's directory.

##Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

For any questions or suggestions, please open an issue in the repository.
