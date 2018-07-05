using System.Collections.Generic;
using AspNetCore.Mvc.Fragments.Datasource;

namespace AspNetCore.Mvc.Fragments.Registry
{
    public interface IFragmentRegistry
    {
        void AddDatasource(IFragmentDatasource datasource);
        List<FragmentInfo> GetAll();
    }
}