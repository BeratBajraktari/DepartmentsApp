using Dapper;
using DepartmentsApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DepartmentsApp.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDbConnection _db;

        public NewsRepository(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public News Add(News news)
        {
            var sql = "INSERT INTO News (Title, Content, PublicationDate, Location, ImageUrl) " +
                      "VALUES(@Title, @Content, @PublicationDate, @Location, @ImageUrl); " +
                      "SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var id = _db.Query<int>(sql, news).Single();
            news.NewsId = id;
            return news;
        }

        public List<News> GetAll()
        {
            var sql = "SELECT * FROM News ORDER BY NewsId DESC";
            return _db.Query<News>(sql).ToList();
        }

        public News GetById(int id)
        {
            var sql = "SELECT * FROM News WHERE NewsId = @NewsId";
            return _db.Query<News>(sql, new { NewsId = id }).FirstOrDefault();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM News WHERE NewsId = @id";
            _db.Execute(sql, new { id });
        }

        public News Update(News news)
        {
            var sql = "UPDATE News SET Title = @Title, Content = @Content, " +
                "PublicationDate = @PublicationDate, Location = @Location, ImageUrl = @ImageUrl " +
                "WHERE NewsId = @NewsId";
            _db.Execute(sql, news);
            return news;
        }

        public List<News> GetLatestNewsExcludingCurrent(int currentNewsId)
        {
            var sql = @"SELECT TOP 3 * FROM News WHERE NewsId != @NewsId ORDER BY PublicationDate DESC";

            return _db.Query<News>(sql, new { NewsId = currentNewsId }).ToList();
        }

        public List<News> GetNewsByPage(int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;
            var sql = @"SELECT * FROM News ORDER BY PublicationDate DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            return _db.Query<News>(sql, new { Offset = offset, PageSize = pageSize }).ToList();
        }



    }
}
