using System.Globalization;
using System.Linq.Expressions;
using AGSR.TestTask.Contexts;
using AGSR.TestTask.DynamicQueryExpressions;
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

    [SwaggerOperation(Summary = "List all Patients.")]
    [HttpGet("patients")]
    public async Task<ActionResult> GetPatients()
    {
        var patients = await _agsrContext.Patients.ToListAsync();

        var patientVMs = _mapper.Map<List<PatientWithOptionalNameViewModel>>(patients);

        return Ok(patientVMs);
    }

    [SwaggerOperation(Summary = "List all Patients, param example: 'eq2013-01-14T00:00:00Z'. eq,ne,lt,gt,ge,le takes time as param; sa,eb,ap takes date as range.")]
    [HttpGet("birthDate")]
    public async Task<ActionResult> GetPatientsByBirthDate([FromQuery] string[] date) // eq2013-01-14T00:00:00Z
    {
        var query = _agsrContext.Patients.AsQueryable();

        if (date?.Length == 0)
        {
            var result = _mapper.Map<List<PatientWithOptionalNameViewModel>>(query);
            return Ok(result);
        }

        foreach (var birthDate in date)
        {
            var filterType = birthDate.Substring(0, 2).ToString().ToLower();
            var datePart = birthDate.Substring(2).ToString();
            var parsedDate = DateTimeOffset.Parse(datePart, CultureInfo.InvariantCulture);
            var param = Expression.Parameter(typeof(PatientModel), "p");
            var member = Expression.Property(param, "BirthDate");

            var leftDate = parsedDate.UtcDateTime.Date;
            var rightDate = parsedDate.UtcDateTime.AddDays(1).Date;

            switch (filterType)
            {
                case "eq":
                    //var expressionEQ = DynamicExpressions.CreateEQExpression(param, member, parsedDate); // eq2013-01-14T00:00:00Z
                    query = query.Where(x => x.BirthDate == parsedDate);
                    break;
                case "ne":
                    //var expressionNE = DynamicExpressions.CreateNEExpression(param, member, parsedDate); // ne2013-01-14T00:00:00Z
                    query = query.Where(x => x.BirthDate != parsedDate);
                    break;

                case "lt":
                    query = query.Where(x => x.BirthDate < parsedDate.UtcDateTime);
                    break;
                case "gt":
                    query = query.Where(x => x.BirthDate > parsedDate.UtcDateTime);
                    break;

                case "ge":
                    query = query.Where(x => x.BirthDate >= parsedDate.UtcDateTime);
                    break;
                case "le":
                    query = query.Where(x => x.BirthDate <= parsedDate.UtcDateTime);
                    break;

                case "sa": // starts after
                    query = query.Where(x => rightDate <= x.BirthDate);
                    break;
                case "eb": // ends before
                    query = query.Where(x => x.BirthDate < leftDate);
                    break;
                case "ap": // the resource value is approximately the same to the parameter value
                    query = query.Where(x => leftDate <= x.BirthDate && x.BirthDate < rightDate);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(filterType);
            }
        }

        var queryString = query.ToQueryString();
        var queryList = query.ToList();

        return Ok(_mapper.Map<List<PatientWithOptionalNameViewModel>>(queryList));
    }

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

    [SwaggerOperation(Summary = "Update single Patient.")]
    [HttpPut()]
    public async Task<ActionResult> UpdatePatient(PatientViewModel patientViewModel)
    {
        var patient = await _agsrContext.Patients.FirstOrDefaultAsync(x => x.Id == patientViewModel.Id);
        if (patient == null)
        {
            return NotFound();
        }

        patient.Use = patientViewModel.Use;
        patient.Family = patientViewModel.Family;
        patient.Given = patientViewModel.Given;
        patient.Gender = patientViewModel.Gender;
        patient.BirthDate = patientViewModel.BirthDate;
        patient.Active = patientViewModel.Active;

        _agsrContext.Patients.Update(patient);
        await _agsrContext.SaveChangesAsync();

        var updatedPatientVM = _mapper.Map<PatientViewModel>(patient);
        return Ok(updatedPatientVM);
    }

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
