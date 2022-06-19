using System;


namespace MiniWalletApi.Constans
{

	public class HttpStatusConstant
	{
		public static class SUCCESS
		{
			public const int OK = 200;
			public const int CREATED = 201;
		}

		public static class CLIENT_ERRORS
		{
			public const int FORBIDDEN = 403;

		}

		public static class SERVER_ERRORS
		{
			public const int GATEWAY_TIMEOUT = 504;

		}

		public static class StatusType
        {
			public const string Success = "success";
			public const string Fail = "fail";
			public const string Error = "error";
		}
	}
}
