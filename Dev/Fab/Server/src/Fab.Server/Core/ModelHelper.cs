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
// <summary>
//   Helper for Entity Framework model container processing.
// </summary>

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
		/// Check is user <paramref name="login"/> name is not used by some one else.
		/// </summary>
		/// <param name="mc">
		/// Entity Framework model container.
		/// </param>
		/// <param name="login">
		/// User login name.
		/// </param>
		/// <returns>
		/// <c>True</c> if user login name is unique.
		/// </returns>
		internal static bool IsLoginAvailable(ModelContainer mc, string login)
		{
			return mc.Users.Where(u => u.Login == login).SingleOrDefault() == null;
		}

		/// <summary>
		/// Get <see cref="User"/> from model container by unique ID.
		/// </summary>
		/// <param name="mc">
		/// Entity Framework model container.
		/// </param>
		/// <param name="userId">
		/// Unique user ID.
		/// </param>
		/// <returns>
		/// Found user object or <c>null</c> otherwise.
		/// </returns>
		internal static User GetUserById(ModelContainer mc, Guid userId)
		{
			User user = mc.Users.Where(u => u.Id == userId).SingleOrDefault();

			if (user == null)
			{
				throw new Exception("User with ID \"" + userId + "\" not found.");
			}

			return user;
		}

		/// <summary>
		/// Get <see cref="Account"/> from model container by unique user ID and account ID.
		/// </summary>
		/// <param name="mc">
		/// Entity Framework model container.
		/// </param>
		/// <param name="userId">
		/// Unique user ID.
		/// </param>
		/// <param name="accountId">
		/// Account ID.
		/// </param>
		/// <returns>
		/// Found account object or <c>null</c> otherwise.
		/// </returns>
		internal static Account GetAccountById(ModelContainer mc, Guid userId, int accountId)
		{
			Account account = mc.Accounts.Where(a => a.Id == accountId && a.User.Id == userId).SingleOrDefault();

			if (account == null)
			{
				throw new Exception("Account with ID = \"" + accountId + "\" for user \"" + userId + "\" not found.");
			}

			return account;
		}

		/// <summary>
		/// Get <see cref="Category"/> from model container by unique user ID and category ID.
		/// </summary>
		/// <param name="mc">
		/// Entity Framework model container.
		/// </param>
		/// <param name="userId">
		/// Unique user ID.
		/// </param>
		/// <param name="categoryId">
		/// Category ID.
		/// </param>
		/// <returns>
		/// Found category object or <c>null</c> otherwise.
		/// </returns>
		internal static Category GetCategoryById(ModelContainer mc, Guid userId, int categoryId)
		{
			Category category = mc.Categories.Where(c => c.Id == categoryId && c.User.Id == userId).SingleOrDefault();

			if (category == null)
			{
				throw new Exception("Category with ID = \"" + categoryId + "\" for user \"" + userId + "\" not found.");
			}

			return category;
		}
	}
}