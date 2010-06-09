// <copyright file="IAccountService.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </editor>
// <summary>
//   Account service.
// </summary>

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fab.Server.Core
{
	/// <summary>
	/// Account service.
	/// </summary>
	[ServiceContract]
	public interface IAccountService
	{
		/// <summary>
		/// Create new account.
		/// </summary>
		/// <param name="userId">
		/// User unique ID for which this account should be created.
		/// </param>
		/// <param name="name">
		/// Account name.
		/// </param>
		/// <param name="assetTypeId">
		/// The asset type ID.
		/// </param>
		[OperationContract]
		void CreateAccount(Guid userId, string name, int assetTypeId);

		/// <summary>
		/// Update account details to new values.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="accountId">
		/// Account ID.
		/// </param>
		/// <param name="name">
		/// Account new name.
		/// </param>
		/// <param name="assetTypeId">
		/// The asset type ID.
		/// </param>
		[OperationContract]
		void UpdateAccount(Guid userId, int accountId, string name, int assetTypeId);

		/// <summary>
		/// Mark account as "deleted".
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="accountId">
		/// Account ID to mark as deleted.
		/// </param>
		[OperationContract]
		void DeleteAccount(Guid userId, int accountId);

		/// <summary>
		/// Retrieve all accounts for user.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <returns>
		/// All accounts.
		/// </returns>
		[OperationContract]
		IList<Account> GetAllAccounts(Guid userId);
	}
}