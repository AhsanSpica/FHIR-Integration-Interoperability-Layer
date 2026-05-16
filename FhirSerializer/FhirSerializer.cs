using IFhirSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace FhirSerializer
{
    public class FhirSerializer : IFhirSerializer.IFhirSerializer
{
        private readonly FhirJsonSerializer _serializer;
        private readonly FhirJsonParser _parser;

        public FhirSerializer()
        {
            
            _serializer = new FhirJsonSerializer(new SerializerSettings
            {
                Pretty = true 
            });

            _parser = new FhirJsonParser(new ParserSettings
            {
                AllowUnrecognizedEnums = false, 
                 AcceptUnknownMembers = false,
                 
                 
            });
        }

        public string FhirR4Serialize(Bundle mappedObject)
        {
            // Serialize the Bundle to JSON
            return _serializer.SerializeToString(mappedObject);
        }

        public string FhirR4SerializeResource(Resource mappedObject)
        {
            // Serialize any Resource to JSON
            return _serializer.SerializeToString(mappedObject);
        }

        public Resource FhirR4DeSerialize(string jsonString)
        {
            // Deserialize JSON to a generic Resource
            return _parser.Parse<Resource>(jsonString);
        }

        public Bundle FhirR4DeSerializeBundle(string jsonString)
        {
            // Deserialize JSON to a Bundle
            return _parser.Parse<Bundle>(jsonString);
        }
    }
    //public class FhirSerializer : IFhirSerializer.IFhirSerializer
    //{
    //    public string FhirR4Serialize(Bundle mappedObject)
    //    {
    //    var serializer = new FhirJsonSerializer();
    //    string json = serializer.SerializeToString(mappedObject);
    //        return json;
    //    }
    //    public string FhirR4SerializeResource(Resource mappedObject)
    //    {
    //        var serializer = new FhirJsonSerializer();
    //        string json = serializer.SerializeToString(mappedObject);
    //        return json;
    //    }
    //    public Resource FhirR4DeSerialize(string jsonString)
    //    {
    //        var deSerializer = new FhirJsonPocoDeserializer();
    //        var json = deSerializer.DeserializeResource(jsonString) ;
    //        return json;
    //    }
    //    public Bundle FhirR4DeSerializeBundle(string jsonString)
    //    {
    //        var deSerializer = new FhirJsonPocoDeserializer();
    //        var json = (Bundle) deSerializer.DeserializeResource(jsonString);
    //        return json;
    //    }
    //}
       
}
