// <copyright file="TransferViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-06-19" />
// <summary>Transfer view model.</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Data;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.PresentationFramework.ViewModels;
using Caliburn.ShellFramework.Results;
using Fab.Client.Models;
using Fab.Client.MoneyServiceReference;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Transfer view model.
	/// </summary>
	[Singleton(typeof(ITransferViewModel))]
	public class TransferViewModel : BaseViewModel, ITransferViewModel
	{
		private int? transactionId;

		/// <summary>
		/// Transaction owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		private DateTime operationDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);

		[DataType(DataType.DateTime)]
		public DateTime OperationDate
		{
			get { return operationDate; }
			set
			{
				operationDate = value;
				NotifyOfPropertyChange(() => OperationDate);
			}
		}

		private string comment;

		public string Comment
		{
			get { return comment; }
			set
			{
				comment = value;
				NotifyOfPropertyChange(() => Comment);
			}
		}

		private string amount;

		[Required]
		[DataType(DataType.Currency)]
		public string Amount
		{
			get { return amount; }
			set
			{
				amount = value;
				NotifyOfPropertyChange(() => Amount);
				NotifyOfPropertyChange(() => CanSave);
			}
		}

		public bool CanSave
		{
			get
			{
				return string.IsNullOrEmpty(Error);
			}
		}

		/// <summary>
		/// Gets or sets <see cref="IAccountsViewModel"/>.
		/// </summary>
		private IAccountsViewModel accountsVM;

		private readonly CollectionViewSource accounts1CollectionViewSource = new CollectionViewSource();
		private readonly CollectionViewSource accounts2CollectionViewSource = new CollectionViewSource();

		public ICollectionView Accounts1
		{
			get
			{
				return accounts1CollectionViewSource.View;
			}
		}

		public ICollectionView Accounts2
		{
			get
			{
				return accounts2CollectionViewSource.View;
			}
		}

		private bool isEditMode;

		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				isEditMode = value;
				NotifyOfPropertyChange(() => IsEditMode);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TransferViewModel"/> class.
		/// </summary>
		/// <param name="validator">Validator for view model data.</param>
		/// <param name="accountsVM">Accounts view model.</param>
		public TransferViewModel(IValidator validator, IAccountsViewModel accountsVM)
			: base(validator)
		{
			this.accountsVM = accountsVM;

			var accounts1 = new BindableCollection<Account>();
			accounts1CollectionViewSource.Source = accounts1;

			this.accountsVM.Reloaded += (sender, args) =>
			{
				accounts1.Clear();

				foreach (var account in this.accountsVM.Accounts)
				{
					accounts1.Add(account as Account);
				}

				if (!accounts1CollectionViewSource.View.IsEmpty)
				{
					accounts1CollectionViewSource.View.MoveCurrentToFirst();
				}
			};

			var accounts2 = new BindableCollection<Account>();
			accounts2CollectionViewSource.Source = accounts2;

			this.accountsVM.Reloaded += (sender, args) =>
			{
				accounts2.Clear();

				foreach (var account in this.accountsVM.Accounts)
				{
					accounts2.Add(account as Account);
				}

				if (!accounts2CollectionViewSource.View.IsEmpty)
				{
					accounts2CollectionViewSource.View.MoveCurrentToFirst();
				}
			};
		}

		public void Clear()
		{
			transactionId = null;
			IsEditMode = false;
			Accounts1.MoveCurrentToFirst();
			Accounts2.MoveCurrentToFirst();
			OperationDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
			Amount = string.Empty;
			Comment = string.Empty;
		}

		public IEnumerable<IResult> Save()
		{
			yield return Show.Busy(new BusyScreen { Message = "Saving..." });

			if (IsEditMode)
			{
				var request = new UpdateTransferResult(
									transactionId.Value,
									userId,
									((Account)Accounts1.CurrentItem).Id,
									userId,
									((Account)Accounts2.CurrentItem).Id,
									OperationDate.Kind == DateTimeKind.Unspecified
										? DateTime.SpecifyKind(OperationDate, DateTimeKind.Local) + DateTime.Now.TimeOfDay
										: OperationDate,
									decimal.Parse(Amount.Trim()),
									Comment != null ? Comment.Trim() : null
								);

				Clear();

				yield return request;
			}
			else
			{
				var request = new AddTransferResult(
									userId,
									((Account)Accounts1.CurrentItem).Id,
									userId,
									((Account)Accounts2.CurrentItem).Id,
									OperationDate.Kind == DateTimeKind.Unspecified
										? DateTime.SpecifyKind(OperationDate, DateTimeKind.Local) + DateTime.Now.TimeOfDay
										: OperationDate,
									decimal.Parse(Amount.Trim()),
									Comment != null ? Comment.Trim() : null
								);

				yield return request;
			}

			yield return Show.NotBusy();
		}

		/// <summary>
		/// Open specific transfer transaction to edit.
		/// </summary>
		/// <param name="transaction">Transaction to edit.</param>
		public void Edit(Transaction transaction)
		{
			transactionId = transaction.Id;

			// Todo: refactor this method!
			var accountsSource1 = accounts1CollectionViewSource.Source as BindableCollection<Account>;

			if (accountsSource1 != null)
			{
				var selectedAccount1 = accountsSource1.Where(a => a.Id == transaction.Postings[1].Account.Id).Single();
				Accounts1.MoveCurrentTo(selectedAccount1);
			}

			var accountsSource2 = accounts2CollectionViewSource.Source as BindableCollection<Account>;

			if (accountsSource2 != null)
			{
				var selectedAccount2 = accountsSource2.Where(a => a.Id == transaction.Postings[0].Account.Id).Single();
				Accounts2.MoveCurrentTo(selectedAccount2);
			}

			OperationDate = transaction.Postings[0].Date.ToLocalTime();
			Amount = transaction.Postings[0].Amount.ToString();
			Comment = transaction.Comment;

			IsEditMode = true;
		}
	}
}