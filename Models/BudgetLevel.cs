using System.Text.Json.Serialization;

public class BudgetLevel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore] // Prevents cycles when serializing
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [JsonIgnore]
    public virtual ICollection<Destination> Destinations { get; set; } = new List<Destination>();
}
