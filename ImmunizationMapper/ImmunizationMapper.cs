using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using IImmunizationService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.GeneralLookups;
using Interface.Models.ImmunizationModels;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class ImmunizationMapper : IEncounterMapper.IImmunizationMapper
    {
        private readonly IImmunizationService.IImmunizationService _immunizationService; 
        private readonly LookUpScoped _lookUpScoped;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
        public ImmunizationMapper(IMapper mapper, 
            IImmunizationService.IImmunizationService immunizationService,
            LookUpScoped lookUpScoped,
            IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _immunizationService = immunizationService;
            _mapper = mapper;
            _lookUpScoped = lookUpScoped;

            _lookUpScoped.FetchAllLookup();
            _fhirSerializer = fhirSerializer;
        }
       
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            // return Map(inputs).GetAwaiter().GetResult();

            var immunizationDTOList =  _immunizationService.GetAllImmunization(inputs.PatientId, inputs.ResourceId).GetAwaiter().GetResult();

            var refuseReason = new GeneralLookup();
            var vaccineSite = new GeneralLookup();
            var vaccineRoute = new GeneralLookup();
            var vaccineFundingSource = new GeneralLookup();
            var vaccineFundingProgram = new GeneralLookup();

            var bundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
             //   Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
            var count = 0;
            foreach (var immunizationDTO in immunizationDTOList)
            {
                var immunizationFHIR = _mapper.Map<Immunization>(immunizationDTO);

                try
                {
                    if (immunizationDTO.RefuseReason != null && immunizationDTO.RefuseReason != 0)
                {
                    refuseReason = _lookUpScoped.GetVaccineRefusalReason((int)immunizationDTO.RefuseReason);
                    vaccineSite = _lookUpScoped.GetVaccineSite((int)immunizationDTO.Site);
                    vaccineRoute = _lookUpScoped.GetVaccineRoute((int)immunizationDTO.Route);
                    vaccineFundingSource = _lookUpScoped.GetVaccineFundingSource((int)immunizationDTO.FundingSource);
                    vaccineFundingProgram = _lookUpScoped.GetVaccineFundingProgram((int)immunizationDTO.FundingProgram);

                    immunizationFHIR.StatusReason = new CodeableConcept
                    {
                        Coding = new List<Coding>
                    {
                    new Coding
                    {
                        Code = refuseReason.Code,
                        Display = refuseReason.Text,
                        System = "http://terminology.hl7.org/CodeSystem/v3-ActReason"
                    }
                    }
                    };
                        if (!string.IsNullOrEmpty(vaccineSite.Code))
                        {
                            immunizationFHIR.Site = new CodeableConcept
                            {
                                Coding = new List<Coding>
                    {
                    new Coding
                    {
                        Code = vaccineSite.Code,
                        Display = vaccineSite.Text,
                        System = "http://terminology.hl7.org/CodeSystem/v3-ActSite"
                    }
                    }

                            };
                        }

                        if (!string.IsNullOrEmpty(vaccineRoute.Code))
                        {
                            immunizationFHIR.Route = new CodeableConcept
                            {
                                Coding = new List<Coding>
                    {
                    new Coding
                    {
                        Code = vaccineRoute.Code,
                        Display = vaccineRoute.Text,
                        System = "http://terminology.hl7.org/CodeSystem/v3-RouteOfAdministration"
                    }
                    }
                            };
                        }

                        if (!string.IsNullOrEmpty(vaccineFundingSource.Code))
                        {
                            immunizationFHIR.FundingSource = new CodeableConcept
                            {
                                Coding = new List<Coding>
                    {
                    new Coding
                    {
                        Code = vaccineFundingSource.Code,
                        Display = vaccineFundingSource.Text,
                        System = "http://terminology.hl7.org/CodeSystem/immunization-funding-source"
                    }
                    }
                            };
                        }

                        if (!string.IsNullOrEmpty(vaccineFundingProgram.Code))
                        {
                            immunizationFHIR.ProgramEligibility = new List<CodeableConcept>
                    {

                        new CodeableConcept
                    {
                         Coding = new List<Coding>
                    {
                    new Coding
                    {
                        Code = vaccineFundingProgram.Code,
                        Display = vaccineFundingProgram.Text,
                        System = "http://loinc.org"
                    }
                    }
                    }
                    };
                        }
                }

                var jsonString = _fhirSerializer.FhirR4SerializeResource(immunizationFHIR);

                var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                bundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized, Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Immunization" } });

               // customBundle.Entry.Add(new Bundle.EntryComponent { Resource = immunizationFHIR });
               count++;
                }
                catch (Exception ex)
                {
                    HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {immunizationFHIR.TypeName}  ");
                }
            }
           // customBundle.Total = customBundle.Entry.Count;
          //  customBundle.Id = Guid.NewGuid().ToString();

            return bundle;
        }
       
    }
}
