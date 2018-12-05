using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap;
using StructureMap.Graph;

namespace Sourceportal.API.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.Assembly("Sourceportal.Services");     
                    scan.Assembly("Sourceportal.DB");



                    scan.WithDefaultConventions();
                });
            //For<IExample>().Use<Example>();
        }

        #endregion
    }
}