// <copyright file="IAccountsViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-13</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-13</date>
// </editor>
// <summary>General accounts view model interface.</summary>

using System.Collections.Generic;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.ApiServiceReference;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// General accounts view model interface.
	/// </summary>
	public interface IAccountsViewModel
	{
		/// <summary>
		/// Gets accounts for specific user.
		/// </summary>
		IObservableCollection<Account> Accounts { get; }

		/// <summary>
		/// Download all accounts for specific user.
		/// </summary>
		/// <returns>Operation result.</returns>
		IEnumerable<IResult> LoadAllAccounts();
	}
}