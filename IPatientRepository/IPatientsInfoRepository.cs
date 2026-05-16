using EHR.Models.Patients;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientsRepository
{
    public interface IPatientsInfoRepository
    {

        Task<List<PatientInfo>> GetAll();

        Task<Tuple<IEnumerable<PatientInfo>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>>> FindDuplicatePatients(PatientInfo patientInfos);


 

        //Task<List<PatientInfo>> SearchPatientByLocbyProvByMrn(PatientDefaultAndReferringProvider patientDefaultAndReferringProvider,string? Mrn);

       
        Task<PatientInfo> GetByID(long PatientId);


         Task<Tuple<IEnumerable<PatientInfoListItem>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>, IEnumerable<PatientInsurancePreference>>> GetPatientDtoCollaborateMd(string patientMRN);


 
 
        Task<PatientInfoById> GetPatientInfoById(long patientId, long PracticeId);
        Task<bool> SyncPatientInfoToACO(PatientInfo patientInfo);

    }
}