// <copyright file="MoneyService.svc.cs" company="HD">
//  Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-01-28" />
// <summary>Money service.</summary>

using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Fab.Server.Core;
using Fab.Server.Core.DTO;

namespace Fab.Server
{
	/// <summary>
	/// Money service.
	/// </summary>
	public class MoneyService : IMoneyService
	{
		#region Implementation of IMoneyService

		#region Accounts

		/// <summary>
		/// Create new account.
		/// </summary>
		/// <param name="userId">User unique ID for which this account should be created.</param>
		/// <param name="name">Account name.</param>
		/// <param name="assetTypeId">The asset type ID.</param>
		public void CreateAccount(Guid userId, string name, int assetTypeId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Account name must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				User user = ModelHelper.GetUserById(mc, userId);
				AssetType assetType = ModelHelper.GetAssetTypeById(mc, assetTypeId);

				var account = new Account
				              	{
				              		Name = name.Trim(), 
				              		Created = DateTime.UtcNow, 
				              		IsDeleted = false, 
				              		User = user,
				              		AssetType = assetType
				              	};

				mc.Accounts.AddObject(account);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Update account details to new values.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="accountId">Account ID.</param>
		/// <param name="name">Account new name.</param>
		/// <param name="assetTypeId">The asset type ID.</param>
		public void UpdateAccount(Guid userId, int accountId, string name, int assetTypeId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Account name must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				Account account = ModelHelper.GetAccountById(mc, accountId);
				AssetType assetType = ModelHelper.GetAssetTypeById(mc, assetTypeId);

				account.Name = name.Trim();
				account.AssetType = assetType;

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Mark account as "deleted".
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="accountId">Account ID to mark as deleted.</param>
		public void DeleteAccount(Guid userId, int accountId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				Account account = ModelHelper.GetAccountById(mc, accountId);

				account.IsDeleted = true;

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Retrieve all accounts for user.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <returns>All accounts.</returns>
		public IList<AccountDTO> GetAllAccounts(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				var assetTypeMapper = ObjectMapperManager.DefaultInstance.GetMapper<AssetType, AssetTypeDTO>();
				var accountMapper = ObjectMapperManager.DefaultInstance.GetMapper<Account, AccountDTO>(
										new DefaultMapConfig()
										.ConvertUsing<AssetType, AssetTypeDTO>(assetTypeMapper.Map));

				return mc.Accounts.Include("AssetType")
					.Where(a => a.User.Id == userId && a.IsDeleted == false)
					.OrderBy(a => a.Created)
					.ToList()
					.Select(accountMapper.Map)
					.ToList();
			}
		}

		/// <summary>
		/// Get current account balance.
		/// </summary>
		/// <param name="userId">Unique user ID.</param>
		/// <param name="accountId">Account ID.</param>
		/// <returns>Current account balance.</returns>
		public decimal GetAccountBalance(Guid userId, int accountId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			decimal balance;

			using (var mc = new ModelContainer())
			{
				// Todo: Fix Sum() of Postings when there is no any posting yet.
				var firstPosting = mc.Postings.Where(p => p.Account.Id == accountId)
					.FirstOrDefault();
				balance = firstPosting != null
				          	? mc.Postings.Where(p => p.Account.Id == accountId)
				          	  	.Sum(p => p.Amount)
				          	: 0;
			}

			return balance;
		}

		#endregion

		#region Categories

		/// <summary>
		/// Create new category.
		/// </summary>
		/// <param name="userId">User unique ID for which this category should be created.</param>
		/// <param name="name">Category name.</param>
		/// <param name="categoryType">Category type.</param>
		public void CreateCategory(Guid userId, string name, byte categoryType)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Category name must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				User user = ModelHelper.GetUserById(mc, userId);

				var category = new Category
				               	{
				               		Name = name.Trim(),
									CategoryType = categoryType,
				               		IsDeleted = false,
				               		User = user
				               	};

				mc.Categories.AddObject(category);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Update category details to new values.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="categoryId">Category ID.</param>
		/// <param name="name">Category new name.</param>
		/// <param name="categoryType">Category new type.</param>
		public void UpdateCategory(Guid userId, int categoryId, string name, byte categoryType)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Category name must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				Category category = ModelHelper.GetCategoryById(mc, categoryId);

				category.Name = name.Trim();
				category.CategoryType = categoryType;

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Mark category as "deleted".
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="categoryId">Category ID to mark as deleted.</param>
		public void DeleteCategory(Guid userId, int categoryId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				Category category = ModelHelper.GetCategoryById(mc, categoryId);

				category.IsDeleted = true;

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Retrieve all categories for user.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <returns>All categories.</returns>
		public IList<CategoryDTO> GetAllCategories(Guid userId)
		{
			using (var mc = new ModelContainer())
			{
				var categoryMapper = ObjectMapperManager.DefaultInstance.GetMapper<Category, CategoryDTO>();

				return mc.Categories.Where(c => c.User.Id == userId && c.IsDeleted == false)
					.OrderBy(c => c.Name)
					.ToList()
					.Select(categoryMapper.Map)
					.ToList();
			}
		}

		#endregion

		#region Transactions

		/// <summary>
		/// Gets all available asset types (i.e. "currency names").
		/// </summary>
		/// <returns>Asset types presented in the system.</returns>
		public IList<AssetTypeDTO> GetAllAssetTypes()
		{
			using (var mc = new ModelContainer())
			{
				var assetTypeMapper = ObjectMapperManager.DefaultInstance.GetMapper<AssetType, AssetTypeDTO>();

				return mc.AssetTypes.ToList()
					.Select(assetTypeMapper.Map)
					.ToList();
			}
		}

		/// <summary>
		/// Deposit (<paramref name="price"/> * <paramref name="quantity"/>) amount to the
		/// <paramref name="accountId"/> of the <paramref name="userId"/> with optional <paramref name="comment"/> and
		/// group it under <paramref name="categoryId"/> if necessary.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="accountId">Account ID.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="price">Price of the item.</param>
		/// <param name="quantity">Quantity of the item.</param>
		/// <param name="comment">Comment notes.</param>
		/// <param name="categoryId">The category ID.</param>
		public void Deposit(
			Guid userId,
			int accountId,
			DateTime operationDate,
			decimal price,
			decimal quantity,
			string comment,
			int? categoryId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();

			using (var mc = new ModelContainer())
			{
				ModelHelper.CreateTransaction(mc, dateTime, accountId, JournalType.Deposit, categoryId, price, quantity, comment);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Withdrawal (<paramref name="price"/> * <paramref name="quantity"/>) amount from the
		/// <paramref name="accountId"/> of the <paramref name="userId"/> with optional <paramref name="comment"/> and
		/// group it under <paramref name="categoryId"/> if necessary.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="accountId">Account ID.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="price">Price of the item.</param>
		/// <param name="quantity">Quantity of the item.</param>
		/// <param name="comment">Comment notes.</param>
		/// <param name="categoryId">The category Id.</param>
		public void Withdrawal(
			Guid userId,
			int accountId,
			DateTime operationDate,
			decimal price,
			decimal quantity,
			string comment,
			int? categoryId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();

			using (var mc = new ModelContainer())
			{
				ModelHelper.CreateTransaction(mc, dateTime, accountId, JournalType.Withdrawal, categoryId, price, quantity, comment);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Transfer from <paramref name="account1Id"/> of <paramref name="user1Id"/> to
		/// <paramref name="account2Id"/> of <paramref name="user2Id"/> the <paramref name="amount"/> of assets
		/// with optional <paramref name="comment"/> about operation.
		/// </summary>
		/// <param name="user1Id">User 1 unique ID.</param>
		/// <param name="account1Id">Account 1 ID.</param>
		/// <param name="user2Id">User 2 unique ID.</param>
		/// <param name="account2Id">Account 2 ID.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="amount">Amount of assets.</param>
		/// <param name="comment">Comment notes.</param>
		public void Transfer(
			Guid user1Id,
			int account1Id,
			Guid user2Id,
			int account2Id,
			DateTime operationDate,
			decimal amount,
			string comment)
		{
			if (user1Id == Guid.Empty)
			{
				throw new ArgumentException("User1 ID must not be empty.");
			}

			if (user2Id == Guid.Empty)
			{
				throw new ArgumentException("User2 ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();

			using (var mc = new ModelContainer())
			{
				ModelHelper.CreateTransfer(mc, dateTime, account1Id, account2Id, amount, comment);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Return full data about single transaction data.
		/// </summary>
		/// <param name="userId">The user unique ID.</param>
		/// <param name="accountId">The account ID.</param>
		/// <param name="transactionId">Transaction ID.</param>
		/// <returns>Single transaction data.</returns>
		public TransactionDTO GetTransaction(Guid userId, int accountId, int transactionId)
		{
			using (var mc = new ModelContainer())
			{
				var accountMapper = ObjectMapperManager.DefaultInstance.GetMapper<Account, AccountDTO>();
				var postingMapper = ObjectMapperManager.DefaultInstance.GetMapper<Posting, PostingDTO>();
				var categoryMapper = ObjectMapperManager.DefaultInstance.GetMapper<Category, CategoryDTO>();
				var transactionMapper = ObjectMapperManager.DefaultInstance.GetMapper<Transaction, TransactionDTO>(
										new DefaultMapConfig()
										.ConvertUsing<Account, AccountDTO>(accountMapper.Map)
										.ConvertUsing<EntityCollection<Posting>, List<PostingDTO>>(postings => postings.Select(postingMapper.Map).ToList())
										.ConvertUsing<Posting, PostingDTO>(postingMapper.Map)
										.ConvertUsing<Category, CategoryDTO>(categoryMapper.Map));

				// Todo: add user ID account ID to the GetTransacionById() method call to
				// join them with transaction ID to prevent unauthorized delete 
				// Do this for all user-aware calls (i.e. Categories, Accounts etc.)
				return transactionMapper.Map(ModelHelper.GetTransacionById(mc, transactionId));
			}
		}

		/// <summary>
		/// Delete specific transaction.
		/// </summary>
		/// <param name="userId">The user unique ID.</param>
		/// <param name="accountId">The account ID.</param>
		/// <param name="transactionId">Transaction ID.</param>
		/// <param name="operationDate">Operation date.</param>
		public void DeleteTransaction(Guid userId, int accountId, int transactionId, DateTime operationDate)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();

			using (var mc = new ModelContainer())
			{
				ModelHelper.DeleteTransaction(mc, transactionId, dateTime);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Update specific deposit or withdrawal transaction.
		/// </summary>
		/// <remarks>
		/// Transfer transaction are not updatable with this method.
		/// To update transfer transaction use <see cref="UpdateTransfer"/> method instead.
		/// </remarks>
		/// <param name="transactionId">Transaction ID.</param>
		/// <param name="userId">User unique ID.</param>
		/// <param name="accountId">Account ID.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="price">Price of the item.</param>
		/// <param name="quantity">Quantity of the item.</param>
		/// <param name="comment">Comment notes.</param>
		/// <param name="categoryId">The category Id.</param>
		/// <param name="isDeposit">
		/// <c>true</c> means that transaction is "Deposit";
		/// <c>false</c> means that transaction is "Withdrawal".
		/// </param>
		public void UpdateTransaction(
			int transactionId,
			Guid userId,
			int accountId,
			DateTime operationDate,
			decimal price,
			decimal quantity,
			string comment,
			int? categoryId,
			bool isDeposit)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();

			using (var mc = new ModelContainer())
			{
				// Todo: add user ID account ID to the GetTransacionById() method call to
				// join them with transaction ID to prevent unauthorized delete 
				// Do this for all user-aware calls (i.e. Categories, Accounts etc.)
				Transaction transaction = ModelHelper.GetTransacionById(mc, transactionId);

				if (transaction.JournalType != (byte)JournalType.Deposit
					&& transaction.JournalType != (byte)JournalType.Withdrawal)
				{
					throw new NotSupportedException(string.Format("Only {0} and {1} journal types supported.", JournalType.Deposit, JournalType.Withdrawal));
				}

				ModelHelper.DeleteTransaction(mc, transactionId, dateTime);
				ModelHelper.CreateTransaction(
					mc,
					dateTime,
					accountId,
					isDeposit
						? JournalType.Deposit
						: JournalType.Withdrawal,
					categoryId,
					price,
					quantity,
					comment);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Update specific transfer transaction.
		/// </summary>
		/// <remarks>
		/// Deposit or withdrawal transactions are not updatable with this method.
		/// To update deposit or withdrawal transaction use <see cref="UpdateTransaction"/> method instead.
		/// </remarks>
		/// <param name="transactionId">Transfer transaction ID.</param>
		/// <param name="user1Id">User 1 unique ID.</param>
		/// <param name="account1Id">Account 1 ID.</param>
		/// <param name="user2Id">User 2 unique ID.</param>
		/// <param name="account2Id">Account 2 ID.</param>
		/// <param name="operationDate">Operation date.</param>
		/// <param name="amount">Amount of assets.</param>
		/// <param name="comment">Comment notes.</param>
		/// <exception cref="NotSupportedException">
		/// Only <see cref="JournalType.Transfer"/> journal type supported.
		/// </exception>
		public void UpdateTransfer(
			int transactionId,
			Guid user1Id,
			int account1Id,
			Guid user2Id,
			int account2Id,
			DateTime operationDate,
			decimal amount,
			string comment)
		{
			if (user1Id == Guid.Empty)
			{
				throw new ArgumentException("User1 ID must not be empty.");
			}

			if (user2Id == Guid.Empty)
			{
				throw new ArgumentException("User2 ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();

			using (var mc = new ModelContainer())
			{
				// Todo: add user ID account ID to the GetTransacionById() method call to
				// join them with transaction ID to prevent unauthorized delete 
				// Do this for all user-aware calls (i.e. Categories, Accounts etc.)
				Transaction transaction = ModelHelper.GetTransacionById(mc, transactionId);

				if (transaction.JournalType != (byte)JournalType.Transfer)
				{
					throw new NotSupportedException(string.Format("Only {0} journal type supported.", JournalType.Transfer));
				}
				
				ModelHelper.DeleteTransaction(mc, transactionId, dateTime);
				ModelHelper.CreateTransfer(mc, dateTime, account1Id, account2Id, amount, comment);
				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Return all not deleted transaction records for specific account.
		/// </summary>
		/// <param name="userId">The user unique ID.</param>
		/// <param name="accountId">The account Id.</param>
		/// <returns>List of transaction records.</returns>
		public IList<TransactionRecord> GetAllTransactions(Guid userId, int accountId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var records = new List<TransactionRecord>();

			// Bug: warning security weakness!
			// Check User.IsDisabled + Account.IsDeleted also
			using (var mc = new ModelContainer())
			{
				var postings = from p in mc.Postings
				               where p.Account.Id == accountId
				                     && !p.Journal.IsDeleted
									 && p.Journal.JournalType != (byte)JournalType.Canceled
				               orderby p.Date, p.Journal.Id
				               select new
				                      	{
				                      		Posting = p,
				                      		p.Journal,
				                      		p.Journal.Category,
				                      		p.Journal.JournalType
				                      	};

				var res = postings.ToList();

				// No transactions take place yet, so nothing to return
				if (res.Count == 0)
				{
					return records;
				}

				var categoryMapper = ObjectMapperManager.DefaultInstance.GetMapper<Category, CategoryDTO>();

				decimal income = 0;
				decimal expense = 0;
				decimal balance = 0;

				foreach (var r in res)
				{
					balance += r.Posting.Amount;

					switch (r.JournalType)
					{
							// Deposit
						case 1:
							income = r.Posting.Amount;
							expense = 0;
							break;

							// Withdrawal
						case 2:
							income = 0;
							expense = -r.Posting.Amount;
							break;

							// Transfer
						case 3:
							income = r.Posting.Amount > 0 ? r.Posting.Amount : 0;	// positive is "TO this account"
							expense = r.Posting.Amount < 0 ? -r.Posting.Amount : 0;	// negative is "FROM this account"
							break;
					}

					records.Add(new TransactionRecord
					            	{
					            		TransactionId = r.Journal.Id,
					            		Date = DateTime.SpecifyKind(r.Posting.Date, DateTimeKind.Utc),
										Category = categoryMapper.Map(r.Category),
					            		Income = income,
					            		Expense = expense,
					            		Balance = balance,
					            		Comment = r.Journal.Comment
					            	});
				}
			}

			return records;
		}

		#endregion

		#endregion
	}
}