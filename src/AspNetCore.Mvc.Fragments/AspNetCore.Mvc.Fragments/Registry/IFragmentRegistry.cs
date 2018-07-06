using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Datasource;

namespace AspNetCore.Mvc.Fragments.Registry
{
    public interface IFragmentRegistry
    {
        void AddDatasource(IFragmentDatasource datasource);
        Task<List<FragmentInfo>> GetAllAsync();
        Task<FragmentInfo> FindAsync(string name);
    }
}