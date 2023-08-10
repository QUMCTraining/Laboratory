using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

namespace LaboratoryProject.Resources
{
    public class LocService
    {
        private readonly IStringLocalizer _localizer;
       
        public LocService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("sharedResource", assemblyName.Name);
        }
        public LocalizedString Loc(String key)
        {
            return _localizer[key];
        }
    }
}