using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Registry;
using AspNetCore.Mvc.Fragments.Remote;

namespace AspNetCore.Mvc.Fragments.Resolver
{
    public class FragmentResolver : IFragmentResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFragmentRegistry _fragmentRegistry;

        public FragmentResolver(IServiceProvider serviceProvider, IFragmentRegistry fragmentRegistry)
        {
            _serviceProvider = serviceProvider;
            _fragmentRegistry = fragmentRegistry;
        }

        public virtual async Task<Fragment> ResolveAsync(string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                FragmentInfo fragmentInfo = await _fragmentRegistry.FindAsync(name);

                if (fragmentInfo.IsRemote)
                {
                    return new RemoteFragment(fragmentInfo);
                }

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