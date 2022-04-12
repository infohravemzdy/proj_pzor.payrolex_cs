using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Procezor.Registry.Factories;

namespace HraveMzdy.Procezor.Payrolex.Registry.Factories
{
    class ServiceConceptFactory : ConceptSpecFactory
    {
        public ServiceConceptFactory()
        {
            this.Providers = BuildProvidersFromAssembly<ServiceConceptFactory>();
        }
    }
}
