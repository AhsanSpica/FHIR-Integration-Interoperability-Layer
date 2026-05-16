using AutoMapper;
using Hl7.Fhir.Model;
 using Interface.Models.DocumentReferenceModels;
using Interface.Models.InterfaceModels;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace FHIRMappers
{
    public class DocumentReferenceMappingProfile : Profile
    {
        public DocumentReferenceMappingProfile()
        {
            CreateMap<PatientDocumentMultipleDto, DocumentReferenceR4>()
          .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow // Assuming UpdatedAt is the last updated time
                }))
                .ForMember(dest => dest.Custodian, opt => opt.MapFrom(src => new ResourceReference($"Practitioner/{src.AssignedBy}")))

              // .ForMember(dest => dest.Type, opt => opt.MapFrom(src => MapDocumentType(src) ))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => new ResourceReference($"Patient/{src.PatientId}")))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.HasValue ? src.Date.Value.ToString("yyyy-MM-ddTHH:mm:sszzz") : null))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.AssignedBy != null ? new List<ResourceReference> { new ResourceReference($"Practitioner/{src.AssignedBy}") } : null))

                .ForMember(dest => dest.Extension, opt => opt.Ignore())
        .ForMember(dest => dest.Content, opt => opt.MapFrom(src => ConvertContentToAttachment(src)));
            //.ForMember(dest => dest.Content, opt => opt.MapFrom(src => new List<DocumentReference.ContentComponent>
            //{
            //new DocumentReference.ContentComponent
            //{
            //    Attachment = new Attachment
            //    {
            //        ContentType = (string)src.Extension,
            //        Url = src.DocumentUri,
            //        Title = src.DisplayName,
            //        // You may need to handle the data field if it's not directly available in your DTO
            //    },
            //    Format = new Coding
            //    {
            //        System = "urn:oid:1.3.6.1.4.1.19376.1.2.3",
            //        Code = "urn:ihe:iti:xds:2017:mimeTypeSufficient"
            //    }
            //}
            //}))
            //  .ForMember(dest => dest.Context, opt => opt.MapFrom(src => getpatientContext(src)));

        }
        // private DocumentReference.ContextComponent getpatientContext(PatientDocumentMultipleDto src)
        // {
        //     return new DocumentReference.ContextComponent {
        //         Encounter = new List <ResourceReference> { new ResourceReference($"Patient/{src.enc}") }
        //     };
        ////     var eventCodeable = new CodeableConcept();
        ////     foreach (var docActionObj in src.documentActions)
        ////     {
        ////         var coding =
        ////                  new Coding
        ////                  {
        ////                      Code = docActionObj.IsSigned.Value ? docActionObj.IsSigned.ToString() : docActionObj.IsReviewed.ToString(),
        ////                      System = "http://terminology.hl7.org/CodeSystem/condition-code",
        ////                      Display = src.ReasonString
        ////                  }



        ////     }

        ////     return new DocumentReference.ContextComponent
        ////     {
        ////         Event = src.documentActions != null ? src.documentActions.Select(action =>
        ////             new CodeableConcept("YourSystem", action.IsSigned ? action.IsSigned.ToString() : , action.Id.ToString() + " - " + action.UserName)
        ////).ToList() : null
        ////     };
        // }
      
        private string getSystemUrl(PatientDocumentMultipleDto patientDocumentMultipleDto)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}/patientdocument/getpatientdocument?DocumentId={patientDocumentMultipleDto.Id}";
        }
        private List<Attachment> ConvertContentToAttachment(PatientDocumentMultipleDto src)
        {
            if (src.DocumentUri == null)
                return null;

            var attachments = new List<Attachment>();

            
                var attachment = new Attachment
                {
                    ContentType =  src.Extension,
                            Url = src.DocumentUri,
                            Title = src.DisplayName,
                };

                attachments.Add(attachment);
            

            return attachments;
        }
        
    }
}
