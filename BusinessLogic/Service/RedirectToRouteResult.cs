using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BusinessLogic.Service
{
    internal class RedirectToRouteResult : IActionResult
    {
        private RouteValueDictionary routeValueDictionary;

        public RedirectToRouteResult(RouteValueDictionary routeValueDictionary)
        {
            this.routeValueDictionary = routeValueDictionary;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}