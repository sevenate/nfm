// <copyright file="AccountsResult.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-11</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-11</date>
// </editor>
// <summary>Accounts result.</summary>

using System;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.ApiServiceReference;

namespace Fab.Client.Models
{
	/// <summary>
	/// Accounts result.
	/// </summary>
	public class AccountsResult : IResult
	{
		private readonly Guid userId;

		public AccountsResult(Guid userId)
        {
            this.userId = userId;
        }

        public Guid UserId
        {
            get { return userId; }
        }
 		
		public Account[] Accounts { get; set; }

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
        {
			var proxy = new AccountServiceClient();
			
			proxy.GetAllAccountsCompleted += (s, e) =>
			{
				if (e.Error != null)
				{
					Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(
						() => Completed(this, new ResultCompletionEventArgs { Error = e.Error }));
				}
				else
				{
					Accounts = e.Result;
					Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => Completed(this, new ResultCompletionEventArgs()));
				}
			};

			proxy.GetAllAccountsAsync(userId: userId);
        }
	}
}