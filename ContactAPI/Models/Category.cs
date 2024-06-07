using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Models
{
    [Table("categories")]
    public class Category
    {
        public int categoryid { get; set; }
        public string categoryname { get; set; }
        public Category() { }
    }
}
