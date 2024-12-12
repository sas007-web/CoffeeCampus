using System.ComponentModel.DataAnnotations;

namespace CoffeeCampus
{
    public class User
    {
        [Key] // Marker primærnøglen
        public int Id { get; set; }

        [Required] // Angiv at Name er påkrævet
        public string Name { get; set; }

        public string Email { get; set; }
    }
}
