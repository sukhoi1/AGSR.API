using AGSR.TestTask.Contexts;
using AGSR.TestTask.Models;
using AGSR.TestTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace AGSR.TestTask.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly AgsrContext _agsrContext;

    public PatientController(AgsrContext agsrContext)
    {
        _agsrContext = agsrContext;
    }

    //[SwaggerOperation(Summary= "List all Patients.")]
    //[HttpGet]
    //public async Task<ActionResult> GetPatients()
    //{
    //    var patients = await _agsrContext.Patients.ToListAsync();

    //    var patientsVM = null;

    //    return patientsVM;
    //}

    [SwaggerOperation(Summary = "Get single Patient.")]
    [HttpGet("{patientId}")]
    public async Task<ActionResult> GetPatient(Guid patientId)
    {
        var patient = await _agsrContext.Patients.FirstOrDefaultAsync(x => x.Id == patientId);
        if (patient == null)
        {
            return NotFound();
        }

        return Ok(new PatientWithOptionalNameViewModel
        {
            Name = new PatientNameViewModel
            {
                Id = patient.Id,
                Family = patient.Family,
                Given = patient.Given,
                Use = patient.Use
            },
            Active = patient.Active,
            BirthDate = patient.BirthDate,
            Gender = patient.Gender,
        });
    }

    [SwaggerOperation(Summary = "Add single Patient.")]
    [HttpPost]
    public async Task<ActionResult> AddPatient(PatientViewModel patientViewModel)
    {
        var patient = await _agsrContext.Patients.AddAsync(new PatientModel
        {
            Family = patientViewModel.Family,
            Given = patientViewModel.Given,
            Use = patientViewModel.Use,
            Active = patientViewModel.Active,
            BirthDate = patientViewModel.BirthDate,
            Gender = patientViewModel.Gender
        });

        return Ok(new PatientWithOptionalNameViewModel
        {
            Name = new PatientNameViewModel
            {
                Id = patient.Entity.Id,
                Family = patient.Entity.Family,
                Given = patient.Entity.Given,
                Use = patient.Entity.Use
            },
            Active = patient.Entity.Active,
            BirthDate = patient.Entity.BirthDate,
            Gender = patient.Entity.Gender,
        });
    }

    //[SwaggerOperation(Summary = "Update single Patient.")]
    //[HttpPut()]
    //public async Task<ActionResult> UpdatePatient(PatientViewModel patientViewModel)
    //{
    //    var entity = await _agsrContext.Patients.FirstOrDefaultAsync(x => x.Id == patientViewModel.Id);
    //    if (entity == null)
    //    {
    //        return NotFound();
    //    }

    //    entity.BirthDate = patientViewModel.BirthDate;
    //    entity.Given = patientViewModel.Given;

    //    await _agsrContext.SaveChangesAsync();

    //    return Ok();
    //}

    [SwaggerOperation(Summary = "Delete single Patient.")]
    [HttpDelete()]
    public async Task<ActionResult> DeletePatient(Guid patientId)
    {
        var patient = await _agsrContext.FindAsync<PatientModel>(patientId);
        if (patient == null)
        {
            return NotFound();
        }

        _ = _agsrContext.Remove(patient);
        await _agsrContext.SaveChangesAsync();

        return Ok();
    }
}
