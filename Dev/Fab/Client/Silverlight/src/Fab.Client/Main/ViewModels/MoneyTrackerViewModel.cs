// <copyright file="MainViewModel.cs" company="HD">
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
// <summary>Main view model.</summary>

using System;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Caliburn.ShellFramework.History;
using Caliburn.ShellFramework.Questions;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Main view model.
	/// </summary>
	[HistoryKey("Main", typeof(MoneyTrackerViewModel))]
	[Singleton(typeof(IMoneyTrackerViewModel))]
	public class MoneyTrackerViewModel : Screen, ISupportCustomShutdown, IMoneyTrackerViewModel
	{
		#region Implementation of ISupportCustomShutdown

		/// <summary>
		/// Creates the shutdown model.
		/// </summary>
		/// <returns>Shutdown model.</returns>
		public ISubordinate CreateShutdownModel()
		{
			return new Question(this, "Are you sure you want to navigate away from this page?", Answer.Yes, Answer.No);
		}

		/// <summary>
		/// Determines whether this instance can shutdown based on the evaluated shutdown model.
		/// </summary>
		/// <param name="shutdownModel">The shutdown model.</param>
		/// <returns><c>true</c> if this instance can shutdown; otherwise, <c>false</c>.</returns>
		public bool CanShutdown(ISubordinate shutdownModel)
		{
			var question = (Question)shutdownModel;
			return question.Answer == Answer.Yes;
		}

		#endregion

		#region Overrides of ScreenBase

		/// <summary>
		/// Determines whether this instance can shutdown.
		/// </summary>
		/// <returns><c>true</c> if this instance can shutdown; otherwise, <c>false</c>.</returns>
		public override bool CanShutdown()
		{
			return true;
		}

		#endregion

		#region Overrides of Object

		//The following overrides insure that all instances of this screen are treated as
		//equal by the screen activation mechanism without forcing a singleton registration
		//in the container.

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="object"/>.</param>
		/// <exception cref="NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
		public override bool Equals(object obj)
		{
			return obj != null && obj.GetType() == GetType();
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			return GetType().GetHashCode();
		}

		#endregion

		/// <summary>
		/// Gets <see cref="AccountsViewModel"/>.
		/// </summary>
		public object Accounts { get; private set; }

		/// <summary>
		/// Gets <see cref="CategoriesViewModel"/>.
		/// </summary>
		public object Categories { get; private set; }

		/// <summary>
		/// Gets <see cref="TransactionsViewModel"/>.
		/// </summary>
		public object Transactions { get; private set; }

		/// <summary>
		/// Gets <see cref="TransactionDetailsViewModel"/>.
		/// </summary>
		public object AddNew { get; private set; }

		/// <summary>
		/// Gets <see cref="TransferViewModel"/>.
		/// </summary>
		public object Transfer { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MoneyTrackerViewModel"/> class.
		/// </summary>
		public MoneyTrackerViewModel(
			IAccountsViewModel accountsVM,
			ICategoriesViewModel categoriesVM,
			ITransactionsViewModel transactionsVM,
			ITransactionDetailsViewModel transactionDetailsVM,
			ITransferViewModel transferViewModel)
		{
			Accounts = accountsVM;
			Categories = categoriesVM;
			Transactions = transactionsVM;
			AddNew = transactionDetailsVM;
			Transfer = transferViewModel;
		}
	}
}