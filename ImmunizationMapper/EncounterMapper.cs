using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.GeneralLookups;
using Interface.Models.InterfaceModels;
using IPractitionerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class EncounterMapper : IEncounterMapper.IEncounterMapper
    {
        private IMapper _mapper;
        private IEncounterService.IEncounterService _encounterService;
        private readonly IProviderService _providerService;
        private readonly LookUpScoped _lookUpScoped;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;

        public EncounterMapper (IMapper mapper,
            IEncounterService.IEncounterService encounterService,
            IProviderService providerService,
            LookUpScoped lookUpScoped,
            IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _mapper = mapper;
            _encounterService = encounterService;
            _providerService = providerService;
            _lookUpScoped = lookUpScoped;
            _lookUpScoped.FetchAllLookup();
            _fhirSerializer = fhirSerializer;
        }
        
        private string RoleName(int? providerId)
        {
          //  var provider = await _providerService.GetProviderV2((long)providerId);
            var generalLookup = _lookUpScoped.GetRoles((int)providerId);
            return generalLookup.Text;
        }

        
    //for fhir parser single record Bundle
    public Bundle MapSync(PatientResourceRecords inputs)
        {
           // var encounterDTOs =  _encounterService.GetPatientEncountersPaged(inputs.PatientId.Value).GetAwaiter().GetResult();
            var encounterInfo =  _encounterService.GetEncounterById(inputs.EncounterId).GetAwaiter().GetResult();

            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
           //     Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
            var count = 0;

         //   foreach (var encounterInfo in encounterDTOs.EncounterInfos)
         //   {
                var encounterFHIR = _mapper.Map<Encounter>(encounterInfo);
             

            try
            {
                Encounter.ParticipantComponent partComp;
                List<Encounter.ParticipantComponent> partComps = new List<Encounter.ParticipantComponent>();
                List<CodeableConcept> Type;
                var providerDTO = _providerService.GetProviderV2((long)encounterInfo.ProviderId).GetAwaiter().GetResult();
                var roleGL = _lookUpScoped.GetRoles((int)providerDTO.RoleId);
                var typeGL = new GeneralLookup();
                var providerId = 0;

                if (encounterInfo.EncounterTypeId.HasValue)
                {
                    typeGL = _lookUpScoped.GetEncounterType(encounterInfo.EncounterTypeId.Value);
                }

                if (encounterInfo.ProviderId.HasValue)
                {
                    providerId = encounterInfo.ProviderId.Value;
                }

                partComp = new Encounter.ParticipantComponent
                {

                    Individual = new ResourceReference($"Practitioner/{providerId}"), // Assuming ProviderId is the FHIR ID for provider
                    Type = new List<CodeableConcept>
                    {
                        new CodeableConcept
                        {
                            
                            Text = roleGL.Text, // Assuming FullName is the combination of FirstName and LastName
                            Coding = new List<Coding>
                            {
                                new Coding
                                { System = "http://terminology.hl7.org/CodeSystem/v3-ParticipationType",
                                    Code = getencounterTypeString(roleGL),  
                               //     System = "your_code_system_url_here",  
                                    Display = roleGL.Text
                                }
                            }
                        }
                    }
                };
                partComps.Add(partComp);

                Type = new List<CodeableConcept>
                 {
                new CodeableConcept
                {
                    Text = encounterInfo.EncounterTypeName,
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            Code =  getencounterTypeString(typeGL),
                            // Assuming EncounterTypeId is the FHIR code
                            Display = encounterInfo.EncounterTypeName
                        }
                    }
                }
                 };

                encounterFHIR.Type = Type;
                encounterFHIR.Participant = partComps;
                
                var jsonString = _fhirSerializer.FhirR4SerializeResource(encounterFHIR);
                var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized, Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Encounter" } });
                count++;
            }
            catch (Exception ex)
            {
                HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {encounterFHIR.TypeName}  ");
            }
          //  }

            customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
        private string getencounterTypeString(GeneralLookup obj)
        {
            if (string.IsNullOrEmpty(obj.Code) )
            {
                return "--";
            }
            else if (obj.Code.Trim() == "")
            { 
                return "--";
            }
            
            else { return obj.Code.Trim(); }
        }
    }
}
