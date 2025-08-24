using YourProjectName.Models;


namespace YourProjectName.Repositories
{
    public class TeacherRepository
    {
        private List<Teacher> _teachers = new List<Teacher>();
        private int _nextId = 1;

        public void AddTeacher(Teacher teacher)
        {
            teacher.Id = _nextId++;
            _teachers.Add(teacher);
        }

        public List<Teacher> GetAllTeachers() => _teachers;

        public void LoadData(List<Teacher> teachers)
        {
            _teachers = teachers;
            _nextId = teachers.Any() ? teachers.Max(t => t.Id) + 1 : 1;
        }
    }
}