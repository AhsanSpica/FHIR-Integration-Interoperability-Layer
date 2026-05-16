 using EHR.Models.Patients;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IPatientsInfrastructure
{
    public interface IPatientsInfoInfrastructure
    {
      
        Task<List<PatientInfo>> GetAll();

        Task<Tuple<IEnumerable<PatientInfo>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>>> FindDuplicatePatients(PatientInfo patientInfos);

 
        Task<PatientInfo> GetByID(long PatientId);


         Task<Tuple<IEnumerable<PatientInfoListItem>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>, IEnumerable<PatientInsurancePreference> >> GetPatientDtoCollaborateMd(string patientMRN);
 
        Task<PatientInfo> GetPatientInfoById(long patientId,long PracticeId);
        Task<bool> SyncPatientInfoToACO(PatientInfo patientInfo);

    }
}