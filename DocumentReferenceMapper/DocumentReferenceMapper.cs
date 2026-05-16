using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using IDocumentReferenceMapper;
using Interface.Models.DocumentReferenceModels;
using Interface.Models.InterfaceModels;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentReferenceMapper
{
  
    public class DocumentReferenceMapper : IDocumentReferenceMapper.IDocumentReferenceMapper
    {
        private readonly IDocumentReferenceService.IDocumentReferenceService _documentReferenceService;
        private readonly IMapper _mapper;
        private readonly LookUpScoped _lookUpScoped;
        public DocumentReferenceMapper(LookUpScoped lookUpScoped, IMapper mapper,IDocumentReferenceService.IDocumentReferenceService documentReferenceService)
        {
            _documentReferenceService = documentReferenceService;
            _mapper = mapper;
            _lookUpScoped = lookUpScoped;
            _lookUpScoped.FetchAllLookup();
        }

        public async Task<CustomBundle> GetPatientDocument(int DocumentId, long AssignedUserId)
        {
            PatientDocumentMultipleDto documentDto = await _documentReferenceService.GetPatientDocument(DocumentId, AssignedUserId);
            var documentReferenceFHIR = _mapper.Map<DocumentReferenceR4>(documentDto);

            var docTypeGL = _lookUpScoped.GetDocumentType(documentDto.DocumentType);
            var docString = documentDto.DocumentType.ToString();
            documentReferenceFHIR.Type = new CodeableConcept
            {
                Text = docTypeGL.Type +", "+docTypeGL.Description,
                Coding = new List<Coding>
                    {
                        new Coding
                        {
                            Code = docString,
                            System = "http://terminology.hl7.org/CodeSystem/condition-code",
                            Display = docTypeGL.Text
                        }
                    }
            };

            var customBundle = new CustomBundle
            {
                Entry = new List<CustomBundleEntry>(),
                Type = Bundle.BundleType.Searchset.ToString(),
                Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
           
                customBundle.Entry.Add(new CustomBundleEntry { Resource = documentReferenceFHIR });
            
            customBundle.Total = customBundle.Entry.Count;
            customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
             
         }
    }
}
