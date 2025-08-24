using System.Collections.Generic;

namespace YourProjectName.Models
{
    
    public class DataStore
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public List<Exam> Exams { get; set; } = new List<Exam>();
        public List<Result> Results { get; set; } = new List<Result>();
    }
}