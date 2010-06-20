using System;
using System.ComponentModel;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.MoneyServiceReference;

namespace Fab.Client.Models
{
	public class DeleteTransactionResult : IResult
	{
		private readonly Guid userId;
		private readonly int accountId;
		private readonly int transactionId;

		public DeleteTransactionResult(Guid userId, int accountId, int transactionId)
		{
			this.userId = userId;
			this.accountId = accountId;
			this.transactionId = transactionId;
		}

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
		{
			var proxy = new MoneyServiceClient();

			proxy.DeleteTransactionCompleted += OnDeleteCompleted;
			proxy.DeleteTransactionAsync(userId,
										 accountId,
										 transactionId,
										 DateTime.UtcNow
										 );
		}

		private void OnDeleteCompleted(object s, AsyncCompletedEventArgs e)
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