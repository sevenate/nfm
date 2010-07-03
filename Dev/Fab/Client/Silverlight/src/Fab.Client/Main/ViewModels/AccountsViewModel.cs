// <copyright file="AccountsViewModel.cs" company="HD">
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
// <summary>Accounts view model.</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	/// Accounts view model.
	/// </summary>
	[Singleton(typeof(IAccountsViewModel))]
	public class AccountsViewModel : BaseViewModel, IAccountsViewModel
	{
		#region Fields

		/// <summary>
		/// Accounts owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("DC57BFF0-57A6-4BFC-9104-5F323ABBEDAB"); // 7F06BFA6-B675-483C-9BF3-F59B88230382

		private readonly BindableCollection<AccountDTO> accounts = new BindableCollection<AccountDTO>();

		private readonly CollectionViewSource accountsCollectionViewSource = new CollectionViewSource();

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountsViewModel"/> class.
		/// </summary>
		/// <param name="validator">Validator for view model data.</param>
		public AccountsViewModel(IValidator validator)
			: base(validator)
		{
			accountsCollectionViewSource.Source = accounts;
		}

		#endregion

		#region Imlementation of IAccountsViewModel

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
		/// Download all accounts for specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		public IEnumerable<IResult> LoadAllAccounts()
		{
			yield return Show.Busy(new BusyScreen { Message = "Loading..." });

			var request = new AccountsResult(userId);
			yield return request;
			
			accounts.Clear();
			accounts.AddRange(request.Accounts);
			accountsCollectionViewSource.View.MoveCurrentToFirst();

			if (Reloaded != null)
			{
				Reloaded(this, EventArgs.Empty);
			}

			yield return Show.NotBusy();
		}

		/// <summary>
		/// Raised right after accounts were reloaded from server.
		/// </summary>
		public event EventHandler<EventArgs> Reloaded;

		#endregion
	}
}