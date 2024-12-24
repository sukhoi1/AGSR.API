namespace AGSR.TestTask.Models;

public class PatientModel
{
    public Guid Id { get; set; }
    public string? Use { get; set; }
    public string Family { get; set; }
    public string[]? Given { get; set; }
    public GenderEnum? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool? Active { get; set; }
}

public enum GenderEnum
{
    Male,
    Female,
    Other,
    Unknown
}
