using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Projeto_Aeronautica_MVC.Helpers
{
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewName)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
