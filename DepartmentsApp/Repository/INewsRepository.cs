using DepartmentsApp.Models;

namespace DepartmentsApp.Repository
{
    public interface INewsRepository
    {
        News Add(News news);
        List<News> GetAll();
        News GetById(int id);
        void Remove(int id);
        News Update(News news);
    }
}
