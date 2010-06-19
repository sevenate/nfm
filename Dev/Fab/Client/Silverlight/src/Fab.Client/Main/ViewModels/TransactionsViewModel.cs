// <copyright file="TransactionsViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-10</date>
// </editor>
// <summary>Transactions view model.</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.Actions;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.PresentationFramework.ViewModels;
using Caliburn.ShellFramework.Results;
using Fab.Client.ApiServiceReference;
using Fab.Client.Models;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Transactions view model.
	/// </summary>
	[Singleton(typeof(ITransactionsViewModel))]
	public class TransactionsViewModel : BaseViewModel, ITransactionsViewModel
	{
		#region Fields

		/// <summary>
		/// Transactions owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		/// <summary>
		/// Gets or sets <see cref="IAccountsViewModel"/>.
		/// </summary>
		private IAccountsViewModel accountsVM;

		/// <summary>
		/// Gets or sets <see cref="ITransactionDetailsViewModel"/>.
		/// </summary>
		private ITransactionDetailsViewModel transactionDetailsVM;

		/// <summary>
		/// Gets or sets <see cref="ITransferViewModel"/>.
		/// </summary>
		private ITransferViewModel transferVM;

		/// <summary>
		/// Corresponding account of transactions.
		/// </summary>
		private Account currentAccount;

		/// <summary>
		/// Gets or sets corresponding account of transactions.
		/// </summary>
		public Account CurrentAccount
		{
			get { return currentAccount; }
			private set
			{
				if (currentAccount != value)
				{
					currentAccount = value;
					NotifyOfPropertyChange(() => CurrentAccount);
					NotifyOfPropertyChange(() => CanDownloadAllTransactions);

					CallAction("DownloadAllTransactions");
				}
			}
		}

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionsViewModel"/> class.
		/// </summary>
		/// <param name="validator">Validator for view model data.</param>
		/// <param name="accountsVM">Accounts view model.</param>
		public TransactionsViewModel(IValidator validator, IAccountsViewModel accountsVM, ITransactionDetailsViewModel transactionDetailsVM, ITransferViewModel transferVM)
			: base(validator)
		{
			TransactionRecords = new BindableCollection<TransactionRecord>();
			this.accountsVM = accountsVM;
			this.transactionDetailsVM = transactionDetailsVM;
			this.transferVM = transferVM;

			this.accountsVM.Accounts.CurrentChanged += (o, eventArgs) =>
			{
				if (!this.accountsVM.Accounts.IsEmpty)
				{
					CurrentAccount = this.accountsVM.Accounts.CurrentItem as Account;
				}
			};
		}

		#endregion

		#region Implementation of ITransactionsViewModel

		/// <summary>
		/// Gets transaction records.
		/// </summary>
		public IObservableCollection<TransactionRecord> TransactionRecords { get; private set; }

		/// <summary>
		/// Download all transactions for specific account of the specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		public IEnumerable<IResult> DownloadAllTransactions()
		{
			yield return Show.Busy(new BusyScreen { Message = "Loading..." });

			var request = new GetAllTransactionsResult(userId, CurrentAccount.Id);
			yield return request;

			TransactionRecords.Clear();
			TransactionRecords.AddRange(request.TransactionRecords);

			if (Reloaded != null)
			{
				Reloaded(this, EventArgs.Empty);
			}

			yield return Show.NotBusy();
		}

		public bool CanDownloadAllTransactions
		{
			get
			{
				return CurrentAccount != null;
			}
		}

		/// <summary>
		/// Raised right after categories were reloaded from server.
		/// </summary>
		public event EventHandler<EventArgs> Reloaded;

		#endregion

		public IEnumerable<IResult> DeleteTransaction(int transactionId)
		{
			yield return Show.Busy(new BusyScreen { Message = "Deleting..." });

			// Remove transaction on server
			var request = new DeleteTransactionResult(userId, CurrentAccount.Id, transactionId);
			yield return request;

			// Remove transaction locally
			var transactionToDelete = TransactionRecords.Where(record => record.TransactionId == transactionId).Single();
			var index = TransactionRecords.IndexOf(transactionToDelete);
			TransactionRecords.Remove(transactionToDelete);

			// Correct remained balance for following transactions
			if (TransactionRecords.Count > 0 && index < TransactionRecords.Count)
			{
				var deletedAmount = transactionToDelete.Income > 0
														? -transactionToDelete.Income
														: transactionToDelete.Expense;

				for (int i = index; i < TransactionRecords.Count; i++)
				{
					TransactionRecords[i].Balance += deletedAmount;
				}
			}

			yield return Show.NotBusy();
		}

		public IEnumerable<IResult> EditTransaction(int transactionId)
		{
			yield return Show.Busy(new BusyScreen { Message = "Load transaction details..." });

			// Remove transaction on server
			var request = new LoadTransactionResult(userId, CurrentAccount.Id, transactionId);
			yield return request;

			// Todo: use JournalType enumeration here instead of byte.
			switch (request.Transaction.JournalType)
			{
				case 1:
				case 2:
					transactionDetailsVM.Edit(request.Transaction);
					break;

				case 3:
					transferVM.Edit(request.Transaction);
					break;

				default:
					throw new NotSupportedException("Transaction with journal type " + request.Transaction.JournalType +
					                                " is not editable.");
			}

			yield return Show.NotBusy();
		}

		private void CallAction(string methodName)
		{
			var view = (DependencyObject) GetView(null);
			var node = (InteractionNode) view.GetValue(DefaultRoutedMessageController.NodeProperty);
			var message = new ActionMessage
			              	{
			              		MethodName = methodName
			              	};
			node.ProcessMessage(message, null);
		}
	}
}