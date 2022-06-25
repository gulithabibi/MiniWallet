using System;


namespace MiniWalletApi.Constans
{

	public class CommonConstant
	{
		public static class WalletStatus
		{
			public const string Enabled = "enabled";
			public const string Disabled = "disabled";
			//
			// TODO: Add constructor logic here
			//
		}

		public static class NegativeMessage
        {
			public const string WalletNotFound = "Wallet not found";
			public const string WalletAlreadyEnable= "Status wallet already enabled";
			public const string ReferenceDuplicated = "Reference ID duplicated";
			public const string WalletDisabled = "Currently wallet disabled";
			public const string BalanceNotEnough = "Balance not enough";
			

            public static class Token {
				public const string TokenNotValid = "Token not valid";
				public const string TokenExpired = "Token has been expired";
			}


        }
    }
}
