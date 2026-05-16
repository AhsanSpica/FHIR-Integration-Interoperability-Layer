using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFhirSerializer
{
    public interface IFhirSerializer
    {
         public string FhirR4Serialize(Bundle fhirObject);
       public  Resource FhirR4DeSerialize(string fhirObject);
       public string FhirR4SerializeResource(Resource mappedObject);
        public Bundle FhirR4DeSerializeBundle(string jsonString);
    }
}
