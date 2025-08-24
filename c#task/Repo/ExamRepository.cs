using YourProjectName.Models;
using System.Collections.Generic;
using System.Linq;

namespace YourProjectName.Repositories
{
    public class ExamRepository
    {
        private List<Exam> _exams = new List<Exam>();
        private Dictionary<int, Exam> _examIndex = new Dictionary<int, Exam>();
        private int _nextId = 1;

        public void AddExam(Exam exam)
        {
            exam.Id = _nextId++;
            _exams.Add(exam);
            _examIndex[exam.Id] = exam;
        }

        public Exam? GetExamById(int id)
        {
            return _examIndex.GetValueOrDefault(id);
        }

        public List<Exam> GetAllExams() => _exams;

        public void LoadData(List<Exam> exams)
        {
            _exams = exams;
            _examIndex = exams.ToDictionary(e => e.Id);
            _nextId = exams.Any() ? exams.Max(e => e.Id) + 1 : 1;
        }
    }
}