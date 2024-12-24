using AGSR.TestTask.Models;
using AGSR.TestTask.ViewModels;
using AutoMapper;

namespace AGSR.TestTask.MappingProfiles.Patients;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<PatientModel, PatientViewModel>();
        CreateMap<PatientViewModel, PatientModel>();

        CreateMap<PatientModel, PatientWithOptionalNameViewModel>()
            .ForPath(m => m.Name.Id, opt => opt.MapFrom(src => src.Id))
            .ForPath(m => m.Name.Use, opt => opt.MapFrom(src => src.Use))
            .ForPath(m => m.Name.Family, opt => opt.MapFrom(src => src.Family))
            .ForPath(m => m.Name.Given, opt => opt.MapFrom(src => src.Given))
            .ForMember(m => m.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(m => m.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(m => m.Active, opt => opt.MapFrom(src => src.Active));
        CreateMap<PatientWithOptionalNameViewModel, PatientModel>()
            .ForPath(m => m.Id, opt => opt.MapFrom(src => src.Name.Id))
            .ForPath(m => m.Use, opt => opt.MapFrom(src => src.Name.Use))
            .ForPath(m => m.Family, opt => opt.MapFrom(src => src.Name.Family))
            .ForPath(m => m.Given, opt => opt.MapFrom(src => src.Name.Given))
            .ForMember(m => m.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(m => m.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(m => m.Active, opt => opt.MapFrom(src => src.Active));
    }
}
