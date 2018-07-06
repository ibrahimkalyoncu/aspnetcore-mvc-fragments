using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Attributes;


namespace AspNetCore.Mvc.Fragments.Datasource
{
    public class FragmentAssamblyDatasource : IFragmentDatasource
    {
        private readonly Assembly _assembly;
        private List<FragmentInfo> _fragmentInfos;

        public FragmentAssamblyDatasource(Assembly assembly)
        {
            _assembly = assembly;
        }

        public async Task<List<FragmentInfo>> GetAllAsync()
        {
            var fragmentType = typeof(Fragment);

            _fragmentInfos = _fragmentInfos ?? _assembly.GetTypes().Where(type => fragmentType.IsAssignableFrom(type)).Select(type => new FragmentInfo
            {
                IsRemote = false,
                Source = _assembly.FullName,
                Name = type.Name.Replace("Fragment", string.Empty),
                FragmentOptions = type.GetCustomAttribute<FragmentOptionsAttribute>()
            }).ToList();

            return await Task.FromResult(_fragmentInfos);
        }
    }
}
