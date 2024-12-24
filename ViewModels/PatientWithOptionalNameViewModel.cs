namespace AGSR.TestTask.ViewModels;

public class PatientWithOptionalNameViewModel
{
    public PatientNameViewModel Name { get; set; }
    public string? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool? Active { get; set; }
}
 