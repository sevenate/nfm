// <copyright file="AdminServiceTests.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-04</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-04</date>
// </editor>
// <summary>Unit tests for AdminService.</summary>

using System;
using System.Linq;
using Fab.Server.Core;
using Xunit;

namespace Fab.Server.Tests
{
	/// <summary>
	/// Unit tests for <see cref="AdminService" />.
	/// </summary>
	public class AdminServiceTests
	{
		#region Admin Service

		/// <summary>
		/// Test <see cref="AdminService.GetAll"/> method.
		/// </summary>
		[Fact]
		public void GetAllUsers()
		{
			var adminService = new AdminService();

			var users = adminService.GetAll();

			Assert.True(users != null && users.Count > 0);
			Assert.True(users[0].IsDisabled);
		}

		/// <summary>
		/// Test <see cref="AdminService.Disable"/> method.
		/// </summary>
		[Fact]
		public void DisableUser()
		{
			var service = new UserService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			var adminService = new AdminService();

			adminService.Disable(userId);

			var users = adminService.GetAll();

			Assert.True(users != null && users.Count > 1);

			User user = users.Where(u => u.Id == userId).Single();
			Assert.True(user.IsDisabled);
		}

		#endregion
	}
}