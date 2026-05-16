using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using Dapper;
using Interface.Misc.Interfaces;
using Interface.Models.BackgroundServices;
using Interface.Misc.Implementation;
using Interface.Models.Medication;
using Hl7.Fhir.Model;
using System.Security.AccessControl;
using Microsoft.Extensions.Configuration;
using Hl7.FhirPath.Sprache;
using GlobalHelpers;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using IFhirSerializer;
using System.Collections.Concurrent;
using Interface.Models.InterfaceModels;
using RestSharp.Serializers;
using static System.Formats.Asn1.AsnWriter;
using Azure.Core;

namespace Interface.Misc.Helpers
{
  
    public class FhirBackgroundService : BackgroundService
    {
        private readonly IFhirService _fhirService;
        private readonly IOptions<AppSetting> _options;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;   

        public FhirBackgroundService(
            IFhirService fhirService,
            IOptions<AppSetting> options,
           IServiceScopeFactory serviceScopeFactory,
           IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _fhirService = fhirService;
            _options = options;
            _serviceScopeFactory = serviceScopeFactory;
            _fhirSerializer = fhirSerializer;
        }
        protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessResourceAsync();
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Runs every hour
            }
        }
        private async System.Threading.Tasks.Task ProcessResourceAsync()
        {
            var scopeLoader = new ScopeLoader();
            var resourceMappings = scopeLoader.LoadResourceMappings();
            object lockObject = new object();
           // var maxConcurrency = 10;
            //   await _fhirService.CreateTokenAsync();

            if (_options.Value.IsBackGroundService)
            {
                // :: Extract pt 1

                HelperMethods.CreateConsoleLog("Background Service Initiating");
                
                foreach (var mapping in resourceMappings)
                {                   
                    var mapperType = Type.GetType(mapping.MapperClass);
                    var mapperInterfaceType = Type.GetType(mapping.MapperInterface);
                    var mapperResourceType = mapping.ResourceType;
                    var isUpdate = mapping.IsUpdate;

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var constructorParams = mapperType.GetConstructors().First().GetParameters();

                        var dependencies = constructorParams.Select(param => scope.ServiceProvider.GetRequiredService(param.ParameterType)).ToArray();

                        var mapperInstance = Activator.CreateInstance(mapperType, dependencies);
 
                        var mapMethod = mapperInterfaceType.GetMethod ("MapSync");
                        var methodNameTest = mapMethod.Name;
                        var classFullNameTest = mapMethod.DeclaringType.FullName;
                               
                        if (mapMethod == null)
                                {
                                    throw new Exception($"Map method not found in {mapping.MapperInterface}");
                                }

                        //HelperMethods.CreateConsoleLog("Inside Mapper Class Loop, Class succesfully invoked " + classFullNameTest + " and method invoked " + methodNameTest);

                        //var idsNotInFhir = await _fhirService.GetNewRecordsByIdentifierAsync(mapping.ResourceType, mapping.StructureDefinition);

                        //HelperMethods.CreateConsoleLog($"count of records not in FHIR SERVER for {mapping.ResourceType}{idsNotInFhir.Count}");


                        if (_options.Value.IsBundlePOST)
                        {
                            await MapAndPostResourceBundle(mapperType, mapperInterfaceType,
              scope, mapping, mapperResourceType);
                        }
                        else {
                            await MapAndPostResource(mapperType, mapperInterfaceType,
              scope, mapping, mapperResourceType);
                        }

                        }//end of scope
                } // end of parent foreach
            }
        }
        // :: Extract pt 1


        private async System.Threading.Tasks.Task
           MapAndPostResourceBundle
          (Type mapperType, Type mapperInterfaceType,
           IServiceScope scope, ResourceMapping mapping, string mapperResourceType)
        {
            var constructorParams = mapperType.GetConstructors().First().GetParameters();

            var dependencies = constructorParams.Select(param => scope.ServiceProvider.GetRequiredService(param.ParameterType)).ToArray();

            var mapperInstance = Activator.CreateInstance(mapperType, dependencies);

            var mapMethod = mapperInterfaceType.GetMethod("MapSync");
            var methodNameTest = mapMethod.Name;
            var classFullNameTest = mapMethod.DeclaringType.FullName;

            if (mapMethod == null)
            {
                throw new Exception($"Map method not found in {mapping.MapperInterface}");
            }

            HelperMethods.CreateConsoleLog("Inside Mapper Class Loop, Class succesfully invoked " + classFullNameTest + " and method invoked " + methodNameTest);

            var recordsInEMR = await _fhirService.ExtractResourceIds(mapping.ResourceType);

            //::TODO DEV_TEST required following call for search records in batches, reduce hits
            //  var tupleResponse = await _fhirService.FetchAndProcessIdentifiersAsync(recordsInEMR, mapping.ResourceType, mapping.StructureDefinition);
            //var idsNotInFhir2 = tupleResponse.RecordsNotInFhir;
            //var recordsUpdate = tupleResponse.RecordsToUpdate;
            //:: DEV-TEST Required above method call

            var idsNotInFhir = recordsInEMR;
          //  var idsNotInFhir = await _fhirService.SearchByIdentifierAsync(recordsInEMR, mapping.ResourceType,mapping.StructureDefinition);

            HelperMethods.CreateConsoleLog($"Count of {mapping.ResourceType} records not in FHIR SERVER: {idsNotInFhir.Count}");

            var postBundle = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
            };

            if (!idsNotInFhir.IsEmpty)
            {

                if (!mapperResourceType.Equals("Immunization") && !mapperResourceType.Equals("Vital") && !mapperResourceType.Equals("Procedure") 
                    && !mapperResourceType.Equals("SmokingStatus") && !mapperResourceType.Equals("Patient"))
                {
                    idsNotInFhir = HelperMethods.getUniquePatientId(idsNotInFhir);
                    
                    HelperMethods.CreateConsoleLog($"{idsNotInFhir.Count} of unique PatientIds for which {mapperResourceType}s are new ");
                }

                //Test for small record slice
                //var limitedRecords = idsNotInFhir.Take(2000).ToList();
                //idsNotInFhir = new ConcurrentBag<PatientResourceRecords>(limitedRecords);

                using (var semaphore = new SemaphoreSlim(15))
                {

                    var tasks = new List<System.Threading.Tasks.Task>();                  

                    foreach (var resourceRecord in idsNotInFhir)
                    {

                        await semaphore.WaitAsync();

                        tasks.Add(System.Threading.Tasks.Task.Run( () =>
                        {
                            try
                            {
                                //    var IdSetResourceType = resourceRecord.ResourceType;

                                //     if (mapperResourceType.Equals(IdSetResourceType))
                                //     {
                                var parameters = new object[] { resourceRecord };

                                // :: Extract pt 2
                                // :: TRANSFORM

                                var fhirResource = (Bundle)mapMethod.Invoke(mapperInstance, parameters);

                                if (fhirResource.Entry != null && fhirResource.Entry.Count > 0)
                                {
                                    foreach (var resourceEntry in fhirResource.Entry)
                                    {
                                        
                                        postBundle.Entry.Add(new Bundle.EntryComponent
                                        { Resource = resourceEntry.Resource, Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = resourceEntry.Resource.TypeName } });
                                    }

                                }
                                //     } // end of if resourcetype is correct 
                            } //end of try
                            finally
                            {
                                semaphore.Release();
                            }
                        })); // end of tasker

                    } //endo of foreach
                    await System.Threading.Tasks.Task.WhenAll(tasks);
                }

                //:: LOAD
                await PostBundle(postBundle, mapperResourceType);

                HelperMethods.CreateConsoleLog($"End of {mapperResourceType} Insert of {idsNotInFhir.Count} Records");
            }
        }

        private async System.Threading.Tasks.Task PostBundle(Bundle fhirResource, string mapperResourceType)
        {
            var jsonstring = _fhirSerializer.FhirR4Serialize(fhirResource);

          //  lock (_fhirService)
           // {
                // :: Load
               await  _fhirService.CreateResourceAsync(jsonstring, mapperResourceType);
          //  }
        }

        private async System.Threading.Tasks.Task MapAndPostResource(Type mapperType, Type mapperInterfaceType,
            IServiceScope scope, ResourceMapping mapping, string mapperResourceType)
        {
            var constructorParams = mapperType.GetConstructors().First().GetParameters();

            var dependencies = constructorParams.Select(param => scope.ServiceProvider.GetRequiredService(param.ParameterType)).ToArray();

            var mapperInstance = Activator.CreateInstance(mapperType, dependencies);

            var mapMethod = mapperInterfaceType.GetMethod("MapSync");
            var methodNameTest = mapMethod.Name;
            var classFullNameTest = mapMethod.DeclaringType.FullName;

            if (mapMethod == null)
            {
                throw new Exception($"Map method not found in {mapping.MapperInterface}");
            }

            HelperMethods.CreateConsoleLog("Inside Mapper Class Loop, Class succesfully invoked " + classFullNameTest + " and method invoked " + methodNameTest);

            var recordsInEMR = await _fhirService.ExtractResourceIds(mapping.ResourceType);

             var idsNotInFhir = await _fhirService.SearchByIdentifierAsync(recordsInEMR, mapping.ResourceType,mapping.StructureDefinition);

            HelperMethods.CreateConsoleLog($"Count of {mapping.ResourceType} records not in FHIR SERVER: {idsNotInFhir.Count}");

            if (!idsNotInFhir.IsEmpty)
            {

                 if (!mapperResourceType.Equals("Immunization") && !mapperResourceType.Equals("Vital") && !mapperResourceType.Equals("Procedure") 
                    && !mapperResourceType.Equals("SmokingStatus") && !mapperResourceType.Equals("Patient"))

                {
                    idsNotInFhir = HelperMethods.getUniquePatientId(idsNotInFhir);
                    HelperMethods.CreateConsoleLog($"{idsNotInFhir.Count} records for {mapperResourceType} resourceType searched by Patientids");
                }

                using (var semaphore = new SemaphoreSlim(25))
                {
                    var tasks = new List<System.Threading.Tasks.Task>();

                    foreach (var resourceRecord in idsNotInFhir)
                    {
                        await semaphore.WaitAsync();

                        tasks.Add(System.Threading.Tasks.Task.Run(async () =>
                        {
                            try
                            {
                              //  var IdSetResourceType = resourceRecord.ResourceType;

                               // if (mapperResourceType.Equals(IdSetResourceType))
                              //  {
                                    var parameters = new object[] { resourceRecord };

                                    // :: Extract pt 2
                                    // :: Transform
                                    var fhirResource = (Bundle)mapMethod.Invoke(mapperInstance, parameters);

                                    if (fhirResource.Entry != null && fhirResource.Entry.Count > 0)
                                    {

                                        var jsonstring = _fhirSerializer.FhirR4Serialize(fhirResource);

                                      //  lock (_fhirService)
                                      //  {
                                            // :: Load
                                         await   _fhirService.CreateResourceAsync(jsonstring, mapperResourceType);
                                      //  }
                                    }
                              //  } // end of if resourcetype is correct 
                            } //end of try
                            finally
                            {
                                semaphore.Release();
                            }
                        })); // end of tasker


                    } //endo of foreach

                    await System.Threading.Tasks.Task.WhenAll(tasks);
                }
                HelperMethods.CreateConsoleLog($"End of {mapperResourceType} Insert of {idsNotInFhir.Count} Records");
              //  await _fhirService.GetResourceCount(mapperResourceType, idsNotInFhir.Count);
            } // end of if ocndition if new ids are not null
        }

       

    }

}
