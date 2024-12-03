using Dapper;
using DepartmentsApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DepartmentsApp.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private IDbConnection db;

        public DepartmentRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Department Find(int id)
        {
            var sql = "Select FROM Departments WHERE DepartmentId = @DepartmentId";
            return db.Query<Department>(sql, new { @DepartmentId = id }).Single();
        }

        public List<Department> GetAll()
        {
            try
            {
                var sql = "SELECT * FROM Departments";
                return db.Query<Department>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve departments.", ex);
            }
        }

        public Department Add(Department department)
        {
            var sql = "INSERT INTO Departments (DepartmentName, ParentId) " +
                      "VALUES(@DepartmentName, @ParentId); " +
                      "SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = db.Query<int>(sql, department).Single();
            department.DepartmentId = id; 
            return department;
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Departments WHERE DepartmentId = @id";
            db.Execute(sql, new { id });
        }

        public Department Update(Department department)
        {
            var sql = "UPDATE Departments SET DepartmentName = @DepartmentName, " +
                "ParentId = @ParentId WHERE DepartmentId = @DepartmentId";
            db.Execute(sql, department);
            return department;
        }
    }
}
