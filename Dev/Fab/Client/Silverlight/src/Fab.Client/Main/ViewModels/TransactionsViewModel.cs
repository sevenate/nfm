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
using System.Collections.ObjectModel;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.PresentationFramework.ViewModels;
using Caliburn.ShellFramework.Results;
using Fab.Client.ApiServiceReference;
using Fab.Client.Models;
using Microsoft.Practices.ServiceLocation;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Transactions view model.
	/// </summary>
	public class TransactionsViewModel : BaseViewModel
	{
		#region Fields

		/// <summary>
		/// Transactions owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		/// <summary>
		/// Corresponding account of transactions.
		/// </summary>
		private readonly int accountId = 5;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionsViewModel"/> class.
		/// </summary>
		public TransactionsViewModel()
			: base(ServiceLocator.Current.GetInstance<IValidator>())
		{
			TransactionRecords = new ObservableCollection<TransactionRecord>();
		}

		#endregion

		/// <summary>
		/// Gets transaction records.
		/// </summary>
		public ObservableCollection<TransactionRecord> TransactionRecords { get; private set; }

		/// <summary>
		/// Download all transactions for specific account of the specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		public IEnumerable<IResult> DownloadAllTransactions()
		{
			yield return Show.Busy(new BusyScreen { Message = "Loading..." });

			var request = new GetAllTransactionsResult(userId, accountId);
			yield return request;

			TransactionRecords.Clear();

			foreach (var record in request.TransactionRecords)
			{
				TransactionRecords.Add(record);
			}

			yield return Show.NotBusy();
		}
	}
}