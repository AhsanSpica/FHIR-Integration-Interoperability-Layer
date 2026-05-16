using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class ImplantableDeviceCore : Device
    { 
        public CodeableConcept Type { get; set; }

        public ResourceReference Patient { get; set; }


        public UdiCarrier UdiCarrier { get; set; }


        // String for a distinct identifier of the device (optional)
        public string DistinctIdentifier { get; set; }

        // DateTime for the device's manufacture date (optional)
        public DateTime ManufactureDate { get; set; }

        // DateTime for the device's expiration date (optional)
        public DateTime ExpirationDate { get; set; }

        // String for the device's lot number (optional)
        public string LotNumber { get; set; }

        // String for the device's serial number (optional)
        public string SerialNumber { get; set; }
    }
    public class UdiCarrier
    {
        public string DeviceIdentifier { get; set; }

        public byte[] CarrierAIDC { get; set; }

        public string CarrierHRF { get; set; }
    }
}
