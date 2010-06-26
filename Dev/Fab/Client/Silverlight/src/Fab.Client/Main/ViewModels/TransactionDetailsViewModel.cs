// <copyright file="TransactionDetailsViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-04-11" />
// <summary>Single transaction details view model.</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
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
	/// Single transaction details view model.
	/// </summary>
	[Singleton(typeof(ITransactionDetailsViewModel))]
	public class TransactionDetailsViewModel : BaseViewModel, ITransactionDetailsViewModel
	{
		#region Fields

		private int? transactionId;

		/// <summary>
		/// Transaction owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		private string price;

		private DateTime operationDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);

		private string quantity = "1";

		private string comment;

		#endregion

		#region Properties

		private readonly CollectionViewSource accountsCollectionViewSource = new CollectionViewSource();

		private readonly CollectionViewSource categoriesCollectionViewSource = new CollectionViewSource();

		/// <summary>
		/// Gets or sets <see cref="IAccountsViewModel"/>.
		/// </summary>
		private IAccountsViewModel accountsVM;

		/// <summary>
		/// Gets or sets <see cref="ICategoriesViewModel"/>.
		/// </summary>
		private ICategoriesViewModel categoriesVM;

		/// <summary>
		/// Gets accounts for specific user.
		/// </summary>
		public ICollectionView Accounts
		{
			get
			{
				return accountsCollectionViewSource.View;
			}
		}

		/// <summary>
		/// Gets accounts for specific user.
		/// </summary>
		public ICollectionView Categories
		{
			get
			{
				return categoriesCollectionViewSource.View;
			}
		}

		private bool isDeposite;

		public bool IsDeposite
		{
			get { return isDeposite; }
			set
			{
				isDeposite = value;
				NotifyOfPropertyChange(() => IsDeposite);
			}
		}

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

		[Required]
		[DataType(DataType.Currency)]
		public string Price
		{
			get { return price; }
			set
			{
				price = value;
				NotifyOfPropertyChange(() => Price);
				NotifyOfPropertyChange(() => CanSave);
			}
		}

		[Required]
		public string Quantity
		{
			get { return quantity; }
			set
			{
				quantity = value;
				NotifyOfPropertyChange(() => Quantity);
				NotifyOfPropertyChange(() => CanSave);
			}
		}

		public string Comment
		{
			get { return comment; }
			set
			{
				comment = value;
				NotifyOfPropertyChange(() => Comment);
			}
		}

		public bool CanSave
		{
			get
			{
				return string.IsNullOrEmpty(Error);
			}
		}

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionDetailsViewModel"/> class.
		/// </summary>
		/// <param name="validator">Validator for view model data.</param>
		/// <param name="accountsVM">Accounts view model.</param>
		/// <param name="categoriesVM">Categories view model.</param>
		public TransactionDetailsViewModel(IValidator validator, IAccountsViewModel accountsVM, ICategoriesViewModel categoriesVM)
			: base(validator)
		{
			this.accountsVM = accountsVM;
			this.categoriesVM = categoriesVM;

			var accounts = new BindableCollection<Account>();
			accountsCollectionViewSource.Source = accounts;

			this.accountsVM.Reloaded += (sender, args) =>
			                       	{
										accounts.Clear();

										foreach (var account in this.accountsVM.Accounts)
										{
											accounts.Add(account as Account);
										}

			                       		if (!accountsCollectionViewSource.View.IsEmpty)
			                       		{
			                       			accountsCollectionViewSource.View.MoveCurrentToFirst();
			                       		}
			                       	};
			
			var categories = new BindableCollection<Category>();
			categoriesCollectionViewSource.Source = categories;

			this.categoriesVM.Reloaded += (sender, args) =>
									{
										categories.Clear();
										categories.AddRange(this.categoriesVM.Categories);
										
										if (!categoriesCollectionViewSource.View.IsEmpty)
										{
											categoriesCollectionViewSource.View.MoveCurrentToFirst();
										}
									};

			categoryFilter = (search, item) =>
			                 	{
									if (string.IsNullOrEmpty(search))
									{
										return true;
									}

									if (item is Category)
									{
										string searchToLower = search.ToLower(CultureInfo.InvariantCulture);
										return (item as Category).Name.ToLower(CultureInfo.InvariantCulture).Contains(searchToLower);
									}

									return false;
			                 	};
		}

		#endregion

		private AutoCompleteFilterPredicate<object> categoryFilter;

		public AutoCompleteFilterPredicate<object> CategoryFilter
		{
			get { return categoryFilter; }
			set
			{
				categoryFilter = value;
				NotifyOfPropertyChange(() => CategoryFilter);
			}
		}

		public Category CurrentCategory
		{
			get
			{
				var currentCategory = (Category) Categories.CurrentItem;
				return currentCategory != null && currentCategory.Id != -1
						? currentCategory
						: null;
			}
			set
			{
				if (value == null)
				{
					Categories.MoveCurrentTo(null);
					NotifyOfPropertyChange(() => CurrentCategory);
					return;
				}

				foreach (var category in Categories)
				{
					if (((Category) category).Id == value.Id)
					{
						if (Categories.MoveCurrentTo(category))
						{
							NotifyOfPropertyChange(() => CurrentCategory);
						}

						return;
					}
				}

				Categories.MoveCurrentTo(null);
				NotifyOfPropertyChange(() => CurrentCategory);
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

		public void Clear()
		{
			transactionId = null;
			IsEditMode = false;
			IsDeposite = false;
			Accounts.MoveCurrentToFirst();
			OperationDate = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Unspecified);
			CurrentCategory = null;
			Price = string.Empty;
			Quantity = "1";
			Comment = string.Empty;
		}

		public IEnumerable<IResult> Save()
		{
			yield return Show.Busy(new BusyScreen { Message = "Saving..." });

			if (IsEditMode)
			{
				// To not update date if it was not changed;
				// Add time of day to updated date only.
				DateTime date = OperationDate.Kind == DateTimeKind.Unspecified
						? DateTime.SpecifyKind(OperationDate, DateTimeKind.Local) + DateTime.Now.TimeOfDay
						: OperationDate;

				var request = new EditTransactionResult(
					transactionId.Value,
					userId,
					((Account) Accounts.CurrentItem).Id,
					date.ToUniversalTime(),
					decimal.Parse(Price.Trim()),
					decimal.Parse(Quantity.Trim()),
					Comment != null ? Comment.Trim() : null,
					CurrentCategory != null
						? CurrentCategory.Id
						: (int?) null,
					IsDeposite
					);

				Clear();

				yield return request;
			}
			else
			{
				// Add time of day to date.
				DateTime date = OperationDate.Kind == DateTimeKind.Unspecified
				                	? DateTime.SpecifyKind(OperationDate, DateTimeKind.Local) + DateTime.Now.TimeOfDay
				                	: OperationDate.Date + DateTime.Now.TimeOfDay;

				var request = new AddTransactionResult(
					userId,
					((Account) Accounts.CurrentItem).Id,
					date.ToUniversalTime(),
					decimal.Parse(Price.Trim()),
					decimal.Parse(Quantity.Trim()),
					Comment != null ? Comment.Trim() : null,
					CurrentCategory != null
						? CurrentCategory.Id
						: (int?) null,
					IsDeposite
					);

				yield return request;
			}

			yield return Show.NotBusy();
		}

		/// <summary>
		/// Open specific deposit or withdrawal transaction to edit.
		/// </summary>
		/// <param name="transaction">Transaction to edit.</param>
		public void Edit(Transaction transaction)
		{
			transactionId = transaction.Id;

			var currentSelectedAccount = accountsVM.Accounts.CurrentItem as Account;
			
			if (currentSelectedAccount != null)
			{
				var accouns = accountsCollectionViewSource.Source as BindableCollection<Account>;
				
				if (accouns != null)
				{
					var account = accouns.Where(account1 => account1.Id == currentSelectedAccount.Id).Single();
					Accounts.MoveCurrentTo(account);
				}
			}

			// Todo: use JournalType enumeration here instead of byte.
			IsDeposite = transaction.JournalType == 1;

			OperationDate = transaction.Postings.First().Date.ToLocalTime();
			CurrentCategory = transaction.Category;
			Price = transaction.Price.ToString();
			Quantity = transaction.Quantity.ToString();
			Comment = transaction.Comment;

			IsEditMode = true;
		}
	}
}