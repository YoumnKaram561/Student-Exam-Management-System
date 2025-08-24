using YourProjectName.Models;
using System.Collections.Generic;
using System.Linq;

namespace YourProjectName.Repositories
{
    public class StudentRepository
    {
        private List<Student> _students = new List<Student>();
        private Dictionary<int, Student> _studentIndex = new Dictionary<int, Student>();
        private int _nextId = 1;

        public void AddStudent(Student student)
        {
            student.Id = _nextId++;
            _students.Add(student);
            _studentIndex[student.Id] = student;
        }

        public Student? GetStudentById(int id)
        {
         
            return _studentIndex.GetValueOrDefault(id);
        }

        public List<Student> GetAllStudents() => _students;

        public void LoadData(List<Student> students)
        {
            _students = students;
            _studentIndex = students.ToDictionary(s => s.Id); // Rebuild the index
            _nextId = students.Any() ? students.Max(s => s.Id) + 1 : 1;
        }
    }
}