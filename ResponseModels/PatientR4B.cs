using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ResponseModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class PatientR4B
    {
        public string ResourceType { get; set; } = "Patient";
        public string Id { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        public bool Active { get; set; }
        public List<HumanName> Name { get; set; }
        public List<ContactPoint> Telecom { get; set; }
        public AdministrativeGender Gender { get; set; }
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
        public PatientR4B Resource { get; set; }
    }

    //public class Address
    //{
    //    public string use { get; set; }
    //    public List<string> line { get; set; }
    //    public string city { get; set; }
    //    public string state { get; set; }
    //    public string postalCode { get; set; }
    //    public string country { get; set; }
    //    public Period period { get; set; }
    //}

    //public class Coding
    //{
    //    public string system { get; set; }
    //    public string code { get; set; }
    //    public string display { get; set; }
    //}

    //public class Communication
    //{
    //    public Language language { get; set; }
    //}

    //public class Extension
    //{
    //    public string url { get; set; }
    //    public string valueCode { get; set; }
    //    public List<Extension> extension { get; set; }
    //    public ValueCoding valueCoding { get; set; }
    //    public string valueString { get; set; }
    //}

    //public class Identifier
    //{
    //    public string system { get; set; }
    //    public string value { get; set; }
    //}

    //public class Language
    //{
    //    public List<Coding> coding { get; set; }
    //    public string text { get; set; }
    //}

    //public class MaritalStatus
    //{
    //    public List<Coding> coding { get; set; }
    //    public string text { get; set; }
    //}

    //public class Meta
    //{
    //    public DateTime lastUpdated { get; set; }
    //}

    //public class Name
    //{
    //    public string use { get; set; }
    //    public string text { get; set; }
    //    public string family { get; set; }
    //    public List<string> given { get; set; }
    //}

    //public class Period
    //{
    //    public DateTime start { get; set; }
    //}



    //public class Telecom
    //{
    //    public string system { get; set; }
    //    public string value { get; set; }
    //    public string use { get; set; }
    //}

    //public class ValueCoding
    //{
    //    public string system { get; set; }
    //    public string code { get; set; }
    //    public string display { get; set; }
    //}


}