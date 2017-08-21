using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace PERI.Prompt.BLL
{
    public class CmsUrlConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var context = new EF.SampleDbContext();
            if (values[parameterName] != null)
            {
                var permalink = values[parameterName].ToString();
                return context.Page.Any(p => p.Permalink == permalink);
            }
            return false;
        }
    }
}
