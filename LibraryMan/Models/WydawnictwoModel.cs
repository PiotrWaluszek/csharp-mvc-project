using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMan.Models{
public class WydawnictwoModel
{
    [Key]
    [Display(Name = "Wydawnictwo")]
    [Required]
    public required string PublisherName { get; set; }

    [Display(Name = "Miejscowość")]
    [Required]
    public required string City { get; set; }

    [Display(Name = "Kraj")]
    [Required]
    public required string Country { get; set; }

    [Display(Name = "Rok Założenia")]
    public int Founded { get; set; }

    [Display(Name = "Opis")]
    public required string Description { get; set; }

}
}
