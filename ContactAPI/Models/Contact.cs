using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Models
{
    [Table("contacts")]
    public class Contact
    {
        public int contactid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int categoryid { get; set; }
        public string subcategory { get; set; }
        public string phonenumber { get; set; }
        public DateTime birthdate { get; set; }

        public Contact() { }
    }
}
