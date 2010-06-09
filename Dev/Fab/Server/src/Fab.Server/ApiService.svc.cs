// <copyright file="ApiService.svc.cs" company="HD">
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
//   Represent public server API available for clients.
// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using Fab.Server.Core;

namespace Fab.Server
{
	/// <summary>
	/// Represent public server API available for clients.
	/// </summary>
	public class ApiService : IUserService, IAccountService, ICategoryService, ITransactionService
	{
		#region Implementation of IUserService

		/// <summary>
		/// Generate unique login name for new user.
		/// </summary>
		/// <returns>Unique login name.</returns>
		public string GenerateUniqueLogin()
		{
			// Todo: use more sophisticate algorithm for uniqueness login generation 
			return "a" + Guid.NewGuid().GetHashCode();
		}

		/// <summary>
		/// Check user login name for uniqueness.
		/// </summary>
		/// <param name="login">User login.</param>
		/// <returns><c>true</c> if user login name is unique.</returns>
		public bool IsLoginAvailable(string login)
		{
			if (string.IsNullOrWhiteSpace(login))
			{
				throw new ArgumentException("Login must not be empty.");
			}

			// Remove leading and closing spaces (user typo)
			string newLogin = login.Trim();

			// Check login min & max length
			if (newLogin.Length < 5 || newLogin.Length > 50)
			{
				return false;
			}

			using (var mc = new ModelContainer())
			{
				return ModelHelper.IsLoginAvailable(mc, newLogin);
			}
		}

		/// <summary>
		/// Register new user with unique login name and password.
		/// </summary>
		/// <param name="login">User login name.</param>
		/// <param name="password">User password.</param>
		/// <returns>Created user ID.</returns>
		public Guid Register(string login, string password)
		{
			if (string.IsNullOrWhiteSpace(login))
			{
				throw new ArgumentException("Login must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentException("Password must not be empty.");
			}

			// Remove leading and closing spaces (user typo)
			string newLogin = login.Trim();

			// Check login min & max length
			if (newLogin.Length < 5 || newLogin.Length > 50)
			{
				return Guid.Empty;
			}

			// Check password min & max length
			if (password.Length < 5 || password.Length > 256)
			{
				return Guid.Empty;
			}

			using (var mc = new ModelContainer())
			{
				// Check login uniqueness
				if (!ModelHelper.IsLoginAvailable(mc, newLogin))
				{
					return Guid.Empty;
				}

				var user = new User
							{
								Id = Guid.NewGuid(), 
								Login = newLogin, 
								Password = password, // Todo: use hash algorithm instead of plain text here!
								Registered = DateTime.UtcNow, 
								IsDisabled = false
							};

				mc.Users.AddObject(user);
				mc.SaveChanges();

				return user.Id;
			}
		}

		/// <summary>
		/// Change user password or email to new values.
		/// </summary>
		/// <param name="userId">User unique ID.</param>
		/// <param name="oldPassword">User old password.</param>
		/// <param name="newPassword">User new password.</param>
		/// <param name="newEmail">User new email.</param>
		public void Update(Guid userId, string oldPassword, string newPassword, string newEmail)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(oldPassword))
			{
				throw new ArgumentException("Old password must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(newPassword))
			{
				throw new ArgumentException("New password must not be empty.");
			}

			// Check password min length
			if (newPassword.Length < 5)
			{
				throw new Exception("New password is too short.");
			}

			// Check password max length
			if (newPassword.Length > 256)
			{
				throw new Exception("New password is too long.");
			}

			using (var mc = new ModelContainer())
			{
				User user = ModelHelper.GetUserById(mc, userId);

				if (user.Password != oldPassword)
				{
					throw new Exception("Old password is incorrect.");
				}

				user.Password = newPassword;
				user.Email = string.IsNullOrWhiteSpace(newEmail)
								? null
				             	: newEmail.Trim();

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Get user ID by unique login name.
		/// </summary>
		/// <param name="login">User unique login name.</param>
		/// <returns>User unique ID.</returns>
		public Guid GetUserId(string login)
		{
			if (string.IsNullOrWhiteSpace(login))
			{
				throw new ArgumentException("Login must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				return mc.Users.Where(u => u.Login == login.Trim())
						.Select(u => u.Id)
						.SingleOrDefault();
			}
		}

		/// <summary>
		/// If user with specified login name have email and this email is match to specified email,
		/// then system will reset current password for this user to auto generated new one
		/// and sent it to the specified email.
		/// </summary>
		/// <param name="login">User login name.</param>
		/// <param name="email">User email.</param>
		public void ResetPassword(string login, string email)
		{
			if (string.IsNullOrWhiteSpace(login))
			{
				throw new ArgumentException("Login must not be empty.");
			}

			if (string.IsNullOrWhiteSpace(email))
			{
				throw new ArgumentException("Email must not be empty.");
			}

			throw new NotImplementedException();
		}

		#endregion

		#region Implementation of IAccountService

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
		public IList<Account> GetAllAccounts(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				return mc.Accounts.Include("AssetType")
						.Where(a => a.User.Id == userId && a.IsDeleted == false)
						.OrderBy(a => a.Created)
						.ToList();
			}
		}

		#endregion

		#region Implementation of ICategoryService

		/// <summary>
		/// Create new category.
		/// </summary>
		/// <param name="userId">User unique ID for which this category should be created.</param>
		/// <param name="name">Category name.</param>
		public void CreateCategory(Guid userId, string name)
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
		public void UpdateCategory(Guid userId, int categoryId, string name)
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
		public IList<Category> GetAllCategories(Guid userId)
		{
			using (var mc = new ModelContainer())
			{
				return mc.Categories.Where(c => c.User.Id == userId && c.IsDeleted == false)
									.OrderBy(c => c.Name)
									.ToList();
			}
		}

		#endregion

		#region Implementation of ITransactionService

		/// <summary>
		/// Gets all available asset types (i.e. "currency names").
		/// </summary>
		/// <returns>Asset types presented in the system.</returns>
		public IList<AssetType> GetAllAssetTypes()
		{
			using (var mc = new ModelContainer())
			{
				return mc.AssetTypes.ToList();
			}
		}

		/// <summary>
		/// Gets all available journal types (i.e. "Deposit", "Withdrawal", "Transfer" etc.).
		/// </summary>
		/// <returns>Journal types presented in the system.</returns>
		public IList<JournalType> GetAllJournalTypes()
		{
			return Enum.GetValues(typeof (JournalType)).Cast<JournalType>().ToList();
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
		/// <param name="categoryId">The category Id.</param>
		public void Deposit(Guid userId, int accountId, DateTime operationDate, decimal price, decimal quantity, string comment, int? categoryId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();
			var amount = price*quantity;

			using (var mc = new ModelContainer())
			{
				var targetAccount = ModelHelper.GetAccountById(mc, accountId);
				var cashAccount = ModelHelper.GetSystemAccount(mc, targetAccount.AssetType.Id);

				var transaction = new Transaction
									{
									  JournalType = (byte)JournalType.Deposit,
									  Price = price,
									  Quantity = quantity,
									  Comment = comment
									};

				if (categoryId.HasValue)
				{
					transaction.Category = ModelHelper.GetCategoryById(mc, categoryId.Value);
				}

				var creditPosting = new Posting
				{
					Date = dateTime,
					Amount = amount,
					Journal = transaction,
					Account = targetAccount,
					AssetType = targetAccount.AssetType
				};

				var debitPosting = new Posting
				{
					Date = dateTime,
					Amount = -amount,
					Journal = transaction,
					Account = cashAccount,
					AssetType = cashAccount.AssetType
				};

				mc.Journals.AddObject(transaction);
				mc.Postings.AddObject(creditPosting);
				mc.Postings.AddObject(debitPosting);

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
		public void Withdrawal(Guid userId, int accountId, DateTime operationDate, decimal price, decimal quantity, string comment, int? categoryId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			var dateTime = operationDate.ToUniversalTime();
			var amount = price * quantity;

			using (var mc = new ModelContainer())
			{
				var targetAccount = ModelHelper.GetAccountById(mc, accountId);
				var cashAccount = ModelHelper.GetSystemAccount(mc, targetAccount.AssetType.Id);

				var transaction = new Transaction
				{
					JournalType = (byte)JournalType.Withdrawal,
					Price = price,
					Quantity = quantity,
					Comment = comment
				};

				if (categoryId.HasValue)
				{
					transaction.Category = ModelHelper.GetCategoryById(mc, categoryId.Value);
				}

				var creditPosting = new Posting
				{
					Date = dateTime,
					Amount = amount,
					Journal = transaction,
					Account = cashAccount,
					AssetType = cashAccount.AssetType
				};

				var debitPosting = new Posting
				{
					Date = dateTime,
					Amount = -amount,
					Journal = transaction,
					Account = targetAccount,
					AssetType = targetAccount.AssetType
				};

				mc.Journals.AddObject(transaction);
				mc.Postings.AddObject(creditPosting);
				mc.Postings.AddObject(debitPosting);

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
		public void Transfer(Guid user1Id, int account1Id, Guid user2Id, int account2Id, DateTime operationDate, decimal amount, string comment)
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
				var sourceAccount = ModelHelper.GetAccountById(mc, account1Id);
				var targetAccount = ModelHelper.GetAccountById(mc, account2Id);

				var transaction = new Transaction
				{
					JournalType = (byte)JournalType.Transfer,
					Price = amount,
					Quantity = 1,
					Comment = comment
				};

				var creditPosting = new Posting
				{
					Date = dateTime,
					Amount = amount,
					Journal = transaction,
					Account = targetAccount,
					AssetType = targetAccount.AssetType
				};

				var debitPosting = new Posting
				{
					Date = dateTime,
					Amount = -amount,
					Journal = transaction,
					Account = sourceAccount,
					AssetType = sourceAccount.AssetType
				};

				mc.Journals.AddObject(transaction);
				mc.Postings.AddObject(creditPosting);
				mc.Postings.AddObject(debitPosting);

				mc.SaveChanges();
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
							   orderby p.Date
							   orderby p.Journal.Id
							   select new
									  {
									  	Posting = p,
										p.Journal,
// ReSharper disable PossibleNullReferenceException
// Note: do NOT use direct cast here because of LinqToEntity -> SQL conversion "not supported" exception
									  	p.Journal.Category,
										p.Journal.JournalType
// ReSharper restore PossibleNullReferenceException
									  };

				var res = postings.ToList();

				// No transactions take place yet, so nothing to return
				if (res.Count == 0)
				{
					return records;
				}

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
													Category = r.Category,
													Income = income,
													Expense = expense,
													Balance = balance,
													Comment = r.Journal.Comment
												});
				}
			}

			return records;
		}

		/// <summary>
		/// Delete specific transaction.
		/// </summary>
		/// <param name="userId">The user unique ID.</param>
		/// <param name="accountId">The account ID.</param>
		/// <param name="transactionId">Transaction ID.</param>
		public void DeleteTransaction(Guid userId, int accountId, int transactionId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				// Todo: add user ID account ID to the GetTransacionById() method call to
				// join them with transaction ID to prevent unauthorized delete 
				// Do this for all user-aware calls (i.e. Categories, Accounts etc.)
				Transaction transaction = ModelHelper.GetTransacionById(mc, transactionId);

				transaction.IsDeleted = true;

				mc.SaveChanges();
			}
		}

		#endregion
	}
}