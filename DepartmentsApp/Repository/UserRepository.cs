using Dapper;
using DepartmentsApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DepartmentsApp.Repository
{
    public class UserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public User FindByUsername(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            return _db.Query<User>(sql, new { Username = username }).FirstOrDefault();
        }

        public void Register(User user)
        {
            var sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
            _db.Execute(sql, user);
        }
    }
}
