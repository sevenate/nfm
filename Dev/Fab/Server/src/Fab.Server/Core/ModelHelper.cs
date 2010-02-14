// <copyright file="ModelHelper.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
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
		/// Check is user <paramref name="login"/> name is not used by some one else.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="login">User login name.</param>
		/// <returns><c>True</c> if user login name is unique.</returns>
		public static bool IsLoginAvailable(ModelContainer mc, string login)
		{
			return mc.Users.Where(u => u.Login == login).SingleOrDefault() == null;
		}

		/// <summary>
		/// Get <see cref="User"/> from database by unique ID.
		/// </summary>
		/// <param name="mc">Entity Framework model container.</param>
		/// <param name="userId">Unique user ID.</param>
		/// <returns>Fount user object or null otherwise.</returns>
		public static User GetUserById(ModelContainer mc, Guid userId)
		{
			var user = mc.Users.Where(u => u.Id == userId).SingleOrDefault();

			if (user == null)
			{
				throw new Exception("User with ID \"" + userId + "\" not found.");
			}

			return user;
		}
	}
}