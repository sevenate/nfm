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
// <summary>Represent public server API available for clients.</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using Fab.Server.Core;

namespace Fab.Server
{
	/// <summary>
	/// Represent public server API available for clients.
	/// </summary>
	public class ApiService : IUserService, IAccountService, ITransactionService
	{
		#region Implementation of IUserService

		/// <summary>
		/// Generate unique login name for new user.
		/// </summary>
		/// <returns>
		/// Unique login name.
		/// </returns>
		public string GenerateUniqueLogin()
		{
			// Todo: use more sophisticate algorithm for uniqueness login generation 
			return "a" + Guid.NewGuid().GetHashCode();
		}

		/// <summary>
		/// Check user login name for uniqueness.
		/// </summary>
		/// <param name="login">
		/// User login.
		/// </param>
		/// <returns>
		/// <c>True</c> if user login name is unique.
		/// </returns>
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
		/// <param name="login">
		/// User login name.
		/// </param>
		/// <param name="password">
		/// User password.
		/// </param>
		/// <returns>
		/// Created user ID.
		/// </returns>
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
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="oldPassword">
		/// User old password.
		/// </param>
		/// <param name="newPassword">
		/// User new password.
		/// </param>
		/// <param name="newEmail">
		/// User new email.
		/// </param>
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
				user.Email = newEmail != null
				             	? newEmail.Trim()
				             	: null;

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Get user ID by unique login name.
		/// </summary>
		/// <param name="login">
		/// User unique login name.
		/// </param>
		/// <returns>
		/// User unique ID.
		/// </returns>
		public Guid GetUserId(string login)
		{
			if (string.IsNullOrWhiteSpace(login))
			{
				throw new ArgumentException("Login must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				return mc.Users.Where(u => u.Login == login.Trim()).Select(u => u.Id).SingleOrDefault();
			}
		}

		/// <summary>
		/// If user with specified login name have email and this email is match to specified email,
		/// then system will reset current password for this user to autogenerated new one
		/// and sent it to the specified email.
		/// </summary>
		/// <param name="login">
		/// User login name.
		/// </param>
		/// <param name="email">
		/// User email.
		/// </param>
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
		/// Create new acount.
		/// </summary>
		/// <param name="userId">
		/// User unique ID for which this account is created.
		/// </param>
		/// <param name="name">
		/// Account name.
		/// </param>
		public void CreateAccount(Guid userId, string name)
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

				var account = new Account
				              {
				              	Name = name.Trim(), 
				              	Created = DateTime.UtcNow, 
				              	IsDeleted = false, 
				              	User = user
				              };

				mc.Accounts.AddObject(account);
				mc.SaveChanges();
			}
		}

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
		public void UpdateAccount(Guid userId, int accountId, string name)
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
				Account account = ModelHelper.GetAccountById(mc, userId, accountId);

				account.Name = name.Trim();

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Mark account as "deleted".
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <param name="accountId">
		/// Account ID to mark as deleted.
		/// </param>
		public void DeleteAccount(Guid userId, int accountId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("User ID must not be empty.");
			}

			using (var mc = new ModelContainer())
			{
				Account account = ModelHelper.GetAccountById(mc, userId, accountId);

				account.IsDeleted = true;

				mc.SaveChanges();
			}
		}

		/// <summary>
		/// Retrieve all accounts for user.
		/// </summary>
		/// <param name="userId">
		/// User unique ID.
		/// </param>
		/// <returns>
		/// All accounts.
		/// </returns>
		public IList<Account> GetAllAccounts(Guid userId)
		{
			using (var mc = new ModelContainer())
			{
				return mc.Accounts.Where(a => a.User.Id == userId && a.IsDeleted == false).OrderBy(a => a.Created).ToList();
			}
		}

		#endregion

		#region Implementation of ITransactionService

		/// <summary>
		/// Gets all available asset types (i.e. "currency names").
		/// </summary>
		/// <returns>
		/// Asset types presented in the system.
		/// </returns>
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
		/// <returns>
		/// Journal types presented in the system.
		/// </returns>
		public IList<JournalType> GetAllJournalTypes()
		{
			using (var mc = new ModelContainer())
			{
				return mc.JournalTypes.ToList();
			}
		}

		#endregion
	}
}