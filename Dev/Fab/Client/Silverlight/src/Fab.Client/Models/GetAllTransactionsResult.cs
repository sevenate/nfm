using System;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.ApiServiceReference;

namespace Fab.Client.Models
{
	public class GetAllTransactionsResult : IResult
    {
        private readonly Guid userId;
        private readonly int accountId;

        public GetAllTransactionsResult(Guid userId, int accountId)
        {
            this.userId = userId;
			this.accountId = accountId;
        }

        public Guid UserId
        {
            get { return userId; }
        }
 		
		public int AccountId
        {
            get { return accountId; }
        }

		public TransactionRecord[] TransactionRecords { get; set; }

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
        {
			var proxy = new TransactionServiceClient();
			
			proxy.GetAllTransactionsCompleted += (s, e) =>
			{
				if (e.Error != null)
				{
					Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(
						() => Completed(this, new ResultCompletionEventArgs { Error = e.Error }));
				}
				else
				{
					TransactionRecords = e.Result;
					Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => Completed(this, new ResultCompletionEventArgs()));
				}
			};

			proxy.GetAllTransactionsAsync(userId: userId, accountId: accountId);
        }
    }
}