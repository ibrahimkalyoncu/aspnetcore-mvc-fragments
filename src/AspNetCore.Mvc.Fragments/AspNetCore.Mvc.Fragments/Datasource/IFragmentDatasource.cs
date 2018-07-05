using System.Collections.Generic;

namespace AspNetCore.Mvc.Fragments.Datasource
{
    public interface IFragmentDatasource
    {
        List<FragmentInfo> GetAll();
    }
}
