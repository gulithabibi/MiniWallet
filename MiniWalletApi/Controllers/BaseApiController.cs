using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiniWalletApi.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {

        protected IActionResult Getresult<T>(T response)
        {
            BaseApiResponse resp = new BaseApiResponse();

           // resp.Status=HttpRespo

            return null;
        }
    }

    public class BaseApiResponse
    {
        public string Data { get; set; }
        public string Status { get; set; }
    }
}
