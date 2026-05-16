using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Interface.Models.InterfaceModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class PatientR4B 
    {

        public string ResourceType => "Patient"; 

        public string Id { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        public bool Active { get; set; }
        public List<HumanName> Name { get; set; }
        public List<ContactPoint> Telecom { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public List<Address> Address { get; set; }
        public CodeableConcept MaritalStatus { get; set; }
        public List<Patient.CommunicationComponent> Communication { get; set; }
        public List<Extension> Extension { get; set; }

    }

    public class CustomBundle
    {
        public string ResourceType { get; } = "Bundle";
        public string Id { get; set;}
        public Meta Meta { get; set; }
        public string Type { get; set; }
        public int Total { get; set; }
        public List<CustomBundleEntry> Entry { get; set; }
        
    }

    public class CustomBundleEntry
    {
       // public string ResourceType { get; } = "Patient";
        public object Resource { get; set; }
    }
 


}