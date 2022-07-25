using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Domain.ActionResults
{
    public class ServerError : ActionResult
    {
        private string message;
        public ServerError(string message)
        {
            this.message = message;
        }
        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(message);

            return Task.FromResult(response);
        }
    }
}
