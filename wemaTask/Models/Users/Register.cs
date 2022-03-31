namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;

public class Register
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Confirmpassword { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? Stateofresidence { get; set; }

    [Required]
    public string? Lga { get; set; }

    [Required]
    public DateTime createdDate { get; set; }


}