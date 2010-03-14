// <copyright file="ITransactionService.cs" company="HD">
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
//   Transaction service.
// </summary>

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fab.Server.Core
{
	/// <summary>
	/// Transaction service.
	/// </summary>
	[ServiceContract]
	public interface ITransactionService
	{
		/// <summary>
		/// Gets all available asset types (i.e. "currency names").
		/// </summary>
		/// <returns>
		/// Asset types presented in the system.
		/// </returns>
		[OperationContract]
		IList<AssetType> GetAllAssetTypes();

		/// <summary>
		/// Gets all available journal types (i.e. "Deposit", "Withdrawal", "Transfer" etc.).
		/// </summary>
		/// <returns>
		/// Journal types presented in the system.
		/// </returns>
		[OperationContract]
		IList<JournalType> GetAllJournalTypes();

		/// <summary>
		/// Deposit (<paramref name="price"/> * <paramref name="quantity"/>) ammout to the
		/// <paramref name="accountId"/> of the <paramref name="userId"/> with optional <paramref name="comment"/> and
		/// group it under <paramref name="categoryId"/> if necessary.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="accountId">
		/// Account ID.
		/// </param>
		/// <param name="price">
		/// Price of the item.
		/// </param>
		/// <param name="quantity">
		/// Quantity of the item.
		/// </param>
		/// <param name="comment">
		/// Comment notes.
		/// </param>
		/// <param name="categoryId">
		/// The category Id.
		/// </param>
		[OperationContract]
		void Deposit(Guid userId, int accountId, decimal price, decimal quantity, string comment, int? categoryId);

		/// <summary>
		/// Withdrawal (<paramref name="price"/> * <paramref name="quantity"/>) ammout from the
		/// <paramref name="accountId"/> of the <paramref name="userId"/> with optional <paramref name="comment"/> and
		/// group it under <paramref name="categoryId"/> if necessary.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="accountId">
		/// Account ID.
		/// </param>
		/// <param name="price">
		/// Price of the item.
		/// </param>
		/// <param name="quantity">
		/// Quantity of the item.
		/// </param>
		/// <param name="comment">
		/// Comment notes.
		/// </param>
		/// <param name="categoryId">
		/// The category Id.
		/// </param>
		[OperationContract]
		void Withdrawal(Guid userId, int accountId, decimal price, decimal quantity, string comment, int? categoryId);

		/// <summary>
		/// Transfer from <paramref name="account1Id"/> of <paramref name="user1Id"/> to
		/// <paramref name="account2Id"/> of <paramref name="user2Id"/> the <paramref name="amount"/> of assets
		/// with optional <paramref name="comment"/> about operation.
		/// </summary>
		/// <param name="user1Id">
		/// User 1 unique ID.
		/// </param>
		/// <param name="account1Id">
		/// Account 1 ID.
		/// </param>
		/// <param name="user2Id">
		/// User 2 unique ID.
		/// </param>
		/// <param name="account2Id">
		/// Account 2 ID.
		/// </param>
		/// <param name="amount">
		/// Ammout of assets.
		/// </param>
		/// <param name="comment">
		/// Comment notes.
		/// </param>
		[OperationContract]
		void Transfer(Guid user1Id, int account1Id, Guid user2Id, int account2Id, decimal amount, string comment);

		/// <summary>
		/// Get current account balance.
		/// </summary>
		/// <param name="userId">
		/// Unique user ID.
		/// </param>
		/// <param name="accountId">
		/// Accound ID.
		/// </param>
		/// <returns>
		/// Current account balance.
		/// </returns>
		[OperationContract]
		decimal GetAccountBalance(Guid userId, int accountId);

		/// <summary>
		/// Return all not deleted transaction records for specific accout.
		/// </summary>
		/// <param name="userId">
		/// The user unique ID.
		/// </param>
		/// <param name="accountId">
		/// The account Id.
		/// </param>
		/// <returns>
		/// List of transaction records.
		/// </returns>
		[OperationContract]
		IList<TransactionRecord> GetAllTransactions(Guid userId, int accountId);
	}
}