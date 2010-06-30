// <copyright file="EditTransactionResult.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-06-12" />
// <summary>Load specific transaction async result.</summary>

using System;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.MoneyServiceReference;

namespace Fab.Client.Models
{
	/// <summary>
	/// Load specific transaction async result.
	/// </summary>
	public class LoadTransactionResult : IResult
	{
		private readonly Guid userId;
		private readonly int accountId;
		private readonly int transactionId;

		public TransactionDTO Transaction { get; set; }

		public LoadTransactionResult(Guid userId, int accountId, int transactionId)
		{
			this.userId = userId;
			this.accountId = accountId;
			this.transactionId = transactionId;
		}

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
		{
			var proxy = new MoneyServiceClient();

			proxy.GetTransactionCompleted += (sender, args) =>
			                                 	{
													if (args.Error != null)
													{
														Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(
															() => Completed(this, new ResultCompletionEventArgs { Error = args.Error }));
													}
													else
													{
														Transaction = args.Result;
														Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => Completed(this, new ResultCompletionEventArgs()));
													}
			                                 	};
			proxy.GetTransactionAsync(userId,
									  accountId,
									  transactionId);
		}
	}
}