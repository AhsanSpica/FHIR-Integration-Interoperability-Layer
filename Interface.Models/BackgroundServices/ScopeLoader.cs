using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.BackgroundServices
{
    public class ScopeSettings
    {
        public List<ResourceMapping> ResourceMappings { get; set; }
    }
    public class ResourceMapping
    {
        public string ResourceType { get; set; }
        public string MapperClass { get; set; }
        public string MapperInterface { get; set; }
        public bool IsUpdate { get; set; }
        public string StructureDefinition { get; set; }
    }
    public class ScopeLoader
    {
        //private static readonly IConfiguration _configuration;

        //public ScopeLoader(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        //public bool IsBackGroundActive ()
        //{
        //   var backService = _configuration.GetSection("IsBackGroundService");
        //    backService
        //    return false;
        
        //}
        public List<ResourceMapping> LoadResourceMappings()
        {
            var scopeSettings = new ScopeSettings();
            var configPath = Path.Combine(AppContext.BaseDirectory, "MapperClass.json");
            var jsonContent = File.ReadAllText(configPath);
            scopeSettings = JsonConvert.DeserializeObject<ScopeSettings>(jsonContent);
            return scopeSettings.ResourceMappings;
        }
    }
}
