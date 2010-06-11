// <copyright file="ModelHelper.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-15</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-15</date>
// </editor>
// <summary>Helper for Entity Framework model container processing.</summary>

using System;
using System.Linq;

namespace Fab.Server.Core
{
	/// <summary>
	/// Helper for Entity Framework model container processing.
	/// </summary>
	internal static class ModelHelper
	{
		/// <summary>
		/// System user ID.
		/// </summary>
		private static readonly Guid SystemUserId = new Guid("6184b6dd-26d0-4d06-ba2c-95c850ccfebe");

		/// <summary>
		/// Check is user <paramref name="login"/> name is not used by some one else.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="login">User login name.</param>
		/// <returns><c>true</c> if user login name is unique.</returns>
		internal static bool IsLoginAvailable(ModelContainer mc, string login)
		{
			return mc.Users.Where(u => u.Login == login)
							.SingleOrDefault() == null;
		}

		/// <summary>
		/// Get <see cref="User"/> from model container by unique ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="userId">Unique user ID.</param>
		/// <returns>User instance.</returns>
		internal static User GetUserById(ModelContainer mc, Guid userId)
		{
			User user = mc.Users.Where(u => u.Id == userId)
							.SingleOrDefault();

			if (user == null)
			{
				throw new Exception("User with ID " + userId + " not found.");
			}

			return user;
		}

		/// <summary>
		/// Get <see cref="Account"/> from model container by unique user ID and account ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="accountId">Account ID.</param>
		/// <returns>Account instance</returns>
		internal static Account GetAccountById(ModelContainer mc, int accountId)
		{
			Account account = mc.Accounts.Include("AssetType")
								.Where(a => a.Id == accountId)
								.SingleOrDefault();

			if (account == null)
			{
				throw new Exception("Account with ID = " + accountId + " not found.");
			}

			return account;
		}

		/// <summary>
		/// Get system "cash" account with specific <paramref name="assetTypeId"/> from model container.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="assetTypeId">The asset type ID.</param>
		/// <returns>System "cash" account instance.</returns>
		private static Account GetSystemAccount(ModelContainer mc, int assetTypeId)
		{
			Account account = mc.Accounts.Include("AssetType")
								.Where(a => a.AssetType.Id == assetTypeId && a.User.Id == SystemUserId)
								.SingleOrDefault();

			if (account == null)
			{
				throw new Exception("System account with asset type ID = " + assetTypeId + " not found.");
			}

			return account;
		}

		/// <summary>
		/// Get <see cref="Category"/> from model container by unique user ID and category ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="categoryId">Category ID.</param>
		/// <returns>Category instance</returns>
		internal static Category GetCategoryById(ModelContainer mc, int categoryId)
		{
			Category category = mc.Categories.Where(c => c.Id == categoryId)
									.SingleOrDefault();

			if (category == null)
			{
				throw new Exception("Category with ID = " + categoryId + " not found.");
			}

			return category;
		}

		/// <summary>
		/// Get <see cref="AssetType"/> from model container by unique ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="assetTypeId">The asset type ID.</param>
		/// <returns>Asset type instance</returns>
		internal static AssetType GetAssetTypeById(ModelContainer mc, int assetTypeId)
		{
			AssetType assetType = mc.AssetTypes.Where(at => at.Id == assetTypeId)
									.SingleOrDefault();

			if (assetType == null)
			{
				throw new Exception("Asset type with ID = " + assetTypeId + " not found.");
			}

			return assetType;
		}

		/// <summary>
		/// Get <see cref="Transaction"/> from model container by unique ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="transactionId">The transaction ID.</param>
		/// <returns>Transaction instance.</returns>
		internal static Transaction GetTransacionById(ModelContainer mc, int transactionId)
		{
			// Todo: consider using not all includes here to increase performance and decrease traffic
			mc.Journals.Include("Category");
			mc.Journals.Include("Postings");
			mc.Postings.Include("Account");
			mc.Postings.Include("AssetType");

			Transaction transacion = mc.Journals.Where(j => j.Id == transactionId && j is Transaction && j.IsDeleted == false)
												.Select(j => j as Transaction)
												.SingleOrDefault();
			if (transacion == null)
			{
				throw new Exception("Transaction with ID = " + transactionId + " not found.");
			}

			return transacion;
		}

		// Todo: add user ID account ID to the GetTransacionById() method call to
		// join them with transaction ID to prevent unauthorized delete 
		// Do this for all user-aware calls (i.e. Categories, Accounts etc.)

		/// <summary>
		/// Delete specific transaction.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="transactionId">The transaction ID.</param>
		/// <param name="operationDate">Operation date.</param>
		internal static void DeleteTransaction(ModelContainer mc, int transactionId, DateTime operationDate)
		{
			Transaction transaction = GetTransacionById(mc, transactionId);
			transaction.IsDeleted = true;

			var deletedJournal = new DeletedJournal
			                     	{
			                     		JournalType = (byte)JournalType.Canceled,
			                     		Comment = transaction.Comment
			                     	};

			mc.Journals.AddObject(deletedJournal);

			foreach (var originalPost in transaction.Postings)
			{
				var newPost = new Posting
				              	{
				              		Journal = deletedJournal,
				              		Date = operationDate,
				              		Amount = -originalPost.Amount,
				              		Account = originalPost.Account,
				              		AssetType = originalPost.AssetType
				              	};

				mc.Postings.AddObject(newPost);
			}
		}

		/// <summary>
		/// Create deposit or withdrawal transaction: (<paramref name="price"/> * <paramref name="quantity"/>) amount of assets
		/// to/from the <paramref name="accountId"/> with optional <paramref name="comment"/> and
		/// group it under <paramref name="categoryId"/> if necessary.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="accountId">Account 1 ID</param>
		/// <param name="journalType"><see cref="JournalType.Deposit"/> or <see cref="JournalType.Withdrawal"/> only</param>
		/// <param name="categoryId">The category ID.</param>
		/// <param name="price">Price of the item.</param>
		/// <param name="quantity">Quantity of the item.</param>
		/// <param name="comment">Comment notes.</param>
		internal static void CreateTransaction(
			ModelContainer mc,
			DateTime operationDate,
			int accountId,
			JournalType journalType,
			int? categoryId,
			decimal price,
			decimal quantity,
			string comment)
		{
			var amount = price * quantity;

			var targetAccount = GetAccountById(mc, accountId);
			var cashAccount = GetSystemAccount(mc, targetAccount.AssetType.Id);

			var transaction = new Transaction
			                  	{
			                  		JournalType = (byte)journalType,
			                  		Price = price,
			                  		Quantity = quantity,
			                  		Comment = comment
			                  	};

			if (categoryId.HasValue)
			{
				transaction.Category = GetCategoryById(mc, categoryId.Value);
			}

			var creditAccount = journalType == JournalType.Deposit
			                    	? targetAccount
			                    	: cashAccount;

			var debitAccount = journalType == JournalType.Deposit
			                   	? cashAccount
			                   	: targetAccount;

			var creditPosting = new Posting
			                    	{
			                    		Date = operationDate,
			                    		Amount = amount,
			                    		Journal = transaction,
			                    		Account = creditAccount,
			                    		AssetType = creditAccount.AssetType
			                    	};

			var debitPosting = new Posting
			                   	{
			                   		Date = operationDate,
			                   		Amount = -amount,
			                   		Journal = transaction,
			                   		Account = debitAccount,
			                   		AssetType = debitAccount.AssetType
			                   	};

			mc.Journals.AddObject(transaction);
			mc.Postings.AddObject(creditPosting);
			mc.Postings.AddObject(debitPosting);
		}

		/// <summary>
		/// Create transfer transaction: the <paramref name="amount"/> of assets are moved
		/// from <paramref name="account1Id"/> to <paramref name="account2Id"/> of
		/// with optional <paramref name="comment"/> about operation.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="account1Id">Account 1 ID.</param>
		/// <param name="account2Id">Account 2 ID.</param>
		/// <param name="amount">Amount of assets.</param>
		/// <param name="comment">Comment notes.</param>
		internal static void CreateTransfer(
			ModelContainer mc,
			DateTime operationDate,
			int account1Id,
			int account2Id,
			decimal amount,
			string comment)
		{
			var sourceAccount = GetAccountById(mc, account1Id);
			var targetAccount = GetAccountById(mc, account2Id);

			var transaction = new Transaction
			                  	{
			                  		JournalType = (byte)JournalType.Transfer,
			                  		Price = amount,
			                  		Quantity = 1,
			                  		Comment = comment
			                  	};

			var creditPosting = new Posting
			                    	{
			                    		Date = operationDate,
			                    		Amount = amount,
			                    		Journal = transaction,
			                    		Account = targetAccount,
			                    		AssetType = targetAccount.AssetType
			                    	};

			var debitPosting = new Posting
			                   	{
			                   		Date = operationDate,
			                   		Amount = -amount,
			                   		Journal = transaction,
			                   		Account = sourceAccount,
			                   		AssetType = sourceAccount.AssetType
			                   	};

			mc.Journals.AddObject(transaction);
			mc.Postings.AddObject(creditPosting);
			mc.Postings.AddObject(debitPosting);
		}
	}
}