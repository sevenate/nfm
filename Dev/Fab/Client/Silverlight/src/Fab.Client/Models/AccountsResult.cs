// <copyright file="AccountsResult.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-04-11" />
// <summary>Accounts result.</summary>

using System;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.MoneyServiceReference;

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
		
		public AccountDTO[] Accounts { get; set; }

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
		{
			var proxy = new MoneyServiceClient();
			
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