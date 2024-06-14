using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMan.Models{
public class KsiazkaModel
{
    [Key]
    [Display(Name = "Nazwa")]
    [Required]
    public required string? BookName { get; set; }

    [Display(Name = "Wydawnictwo")]
    [Required]
    public required string PublisherName { get; set; }

    [Display(Name = "Gatunek")]
    [Required]
    public required string Genre { get; set; }

    [Display(Name = "Średnia Ocena")]
    public double AverageRating { get; set; }

    // Relacja wiele do jednego z Browar
    [ForeignKey("PublisherName")]
    public WydawnictwoModel? WydawnictwoModel { get; set; }

}
}
