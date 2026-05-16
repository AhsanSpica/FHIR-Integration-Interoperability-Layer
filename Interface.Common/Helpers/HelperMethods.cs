
using Hl7.Fhir.Model;
using Interface.Models.Auth;
using Interface.Models.BackgroundServices;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Interface.Misc.Helpers
{
    public static class HelperMethods
    {
        private static IConfiguration _configuration;

         /// <summary>
        /// Decoding querystring
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public static NameValueCollection DecodeQueryString(string toEncode)
        {
            //remove special characters from querystring
            toEncode = System.Net.WebUtility.UrlDecode(toEncode);
            byte[] data = Convert.FromBase64String(toEncode);
            string decodedString = Encoding.UTF8.GetString(data);
            return HttpUtility.ParseQueryString(decodedString);
        }
        private static readonly Dictionary<string, Type> ResourceTypeMap = new Dictionary<string, Type>
    {
        { "Patient", typeof(Patient) },
        { "Encounter", typeof(Encounter) },
        { "CareTeam", typeof(CareTeam) },
        { "AllergyIntolerance", typeof(AllergyIntolerance) },
        { "Goal", typeof(Goal) },
        { "Immunization", typeof(Immunization) },
        { "MedicationStatement", typeof(MedicationStatement) },
        { "MedicationRequest", typeof(MedicationRequest) },
        { "SmokingStatus", typeof(Observation) },
        { "Procedure", typeof(Procedure) },
        { "Condition", typeof(Condition) },
        { "Vital", typeof(Observation) }
        // Add other resource types as needed
    };

        public static Type GetResourceType(string resourceTypeName)
        {
            if (ResourceTypeMap.TryGetValue(resourceTypeName, out Type resourceType))
            {
                return resourceType;
            }

            throw new ArgumentException($"Unknown resource type: {resourceTypeName}");
        }

      
        public static ConcurrentBag<PatientResourceRecords> getUniquePatientId(ConcurrentBag<PatientResourceRecords> recordsList)
        {
            var uniqueRecords = new Dictionary<long, PatientResourceRecords>();
            ConcurrentBag < PatientResourceRecords > result = new ConcurrentBag<PatientResourceRecords> ();
            foreach (var record in recordsList) 
            {
                if (record.PatientId.HasValue && !uniqueRecords.ContainsKey(record.PatientId.Value))
                {
                    uniqueRecords.Add(record.PatientId.Value, record);
                }
            }
            var uniquePatientRecords = uniqueRecords.Values.ToList();

            foreach (var record in uniquePatientRecords)
            {
                result.Add(record);
            }

            return result;
        }
        public static void CreateConsoleLog(string msg)
        {
            System.Diagnostics.Debug.WriteLine("*************************************");
            System.Diagnostics.Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine("*************************************");
        }
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static bool IsNotNullOrEmpty(dynamic obj)
        {
            if (obj != null && !obj.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsNullOrEmpty(dynamic obj)
        {
            if (obj == null || obj.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove Div tag from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string GetUnescapedXml(string escapedXmlString)
        {
            // Create a SecurityElement object to unescape the XML
            string unescapedXml = WebUtility.HtmlDecode(escapedXmlString);

            // Return the unescaped XML
            return unescapedXml;
        }

        public static DateTimeOffset? ParseDateTimeOffset(DateTimeOffset value)
        {
            DateTimeOffset result;

            // Try to parse the value as ISO string with any culture
            if (DateTimeOffset.TryParseExact(value.ToString("O"), "O", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return null;
        }
        public static string SerializeToUnescapedXml<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Configure the XmlWriter settings to produce unescaped XML
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false, // Include the XML declaration
                Indent = true,
                NewLineHandling = NewLineHandling.None,
                NewLineChars = ""
            };

            using (StringWriter writer = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                serializer.Serialize(xmlWriter, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Getting Reporty Type and Name from querystring
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="reportType"></param>
        /// <param name="reportName"></param>
        public static void QueryStringToNavigationDetail(string Params, ref int reportType, ref string reportName)
        {
            byte[] data = Convert.FromBase64String(Params);
            string DecodedQueryString = Encoding.UTF8.GetString(data);
            List<string> AllKeys = DecodedQueryString.Split('&').ToList();

            string reportTypeStr = AllKeys.FirstOrDefault(x => x.ToLower().Contains("reporttype"));
            if (reportTypeStr != null)
            {
                var splitReportType = reportTypeStr.Split("=");
                reportType = Convert.ToInt32(splitReportType[1]);
            }


            reportName = AllKeys.FirstOrDefault(x => x.ToLower().Contains("reportname"));
            if (reportName != null)
            {
                var splitReportName = reportName.Split("=");
                reportName = splitReportName[1];
            }

        }
        //public async static Task<bool> ValidateNPI(string npi)
        //{
        //    bool validate = false;
        //    var client = new RestClient();

        //    var NPPESURL = _configuration.GetSection("AppSetting:NPPES:URL").Value;
        //    string apiUrl = $"" + NPPESURL + "number=" + npi + "&enumeration_type=NPI-1&taxonomy_description=&name_purpose=&first_name=&use_first_name_alias=&last_name=&organization_name=&address_purpose=&city=&state=&postal_code=&country_code=&limit=&skip=&pretty=&version=2.1";

        //    var request = new RestRequest(apiUrl, Method.Get);

        //    var response = await client.ExecuteAsync(request);

        //    if (response.IsSuccessful)
        //    {
        //        var jsonDocument = JsonDocument.Parse(response.Content);

        //        var root = JsonSerializer.Deserialize<NppesRoot>(jsonDocument.RootElement.GetRawText());
        //        if (root.results!=null)
        //        {
        //            foreach (var dude in root.results)
        //            {
        //                if (npi.Equals(dude.number))
        //                {
        //                    validate = true;

        //                }
        //            }
        //        }
        //    }

        //    return validate;
        //}

        ////returns the entire result after reading it as stream
        //public async static Task<NppesRoot> ValidateReturnNpi(NpiSearch npiSearch)
        //{
        //    NppesRoot resultRoot = new NppesRoot();
        //    NppesRoot root = new NppesRoot();
        //    var client = new RestClient();

        //    var NPPESURL = _configuration.GetSection("AppSetting:NPPES:URL").Value;
        //    //string apiUrl = $"" + NPPESURL + "number=" + npiSearch.Npi + "&enumeration_type="+npiSearch.EnumerationType+"&taxonomy_description="+npiSearch.TaxonomyDescription+"&name_purpose="+npiSearch.NamePurpose+"&first_name="+npiSearch.FirstName+"&use_first_name_alias=&last_name="+npiSearch.LastName+"&organization_name="+npiSearch.OrganizationName+"&address_purpose="+npiSearch.AddressPurpose+"&city="+npiSearch.City+"&state="+npiSearch.State+"&postal_code="+npiSearch.PostalCode+"&country_code="+npiSearch.CountryCode+"&limit="+npiSearch.Limit+"&skip="+npiSearch.Skip+"&pretty="+npiSearch.Pretty+"&version=2.1";
        //    string apiUrl = $"{NPPESURL}number={npiSearch.Npi}&enumeration_type=NPI-1&first_name={npiSearch.FirstName}&last_name={npiSearch.LastName}&version=2.1";

        //    var request = new RestRequest(apiUrl, Method.Get);

        //    var response = await client.ExecuteAsync(request);

        //    if (response.IsSuccessful)
        //    {
        //        var jsonDocument = JsonDocument.Parse(response.Content!);

        //        root = JsonSerializer.Deserialize<NppesRoot>(jsonDocument.RootElement.GetRawText())!;
        //    //    foreach (var result in root.results)
        //    //    {
        //    //        foreach (var taxonomy in result.taxonomies)
        //    //        {
        //    //            if (taxonomy.primary)
        //    //            {
        //    //                resultRoot.results.Add(result);
        //    //            }
        //    //        }
        //    //    }
        //    }
        //    return root;
        //}

        /// <summary>
        /// Mapping Filter from querystring
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        //public static SearchReportFilter QueryStringToReportModel(string Params)
        //{
        //    byte[] data = Convert.FromBase64String(Params);
        //    string DecodedQueryString = Encoding.UTF8.GetString(data);
        //    List<string> AllKeys = DecodedQueryString.Split('&').ToList();
        //    int AcoKey = 0;
        //    int OrganizationID = 0;
        //    string practicename;
        //    string npi;
        //    string AKey = AllKeys.FirstOrDefault(x => x.ToLower().Contains("akey"));
        //    if (AKey != null)
        //    {
        //        AKey = AKey.Split('=')[1];
        //        int.TryParse(AKey, out AcoKey);

        //    }
        //    practicename = AllKeys.FirstOrDefault(x => x.ToLower().Contains("practicename"));


        //    if (practicename != null)
        //    {
        //        practicename = practicename.Split('=')[1];
        //        string[] splitpracticename = practicename.Split('-');
        //        if (splitpracticename.Length > 1)
        //        {
        //            practicename = splitpracticename[1];

        //        }
        //    }

        //    npi = AllKeys.FirstOrDefault(x => x.ToLower().Contains("npi"));
        //    if (npi != null)
        //    {
        //        string[] splitnpi = npi.Split('=');
        //        npi = splitnpi[1];

        //    }

        //    string PractID = AllKeys.FirstOrDefault(x => x.ToLower().Contains("prid"));

        //    if (PractID != null)
        //    {
        //        var splipract = PractID.Split('=');

        //        PractID = splipract[1];
        //    }

        //    string FilterValue = AllKeys.FirstOrDefault(x => x.ToLower().Contains("fval"));

        //    if (FilterValue != null)
        //    {
        //        var spli = FilterValue.Split('=');

        //        FilterValue = spli[1];
        //    }


        //    string UserID = AllKeys.FirstOrDefault(x => x.ToLower().Contains("uid"));
        //    if (UserID != null)
        //    {
        //        UserID = UserID.Split('=')[1];
        //        //HttpContext.Session.SetString("UserID", UserID);
        //        //Session["UserID"] = UserID;
        //    }
        //    //disease filter added in query string params
        //    string Disease = AllKeys.FirstOrDefault(x => x.ToLower().Contains("diseasetypeid"));
        //    if (Disease != null)
        //    {
        //        Disease = Disease.Split('=')[1];
        //    }
        //    //get population status from query string
        //    string populationStatus = AllKeys.FirstOrDefault(x => x.ToLower().Contains("populationstatus"));
        //    if (populationStatus != null)
        //    {
        //        populationStatus = populationStatus.Split('=')[1];
        //    }

        //    //string reportTypeStr = AllKeys.FirstOrDefault(x => x.ToLower().Contains("reporttype"));
        //    //if (reportTypeStr != null)
        //    //{
        //    //    var splitReportType = reportTypeStr.Split("=");
        //    //    reportType = Convert.ToInt32(splitReportType[1]);
        //    //}


        //    //reportName = AllKeys.FirstOrDefault(x => x.ToLower().Contains("reportname"));
        //    //if (reportName != null)
        //    //{
        //    //    var splitReportName = reportName.Split("=");
        //    //    reportName = splitReportName[1];
        //    //}

        //    SearchReportFilter Model = new SearchReportFilter();
        //    Model.AcoKey = AcoKey.ToString();




        //    Model.AcoKey = AcoKey.ToString();
        //    Model.OrganizationID = OrganizationID;
        //    //Model.PracticePCP = PractID;
        //    //Model.NPI = npi;
        //    //Model.PracticeName = practicename;
        //    //Model.FilterValue = FilterValue;
        //    //Model.Disease = Disease;
        //    //Model.PopulationStatus = populationStatus;
        //    return Model;
        //}


        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        public static long ReturnLongValue(long val)
        {
            string regex = "^0+(?!$)";

            var str = Regex.Replace(val.ToString(), regex, "");
            return (long)Convert.ToDouble(str);
        }
        public static T DeserializeXMLToObject<T>(string Xml) where T : new()
        {
            if (string.IsNullOrEmpty(Xml))
            {
                return new T();
            }
            try
            {
                using (var stringReader = new StringReader(Xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader)!;
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }
        public static DateTimeOffset GetCurrentDateTime()
        {
            return DateTimeOffset.UtcNow;
        }

        public static TimeSpan MinuteToTimespan(int minutes) {
            return new TimeSpan((int)(minutes / 60) , (int)(minutes % 60), 0);
        }
    }
}
