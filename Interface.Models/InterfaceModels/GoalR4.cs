using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Goal;

namespace Interface.Models.InterfaceModels
{
    public class GoalR4 
    {
        public string ResourceType { get; set; } = "Goal";
        public string? Id { get; set; }
        public string? LifecycleStatus { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier {  get; set; }
        public CodeableConcept? Description { get; set; }
        public ResourceReference Subject { get; set; }
        public List<TargetComponent> Target { get; set; }
        public List<Annotation> Note { get; set; }
    }
}
