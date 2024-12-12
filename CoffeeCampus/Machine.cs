using System.ComponentModel.DataAnnotations;

namespace CoffeeCampus
{
    public class Machine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }
    }
}
