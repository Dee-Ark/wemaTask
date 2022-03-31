namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Stateofresidence { get; set; }
    public string? Lga { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }
}