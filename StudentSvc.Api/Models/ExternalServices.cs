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
