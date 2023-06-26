using System.ComponentModel.DataAnnotations;

namespace Mealthy.Mealthy.Resources;

public class SaveMarketResource
{
    [Required]
    public string storeName { get; set; }
    [Required]
    public string description { get; set; }
    [Required]
    public string firstName { get; set; }
    [Required]
    public string lastName { get; set; }
    [Required]
    public string ruc { get; set; }
    [Required]
    public string email { get; set; }
    [Required]
    public string password { get; set; }
    [Required]
    public string location { get; set; }
    [Required]
    public string phone { get; set; }
    [Required]
    public string photo { get; set; }
}