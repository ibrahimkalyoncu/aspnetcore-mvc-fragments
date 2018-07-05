using System.Collections.Generic;
using System.Linq;
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

        public List<FragmentInfo> GetAll()
        {
            return _datasources.SelectMany(d => d.GetAll()).ToList();
        }
    }
}
