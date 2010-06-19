using System;
using System.ComponentModel;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.ApiServiceReference;

namespace Fab.Client.Models
{
	public class AddTransferResult : IResult
	{
		private readonly Guid user1Id;
		private readonly int account1Id;
		private readonly Guid user2Id;
		private readonly int account2Id;
		private readonly DateTime operationDate;
		private readonly decimal amount;
		private readonly string comment;

		public AddTransferResult(Guid user1Id, int account1Id, Guid user2Id, int account2Id, DateTime operationDate, decimal amount, string comment)
		{
			this.user1Id = user1Id;
			this.account1Id = account1Id;
			this.user2Id = user2Id;
			this.account2Id = account2Id;
			this.operationDate = operationDate;
			this.amount = amount;
			this.comment = comment;
		}

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
		{
			var proxy = new TransactionServiceClient();

			proxy.TransferCompleted += OnTransferCompleted;
			proxy.TransferAsync(user1Id,
								account1Id,
								user2Id,
								account2Id,
								operationDate,
								amount,
								comment
								);
		}

		private void OnTransferCompleted(object s, AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(
					() => Completed(this, new ResultCompletionEventArgs { Error = e.Error }));
			}
			else
			{
				Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => Completed(this, new ResultCompletionEventArgs()));
			}
		}
	}
}