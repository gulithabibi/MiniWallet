using MiniWalletApi.Constans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniWalletApi.Libraries
{
    public class BaseApiResponse
    {
        public string Status { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public object Data { get; set; }

        #region Success Response
        public static BaseApiResponse GetSuccessResponse(Object obj, HttpStatusCode httpStatus)
        {
            BaseApiResponse response = new BaseApiResponse();
            response.Status = HttpStatusConstant.StatusType.Success;
            response.HttpStatus =httpStatus;
            response.Data = obj;
            return response;
        }
        public static BaseApiResponse GetOkResponse(Object obj)
        {
            return GetSuccessResponse(obj,HttpStatusCode.OK);
        }

        public static BaseApiResponse GetCreatedResponse(Object obj)
        {
            return GetSuccessResponse(obj,HttpStatusCode.Created);
        }
        #endregion


        #region Fail Response
        public static BaseApiResponse GetFailResponse(string errorMessage, HttpStatusCode httpStatus)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("error", errorMessage);

            BaseApiResponse response = new BaseApiResponse();
            response.Status = HttpStatusConstant.StatusType.Fail;
            response.HttpStatus = httpStatus;
            response.Data = data;
            return response;
        }
        public static BaseApiResponse GetBadRequstResponse(string errorMessage)
        {
            return GetFailResponse(errorMessage, HttpStatusCode.BadRequest);
        }

        public static BaseApiResponse GetNotFoundResponse(string errorMessage)
        {
            return GetFailResponse(errorMessage, HttpStatusCode.NotFound);
        }
        public static BaseApiResponse GetUnauthorizedResponse(string errorMessage=null)
        {
            errorMessage = errorMessage==null ? "Unauthorized" : errorMessage;
            return GetFailResponse(errorMessage, HttpStatusCode.Unauthorized);
        }

        #endregion 

        public static BaseApiResponse GetErrorResponse(string errorMessage, HttpStatusCode httpStatus)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("error", errorMessage);

            BaseApiResponse response = new BaseApiResponse();
            response.Status = HttpStatusConstant.StatusType.Error;
            response.HttpStatus = httpStatus;
            response.Data = data;
            return response;
        }



       



        
    }
}
