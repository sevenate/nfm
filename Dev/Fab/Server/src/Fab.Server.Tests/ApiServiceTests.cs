// <copyright file="ApiServiceTests.cs" company="HD">
//  Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" />
// <summary>Unit tests for ApiService.</summary>

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

			Assert.NotEqual(Guid.Empty, userId);
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

			Assert.NotEqual(Guid.Empty, userId);
			Assert.NotEqual(Guid.Empty, actualUserId);
			Assert.Equal(userId, actualUserId);
		}

		/// <summary>
		/// Test <see cref="ApiService.ResetPassword"/> method.
		/// </summary>
		[Fact(Skip = "not implemented")]
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
			const string expectedAccountName = "Test Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.CreateAccount(userId, expectedAccountName, 1);

			var accounts = service.GetAllAccounts(userId);
			Assert.Equal(1, accounts.Count);
			Assert.Equal(expectedAccountName, accounts[0].Name);
		}

		/// <summary>
		/// Test <see cref="ApiService.UpdateAccount"/> method.
		/// </summary>
		[Fact]
		public void UpdateAccount()
		{
			const string accountName = "Test Account";
			const string expectedNewAccountName = "Renamed Account";
			const int assetType = 1;
			const int expectedNewAssetType = 2;
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, accountName, assetType);
			var accounts = service.GetAllAccounts(userId);

			service.UpdateAccount(userId, accounts[0].Id, expectedNewAccountName, expectedNewAssetType);

			accounts = service.GetAllAccounts(userId);
			Assert.Equal(1, accounts.Count);
			Assert.Equal(expectedNewAccountName, accounts[0].Name);
			Assert.Equal(expectedNewAssetType, accounts[0].AssetType.Id);
		}

		/// <summary>
		/// Test <see cref="ApiService.DeleteAccount"/> method.
		/// </summary>
		[Fact]
		public void DeleteAccount()
		{
			const string accountName = "Test Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, accountName, 1);
			var accounts = service.GetAllAccounts(userId);

			service.DeleteAccount(userId, accounts[0].Id);

			accounts = service.GetAllAccounts(userId);
			Assert.Equal(0, accounts.Count);
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAllAccounts"/> method.
		/// </summary>
		[Fact]
		public void GetAllAccounts()
		{
			const string expectedAccountName = "Test Account";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, expectedAccountName, 1);

			var accounts = service.GetAllAccounts(userId);

			Assert.Equal(1, accounts.Count);
			Assert.Equal(expectedAccountName, accounts[0].Name);
		}

		#endregion

		#region Category Service

		/// <summary>
		/// Test <see cref="ApiService.CreateCategory"/> method.
		/// </summary>
		[Fact]
		public void CreateCategory()
		{
			const string expectedCategoryName = "Test Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.CreateCategory(userId, expectedCategoryName);

			var categories = service.GetAllCategories(userId);
			Assert.Equal(1, categories.Count);
			Assert.Equal(expectedCategoryName, categories[0].Name);
		}

		/// <summary>
		/// Test <see cref="ApiService.UpdateCategory"/> method.
		/// </summary>
		[Fact]
		public void UpdateCategory()
		{
			const string categoryName = "Test Category";
			const string expectedNewCategoryName = "Renamed Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateCategory(userId, categoryName);
			var categories = service.GetAllCategories(userId);

			service.UpdateCategory(userId, categories[0].Id, expectedNewCategoryName);

			categories = service.GetAllCategories(userId);
			Assert.Equal(1, categories.Count);
			Assert.Equal(expectedNewCategoryName, categories[0].Name);
		}

		/// <summary>
		/// Test <see cref="ApiService.DeleteCategory"/> method.
		/// </summary>
		[Fact]
		public void DeleteCategory()
		{
			const string categoryName = "Test Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateCategory(userId, categoryName);
			var categories = service.GetAllCategories(userId);

			service.DeleteCategory(userId, categories[0].Id);

			categories = service.GetAllCategories(userId);
			Assert.Equal(0, categories.Count);
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAllCategories"/> method.
		/// </summary>
		[Fact]
		public void GetAllCategories()
		{
			const string expectedCategoryName = "Test Category";
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateCategory(userId, expectedCategoryName);

			var categories = service.GetAllCategories(userId);

			Assert.Equal(1, categories.Count);
			Assert.Equal(expectedCategoryName, categories[0].Name);
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

			var assets = service.GetAllAssetTypes();

			Assert.Equal(4, assets.Count);
		}

		/// <summary>
		/// Test <see cref="ApiService.Deposit"/> method.
		/// </summary>
		[Fact]
		public void Deposit()
		{
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, "Test Account", 1);
			var accounts = service.GetAllAccounts(userId);
			service.CreateCategory(userId, "Test Category");
			var categories = service.GetAllCategories(userId);

			service.Deposit(userId, accounts[0].Id, DateTime.Now, 25, 2, "Some income comment", categories[0].Id);
		}

		/// <summary>
		/// Test <see cref="ApiService.Withdrawal"/> method.
		/// </summary>
		[Fact]
		public void Withdrawal()
		{
			var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, "Test Account", 1);
			IList<Account> accounts = service.GetAllAccounts(userId);
			service.CreateCategory(userId, "Test Category");
			IList<Category> categories = service.GetAllCategories(userId);

			service.Withdrawal(userId, accounts[0].Id, DateTime.Now, 13, 10, "Some expense comment", categories[0].Id);
		}

		/// <summary>
		/// Test <see cref="ApiService.Transfer"/> method.
		/// </summary>
		[Fact]
		public void Transfer()
		{
			var service = new ApiService();
			Guid userId1 = service.Register("testUser1" + Guid.NewGuid(), "testPassword");
			Guid userId2 = service.Register("testUser2" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId1, "Test Account 1", 1);
			service.CreateAccount(userId2, "Test Account 2", 1);
			var accounts1 = service.GetAllAccounts(userId1);
			var accounts2 = service.GetAllAccounts(userId2);

			service.Transfer(userId1, accounts1[0].Id, userId2, accounts2[0].Id, DateTime.Now, 78, "Some transfer comment");
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAccountBalance"/> method.
		/// </summary>
		[Fact]
		public void GetAccountBalance()
		{
			var service = new ApiService();
			Guid userId1 = service.Register("testUser1" + Guid.NewGuid(), "testPassword");
			Guid userId2 = service.Register("testUser2" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId1, "Test Account 1", 1);
			service.CreateAccount(userId2, "Test Account 2", 1);
			var accounts1 = service.GetAllAccounts(userId1);
			var accounts2 = service.GetAllAccounts(userId2);
			service.CreateCategory(userId1, "Test Category 1");
			var categories1 = service.GetAllCategories(userId1);

			service.Deposit(userId1, accounts1[0].Id, DateTime.Now, 25, 10, "Some income comment", null);

			var balance = service.GetAccountBalance(userId1, accounts1[0].Id);
			Assert.Equal(250, balance);

			service.Withdrawal(userId1, accounts1[0].Id, DateTime.Now, 10, 5, "Some expense comment", categories1[0].Id);

			balance = service.GetAccountBalance(userId1, accounts1[0].Id);
			Assert.Equal(200, balance);

			balance = service.GetAccountBalance(userId2, accounts2[0].Id);
			Assert.Equal(0, balance);

			service.Transfer(userId1, accounts1[0].Id, userId2, accounts2[0].Id, DateTime.Now, 75, "Some transfer comment");

			balance = service.GetAccountBalance(userId1, accounts1[0].Id);
			Assert.Equal(125, balance);

			balance = service.GetAccountBalance(userId2, accounts2[0].Id);
			Assert.Equal(75, balance);
		}

		/// <summary>
		/// Test <see cref="ApiService.GetAllTransactions"/> method.
		/// </summary>
		[Fact]
		public void GetAllTransactions()
		{
			var service = new ApiService();
			Guid userId = service.Register("testUser1" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, "Test Account 1", 1);
			var accounts = service.GetAllAccounts(userId);
			service.CreateCategory(userId, "Test Category 1");
			var categories = service.GetAllCategories(userId);

			service.Deposit(userId, accounts[0].Id, DateTime.Now, 25, 10, "Some income comment", null);
			service.Withdrawal(userId, accounts[0].Id, DateTime.Now, 10, 5, "Some expense comment", categories[0].Id);

			var transactionRecords = service.GetAllTransactions(userId, accounts[0].Id);

			Assert.True(transactionRecords != null);
			Assert.True(transactionRecords.Count  == 2);
		}


		/// <summary>
		/// Test <see cref="ApiService.UpdateTransaction"/> method.
		/// </summary>
		[Fact]
		public void UpdateTransaction()
		{
			var service = new ApiService();
			Guid userId = service.Register("testUser1" + Guid.NewGuid(), "testPassword");
			service.CreateAccount(userId, "Test Account 1", 1);
			var accounts = service.GetAllAccounts(userId);
			service.CreateCategory(userId, "Test Category 1");
			var categories = service.GetAllCategories(userId);

			service.Deposit(userId, accounts[0].Id, DateTime.Now, 25, 10, "Some income comment", null);
			service.Withdrawal(userId, accounts[0].Id, DateTime.Now, 10, 5, "Some expense comment", categories[0].Id);
			
			var balance = service.GetAccountBalance(userId, accounts[0].Id);
			Assert.Equal(200, balance);

			var transactionRecords = service.GetAllTransactions(userId, accounts[0].Id);
			var transactionId = transactionRecords[0].TransactionId;

			Assert.True(transactionRecords != null);
			Assert.True(transactionRecords.Count == 2);

			service.UpdateTransaction(transactionId, userId, accounts[0].Id, DateTime.Now, 32, 20, "Updated income", categories[0].Id, false);

			balance = service.GetAccountBalance(userId, accounts[0].Id);
			Assert.Equal(-690, balance);
		}

		#endregion
	}
}