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


    /// <summary>
    /// In order to avoid unintentional overlapping of ranges, requests can increase the specificity of their request. For example, the test for sa2013-01-14T00:00:00 is a much clearer test.
    /// </summary>
    [SwaggerOperation(Summary = "List all Patients, param example: 'eq2013-01-14T00:00:00Z'.")]
    [HttpGet("birthDate")]
    public async Task<ActionResult> GetPatientsByBirthDate([FromQuery] string birthDate = "eq2013-01-14T00:00:00Z")
    {
        var query = _agsrContext.Patients.AsQueryable();

        if (birthDate == null)
        {
            var result = _mapper.Map<List<PatientWithOptionalNameViewModel>>(query);
            return Ok(result);
        }

        try
        {
            var filterType = birthDate.Substring(0, 2).ToString().ToLower();
            var datePart = birthDate.Substring(2).ToString();
            var date = DateTimeOffset.Parse(datePart, CultureInfo.InvariantCulture);

            var param = Expression.Parameter(typeof(PatientModel), "p");
            var member = Expression.Property(param, "BirthDate");

            switch (filterType)
            {
                case "eq":
                    var expressionEQ = DynamicExpressions.CreateEQExpression(param, member, date); // eq2013-01-14T00:00:00Z
                    query = query.Where(expressionEQ);
                    break;
                case "ne":
                    var expressionNE = DynamicExpressions.CreateNEExpression(param, member, date); // ne2013-01-14T00:00:00Z
                    query = query.Where(expressionNE);
                    break;

                case "lt":
                    query = query.Where(x => x.BirthDate < date.UtcDateTime);
                    break;
                case "gt":
                    query = query.Where(x => x.BirthDate > date.UtcDateTime);
                    break;

                case "ge":
                    query = query.Where(x => x.BirthDate >= date.UtcDateTime);
                    break;
                case "le":
                    query = query.Where(x => x.BirthDate <= date.UtcDateTime);
                    break;

                case "sa": // starts after
                    var rightDate = date.UtcDateTime;
                    query = query.Where(x => rightDate < x.BirthDate);
                    break;
                case "eb": // ends before
                    var leftDate = date.UtcDateTime;
                    query = query.Where(x => x.BirthDate < leftDate);
                    break;
                case "ap": // the resource value is approximately the same to the parameter value
                    var leftDateAP = date.UtcDateTime;
                    var rightDateAP = date.UtcDateTime;
                    query = query.Where(x => leftDateAP <= x.BirthDate && x.BirthDate <= rightDateAP);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(filterType);
            }

            var queryString = query.ToQueryString();
            var queryList = query.ToList();

            return Ok(_mapper.Map<List<PatientWithOptionalNameViewModel>>(queryList));
        }
        catch (Exception e)
        { 
            return BadRequest(e.Message);
        }
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
