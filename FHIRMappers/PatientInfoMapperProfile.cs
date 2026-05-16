using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using Hl7.Fhir.Utility;
using Interface.Models.EncounterModels;
using Interface.Models.GeneralLookups;
using Interface.Models.InterfaceModels;
using Interface.Models.Patients;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using static System.Net.WebRequestMethods;


namespace FHIR.Interface.Helpers
{
    public class PatientFhirR4MappingProfile
    {

        private readonly LookUpScoped _lookUpScoped;
        public PatientFhirR4MappingProfile(LookUpScoped lookUpScoped)
        {
            _lookUpScoped = lookUpScoped;
        }

        public Patient MapToPatient(PatientInfo patientInfo)
        {
            var maritialLookUp = new GeneralLookup();
            var careTeam = new CareTeam();


            if (patientInfo.MaritalStatus.HasValue || patientInfo.MaritalStatus.HasValue)

            {
                maritialLookUp = _lookUpScoped.GetMaritalStatus(patientInfo.MaritalStatus.Value);
            }
            else
            {
                maritialLookUp = _lookUpScoped.GetMaritalStatus(600);
            }

            var patientR4B = new Patient
            {

                //  Id = patientInfo.Id.ToString(),
                //patientInfo.SSN

                Active = patientInfo.IsStatusActive.HasValue,
                BirthDate = patientInfo.DateOfBirth.ToFhirDate(),
                Gender = patientInfo.CurrentGender.HasValue ? mapAdministrativeGender(patientInfo.CurrentGender) : null,
                Meta = new Meta { LastUpdated = patientInfo.UpdatedAt ?? DateTimeOffset.Now },
                Identifier = MapIdentifiers(patientInfo.MRN, patientInfo.Id, patientInfo.SSN) ?? null ,
                Name = MapNames(patientInfo),
                Telecom = MapTelecoms(patientInfo.PatientPhones),
                Address = MapAddresses(patientInfo.PatientAddresses) ?? null,
                MaritalStatus = MapMaritalStatus(maritialLookUp.Code, maritialLookUp.Text, maritialLookUp.Text),
                Communication = MapCommunications(patientInfo),
                Extension = MapExtensions(patientInfo)
            };

            return patientR4B;
        }
        private AdministrativeGender mapAdministrativeGender(int? currentGender)
        {
            var genderLookup = _lookUpScoped.GetGenderSex(currentGender.Value);

            switch (genderLookup.Text.ToLower())
            {
                case "male":
                    return AdministrativeGender.Male;
                case "female":
                    return AdministrativeGender.Female;
                case "unknown":
                    return AdministrativeGender.Unknown;
                default:
                    return AdministrativeGender.Unknown;
            }


        }
        private string getSystemUrl(long Id)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}/patient/getpatientbyid?PatientId={Id}";
        }

        private List<Identifier> MapIdentifiers(string mrn, long id, string ssn)
        {
            var mappedIdentifiers = new List<Identifier>();

            if (!string.IsNullOrWhiteSpace(mrn))
            {
                mappedIdentifiers.Add(new Identifier { System = getSystemUrl(id), Value = mrn });
            }
            if (!string.IsNullOrWhiteSpace(ssn))
            {
                mappedIdentifiers.Add(new Identifier { System = "http://hl7.org/fhir/sid/us-ssn", Value = ssn });
            }
            return mappedIdentifiers;
        }


        private List<HumanName> MapNames(PatientInfo patientInfo)
        {
            var mappedNames = new List<HumanName>();
            if (string.IsNullOrEmpty(patientInfo.LastName))
            { patientInfo.LastName = " "; }
            if (string.IsNullOrEmpty(patientInfo.FirstName))
            { patientInfo.FirstName = " "; }
            mappedNames.Add(new HumanName { Family = patientInfo.LastName, Given = new List<string> { patientInfo.FirstName } });

            return mappedNames;
        }

        private List<ContactPoint> MapTelecoms(List<PatientPhone>? telecoms)
        {
            var mappedTelecoms = new List<ContactPoint>();

            if (telecoms != null)
            {
                foreach (var telecom in telecoms)
                {
                    var phoneNumberTypeLookUp = _lookUpScoped.GetPhoneNumberType(telecom.PhoneNumberType.Value);

                    if (PhoneTypeMapping.TryGetValue(phoneNumberTypeLookUp.Text.ToLower(), out var system))
                    {
                        PhoneUseMapping.TryGetValue(phoneNumberTypeLookUp.Text.ToLower(), out var use);
                        mappedTelecoms.Add(new ContactPoint { System = system, Value = handleTelecomNull(telecom), Use = use, Rank = telecom.Ranking });
                    }
                    else
                    {
                        mappedTelecoms.Add(new ContactPoint { System = ContactPoint.ContactPointSystem.Phone, Value = telecom.PhoneNumber });
                    }
                }
            }

            return mappedTelecoms;
        }
        private string handleTelecomNull(PatientPhone telecom)
        {
            var temp = telecom.PhoneNumber != null ? telecom.PhoneNumber.Trim() : "-";
            temp = temp.Trim() != "" ? temp : "-";
            return temp;
        }
        private static readonly Dictionary<string, ContactPoint.ContactPointSystem> PhoneTypeMapping = new Dictionary<string, ContactPoint.ContactPointSystem>
        {
            { "work", ContactPoint.ContactPointSystem.Phone },
            { "home", ContactPoint.ContactPointSystem.Phone },
           // { "mobile", ContactPoint.ContactPointSystem.Phone },
            { "pager", ContactPoint.ContactPointSystem.Pager },
            { "fax", ContactPoint.ContactPointSystem.Fax },
            { "other", ContactPoint.ContactPointSystem.Other },
            { "skype", ContactPoint.ContactPointSystem.Other },
            { "mobile", ContactPoint.ContactPointSystem.Sms }

        };

        private static readonly Dictionary<string, ContactPoint.ContactPointUse> PhoneUseMapping = new Dictionary<string, ContactPoint.ContactPointUse>
        {
            { "work", ContactPoint.ContactPointUse.Work },
            { "home", ContactPoint.ContactPointUse.Home },
            { "mobile", ContactPoint.ContactPointUse.Mobile }

        };
        private List<Address> MapAddresses(List<PatientAddress> addresses)
        {
            var mappedAddresses = new List<Address>();
            var addressOne = new Address();
            foreach (var address in addresses)
            {
                addressOne.City = address.City;
                addressOne.Country = _lookUpScoped.Country;
                addressOne.Line = new List<string> { $"{address.AddressLine1}, {address.AddressLine2}" };
                addressOne.PostalCode = address.Zip;
                addressOne.State = address.State;
                mappedAddresses.Add(addressOne);
            }
            return mappedAddresses;
        }

        private CodeableConcept MapMaritalStatus(string code, string display, string text)
        {
            return new CodeableConcept
            {
                Coding = new List<Coding> { new Coding { Code = code, Display = display, System = "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus" } },
                Text = text
            };
        }

        private List<Patient.CommunicationComponent> MapCommunications(PatientInfo patientInfo)
        {
            var languageLookUp = new GeneralLookup();
            if (patientInfo.PreferredLanguage != null)

            {
                _lookUpScoped.GetLanguages(patientInfo.PreferredLanguage.Value);

                languageLookUp = _lookUpScoped.GetLanguages(patientInfo.PreferredLanguage.Value);
            }
            else
            {
                languageLookUp = _lookUpScoped.GetLanguages(3);
            }


            var mappedCommunications = new List<Patient.CommunicationComponent>();

            mappedCommunications.Add(new Patient.CommunicationComponent { Language = new CodeableConcept { Text = languageLookUp.Text }, Preferred = true });

            return mappedCommunications;
        }

        private List<Extension> MapExtensions(PatientInfo patientInfo)
        {
            var mappedExtensions = new List<Extension>();

            var sexualOrientationLookUp = new GeneralLookup();
            var tribalAffiliationLookUp = new GeneralLookup();
            var birthSexLookUp = new GeneralLookup();
            var genderIdentityLookUp = new GeneralLookup();
            var currentGenderLookUp = new GeneralLookup();


            var ethnicityList = new List<GeneralLookup>();

            if (patientInfo.BirthSex.HasValue)

            {
                birthSexLookUp = _lookUpScoped.GetBirthSex(patientInfo.BirthSex.Value);
            }

            if (patientInfo.CurrentGender.HasValue)
            {
                currentGenderLookUp = _lookUpScoped.GetGenderSex(patientInfo.CurrentGender.Value);
            }

            if (patientInfo.GenderIdentity.HasValue)

            {
                genderIdentityLookUp = _lookUpScoped.GetGenderIdentity(patientInfo.GenderIdentity.Value);
            }

            if (patientInfo.SexualOrientation.HasValue || patientInfo.SexualOrientation.HasValue)

            {
                sexualOrientationLookUp = _lookUpScoped.GetSexualOrientation(patientInfo.SexualOrientation.Value);
            }

            if (patientInfo.TribalAffiliation.HasValue || patientInfo.TribalAffiliation.HasValue)

            {
                tribalAffiliationLookUp = _lookUpScoped.GetTribalAffiliation(patientInfo.TribalAffiliation.Value);
            }

            var raceExtensions = new List<Extension>();
            
            bool textRaceDisp = false;

            if (patientInfo.patientRaces.Any() && patientInfo.patientRaces.Count > 0)
            {

                foreach (var race in patientInfo.patientRaces)
                {
                    var raceFound = _lookUpScoped.GetRace(int.Parse(race.RaceId.ToString()));
                    raceExtensions.Add(new Extension
                    {
                        Url = "http://terminology.hl7.org/CodeSystem/v3-Race",
                        Value =
                        new Coding
                        {
                            System = "urn:oid:2.16.840.1.113883.6.238",
                            Code = raceFound.Code,
                            Display = raceFound.Text
                        }
                    });

                    if (textRaceDisp == false)
                    {
                        textRaceDisp = true;

                        raceExtensions.Add(new Extension
                        {
                            Url = "text",
                            Value = new FhirString(raceFound.Text)
                        });

                    }
                }

                if (raceExtensions.Count > 1)
                {
                    mappedExtensions.Add(new Extension { Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race", Extension = raceExtensions });
                }
                else
                {
                    mappedExtensions.Add(raceExtensions.First());
                }
            }


            var ethnicityExtensions = new List<Extension>();

            if (patientInfo.patientEthnicities.Any() && patientInfo.patientEthnicities.Count>0)
            {
                bool ombAdded = false;
                bool textEthnicDisp = false;
                
                foreach (var patientEthnicity in patientInfo.patientEthnicities)
                {
                    var ethnicityLookUp = _lookUpScoped.GetEthnicity(int.Parse(patientEthnicity.EthnicityId.ToString()));
                    string ethnicityUrl = string.Empty;
                    string code = ethnicityLookUp.Code.Trim();
                    string display = ethnicityLookUp.Text;
                    

                    if (code.Equals("2135-2") || code.Equals("2186-5"))
                    {
                        if (ombAdded==false)
                        {
                            ombAdded = true;
                            ethnicityUrl = "ombCategory";

                            ethnicityExtensions.Add(new Extension
                            {
                                Url = ethnicityUrl,
                                Value = new Coding
                                {
                                    System = "urn:oid:2.16.840.1.113883.6.238",
                                    Code = code,
                                    Display = display
                                }
                            });

                            textEthnicDisp = true;
                            ethnicityExtensions.Add(new Extension
                            {
                                Url = "text",
                                Value = new FhirString(display)
                            });
                        }
                    }
                    else
                    {
                        ethnicityUrl = "http://hl7.org/fhir/us/core/ValueSet/detailed-ethnicity";

                        ethnicityExtensions.Add(new Extension
                        {
                            Url = ethnicityUrl,
                            Value = new Coding
                            {
                                System = "urn:oid:2.16.840.1.113883.6.238",
                                Code = code,
                                Display = display
                            }
                        });

                        if (textEthnicDisp == false)
                        {
                            textEthnicDisp = true;

                            ethnicityExtensions.Add(new Extension
                            {
                                Url = "text",
                                Value = new FhirString(display)
                            });
                        }
                    }
                    
                }

            if (ethnicityExtensions.Count > 1)
            {
                mappedExtensions.Add(new Extension
                {
                    Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity",
                    Extension = ethnicityExtensions
                });
            }
            else
            {
                mappedExtensions.Add(ethnicityExtensions.First());
            }
        }

            // Mother's Maiden Name

            Extension mothersMaidenNameExtension = new Extension();

           
            if (!string.IsNullOrEmpty(patientInfo.MaidenName))
            {
                mothersMaidenNameExtension = new Extension
                {
                    Url = "http://hl7.org/fhir/StructureDefinition/patient-mothersMaidenName",
                    Value = new FhirString(patientInfo.MaidenName)
                };
                mappedExtensions.Add(mothersMaidenNameExtension);
            }

            // Birth Sex
            var birthSexExtension = new Extension();

            if (!string.IsNullOrEmpty(birthSexLookUp.Text))
            {
                birthSexExtension = new Extension
                {
                    Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-birthsex",

                    Value = new Code(birthSexLookUp.Code)

                };
                mappedExtensions.Add(birthSexExtension);
            }

            // Sex
            var sexExtension = new Extension();
            if (patientInfo.CurrentGender != null && patientInfo.CurrentGender.HasValue)

            {
                sexExtension = new Extension
                {
                    Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-sex",
                    Value = new Code(currentGenderLookUp.Text),
                };
                mappedExtensions.Add(sexExtension);
            }

            //   Birth Place

            if (!patientInfo.PatientAddresses.IsNullOrEmpty())
            {
                var birthPlaceExtension = new Extension
                {
                    Url = "http://hl7.org/fhir/StructureDefinition/patient-birthPlace",
                    Value = new Address
                    {
                        City = patientInfo.PatientAddresses.FirstOrDefault().City,
                        State = patientInfo.PatientAddresses.FirstOrDefault().State,
                        Country = _lookUpScoped.Country,
                        Line = patientInfo.PatientAddresses
                        .Select(address => string.Join(", ", address.AddressLine1, address.AddressLine2).TrimEnd(',', ' '))
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                    }
                };
                mappedExtensions.Add(birthPlaceExtension);
            }

            // Gender Identity
            if (!string.IsNullOrEmpty(genderIdentityLookUp.Text))
            {
                var genderIdentityExtension = new Extension
                {
                    Url = "http://hl7.org/fhir/StructureDefinition/gender-identity",
                    Value = new CodeableConcept
                    {
                        Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://snomed.info/sct",
                            Code = genderIdentityLookUp.Code,
                            Display = genderIdentityLookUp.Text
                        }
                    }
                    }
                };
                mappedExtensions.Add(genderIdentityExtension);
            }

            // Tribal Affiliation
            var tribalAffiliationExtension = new Extension();
            if (!string.IsNullOrEmpty(tribalAffiliationLookUp.Text))
            {
                tribalAffiliationExtension = new Extension
                {
                    Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-tribal-affiliation",
                    // Url = Helper.GetFHIRURL("tribal-affiliation").Value,

                    Extension = new List<Extension>
                {
                    new Extension
                    {
                        Url = "tribalEntity",
                        Value = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "http://terminology.hl7.org/CodeSystem/v3-TribalEntityUS",
                                    Code = tribalAffiliationLookUp.Code,
                                    Display = tribalAffiliationLookUp.Text
                                }
                            }
                        }
                    }
                }
                };
                mappedExtensions.Add(tribalAffiliationExtension);
            }
            return mappedExtensions;
        }
    }

   


}

