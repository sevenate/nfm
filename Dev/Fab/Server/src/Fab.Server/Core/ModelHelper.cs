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
		internal static Account GetSystemAccount(ModelContainer mc, int assetTypeId)
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
		/// Get <see cref="JournalType"/> from model container by unique ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="journalTypeId">The journal type ID.</param>
		/// <returns>Journal type instance</returns>
		internal static JournalType GetJournalTypeById(ModelContainer mc, int journalTypeId)
		{
			JournalType journalType = mc.JournalTypes.Where(jt => jt.Id == journalTypeId)
										.SingleOrDefault();

			if (journalType == null)
			{
				throw new Exception("Journal type with ID = " + journalTypeId + " not found.");
			}

			return journalType;
		}

		/// <summary>
		/// Get <see cref="Transaction"/> from model container by unique ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="transactionId">The transaction ID.</param>
		/// <returns>Transaction instance.</returns>
		internal static Transaction GetTransacionById(ModelContainer mc, int transactionId)
		{
			Transaction transacion = mc.Journals.Where(j => j.Id == transactionId && j is Transaction)
												.Select(j => j as Transaction)
												.SingleOrDefault();

			if (transacion == null)
			{
				throw new Exception("Transaction with ID = " + transactionId + " not found.");
			}

			return transacion;
		}
	}
}