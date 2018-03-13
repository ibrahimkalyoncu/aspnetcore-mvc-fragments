using System;
using System.Linq;

namespace AspNetCore.Mvc.Fragments.Resolver
{
    public class FragmentResolver : IFragmentResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public FragmentResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual Fragment Resolve(string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                var fragmentName = $"{name}{Constants.FragmentTypeSuffix}";
                var fragmentType = typeof(Fragment);
                var type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.DefinedTypes).FirstOrDefault(t => t.Name == fragmentName && fragmentType.IsAssignableFrom(t));
                if (type != null)
                {
                    var constructor = type.GetConstructors().First();
                    var serviceCollection = constructor.GetParameters().Select(p => _serviceProvider.GetService(p.ParameterType));
                    return constructor.Invoke(serviceCollection.ToArray()) as Fragment;
                }
            }

            return null;
        }
    }
}