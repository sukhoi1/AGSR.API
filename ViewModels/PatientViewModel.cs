using AGSR.TestTask.Models;

namespace AGSR.TestTask.ViewModels;

public class PatientViewModel
{
    public Guid Id { get; set; }
    public string? Use { get; set; }
    public string Family { get; set; }
    public string[]? Given { get; set; }
    public string? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool? Active { get; set; }
}
