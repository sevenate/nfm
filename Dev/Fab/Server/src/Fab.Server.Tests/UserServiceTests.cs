// <copyright file="UserServiceTests.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-02-04" />
// <summary>Unit tests for UserService.</summary>

using System;
using Xunit;

namespace Fab.Server.Tests
{
	/// <summary>
	/// Unit tests for <see cref="UserService"/>.
	/// </summary>
	public class UserServiceTests
	{
		#region User Service

		/// <summary>
		/// Test <see cref="UserService.Register"/> method.
		/// </summary>
		[Fact]
		public void RegisterNewUser()
		{
			var service = new UserService();

			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			Assert.NotEqual(Guid.Empty, userId);
		}

		/// <summary>
		/// Test <see cref="UserService.IsLoginAvailable"/> method.
		/// </summary>
		[Fact]
		public void CheckIsLoginAvailable()
		{
			var service = new UserService();

			bool isAvailable = service.IsLoginAvailable("testUser" + Guid.NewGuid());

			Assert.True(isAvailable);
		}

		/// <summary>
		/// Test <see cref="UserService.IsLoginAvailable"/> method.
		/// </summary>
		[Fact]
		public void CheckIsLoginNotAvailable()
		{
			string login = "testUser" + Guid.NewGuid();
			var service = new UserService();
			service.Register(login, "testPassword");

			bool isAvailable = service.IsLoginAvailable(login);

			Assert.False(isAvailable);
		}

		/// <summary>
		/// Test <see cref="UserService.GenerateUniqueLogin"/> method.
		/// </summary>
		[Fact]
		public void GenerateUniqueUserLogin()
		{
			var service = new UserService();

			string uniqueLogin = service.GenerateUniqueLogin();

			bool isAvailable = service.IsLoginAvailable(uniqueLogin);
			Assert.True(isAvailable);
		}

		/// <summary>
		/// Test <see cref="UserService.Update"/> method.
		/// </summary>
		[Fact]
		public void UpdateUser()
		{
			var service = new UserService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.Update(userId, "testPassword", "newTestPassword", "new@email");
		}

		/// <summary>
		/// Test <see cref="UserService.GetUserId"/> method.
		/// </summary>
		[Fact]
		public void GetUserId()
		{
			string login = "testUser" + Guid.NewGuid();
			var service = new UserService();
			Guid userId = service.Register(login, "testPassword");

			Guid actualUserId = service.GetUserId(login);

			Assert.NotEqual(Guid.Empty, userId);
			Assert.NotEqual(Guid.Empty, actualUserId);
			Assert.Equal(userId, actualUserId);
		}

		/// <summary>
		/// Test <see cref="UserService.ResetPassword"/> method.
		/// </summary>
		[Fact(Skip = "not implemented")]
		public void ResetPassword()
		{
			string login = "testUser" + Guid.NewGuid();
			var service = new UserService();
			Guid userId = service.Register(login, "testPassword");
			service.Update(userId, "testPassword", "newTestPassword", "new@email");

			service.ResetPassword(login, "new@email");
		}

		#endregion
	}
}