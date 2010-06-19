// <copyright file="EditTransactionResult.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-12</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-12</date>
// </editor>
// <summary>Load specific transaction async result.</summary>

using System;
using System.ComponentModel;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.ApiServiceReference;

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

		public Transaction Transaction { get; set; }

		public LoadTransactionResult(Guid userId, int accountId, int transactionId)
		{
			this.userId = userId;
			this.accountId = accountId;
			this.transactionId = transactionId;
		}

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
		{
			var proxy = new TransactionServiceClient();

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