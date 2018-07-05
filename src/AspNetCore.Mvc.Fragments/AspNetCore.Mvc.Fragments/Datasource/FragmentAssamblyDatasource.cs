﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNetCore.Mvc.Fragments.Attributes;

namespace AspNetCore.Mvc.Fragments.Datasource
{
    public class FragmentAssamblyDatasource : IFragmentDatasource
    {
        private readonly Assembly _assembly;

        public FragmentAssamblyDatasource(Assembly assembly)
        {
            _assembly = assembly;
        }

        public List<FragmentInfo> GetAll()
        {
            var fragmentType = typeof(Fragment);
            return _assembly.GetTypes().Where(type => fragmentType.IsAssignableFrom(type)).Select(type => new FragmentInfo
            {
                Name = type.Name.Replace("Fragment", string.Empty),
                FragmentOptions = type.GetCustomAttribute<FragmentOptionsAttribute>()
            }).ToList();
        }
    }
}
