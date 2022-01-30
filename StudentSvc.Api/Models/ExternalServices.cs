using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentSvc.Api.Models
{
    public class ExternalServices
    {
        public List<ExternalService> Services { get; set; }
        public ExternalService this[string name]
        {
            get
            {
                return this.Services?.FirstOrDefault(x => x.Name.ToUpperInvariant() == name.ToUpperInvariant()) ?? null;
            }
        }

        public string GetFullPath(string serviceName, string link)
        {
            var service = this.Services.FirstOrDefault(x => x.Name.ToUpperInvariant() == serviceName.ToUpperInvariant());
            if (service== null)
            {
                throw new Exception("Service is missing");
            }
            else
            {
                var arg = service.Args.FirstOrDefault(x => x.Name.ToUpperInvariant() == link.ToUpperInvariant()) ?? throw new Exception("link address is missing.");
                return $"{service.Base}/{arg.Link}";
            }
        }
    }

    public class ExternalService
    {
        public string Name { get; set; }
        public string Base { get; set; }
        public List<Arglink> Args { get; set; }
    }

    public class Arglink
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
