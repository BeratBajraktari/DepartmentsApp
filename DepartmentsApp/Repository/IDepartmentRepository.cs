using DepartmentsApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DepartmentsApp.Repository
{
    public interface IDepartmentRepository
    {
        Department Find(int id);
        List<Department> GetAll();

        Department Add(Department department);
        Department Update(Department department);

        void Remove(int id);
    }
}
