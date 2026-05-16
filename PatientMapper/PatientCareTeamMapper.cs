using AutoMapper;
using FHIR.Interface.Helpers;
using FHIRMappers;
using GlobalHelpers;
using Hl7.Fhir.Model;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.GeneralLookups;
using Interface.Models.InterfaceModels;
using Interface.Models.Patients;
using IPatientMapper;
using IPatientService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientMapper
{
    public class PatientCareTeamMapper : IPatientCareTeamMapper
    {
        private readonly IMapper _mapper;
        private readonly LookUpScoped _lookUpScoped;
        private readonly IPatientCareTeamService _patientCareTeamService;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
         public PatientCareTeamMapper(LookUpScoped lookUpScoped, IMapper mapper, IPatientCareTeamService patientCareTeamService,
             IFhirSerializer.IFhirSerializer fhirSerializer) 
        {
            _mapper = mapper;
            _lookUpScoped = lookUpScoped;
            _lookUpScoped.FetchAllLookup();
            _patientCareTeamService = patientCareTeamService;
            _fhirSerializer = fhirSerializer;
        }
       
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            // return Map(inputs).GetAwaiter().GetResult();

            var careTeamDTOs = _patientCareTeamService.GetListByPatientID(inputs.PatientId,inputs.ResourceId).GetAwaiter().GetResult();
            var careTeamMembers = _patientCareTeamService.GetMemeberListByPatientID(inputs.PatientId.Value).GetAwaiter().GetResult();
            
            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
             //   Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };


            foreach (var careTeamDTO in careTeamDTOs)
            {
                var careTeam = _mapper.Map<CareTeam>(careTeamDTO);
                var roleName = new GeneralLookup();
                var count = 0;

                List<CareTeam.ParticipantComponent> participants = new List<CareTeam.ParticipantComponent>();

                foreach (var member in careTeamMembers)
                {
                    if (member.Specialty != null)
                    { member.SpecialtyName = _lookUpScoped.GetSpeciality((int)member.Specialty).Name; }
                    else { member.SpecialtyName = ""; }

                    if (member.RelationWithPatient != null) { roleName = _lookUpScoped.GetCareTeamMemberRelation((int)member.RelationWithPatient); }

                    if (careTeamDTO.Id.Equals(member.PatientCareTeamId.ToString()))
                    {
                        var participant = _mapper.Map<CareTeam.ParticipantComponent>(member);
                        var roles = new List<CodeableConcept>();
                        roles.Add(new CodeableConcept
                        {
                            Coding = new List<Coding>
                          {
                            new Coding
                            {
                              System = "http://terminology.hl7.org/CodeSystem/care-team-role",
                              Code = roleName.Code,
                              Display = roleName.Text
                            }
                          }
                        });
                        participant.Role = roles;
                        participants.Add(participant);
                    }
                }
                careTeam.Participant = participants;

                var jsonString = _fhirSerializer.FhirR4SerializeResource(careTeam);


                var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);


                customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                    Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "CareTeam" }
                });
            }
           
            //foreach (var careTeam in careTeams)
            //{
            //    customBundle.Entry.Add(new Bundle.EntryComponent { Resource = careTeam });
            //}
          //  customBundle.Total = customBundle.Entry.Count;
          //  customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
    }
}
