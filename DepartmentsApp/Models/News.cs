using System.ComponentModel.DataAnnotations;

namespace DepartmentsApp.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }  
        public DateTime PublicationDate { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }  
    }
}
