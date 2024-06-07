using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Models
{
    [Table("users")]
    public class User
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public User() { }
    }
}
