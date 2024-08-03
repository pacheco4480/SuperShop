using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SuperShop.Helpers
{
    public class NotFoundViewResult : ViewResult
    {
        //Construtor
        public NotFoundViewResult(string viewName)
        {
            //viewName é uma propriedade que herdou do ViewResult
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        } 

    }
}
