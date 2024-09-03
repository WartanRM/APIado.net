using System.ComponentModel.DataAnnotations;

namespace APIDemo.Models
{
    public class PeopleData
    {
        [Key]
        public String Id { get; set; }
        public int Age { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
}
