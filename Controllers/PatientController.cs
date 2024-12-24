using AGSR.TestTask.Contexts;
using AGSR.TestTask.Models;
using AGSR.TestTask.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace AGSR.TestTask.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly AgsrContext _agsrContext;
    private readonly IMapper _mapper;

    public PatientController(AgsrContext agsrContext, IMapper mapper)
    {
        _agsrContext = agsrContext;
        _mapper = mapper;
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

        var patientVM = _mapper.Map<PatientWithOptionalNameViewModel>(patient);

        return Ok(patientVM);
    }

    [SwaggerOperation(Summary = "Add single Patient.")]
    [HttpPost]
    public async Task<ActionResult> AddPatient(PatientViewModel patientVM)
    {
        var patient = _mapper.Map<PatientModel>(patientVM);

        var patientEntity = await _agsrContext.Patients.AddAsync(patient);
        await _agsrContext.SaveChangesAsync();

        return Ok(patientEntity.Entity);
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
