using System.ComponentModel.DataAnnotations.Schema;

namespace ContactAPI.Models
{
    [Table("subcategories")]
    public class Subcategory
    {
        public int subcategoryid { get; set; }
        public string subcategoryname { get; set;}

        public Subcategory() { }

    }
}
