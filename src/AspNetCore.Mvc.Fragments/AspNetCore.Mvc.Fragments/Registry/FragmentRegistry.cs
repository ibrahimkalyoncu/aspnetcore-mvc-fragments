using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Datasource;

namespace AspNetCore.Mvc.Fragments.Registry
{
    public class FragmentRegistry : IFragmentRegistry
    {
        private readonly List<IFragmentDatasource> _datasources;

        public FragmentRegistry()
        {
            _datasources = new List<IFragmentDatasource>();
        }

        public void AddDatasource(IFragmentDatasource datasource)
        {
            _datasources.Add(datasource);
        }

        public async Task<List<FragmentInfo>> GetAllAsync()
        {
            var fragments = new List<FragmentInfo>();

            foreach (var fragmentDatasource in _datasources)
            {
                fragments.AddRange(await fragmentDatasource.GetAllAsync());
            }

            return fragments;
        }
    }
}
