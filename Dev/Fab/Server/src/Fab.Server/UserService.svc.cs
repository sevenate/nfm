// <copyright file="UserService.svc.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-01-28" />
// <summary>User service.</summary>

using System;
using System.Linq;
using Fab.Server.Core;

namespace Fab.Server
{
	/// <summary>
	/// User service.
	/// </summary>
	public class UserService : IUserService
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

			// Check login min length
			if (newLogin.Length < 5)
			{
				throw new Exception("Login name is too short. Minimum length is 5.");
			}

			// Check login max length
			if (newLogin.Length > 50)
			{
				throw new Exception("Login name is too long. Maximum length is 50.");
			}

			// Check password min length
			if (password.Length < 5)
			{
				throw new Exception("New password is too short. Minimum length is 5.");
			}

			// Check password max length
			if (password.Length > 256)
			{
				throw new Exception("New password is too long. Maximum length is 256.");
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
				throw new Exception("New password is too short. Minimum length is 5.");
			}

			// Check password max length
			if (newPassword.Length > 256)
			{
				throw new Exception("New password is too long. Maximum length is 256.");
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
	}
}
