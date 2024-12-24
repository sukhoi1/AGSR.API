using AGSR.TestTask.Models;

namespace AGSR.TestTask.ViewModels;

public class PatientWithOptionalNameViewModel
{
    public PatientNameViewModel Name { get; set; }
    public GenderEnum? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public bool? Active { get; set; }
}
 