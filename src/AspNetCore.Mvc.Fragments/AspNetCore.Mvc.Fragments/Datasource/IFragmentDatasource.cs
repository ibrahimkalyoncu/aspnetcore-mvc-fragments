using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Datasource
{
    public interface IFragmentDatasource
    {
        Task<List<FragmentInfo>> GetAllAsync();
    }
}
