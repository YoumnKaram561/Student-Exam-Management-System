using YourProjectName.Models;


namespace YourProjectName.Repositories
{
    public class ResultRepository
    {
        private List<Result> _results = new List<Result>();
        private int _nextId = 1;

        public void AddResult(Result result)
        {
            result.Id = _nextId++;
            _results.Add(result);
        }

        public List<Result> GetAllResults() => _results;

        public void LoadData(List<Result> results)
        {
            _results = results;
            _nextId = results.Any() ? results.Max(r => r.Id) + 1 : 1;
        }
    }
}