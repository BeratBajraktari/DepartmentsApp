using DepartmentsApp.Models;

namespace DepartmentsApp.Repository
{
    public interface INewsRepository
    {
        News Add(News news);
        List<News> GetAll();
        List<News> GetLatestNewsExcludingCurrent(int currentNewsId); 
        News GetById(int id);
        void Remove(int id);
        News Update(News news);
        List<News> GetNewsByPage(int pageNumber, int pageSize);
    }
}
