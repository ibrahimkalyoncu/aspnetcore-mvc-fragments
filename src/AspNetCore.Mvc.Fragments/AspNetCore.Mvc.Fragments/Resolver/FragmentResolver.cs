using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Http;
using AspNetCore.Mvc.Fragments.Registry;
using AspNetCore.Mvc.Fragments.Remote;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCore.Mvc.Fragments.Resolver
{
    public class FragmentResolver : IFragmentResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFragmentRegistry _fragmentRegistry;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientProvider _httpClientProvider;

        public FragmentResolver(IServiceProvider serviceProvider, IFragmentRegistry fragmentRegistry, IMemoryCache memoryCache, IHttpClientProvider httpClientProvider)
        {
            _serviceProvider = serviceProvider;
            _fragmentRegistry = fragmentRegistry;
            _memoryCache = memoryCache;
            _httpClientProvider = httpClientProvider;
        }

        public virtual async Task<Fragment> ResolveAsync(string name)
        {
            if (string.IsNullOrEmpty(name) == false)
            {
                FragmentInfo fragmentInfo = await _fragmentRegistry.FindAsync(name);

                if (fragmentInfo.IsRemote)
                {
                    return new RemoteFragment(fragmentInfo, _httpClientProvider, _memoryCache);
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