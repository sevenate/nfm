// <copyright file="ApiServiceTests.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-30</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-30</date>
// </editor>
// <summary>
//   Unit tests for ApiService.
// </summary>

using System;
using System.Collections.Generic;
using Fab.Server.Core;
using Xunit;

namespace Fab.Server.Tests
{
	/// <summary>
	/// Unit tests for <see cref="ApiService"/>.
	/// </summary>
	public class ApiServiceTests
	{
		#region User Service

		/// <summary>
		/// Test <see cref="ApiService.Register"/> method.
		/// </summary>
		[Fact]
		public void RegisterNewUser()
		{
			var service = new ApiService();

			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			Assert.True(userId != Guid.Empty);
		}

		/// <summary>
		/// Test <see cref="ApiService.IsLoginAvailable"/> method.
		/// </summary>
		[Fact]
		public void CheckIsLoginAvailable()
		{
			var service = new ApiService();

			bool isAvailable = service.IsLoginAvailable("testUser" + Guid.NewGuid());

			Assert.True(isAvailable);
		}

		/// <summary>
		/// Test <see cref="ApiService.IsLoginAvailable"/> method.
		/// </summary>
		[Fact]
		public void CheckIsLoginNotAvailable()
		{
			string login = "testUser" + Guid.NewGuid();
			var service = new ApiService();
			service.Register(login, "testPassword");

			bool isAvailable = service.IsLoginAvailable(login);

			Assert.False(isAvailable);
		}

		/// <summary>
		/// Test <see cref="ApiService.GenerateUniqueLogin"/> method.
		/// </summary>
		[Fact]
		public void GenerateUniqueUserLogin()
		{
			var service = new ApiService();

			string uniqueLogin = service.GenerateUniqueLogin();

			bool isAvailable = service.IsLoginAvailable(uniqueLogin);
			Assert.True(isAvailable);
		}

		/// <summary>
		/// Test <see cref="ApiService.Update"/> method.
		/// </summary>
		[Fact]
		public void UpdateUser()
		{
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.Update(userId, "testPassword", "newTestPassword", "new@email");
		}

		/// <summary>
		/// Test <see cref="ApiService.GetUserId"/> method.
		/// </summary>
		[Fact]
		public void GetUserId()
		{
			string login = "testUser" + Guid.NewGuid();
			var service = new ApiService();
			Guid userId = service.Register(login, "testPassword");

			Guid actualUserId = service.GetUserId(login);

			Assert.True(userId != Guid.Empty);
			Assert.True(actualUserId != Guid.Empty);
			Assert.True(userId == actualUserId);
		}

		/// <summary>
		/// Test <see cref="ApiService.ResetPassword"/> method.
		/// </summary>
		[Fact]
		public void ResetPassword()
		{
			string login = "testUser" + Guid.NewGuid();
			var service = new ApiService();
			Guid userId = service.Register(login, "testPassword");
			service.Update(userId, "testPassword", "newTestPassword", "new@email");

			service.ResetPassword(login, "new@email");
		}

		#endregion

		#region Account Service

		/// <summary>
		/// Test <see cref="ApiService.CreateAccount"/> method.
		/// </summary>
		[Fact]
		public void CreateAccount()
		{
			string accountName = "Test Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.CreateAccount(userId, accountName);

			IList<Account> accounts = service.GetAllAccounts(userId);
			Assert.Equal(accounts.Count, 1);
			Assert.Equal(accounts[0].Name, accountName);
		}

		/// <summary>
		/// Test <see cref="ApiService.UpdateAccount"/> method.
		/// </summary>
		[Fact]
		public void UpdateAccount()
		{
			string accountName = "Test Account";
			string newAccountName = "Renamed Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, accountName);
			IList<Account> accounts = service.GetAllAccounts(userId);

			service.UpdateAccount(userId, accounts[0].Id, newAccountName);

			accounts = service.GetAllAccounts(userId);
			Assert.Equal(accounts.Count, 1);
			Assert.Equal(accounts[0].Name, newAccountName);
		}

		/// <summary>
		/// Test <see cref="ApiService.DeleteAccount"/> method.
		/// </summary>
		[Fact]
		public void DeleteAccount()
		{
			string accountName = "Test Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, accountName);
			IList<Account> accounts = service.GetAllAccounts(userId);

			service.DeleteAccount(userId, accounts[0].Id);

			accounts = service.GetAllAccounts(userId);
			Assert.Equal(accounts.Count, 0);
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAllAccounts"/> method.
		/// </summary>
		[Fact]
		public void GetAllAccounts()
		{
			string accountName = "Test Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, accountName);

			IList<Account> accounts = service.GetAllAccounts(userId);

			Assert.Equal(accounts.Count, 1);
			Assert.Equal(accounts[0].Name, accountName);
		}

		#endregion

		#region Category Service

		/// <summary>
		/// Test <see cref="ApiService.CreateCategory"/> method.
		/// </summary>
		[Fact]
		public void CreateCategory()
		{
			string categoryName = "Test Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.CreateCategory(userId, categoryName);

			IList<Category> categories = service.GetAllCategories(userId);
			Assert.Equal(categories.Count, 1);
			Assert.Equal(categories[0].Name, categoryName);
		}

		/// <summary>
		/// Test <see cref="ApiService.UpdateCategory"/> method.
		/// </summary>
		[Fact]
		public void UpdateCategory()
		{
			string categoryName = "Test Category";
			string newCategoryName = "Renamed Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateCategory(userId, categoryName);
			IList<Category> categories = service.GetAllCategories(userId);

			service.UpdateCategory(userId, categories[0].Id, newCategoryName);

			categories = service.GetAllCategories(userId);
			Assert.Equal(categories.Count, 1);
			Assert.Equal(categories[0].Name, newCategoryName);
		}

		/// <summary>
		/// Test <see cref="ApiService.DeleteCategory"/> method.
		/// </summary>
		[Fact]
		public void DeleteCategory()
		{
			string categoryName = "Test Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateCategory(userId, categoryName);
			IList<Category> categories = service.GetAllCategories(userId);

			service.DeleteCategory(userId, categories[0].Id);

			categories = service.GetAllCategories(userId);
			Assert.Equal(categories.Count, 0);
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAllCategories"/> method.
		/// </summary>
		[Fact]
		public void GetAllCategories()
		{
			string categoryName = "Test Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateCategory(userId, categoryName);

			IList<Category> categories = service.GetAllCategories(userId);

			Assert.Equal(categories.Count, 1);
			Assert.Equal(categories[0].Name, categoryName);
		}

		#endregion

		#region Transaction Service

		/// <summary>
		/// Test <see cref="ApiService.GetAllAssetTypes"/> method.
		/// </summary>
		[Fact]
		public void GetAllAssetTypes()
		{
			var service = new ApiService();

			IList<AssetType> assets = service.GetAllAssetTypes();

			Assert.True(assets.Count == 4);
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAllJournalTypes"/> method.
		/// </summary>
		[Fact]
		public void GetAllJournalTypes()
		{
			var service = new ApiService();

			IList<JournalType> journals = service.GetAllJournalTypes();

			Assert.True(journals.Count == 3);
		}

		#endregion
	}
}