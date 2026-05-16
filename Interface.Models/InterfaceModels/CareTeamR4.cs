using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class CareTeamR4 
        //:CareTeam
    {
        public string ResourceType => "CareTeam";
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        //    public CareTeamR4() : base() { }
        public string Id {  get; set; }
        public ResourceReference Subject { get; set; }
        public string Name { get; set; }
        public List<CareTeam.ParticipantComponent> Participant { get; set; }
        public CareTeam.CareTeamStatus Status { get; set; }

         
    }
}
