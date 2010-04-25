// <copyright file="TransactionDetailsViewModel.cs" company="HD">
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
// <summary>Single transaction details view model.</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using Caliburn.Core.IoC;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.PresentationFramework.ViewModels;
using Caliburn.ShellFramework.Results;
using Fab.Client.ApiServiceReference;
using Fab.Client.Models;
using Microsoft.Practices.ServiceLocation;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Single transaction details view model.
	/// </summary>
	[Singleton(typeof(ITransactionDetailsViewModel))]
	public class TransactionDetailsViewModel : BaseViewModel, ITransactionDetailsViewModel
	{
		#region Fields

		/// <summary>
		/// Transaction owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		/// <summary>
		/// Corresponding account of the transaction.
		/// </summary>
		private readonly int accountId = 5;

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionDetailsViewModel"/> class.
		/// </summary>
		public TransactionDetailsViewModel()
			: base(ServiceLocator.Current.GetInstance<IValidator>())
		{
			AccountsVM = ServiceLocator.Current.GetInstance<IAccountsViewModel>();
			CategoriesVM = ServiceLocator.Current.GetInstance<ICategoriesViewModel>();
		}

		#endregion

		/// <summary>
		/// Gets or sets <see cref="IAccountsViewModel"/>.
		/// </summary>
		private IAccountsViewModel AccountsVM { get; set; }

		/// <summary>
		/// Gets or sets <see cref="ICategoriesViewModel"/>.
		/// </summary>
		private ICategoriesViewModel CategoriesVM { get; set; }

		/// <summary>
		/// Gets accounts for specific user.
		/// </summary>
		public IObservableCollection<Account> Accounts
		{
			get
			{
				return AccountsVM.Accounts;
			}
		}

		/// <summary>
		/// Gets accounts for specific user.
		/// </summary>
		public IObservableCollection<Category> Categories
		{
			get
			{
				return CategoriesVM.Categories;
			}
		}

		public bool IsDeposite { get; set; }

		private string price;

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

		private string quantity;

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

		public bool CanSave
		{
			get
			{
				return string.IsNullOrEmpty(Error);
			}
		}

		public IEnumerable<IResult> Save()
		{
			yield return Show.Busy(new BusyScreen { Message = "Saving..." });

			var proxy = new TransactionServiceClient();

			var request = new AddTransactionResult(
				userId: userId,
				accountId: accountId,
				price: decimal.Parse(Price.Trim()),
				quantity: decimal.Parse(Quantity.Trim()),
				comment: Comment != null ? Comment.Trim() : null,
				categoryId: null,//(int)CategoryComboBox.SelectedValue
				isDeposit: IsDeposite
				);

			yield return request;

			yield return Show.NotBusy();
		}
	}
}