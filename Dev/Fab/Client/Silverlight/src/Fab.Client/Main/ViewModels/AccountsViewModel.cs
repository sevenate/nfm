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
	/// Accounts view model.
	/// </summary>
	public class AccountsViewModel : BaseViewModel
	{
		#region Fields

		/// <summary>
		/// Accounts owner ID.
		/// </summary>
		private readonly Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		#endregion

		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountsViewModel"/> class.
		/// </summary>
		public AccountsViewModel()
			: base(ServiceLocator.Current.GetInstance<IValidator>())
		{
			Accounts = new ObservableCollection<Account>();
		}

		#endregion

		/// <summary>
		/// Gets accounts for specific user.
		/// </summary>
		public ObservableCollection<Account> Accounts { get; private set; }

		/// <summary>
		/// Download all accounts for specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		public IEnumerable<IResult> LoadAllAccounts()
		{
			yield return Show.Busy(new BusyScreen { Message = "Loading..." });

			var request = new AccountsResult(userId);
			yield return request;

			Accounts.Clear();

			foreach (var record in request.Accounts)
			{
				Accounts.Add(record);
			}

			yield return Show.NotBusy();
		}
	}
}